
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Models.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectContext _context;

        public UserRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.User.FindAsync(userId);
        }

        public User GetUserById(int userId)
        {
            return _context.User.Find(userId);
        }

        public async Task<int> SaveUser(User user)
        {
            _context.User.Add(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateUser(SimpleUserDTO userDTO)
        {
            var user = _context.User.Find(userDTO.Id);

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.id == id);
        }

        public async Task<int> ArchiveUser(int id)
        {
            var user = _context.User.Find(id);
            user.ArchivedAt = DateTime.Now;
            return await _context.SaveChangesAsync();     
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.User.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersByTeamId(int teamId)
        {
            return await _context.User.Where(u => u.UserTeams.Any(ut => ut.TeamId == teamId)).ToListAsync();
        }
    }

    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        User GetUserById(int userID);
        Task<int> SaveUser(User user);
        Task<bool> UpdateUser(SimpleUserDTO user);
        Task<int> ArchiveUser(int userId);
        Task<User> GetUserByEmail(string email);
        Task<List<User>> GetUsersByTeamId(int teamId);
    }
}
