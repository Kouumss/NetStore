using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using StoreNet.Application.Layer.Factories;

using AutoMapper;

namespace StoreNet.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IEntityFactory _entityFactory;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IAddressRepository addressRepository, IEntityFactory entityFactory, IMapper mapConfig)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _entityFactory = entityFactory;
            _mapper = mapConfig; 
        }

        public async Task<ApiResponse<List<CustomerResponseDTO>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAllCustomersAsync();

                var customerResponseDtos = _mapper.Map<List<Customer>, List<CustomerResponseDTO>>(customers);

                return new ApiResponse<List<CustomerResponseDTO>>(200, customerResponseDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CustomerResponseDTO>>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CustomerResponseDTO>> RegisterCustomerAsync(CustomerRegistrationDTO customerDto)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetByEmailAsync(customerDto.Email.ToLower());
                if (existingCustomer is not null)
                {
                    return new ApiResponse<CustomerResponseDTO>(400, "Email is already in use.");
                }

                var customer = _entityFactory.CreateEntity<Customer>();
                customer.FirstName = customerDto.FirstName;
                customer.LastName = customerDto.LastName;
                customer.Email = customerDto.Email;
                customer.PhoneNumber = customerDto.PhoneNumber;
                customer.DateOfBirth = customerDto.DateOfBirth;
                customer.IsActive = true;
                customer.Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password);

                await _customerRepository.AddAsync(customer);

                var address = _entityFactory.CreateEntity<Address>();
                address.CustomerId = customer.Id;
                address.AddressLine1 = customerDto.AddressLine1;
                address.AddressLine2 = customerDto.AddressLine2;
                address.City = customerDto.City;
                address.State = customerDto.State;
                address.PostalCode = customerDto.PostalCode;
                address.Country = customerDto.Country;

                await _addressRepository.AddAsync(address);

                customer.Addresses = new List<Address> { address };
                await _customerRepository.UpdateAsync(customer);

                // Map the created customer to a DTO
                var customerResponse = _mapper.Map<Customer, CustomerResponseDTO>(customer);

                return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto)
        {
            try
            {
                var customer = await _customerRepository.GetByEmailAsync(loginDto.Email);
                if (customer is null)
                {
                    return new ApiResponse<LoginResponseDTO>(401, "Invalid email or password.");
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.Password);
                if (!isPasswordValid)
                {
                    return new ApiResponse<LoginResponseDTO>(401, "Invalid email or password.");
                }

                var loginResponse = new LoginResponseDTO
                {
                    Message = "Login successful.",
                    CustomerId = customer.Id,
                    CustomerName = $"{customer.FirstName} {customer.LastName}"
                };

                return new ApiResponse<LoginResponseDTO>(200, loginResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CustomerResponseDTO>> GetCustomerByIdAsync(string id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer is null)
                {
                    return new ApiResponse<CustomerResponseDTO>(404, "Customer not found.");
                }

                var customerResponse = _mapper.Map<Customer, CustomerResponseDTO>(customer);

                // If the customer has addresses, map them as well
                if (customer.Addresses?.Any() == true)
                {
                    customerResponse.Addresses = _mapper.Map<List<Address>, List<AddressResponseDTO>>(customer.Addresses.ToList());
                }

                // If the customer has orders, map them as well
                if (customer.Orders?.Any() == true)
                {
                    customerResponse.Orders = _mapper.Map<List<Order>, List<OrderResponseDTO>>(customer.Orders.ToList());
                }

                return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCustomerAsync(CustomerUpdateDTO customerDto)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(customerDto.CustomerId);
                if (customer is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
                }

                if (customer.Email != customerDto.Email && await _customerRepository.AnyAsync(c => c.Email == customerDto.Email))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Email is already in use.");
                }

                customer.FirstName = customerDto.FirstName;
                customer.LastName = customerDto.LastName;
                customer.Email = customerDto.Email;
                customer.PhoneNumber = customerDto.PhoneNumber;
                customer.DateOfBirth = customerDto.DateOfBirth;

                await _customerRepository.UpdateAsync(customer);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Customer with Id {customerDto.CustomerId} updated successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCustomerAsync(string id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
                }

                customer.IsActive = false;
                await _customerRepository.UpdateAsync(customer);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Customer with Id {id} deleted successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> ChangePasswordAsync(ChangePasswordDTO changePasswordDto)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(changePasswordDto.CustomerId);
                if (customer is null || !customer.IsActive)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found or inactive.");
                }

                bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, customer.Password);
                if (!isCurrentPasswordValid)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(401, "Current password is incorrect.");
                }

                customer.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                await _customerRepository.UpdateAsync(customer);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = "Password changed successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
