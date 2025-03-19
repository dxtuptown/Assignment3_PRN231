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
    public class OrdersController : ODataController
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: odata/Category
        [HttpGet("GetAll")]
        [EnableQuery]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetAll()
        {
            return Ok(_orderRepository.GetAll());
        }

        [EnableQuery]
        [HttpGet("get-by-id")] // GET /odata/Category(1)
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetById([FromODataUri] int key)
        {
            var author = _orderRepository.GetById(key);
            if (author == null) return NotFound();
            return Ok(author);
        }

        // POST: odata/Category
        [HttpPost("Create")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Create([FromBody] OrderDTO order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _orderRepository.Add(order);
            // Use the standard Created method
            return Created($"odata/Order({order.OrderID})", order);
        }

        // PUT: odata/Category(1)
        [HttpPut("Update")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Update(int key, [FromBody] OrderDTO order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != order.OrderID) return BadRequest();

            _orderRepository.Update(order);
            return Ok("Update Success");
        }

        // DELETE: odata/Category(1)
        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Remove(int key)
        {
            var author = _orderRepository.GetById(key);
            if (author == null) return NotFound();

            _orderRepository.Delete(key);
            return Ok("Delete Success");
        }

    }
}
