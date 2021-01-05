using Project.Models;
using Project.Models.DTO;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service
{
    public class TeamService
    {
        private readonly ITeamRepository teamRepository;
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;

        public TeamService(TeamRepository teamRepository, UserRepository userRepository, ProductRepository productRepository)
        {
            this.teamRepository = teamRepository;
            this.userRepository = userRepository;
            this.productRepository = productRepository;
        }

        public async Task<TeamDTO> GetTeamById(int teamId)
        {
            var team = await teamRepository.GetTeamById(teamId);

            if (team == null)
                return null;

            var teamLeader = await userRepository.GetUserByIdAsync(team.LeaderId);
            var teamMembers = await userRepository.GetUsersByTeamId(teamId);
            var product = await productRepository.GetProductByTeamId(teamId);
            SimpleProductDTO productDTO= null;

            if (product != null)
            {
                productDTO = new SimpleProductDTO() { Id = product.Id, Name = product.Name };
            }

            return new TeamDTO()
            {
                Id = team.Id,
                Name = team.Name,
                CreatedAt = team.CreatedAt,
                TeamCode = team.TeamCode,
                TeamLeader = new SimpleUserDTO() { Id = teamLeader.id, FirstName = teamLeader.FirstName, LastName = teamLeader.LastName, Email = teamLeader.Email },
                Members = teamMembers.Select(m => new SimpleUserDTO() { Id = m.id, FirstName = m.FirstName, LastName = m.LastName, Email = m.Email }).ToList(),
                Product = productDTO
            };
        }

        public async Task<TeamDTO> UpdateTeam(TeamUpdateDTO team)
        {
            var preUpdateTeam = await teamRepository.GetTeamById(team.Id);
            if (preUpdateTeam == null)
            {
                return null;
            }

            preUpdateTeam.Name = team.Name;
            preUpdateTeam.LeaderId = team.TeamLeader;

            await teamRepository.UpdateTeam(preUpdateTeam);

            return await GetTeamById(team.Id);
        }

        public async Task<TeamDTO> SaveTeam(TeamUpdateDTO team)
        {
            var teamToSave = new Team()
            {
                Name = team.Name,
                LeaderId = team.TeamLeader,
                CreatedAt = DateTime.Now
            };

            await teamRepository.SaveTeam(teamToSave);

            return await GetTeamById(teamToSave.Id);
        }

        public async Task<int> DeleteTeam(int teamId)
        {
            var team = await teamRepository.GetTeamById(teamId);
            return await teamRepository.DeleteTeam(team);
        }

        public async Task<TeamDTO> JoinTeam(TeamSignupDTO signupDTO)
        {
            var team = await teamRepository.JoinTeam(signupDTO.UserId, signupDTO.TeamCode);

            if (team == null)
            {
                return null;
            }

            return await GetTeamById(team.Id);
        }

        public async Task<int> RemoveTeamMember(TeamSignoutDTO signoutDTO)
        {
            return await teamRepository.RemoveTeamMember(signoutDTO.UserId, signoutDTO.TeamId);
        }
    }
}
