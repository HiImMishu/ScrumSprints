using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Models.DTO;
using Project.Service;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly TeamService teamService;

        public TeamsController(TeamService teamService)
        {
            this.teamService = teamService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = await teamService.GetTeamById(id);
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (team != null && (team.TeamLeader.Id != userId && !team.Members.Any(m => m.Id == userId)))
            {
                return Forbid();
            }

            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam([FromRoute] int id, [FromBody] TeamUpdateDTO team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.Id)
            {
                return BadRequest();
            }

            var preUpdatedTeam = await teamService.GetTeamById(id);
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (preUpdatedTeam.TeamLeader.Id != userId)
            {
                return Forbid();
            }

            if (!preUpdatedTeam.Members.Any(m => m.Id == team.TeamLeader))
            {
                return BadRequest(new { message = "New Team Leader must be a team member." });
            }

            var updatedTeam = await teamService.UpdateTeam(team);

            if (updatedTeam == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostTeam([FromBody] TeamUpdateDTO team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            team.TeamLeader = userId;

            var savedTeam = await teamService.SaveTeam(team);

            return CreatedAtAction("GetTeam", new { id = savedTeam.Id }, savedTeam);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var teamToDelete = await teamService.GetTeamById(id);

            if (teamToDelete == null)
            {
                return NotFound();
            }

            if (userId != teamToDelete.TeamLeader.Id)
            {
                return Forbid();
            }

            await teamService.DeleteTeam(id);

            return Ok();
        }

        [HttpPost("members")]
        public async Task<IActionResult> JoinTeam([FromBody] TeamSignupDTO signupDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (userId != signupDTO.UserId)
            {
                return Forbid();
            }

            var team = await teamService.JoinTeam(signupDTO);

            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        [HttpDelete("members")]
        public async Task<IActionResult> LeaveTeam([FromBody] TeamSignoutDTO signoutDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var team = await teamService.GetTeamById(signoutDTO.TeamId);

            if (team == null || !team.Members.Any(m => m.Id == signoutDTO.UserId))
            {
                return NotFound();
            }

            if (team.TeamLeader.Id != userId && signoutDTO.UserId != userId)
            {
                return Forbid();
            }

            await teamService.RemoveTeamMember(signoutDTO);
            return Ok();
        }

    }
}