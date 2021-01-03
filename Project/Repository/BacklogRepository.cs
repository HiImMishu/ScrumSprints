using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Repository
{
    public class BacklogRepository : IBacklogRepository
    {
        private readonly ProjectContext _context;

        public BacklogRepository(ProjectContext context)
        {
            _context = context;
        }

        public  SprintBacklog GetBacklogById(int backlogId)
        {
            return _context.Backlog.Find(backlogId);
        }

        public async Task<List<SprintBacklog>> GetProductBacklogs(int productId)
        {
            return await _context.Backlog.Where(b => b.ProductId == productId).ToListAsync();
        }

        public async Task<int> SaveBacklog(SprintBacklog sprintBacklog)
        {
            _context.Backlog.Add(sprintBacklog);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateBacklog(SprintBacklog sprintBacklog)
        {
            _context.Entry(sprintBacklog).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteBacklog(SprintBacklog sprintBacklog)
        {
            _context.Backlog.Remove(sprintBacklog);
            return await _context.SaveChangesAsync();
        }
    }

    public interface IBacklogRepository
    {
        SprintBacklog GetBacklogById(int backlogId);
        Task<List<SprintBacklog>> GetProductBacklogs(int productId);
        Task<int> SaveBacklog(SprintBacklog sprintBacklog);
        Task<int> UpdateBacklog(SprintBacklog sprintBacklog);
        Task<int> DeleteBacklog(SprintBacklog sprintBacklog);
    }
}
