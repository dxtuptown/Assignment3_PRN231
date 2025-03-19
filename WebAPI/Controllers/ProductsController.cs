using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using DataAccess.DTO;
using Microsoft.AspNetCore.OData.Formatter;

namespace WebAPI.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ODataController
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: odata/Product
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IActionResult GetAll()
        {
            return Ok(_productRepository.GetAll());
        }

        [EnableQuery]
        [Authorize(Roles = "Admin")]
        [HttpGet("get-by-id")] // GET /odata/Product(1)
        public IActionResult GetById([FromODataUri] int key)
        {
            var author = _productRepository.GetById(key);
            if (author == null) return NotFound();
            return Ok(author);
        }

        // POST: odata/Product
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] ProductDTO product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            product.ProductID = 0;
            _productRepository.Add(product);
            // Use the standard Created method
            return Created($"odata/Authors({product.ProductID})", product);
        }

        // PUT: odata/Product(1)
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public IActionResult Update(int key, [FromBody] ProductDTO product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            _productRepository.Update(product);
            return Ok("Update Success");
        }

        // DELETE: odata/Product(1)
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete")]
        public IActionResult Remove(int key)
        {
            var author = _productRepository.GetById(key);
            if (author == null) return NotFound();

            _productRepository.Delete(key);
            return Ok("Delete Success");
        }
    }
}
