using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SL_Api_Ecommerce.Models;
using SL_Api_Ecommerce.Models.Dtos;
using SL_Api_Ecommerce.Repository;
using SL_Api_Ecommerce.Repository.IRepository;

namespace SL_Api_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            var productsDto = _mapper.Map<List<ProductDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{productId:int}", Name ="GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProduct(int productId)
        {
            var product = _productRepository.GetProduct(productId);
            if (product == null)
            {
                return NotFound($"El produto con el id {productId} no existe");
            }
            var productDto = _mapper.Map<List<ProductDto>>(product);

            return Ok(productDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProduct([FromBody] CreateProductDto request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }

            if (_productRepository.ProductExists(request.Name))
            {
                ModelState.AddModelError("CustomError", "El producto ya existe.");
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryExists(request.CategoryId))
            {
                ModelState.AddModelError("CustomError", $"La categoria con el {request.CategoryId} no existe.");
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(request);

            if(!_productRepository.CreateProduct(product))
            {
                ModelState.AddModelError("CustomError", $"Algo salió mal al guardar el registro {product.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetProduct", new { productId = product.ProductId }, product);
        }
    }
}