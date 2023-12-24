using AutoMapper;
using BnB.Api.Dto;
using BnB.Api.Dto.Brand;
using BnB.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BnB.Api.Controllers
{
    [Route("brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public BrandController(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            try
            {
                var brands = await _brandRepository.GetBrands();
                var brandDtos = _mapper.Map<IEnumerable<BrandDto>>(brands);

                _response.Data = brandDtos;
                _response.Message = "Get brands successfully!";
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