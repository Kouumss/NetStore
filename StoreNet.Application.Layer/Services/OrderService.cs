using System.Security.Cryptography;
using StoreNet.Application.Layer.Factories;
using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using AutoMapper;

namespace StoreNet.Application.Layer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressService _addressService;
        private readonly IEntityFactory _entityFactory;
        private readonly IMapper _mapper; 

        private static readonly Dictionary<OrderStatus, List<OrderStatus>> AllowedStatusTransitions = new()
        {
            { OrderStatus.Pending, new List<OrderStatus> { OrderStatus.Processing, OrderStatus.Canceled } },
            { OrderStatus.Processing, new List<OrderStatus> { OrderStatus.Shipped, OrderStatus.Canceled } },
            { OrderStatus.Shipped, new List<OrderStatus> { OrderStatus.Delivered } },
            { OrderStatus.Delivered, new List<OrderStatus>() },
            { OrderStatus.Canceled, new List<OrderStatus>() }
        };

        public OrderService(
            IOrderRepository orderRepository,
            IProductService productService,
            ICustomerRepository customerRepository,
            IAddressService addressService,
            IEntityFactory entityFactory,
            IMapper mapper) 
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _customerRepository = customerRepository;
            _addressService = addressService;
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<OrderResponseDTO>> CreateOrderAsync(OrderCreateDTO orderDto)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(orderDto.CustomerId);
                if (customer == null)
                    return new ApiResponse<OrderResponseDTO>(400, "Customer not found.");

                var billingAddress = (await _addressService.GetBillingAddressAsync(orderDto.CustomerId)).Data;
                var shippingAddress = (await _addressService.GetShippingAddressAsync(orderDto.CustomerId)).Data;

                if (billingAddress == null || shippingAddress == null)
                    return new ApiResponse<OrderResponseDTO>(400, "Invalid addresses.");

                var orderItems = await ProcessOrderItems(orderDto.OrderItems);
                if (orderItems == null || !orderItems.Any())
                    return new ApiResponse<OrderResponseDTO>(400, "One or more products are invalid or out of stock.");

                decimal totalBaseAmount = orderItems.Sum(item => item.TotalPrice);
                decimal totalDiscountAmount = orderItems.Sum(item => item.Discount);
                decimal shippingCost = 10.00m;
                decimal totalAmount = totalBaseAmount - totalDiscountAmount + shippingCost;

                var order = _entityFactory.CreateEntity<Order>();
                order.OrderNumber = GenerateOrderNumber();
                order.CustomerId = orderDto.CustomerId;
                order.OrderDate = DateTime.UtcNow;
                order.BillingAddressId = billingAddress.Id;
                order.ShippingAddressId = shippingAddress.Id;
                order.TotalBaseAmount = totalBaseAmount;
                order.TotalDiscountAmount = totalDiscountAmount;
                order.ShippingCost = shippingCost;
                order.TotalAmount = totalAmount;
                order.OrderStatus = OrderStatus.Pending;
                order.OrderItems = orderItems;

                await _orderRepository.CreateOrderAsync(order);

                var orderResponse = _mapper.Map<OrderResponseDTO>(order);
                return new ApiResponse<OrderResponseDTO>(200, orderResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        private async Task<List<OrderItem>> ProcessOrderItems(List<OrderItemCreateDTO> orderItemsDto)
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in orderItemsDto)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);

                // Vérification de la validité du produit
                if (product is null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }

                if (product.Data.StockQuantity < item.Quantity)
                {
                    throw new Exception($"Insufficient stock for product with ID {item.ProductId}.");
                }

                var price = product.Data.Price;
                var stockQuantity = product.Data.StockQuantity;
                var totalPrice = price * item.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = price,
                    TotalPrice = totalPrice
                };

                // Mise à jour du stock
                product.Data.StockQuantity -= item.Quantity;
                await _productService.UpdateProductAsync(new ProductUpdateDTO
                {
                    Id = product.Data.Id,
                    StockQuantity = product.Data.StockQuantity
                });

                orderItems.Add(orderItem);
            }

            return orderItems;
        }


        public async Task<ApiResponse<OrderResponseDTO>> GetOrderByIdAsync(string orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order is null)
                    return new ApiResponse<OrderResponseDTO>(404, "Order not found.");

                var orderResponse = _mapper.Map<OrderResponseDTO>(order);
                return new ApiResponse<OrderResponseDTO>(200, orderResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<OrderResponseDTO>>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();
                var orderList = _mapper.Map<List<OrderResponseDTO>>(orders);
                return new ApiResponse<List<OrderResponseDTO>>(200, orderList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderResponseDTO>>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<OrderResponseDTO>>> GetOrdersByCustomerAsync(string customerId)
        {
            try
            {
                var customerOrders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
                var orderList = _mapper.Map<List<OrderResponseDTO>>(customerOrders);
                return new ApiResponse<List<OrderResponseDTO>>(200, orderList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderResponseDTO>>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateOrderStatusAsync(OrderStatusUpdateDTO statusDto)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(statusDto.OrderId);
                if (order is null)
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Order not found.");

                if (!AllowedStatusTransitions.TryGetValue(order.OrderStatus, out var allowedStatuses) ||
                    !allowedStatuses.Contains(statusDto.OrderStatus))
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Invalid status transition.");

                order.OrderStatus = statusDto.OrderStatus;
                await _orderRepository.UpdateOrderAsync(order);

                return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
                {
                    Message = "Order status updated successfully."
                });
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        private string GenerateOrderNumber()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfff");
            return $"ORD-{timestamp}-{RandomNumber(1000, 9999)}";
        }

        private int RandomNumber(int min, int max)
        {
            if (min > max)
                throw new ArgumentException("min must be less or equal to max.");
            return RandomNumberGenerator.GetInt32(min, max + 1);
        }
    }
}
