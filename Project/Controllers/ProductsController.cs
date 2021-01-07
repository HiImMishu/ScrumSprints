using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.DTO;
using Project.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService productService;
        private readonly TeamService teamService;

        public ProductsController(ProductService productService, TeamService teamService)
        {
            this.productService = productService;
            this.teamService = teamService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await productService.GetProductById(id);
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (product == null)
            {
                return NotFound();
            }

            TeamDTO team = null;
            if (product.DevTeam != null)
            {
                team = await teamService.GetTeamById(product.DevTeam.Id);
                if (team.Members.Any(m => m.Id == userId) || team.TeamLeader.Id == userId)
                {
                    return Ok(product);
                }
            }

            if (product.Owner.Id != userId)
            {
                return Forbid();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (product.OwnerId != userId)
            {
                return Forbid();
            }

            var savedProduct = await productService.SaveProduct(product);
            return CreatedAtAction("GetProduct", new { id = savedProduct.Id }, savedProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam([FromRoute] int id, [FromBody] ProductUpdateDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var preUpdatedProduct = await productService.GetProductById(product.Id);

            if (preUpdatedProduct == null)
            {
                return NotFound();
            }

            if (preUpdatedProduct.Owner.Id != userId)
            {
                return Forbid();
            }

            try
            {
                await productService.UpdateProduct(product);
                return NoContent();
            } 
            catch(ArgumentException e)
            {
                return NotFound(new { error = "Team not found." });
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var productToDelete = await productService.GetProductById(id);

            if (productToDelete == null)
            {
                return NotFound();
            }

            if (productToDelete.Owner.Id != userId)
            {
                return Forbid();
            }

            await productService.DeleteProduct(id);
            return Ok();
        }
    }
}
