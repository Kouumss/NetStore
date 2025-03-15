using StoreNet.Domain.Layer.DTOs;

namespace StoreNet.Domain.Layer.Interfaces;

public interface IAddressService
{
    Task<ApiResponse<AddressResponseDTO>> CreateAddressAsync(AddressCreateDTO addressDto);
    Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(string id);
    Task<ApiResponse<List<AddressResponseDTO>>> GetAddressesByCustomerAsync(string customerId);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(AddressDeleteDTO addressDeleteDTO);
    Task<ApiResponse<AddressResponseDTO>> GetBillingAddressAsync(string customerId);
    Task<ApiResponse<AddressResponseDTO>> GetShippingAddressAsync(string customerId);
}
