using AutoMapper;
using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using StoreNet.Application.Layer.Factories;

namespace StoreNet.Application.Layer.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEntityFactory _entityFactory;
        private readonly IMapper _mapper; 

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IEntityFactory entityFactory, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _entityFactory = entityFactory;
            _mapper = mapper;
        }

        // Crée un produit
        public async Task<ApiResponse<ProductResponseDTO>> CreateProductAsync(ProductCreateDTO productDto)
        {
            try
            {
                // Vérifier si le nom du produit existe déjà
                var existingProduct = await _productRepository.GetByNameAsync(productDto.Name);
                if (existingProduct is not null)
                {
                    return new ApiResponse<ProductResponseDTO>(400, "Product name already exists.");
                }

                // Vérifier si la catégorie existe
                var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId);
                if (category is null)
                {
                    return new ApiResponse<ProductResponseDTO>(400, "Specified category does not exist.");
                }

                // Utilisation de la factory pour créer un produit
                var product = _entityFactory.CreateEntity<Product>();

                // Mapper ProductCreateDTO vers Product
                _mapper.Map(productDto, product);

                // Ajouter le produit au repository
                await _productRepository.AddAsync(product);

                // Mapper le modèle Product vers ProductResponseDTO
                var productResponse = _mapper.Map<ProductResponseDTO>(product);

                return new ApiResponse<ProductResponseDTO>(200, productResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // Récupère un produit par son ID
        public async Task<ApiResponse<ProductResponseDTO>> GetProductByIdAsync(string id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product is null)
                {
                    return new ApiResponse<ProductResponseDTO>(404, "Product not found.");
                }

                // Mapper le modèle Product vers ProductResponseDTO
                var productResponse = _mapper.Map<ProductResponseDTO>(product);

                return new ApiResponse<ProductResponseDTO>(200, productResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // Met à jour un produit
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductAsync(ProductUpdateDTO productDto)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productDto.Id);
                if (product is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found.");
                }

                // Vérifier si la catégorie existe
                if (await _categoryRepository.GetByIdAsync(productDto.CategoryId) is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Specified category does not exist.");
                }

                // Mapper ProductUpdateDTO vers Product
                _mapper.Map(productDto, product);

                await _productRepository.UpdateAsync(product);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Product with Id {productDto.Id} updated successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // Supprime (soft delete) un produit
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteProductAsync(string id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found.");
                }

                // Soft delete (marquer comme non disponible)
                product.IsAvailable = false;
                await _productRepository.UpdateAsync(product);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Product with Id {id} deleted successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // Récupère tous les produits
        public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();

                // Mapper la liste de produits vers une liste de ProductResponseDTO
                var productList = _mapper.Map<List<ProductResponseDTO>>(products);

                return new ApiResponse<List<ProductResponseDTO>>(200, productList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductResponseDTO>>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // Récupère les produits d'une catégorie spécifique
        public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsByCategoryAsync(string categoryId)
        {
            try
            {
                var products = await _productRepository.GetAllByCategoryAsync(categoryId);

                if (products is null || products.Count == 0)
                {
                    return new ApiResponse<List<ProductResponseDTO>>(404, "No products found in the specified category.");
                }

                // Mapper la liste de produits vers une liste de ProductResponseDTO
                var productList = _mapper.Map<List<ProductResponseDTO>>(products);

                return new ApiResponse<List<ProductResponseDTO>>(200, productList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductResponseDTO>>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // Met à jour le statut de disponibilité du produit
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductStatusAsync(ProductStatusUpdateDTO productStatusUpdateDTO)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productStatusUpdateDTO.ProductId);
                if (product is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found.");
                }

                // Mapper ProductStatusUpdateDTO vers Product
                _mapper.Map(productStatusUpdateDTO, product);

                await _productRepository.UpdateAsync(product);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Product with Id {productStatusUpdateDTO.ProductId} status updated successfully."
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
