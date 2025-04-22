using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiVersioning.Data;
using ApiVersioning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication3.Helpers;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("4.0")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/products
        [Authorize]
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            //var products = FileHelper.LoadProducts();
            IQueryable<Product> products = _dbContext.Products;

            if (products.Count() == 0)
                return NoContent(); //204

            return Ok(products);  //200
        }

        // GET: api/products/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var products = FileHelper.LoadProducts();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found." }); // 404

            return Ok(product);  //200
        }

        // POST new product
        [HttpPost]
        public IActionResult Post([FromBody] Product newProduct)
        {
            if (newProduct == null || newProduct.Id <= 0 ||string.IsNullOrWhiteSpace(newProduct.Name) || newProduct.Price <= 0)
                return BadRequest(new { message = "Invalid product data." }); // 400

            var products = FileHelper.LoadProducts();
            newProduct.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            products.Add(newProduct);
            FileHelper.SaveProducts(products);

            return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct); // 201
        }

        // PUT: api/products/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
                return BadRequest("Product data is required.");  //400

            if (string.IsNullOrWhiteSpace(updatedProduct.Name))
                return BadRequest("Product name is required.");  //400

            if (updatedProduct.Price < 0)
                return BadRequest("Product price must be non-negative.");  //400

            var products = FileHelper.LoadProducts();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found." }); // 404

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            FileHelper.SaveProducts(products);
            return NoContent(); //204
        }

        // DELETE: api/products/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var products = FileHelper.LoadProducts();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found." }); // 404

            products.Remove(product);
            FileHelper.SaveProducts(products);
            return NoContent();  //204
        }
    }
}