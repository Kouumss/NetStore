using StoreNet.Domain.Layer.DTOs;

namespace StoreNet.Domain.Layer.Interfaces;

public interface IAddressService
{
    Task<ApiResponse<AddressResponseDTO>> CreateAddressAsync(AddressCreateDTO addressDto);
    Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(Guid id);
    Task<ApiResponse<List<AddressResponseDTO>>> GetAddressesByCustomerAsync(Guid customerId);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(AddressDeleteDTO addressDeleteDTO);
}
