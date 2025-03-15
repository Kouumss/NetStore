using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreNet.Application.Services;
using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.API.Layer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("GetAllCustomers")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerResponseDTO>>>> GetAllCustomers()
    {
        var response = await _customerService.GetAllCustomersAsync();

        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    // Registers a new customer.
    [HttpPost("RegisterCustomer")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> RegisterCustomer([FromBody] CustomerRegistrationDTO customerDto)
    {
        var response = await _customerService.RegisterCustomerAsync(customerDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }
    // Logs in a customer.
    [HttpPost("Login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody] LoginDTO loginDto)
    {
        var response = await _customerService.LoginAsync(loginDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }
    // Retrieves customer details by ID.
    [HttpGet("GetCustomerById/{id}")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> GetCustomerById(string id)
    {
        var response = await _customerService.GetCustomerByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }
    // Updates customer details.
    [HttpPut("UpdateCustomer")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateCustomer([FromBody] CustomerUpdateDTO customerDto)
    {
        var response = await _customerService.UpdateCustomerAsync(customerDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }
    // Deletes a customer by ID.
    [HttpDelete("DeleteCustomer/{id}")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCustomer(string id)
    {
        var response = await _customerService.DeleteCustomerAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }
    // Changes the password for an existing customer.
    [HttpPost("ChangePassword")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
    {
        var response = await _customerService.ChangePasswordAsync(changePasswordDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }
}
