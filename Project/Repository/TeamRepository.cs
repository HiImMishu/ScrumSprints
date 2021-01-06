using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ProjectContext _context;

        public TeamRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> GetLeadedTeamsByUserId(int userId)
        {
            return await _context.Team.Where(t => t.LeaderId == userId).ToListAsync();
        }

        public async Task<List<Team>> GetParticipatedTeamsByUserId(int userId)
        {
            return await _context.Team.Where(t => t.UserTeams.Any(ut => ut.UserId == userId)).ToListAsync();
        }

        public async Task<Team> GetTeamById(int teamId)
        {
            return await _context.Team.FindAsync(teamId);
        }

        public async Task<int> UpdateTeam(Team team)
        {
            _context.Entry(team).State = EntityState.Modified;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveTeam(Team team)
        {
            _context.Team.Add(team);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteTeam(Team team)
        {
            _context.Team.Remove(team);
            return await _context.SaveChangesAsync();
        }

        public async Task<Team> JoinTeam(int userId, string teamCode)
        {
            var team = _context.Team.Where(t => t.TeamCode == teamCode).Include(t => t.UserTeams).FirstOrDefault();

            if (team == null)
            {
                return null;
            }

            if (team.UserTeams != null && team.UserTeams.Any(ut => ut.UserId == userId))
            {
                return team;
            }
            
            var userTeam = new UserTeam() { TeamId = team.Id, UserId = userId };

            if (team.UserTeams == null)
            {
                team.UserTeams = new List<UserTeam>() { userTeam };
            }
            else
            {
                team.UserTeams.Add(userTeam);
            }

            await _context.SaveChangesAsync();

            return team;
        }

        public async Task<int> RemoveTeamMember(int memberId, int teamId)
        {
            var member = _context.Team.Include(t => t.UserTeams).ToList().Find(t => t.Id == teamId).UserTeams.Where(ut => ut.UserId == memberId).First();
            _context.Team.Find(teamId).UserTeams.Remove(member);

            return await _context.SaveChangesAsync();
        }
    }

    public interface ITeamRepository
    {
        Task<List<Team>> GetLeadedTeamsByUserId(int userId);
        Task<List<Team>> GetParticipatedTeamsByUserId(int userId);
        Task<Team> GetTeamById(int teamId);
        Task<int> UpdateTeam(Team team);
        Task<int> SaveTeam(Team team);
        Task<int> DeleteTeam(Team team);
        Task<Team> JoinTeam(int userId, string teamCode);
        Task<int> RemoveTeamMember(int memberId, int teamId);
    }
}
