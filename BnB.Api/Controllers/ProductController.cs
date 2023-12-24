using AutoMapper;
using BnB.Api.Dto;
using BnB.Api.Dto.Product;
using BnB.Api.Dto.Review;
using BnB.Api.Enums;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BnB.Api.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public ProductController(IImageRepository imageRepository, IReviewRepository reviewRepository, IProductRepository productRepository,IMapper mapper)
        {
            _imageRepository = imageRepository;
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
                foreach (var productDto in productDtos)
                {
                    var images = await _imageRepository.GetImagesByProductId(productDto.Id);
                    productDto.Images = images;
                    var reviews = await _reviewRepository.GetReviewsByProductId(productDto.Id);
                    var totalRating = reviews.Sum(r => r.Rating);
                    productDto.Rating = totalRating / reviews.Count();
                }

                _response.Data = productDtos;
                _response.Message = "Get products successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);

                if (product is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Product not found!";
                    return NotFound(_response);
                }
                
                var productDto = _mapper.Map<ProductDto>(product);
                var images = await _imageRepository.GetImagesByProductId(productDto.Id);
                productDto.Images = images;

                _response.Data = productDto;
                _response.Message = "Get the product successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }
        
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            try
            {
                var newProduct = _mapper.Map<Product>(createProductDto);
                var createdProduct = await _productRepository.CreateProduct(newProduct);

                if (createProductDto.Images != null)
                {
                    await _imageRepository.CreateImagesByProductId(createdProduct.Id, createProductDto.Images);
                }
                
                if (createdProduct is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Failed";
                    return BadRequest(_response);
                }
                
                _response.Message = "Create new product successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] CreateProductDto updateProductDto)
        {
            try
            {
                var product = _mapper.Map<Product>(updateProductDto);
                var updatedProduct = await _productRepository.UpdateProduct(id, product);
                
                if (updatedProduct is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Product not found";
                    return NotFound(_response);
                }
                
                _response.Message = "Update the product successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var isProductDeleted = await _productRepository.DeleteProductById(id);
                if (!isProductDeleted)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Product not found";
                    return NotFound(_response);
                }

                _response.Message = "Delete the product successfully!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

            return Ok(_response);
        }
    }
}
