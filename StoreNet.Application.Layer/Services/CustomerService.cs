// Couche Application
using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ApiResponse<List<CustomerResponseDTO>>> GetAllCustomersAsync()
    {
        try
        {
            var customers = await _customerRepository.GetAllCustomersAsync();

            // Map to response DTOs if necessary
            var customerResponseDtos = customers.Select(c => new CustomerResponseDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                DateOfBirth = c.DateOfBirth
            }).ToList();

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
            // Vérification si l'email existe déjà via GetEmailAsync
            var existingCustomer = await _customerRepository.GetByEmailAsync(customerDto.Email.ToLower());

            if (existingCustomer is not null)
            {
                return new ApiResponse<CustomerResponseDTO>(400, "Email is already in use.");
            }

            // Création de l'entité Customer à partir du DTO
            var customer = new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                PhoneNumber = customerDto.PhoneNumber,
                DateOfBirth = customerDto.DateOfBirth,
                IsActive = true,
                Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password)
            };

            // Sauvegarde du client dans la base de données via le repository
            await _customerRepository.AddAsync(customer);

            // Préparation de la réponse
            var customerResponse = new CustomerResponseDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth = customer.DateOfBirth
            };

            return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
        }
        catch (Exception ex)
        {
            // Gestion des erreurs
            return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto)
    {
        try
        {
            // Recherche du client par email
            var customer = await _customerRepository.GetByEmailAsync(loginDto.Email);
            if (customer is null)
            {
                return new ApiResponse<LoginResponseDTO>(401, "Invalid email or password.");
            }

            // Vérification du mot de passe avec BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.Password);
            if (!isPasswordValid)
            {
                return new ApiResponse<LoginResponseDTO>(401, "Invalid email or password.");
            }

            // Préparation de la réponse
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
            // Gestion des erreurs
            return new ApiResponse<LoginResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CustomerResponseDTO>> GetCustomerByIdAsync(Guid id)
    {
        try
        {
            // Récupération du client par ID
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null || !customer.IsActive)
            {
                return new ApiResponse<CustomerResponseDTO>(404, "Customer not found.");
            }

            // Préparation de la réponse
            var customerResponse = new CustomerResponseDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth = customer.DateOfBirth
            };

            return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
        }
        catch (Exception ex)
        {
            // Gestion des erreurs
            return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCustomerAsync(CustomerUpdateDTO customerDto)
    {
        try
        {
            // Recherche du client par ID
            var customer = await _customerRepository.GetByIdAsync(customerDto.CustomerId);
            if (customer is null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
            }

            // Vérification si l'email est déjà utilisé par un autre client
            if (customer.Email != customerDto.Email && await _customerRepository.AnyAsync(c => c.Email == customerDto.Email))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400, "Email is already in use.");
            }

            // Mise à jour des informations du client
            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.PhoneNumber = customerDto.PhoneNumber;
            customer.DateOfBirth = customerDto.DateOfBirth;

            // Sauvegarde des modifications
            await _customerRepository.UpdateAsync(customer);

            // Préparation de la réponse
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Customer with Id {customerDto.CustomerId} updated successfully."
            };

            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            // Gestion des erreurs
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCustomerAsync(Guid id)
    {
        try
        {
            // Recherche du client par ID
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
            }

            // Soft delete (désactivation du client)
            customer.IsActive = false;
            await _customerRepository.UpdateAsync(customer);

            // Préparation de la réponse
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Customer with Id {id} deleted successfully."
            };

            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            // Gestion des erreurs
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> ChangePasswordAsync(ChangePasswordDTO changePasswordDto)
    {
        try
        {
            // Recherche du client par ID
            var customer = await _customerRepository.GetByIdAsync(changePasswordDto.CustomerId);
            if (customer is null || !customer.IsActive)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found or inactive.");
            }

            // Vérification du mot de passe actuel
            bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, customer.Password);
            if (!isCurrentPasswordValid)
            {
                return new ApiResponse<ConfirmationResponseDTO>(401, "Current password is incorrect.");
            }

            // Hashage du nouveau mot de passe
            customer.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            await _customerRepository.UpdateAsync(customer);

            // Préparation de la réponse
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = "Password changed successfully."
            };

            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            // Gestion des erreurs
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
}
