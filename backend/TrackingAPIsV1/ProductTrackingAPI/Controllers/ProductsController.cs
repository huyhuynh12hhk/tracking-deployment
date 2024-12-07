using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductTrackingAPI.DTOs;
using ProductTrackingAPI.Models.Products;
using ProductTrackingAPI.Services;

namespace ProductTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService productService;
        private readonly IMapper mapper;

        public ProductsController(ProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string? query=null)
        {
            //  Console.WriteLine($">>> Search term is: {query??"not set"}");
            var result = await productService
                .GetProducts(
                    query==null?null: 
                    (
                        e => e.Name.ToLower().Contains(query.ToLower()) 
                        //|| e.Id.Contains(query)
                        || e.Supplier.FullName.ToLower().Contains(query.ToLower())
                        //|| e.Supplier.Id.Contains(query)
                    )
                );
            return Ok(BaseResponse<List<ProductView>>.Success(mapper.Map<List<ProductView>>(result)));
        }

        [HttpGet("suppliers/{supplierId}")]
        public async Task<IActionResult> GetProductsBySupplier([FromRoute]string supplierId)
        {
            var result = await productService.GetProducts(e=>e.SupplierId == supplierId);
            return Ok(BaseResponse<List<ProductView>>.Success(mapper.Map<List<ProductView>>(result)));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute]string id)
        {
            var result = await productService.GetProduct(e=>e.Id == id);

            if(result == null)
            {
                return NotFound();
            }


            return Ok(BaseResponse<ProductDetailView>.Success(result));
        }

        [HttpPost()]
        public async Task<IActionResult> AddProduct([FromBody] AddProductModel request)
        {
            var result = await productService.AddProduct(request);

            if (result == null)
            {
                return NotFound();
            }


            return Ok(BaseResponse<ProductDetail>.Success(result));
        }

        [HttpPut("info/{id}")]
        public async Task<IActionResult> UpdateProductInfo([FromRoute] string id, [FromBody]EditProductInfoModel request)
        {
            var result = await productService.UpdateProductInfo(id, request);

            if (result == null)
            {
                return NotFound();
            }


            return NoContent();
        }

        [HttpPut("origin/{id}")]
        public async Task<IActionResult> UpdateProductOrigins([FromRoute] string id, [FromBody] Dictionary<string, string> request)
        {
            var result = await productService.UpdateProductOrigins(id, request);

            if (result == false)
            {
                return BadRequest();
            }


            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            var result = await productService.DeleteProduct(id);

            if (result == null)
            {
                return NotFound();
            }


            return NoContent();
        }
    }
}
