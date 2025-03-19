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
using DataAccess.DAO;

namespace WebAPI.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ODataController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/Categories
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IActionResult GetAll()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("You are not logged in");
            }


            if (HttpContext.Request.Cookies.TryGetValue("AuthToken", out string token))
            {
                Console.WriteLine($"Token from Cookie: {token}");
            }
            else
            {
                Console.WriteLine("No token found in cookies.");
            }

            return Ok(_categoryRepository.GetAll());

        }

        [Authorize(Roles = "Admin")]
        [EnableQuery]
        [HttpGet("get-by-id")] // GET /odata/Category(1)
        public IActionResult GetById([FromODataUri] int key)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("You not loggin now");
            }
            var author = _categoryRepository.GetById(key);
            if (author == null) return NotFound();
            return Ok(author);
        }

        // POST: odata/Category
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] CategoryDTO category)
        {
            _categoryRepository.Add(category);
            return Created($"odata/Category({category.CategoryID})", category);
        }

        // PUT: odata/Category(1)
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public IActionResult Update(int key, [FromBody] CategoryDTO category)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("You not loggin now");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != category.CategoryID) return BadRequest();

            _categoryRepository.Update(category);
            return Ok("Update Success");
        }

        // DELETE: odata/Category(1)
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete")]
        public IActionResult Remove(int key)
        {

            var author = _categoryRepository.GetById(key);
            if (author == null) return NotFound();

            _categoryRepository.Delete(key);
            return Ok("Delete Success");
        }
    }
}
