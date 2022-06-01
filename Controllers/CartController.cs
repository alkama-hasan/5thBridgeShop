
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using _5thBridgeShop.Models;
namespace _5thBridgeShop.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartProducts()
        {
            if (_context.Carts == null)
            {
                return BadRequest();
            }
           var CartProducts = (from p in _context.Products  join c in _context.Carts on p.Id equals c.ProductId
                          select new {
                              ProductId = p.Id, 
                              Rating = p.Rating,
                              ProductName = p.ProductName,
                              Desccription = p.description
                          }).ToList();
            return Ok(CartProducts);
           
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> PostCartProduct(Cart cartProduct)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'ProductContext.Carts'  is null.");
            }
            _context.Carts.Add(cartProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartProducts), new { id = cartProduct.Id }, cartProduct);
        }

    }
}
