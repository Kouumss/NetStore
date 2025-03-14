﻿using StoreNet.Domain.Layer.DTOs;

namespace StoreNet.Domain.Layer.Interfaces;

public interface ICustomerService
{
    Task<ApiResponse<ConfirmationResponseDTO>> ChangePasswordAsync(ChangePasswordDTO changePasswordDto);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteCustomerAsync(string id);
    Task<ApiResponse<List<CustomerResponseDTO>>> GetAllCustomersAsync();
    Task<ApiResponse<CustomerResponseDTO>> GetCustomerByIdAsync(string id);
    Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto);
    Task<ApiResponse<CustomerResponseDTO>> RegisterCustomerAsync(CustomerRegistrationDTO customerDto);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateCustomerAsync(CustomerUpdateDTO customerDto);
}
