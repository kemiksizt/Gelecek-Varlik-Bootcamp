using Microsoft.AspNetCore.Mvc;
using Week3.API.Infrastructure;
using Week3.Model;
using Week3.Model.Product;
using Week3.Service.Product;

namespace Week3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }

        [HttpPost("Delete")]
        [ServiceFilter(typeof(LoginFilter))]
        public General<ProductViewModel> DeleteUser(int id, [FromBody] ProductViewModel product)
        {
            return productService.DeleteProduct(id, product);
        }

        [HttpGet("{Id}")]
        public General<ProductViewModel> GetProductListById(int id, ProductViewModel product)
        {
            return productService.GetProductListById(id, product);
        }


        [HttpPost("Insert")]
        public General<ProductViewModel> InsertProduct([FromBody] ProductViewModel newProduct)
        {
            return productService.InsertProduct(newProduct);
        }

        [HttpGet]
        public General<ProductViewModel> GetProducts()
        {
            return productService.GetProducts();
        }

        [HttpPut("{id}")]
        public General<ProductViewModel> UpdateProduct(int id, [FromBody] ProductViewModel product)
        {
            return productService.UpdateProduct(id, product);
        }
        /*
        [HttpDelete("{id}")]
        public General<ProductViewModel> DeleteProduct(int id)
        {
            return productService.DeleteProduct(id);
        }
        */


        [HttpGet]
        [Route("Sort")]
        public General<ListProductViewModel> SortProduct([FromQuery] string param)
        {
            return productService.SortProduct(param);
        }

        [HttpGet]
        [Route("Filter")]
        public General<ListProductViewModel> FilterProducts([FromQuery] string param)
        {
            return productService.FilterProduct(param);
        }

        [HttpGet]
        [Route("Paginate")]
        public General<ListProductViewModel> PaginateProduct([FromQuery] int productByPage, [FromQuery]  int pageNo)
        {
            return productService.PaginateProduct(productByPage, pageNo);
        }
    }
    
}
