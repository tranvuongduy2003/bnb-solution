using AutoMapper;
using BnB.Api.Dto;
using BnB.Api.Dto.Category;
using BnB.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BnB.Api.Controllers
{
    [Route("categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public CategoryController(ICategoryRepository categoryRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetCategories();
                var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                _response.Data = categoryDtos;
                _response.Message = "Get categories successfully!";
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
