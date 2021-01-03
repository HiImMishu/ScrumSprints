using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class SprintController : ControllerBase
    {
        private readonly SprintService sprintService;
        private readonly ProductService productService;

        public SprintController(SprintService sprintService, ProductService productService)
        {
            this.sprintService = sprintService;
            this.productService = productService;
        }

        [HttpPost("{productId}/sprints")]
        public async Task<IActionResult> PostSprint([FromRoute] int productId, [FromBody] SprintBacklog sprint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (sprint.ProductId != productId)
            {
                return BadRequest();
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var product = await productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            if (product.Owner.Id != userId)
            {
                return Forbid();
            }

            var savedSprint = await sprintService.SaveSprint(sprint);
            return CreatedAtAction("PostSprint", new { prdouctId = productId }, savedSprint);
        }

        [HttpPut("{productId}/sprints/{sprintId}")]
        public async Task<IActionResult> PutSprint([FromRoute] int productId, [FromRoute] int sprintId, [FromBody] SprintBacklog sprint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (sprint.ProductId != productId || sprint.Id != sprintId)
            {
                return BadRequest();
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var product = await productService.GetProductById(productId);
            var sprintToUpdate = await sprintService.GetBacklogById(sprintId);

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            if (sprintToUpdate == null)
            {
                return NotFound(new { message = "Sprint not found." });
            }

            if (product.Owner.Id != userId)
            {
                return Forbid();
            }
            await sprintService.UpdateSprint(sprint);

            return NoContent();
        }

        [HttpDelete("{productId}/sprints/{sprintID}")]
        public async Task<IActionResult> DeleteSprint([FromRoute] int productId, [FromRoute] int sprintId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var product = await productService.GetProductById(productId);
            var sprintToUpdate = await sprintService.GetBacklogById(sprintId);

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            if (sprintToUpdate == null)
            {
                return NotFound(new { message = "Sprint not found." });
            }

            if (product.Owner.Id != userId)
            {
                return Forbid();
            }

            await sprintService.DeleteSprint(sprintId);
            return Ok();
        }
    }
}
