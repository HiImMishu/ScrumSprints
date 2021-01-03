using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Service;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("/api/products")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService itemService;
        private readonly ProductService productService;
        private readonly TeamService teamService;

        public ItemsController(ItemService itemService, ProductService productService, TeamService teamService)
        {
            this.itemService = itemService;
            this.productService = productService;
            this.teamService = teamService;
        }

        [HttpPost("{productId}/items")]
        public async Task<IActionResult> PostItem([FromRoute] int productId, [FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (productId != item.ProductId)
            {
                return BadRequest();
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var product = await productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            if (product.Owner.Id != userId)
            {
                return Forbid();
            }

            var savedItem = await itemService.SaveItem(item);
            return CreatedAtAction("PostItem", new { prductId = productId }, savedItem);
        }

        [HttpPut("{productId}/items/{itemId}")]
        public async Task<IActionResult> PutItem([FromRoute] int productId, [FromRoute] int itemId, [FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (productId != item.ProductId || itemId != item.ItemId)
            {
                return BadRequest();
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var product = await productService.GetProductById(productId);
            var itemToUpdate = await itemService.GetItemById(itemId);

            if (product == null || itemToUpdate == null)
            {
                return NotFound();
            }

            var team = await teamService.GetTeamById(product.DevTeam.Id);

            if (product.Owner.Id != userId && team.Members.All(m => m.Id != userId))
            {
                return Forbid();
            }

            if (product.Owner.Id != userId && item.ModifiedBy != userId)
            {
                return Forbid();
            }

            if (await itemService.UpdateItem(item) == null)
            {
                return NotFound(new { message = "Sprint not found." });
            }

            return NoContent();
        }

        [HttpDelete("{productID}/items/{itemId}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int productId, [FromRoute] int itemId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await productService.GetProductById(productId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            if (await itemService.GetItemById(itemId) == null)
            {
                return NotFound(new { message = "Item not found." });
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (product.Owner.Id != userId)
            {
                return Forbid();
            }

            await itemService.DeleteItem(itemId);
            return Ok();
        }
    }
}
