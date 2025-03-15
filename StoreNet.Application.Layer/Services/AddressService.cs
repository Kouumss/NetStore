using AutoMapper;
using StoreNet.Application.Layer.Factories;
using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.Application.Layer.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IEntityFactory _entityFactory;
        private readonly IMapper _mapConfig; // Ajouter MapConfig

        public AddressService(IAddressRepository addressRepository, IEntityFactory entityFactory, IMapper mapConfig)
        {
            _addressRepository = addressRepository;
            _entityFactory = entityFactory;
            _mapConfig = mapConfig; // Initialiser MapConfig
        }

        public async Task<ApiResponse<AddressResponseDTO>> CreateAddressAsync(AddressCreateDTO addressDto)
        {
            try
            {
                // Vérifier si une adresse existe déjà pour ce client
                var existingAddress = await _addressRepository.GetByCustomerIdAsync(addressDto.CustomerId);
                if (existingAddress is not null)
                {
                    // Si une adresse existe déjà pour le client, empêcher de créer une nouvelle adresse
                    return new ApiResponse<AddressResponseDTO>(400, "Customer already has an address.");
                }

                // Convertir le string "AddressType" en enum AddressType
                if (!Enum.TryParse(addressDto.AddressType, true, out AddressType addressTypeEnum))
                {
                    return new ApiResponse<AddressResponseDTO>(400, "Invalid address type.");
                }

                // Créer une nouvelle adresse via la factory
                var address = _entityFactory.CreateEntity<Address>();
                address.CustomerId = addressDto.CustomerId;
                address.AddressLine1 = addressDto.AddressLine1;
                address.AddressLine2 = addressDto.AddressLine2;
                address.City = addressDto.City;
                address.State = addressDto.State;
                address.PostalCode = addressDto.PostalCode;
                address.Country = addressDto.Country;
                address.AddressType = addressTypeEnum;

                // Ajouter l'adresse dans la base de données
                await _addressRepository.AddAsync(address);

                // Utilisation de MapConfig pour mapper l'entité en DTO
                var addressResponse = _mapConfig.Map<Address, AddressResponseDTO>(address);

                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(string id)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(id);
                if (address is null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Address not found.");
                }

                // Utilisation de MapConfig pour mapper l'entité en DTO
                var addressResponse = _mapConfig.Map<Address, AddressResponseDTO>(address);

                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AddressResponseDTO>> GetBillingAddressAsync(string customerId)
        {
            try
            {
                var billingAddress = await _addressRepository.GetAddressByTypeAsync(customerId, AddressType.Billing);
                if (billingAddress is null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Billing address not found.");
                }

                // Utilisation de MapConfig pour mapper l'entité en DTO
                var addressResponse = _mapConfig.Map<Address, AddressResponseDTO>(billingAddress);

                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AddressResponseDTO>> GetShippingAddressAsync(string customerId)
        {
            try
            {
                var shippingAddress = await _addressRepository.GetAddressByTypeAsync(customerId, AddressType.Shipping);
                if (shippingAddress is null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Shipping address not found.");
                }

                // Utilisation de MapConfig pour mapper l'entité en DTO
                var addressResponse = _mapConfig.Map<Address, AddressResponseDTO>(shippingAddress);

                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AddressResponseDTO>>> GetAddressesByCustomerAsync(string customerId)
        {
            try
            {
                var addresses = await _addressRepository.GetByCustomerIdAsync(customerId);
                if (addresses is null || !addresses.Any())
                {
                    return new ApiResponse<List<AddressResponseDTO>>(404, "No addresses found for this customer.");
                }

                // Utilisation de MapConfig pour mapper les entités en DTO
                var addressResponses = _mapConfig.Map<List<Address>, List<AddressResponseDTO>>(addresses);

                return new ApiResponse<List<AddressResponseDTO>>(200, addressResponses);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AddressResponseDTO>>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(addressDto.AddressId);
                if (address is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found.");
                }

                Enum.TryParse(addressDto.AddressType, true, out AddressType addressTypeEnum);

                address.AddressLine1 = addressDto.AddressLine1;
                address.AddressLine2 = addressDto.AddressLine2;
                address.City = addressDto.City;
                address.State = addressDto.State;
                address.PostalCode = addressDto.PostalCode;
                address.Country = addressDto.Country;
                address.AddressType = addressTypeEnum;

                await _addressRepository.UpdateAsync(address);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDto.AddressId} updated successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(AddressDeleteDTO addressDeleteDTO)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(addressDeleteDTO.AddressId);
                if (address is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found.");
                }

                await _addressRepository.DeleteAsync(address);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDeleteDTO.AddressId} deleted successfully."
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
