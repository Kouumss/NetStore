using Microsoft.AspNetCore.Mvc;
using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.API.Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        // Injecting the ProductService
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        // Creates a new product.
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            var response = await _productService.CreateProductAsync(productDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Retrieves a product by ID.
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> GetProductById(string id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Updates an existing product.
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateProduct([FromBody] ProductUpdateDTO productDto)
        {
            var response = await _productService.UpdateProductAsync(productDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Deletes a product by ID.
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteProduct(string id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Retrieves all products.
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<ApiResponse<List<ProductResponseDTO>>>> GetAllProducts()
        {
            var response = await _productService.GetAllProductsAsync();
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Retrieves all products by category.
        [HttpGet("GetAllProductsByCategory/{categoryId}")]
        public async Task<ActionResult<ApiResponse<List<ProductResponseDTO>>>> GetAllProductsByCategory(string categoryId)
        {
            var response = await _productService.GetAllProductsByCategoryAsync(categoryId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Update Product Status
        [HttpPut("UpdateProductStatus")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateProductStatus(ProductStatusUpdateDTO productStatusUpdateDTO)
        {
            var response = await _productService.UpdateProductStatusAsync(productStatusUpdateDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
