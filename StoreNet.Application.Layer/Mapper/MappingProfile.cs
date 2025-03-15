using AutoMapper;
using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping pour l'entité Address et ses DTOs
        CreateMap<Address, AddressResponseDTO>()
            .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType.ToString())); // Convertir AddressType en string

        CreateMap<AddressCreateDTO, Address>()
            .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => Enum.Parse<AddressType>(src.AddressType))); // Convertir string en AddressType

        // Mapping pour l'entité Customer et ses DTOs
        CreateMap<CustomerRegistrationDTO, Customer>();
        CreateMap<Customer, CustomerResponseDTO>()
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses))
            .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders));

        // Mapping pour l'entité Order et ses DTOs
        CreateMap<OrderCreateDTO, Order>();
        CreateMap<Order, OrderItemResponseDTO>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id)); 
        CreateMap<Order, OrderResponseDTO>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

        // Mapping pour l'entité OrderItem et ses DTOs
        CreateMap<OrderItemCreateDTO, OrderItem>();
        CreateMap<OrderItem, OrderItemResponseDTO>();

        // Mapping pour l'entité Product et ses DTOs
        CreateMap<Product, ProductResponseDTO>();
        CreateMap<ProductCreateDTO, Product>();

        CreateMap<ProductUpdateDTO, Product>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())  // Ignorer la mise à jour de l'ID (si nécessaire)
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());  // Ignorer la mise à jour de la date de création (si nécessaire)


        // Mapping pour AddressCreateDTO, qui est spécifique au processus d'enregistrement.
        CreateMap<AddressCreateDTO, Address>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore()) // Exclure Customer car c'est une clé étrangère
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));

        // Cart and CartItem mappings
        CreateMap<Cart, CartResponseDTO>()
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

        CreateMap<CartItem, CartItemResponseDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));  // Mapping custom pour ProductName

    }
}
