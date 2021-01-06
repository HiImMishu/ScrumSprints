using Project.Models;
using Project.Models.DTO;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service
{
    public class SprintService
    {
        private readonly IBacklogRepository backlogRepository;

        public SprintService(BacklogRepository backlogRepository)
        {
            this.backlogRepository = backlogRepository;
        }

        public async Task<BacklogDTO> GetBacklogById(int backlogId)
        {
            var backlog = backlogRepository.GetBacklogById(backlogId);

            if (backlog == null)
            {
                return null;
            }

            return new BacklogDTO()
            {
                Id = backlog.Id,
                Description = backlog.Description,
                StartTime = backlog.StartTime,
                EndTime = backlog.EndTime
            };
        }

        public async Task<BacklogDTO> SaveSprint(SprintBacklog sprintBacklog)
        {
            sprintBacklog.Id = 0;
            await backlogRepository.SaveBacklog(sprintBacklog);
            return await GetBacklogById(sprintBacklog.Id);
        }

        public async Task<BacklogDTO> UpdateSprint(SprintBacklog sprintBacklog)
        {
            var sprintToUpdate = backlogRepository.GetBacklogById(sprintBacklog.Id);
            sprintToUpdate.StartTime = sprintBacklog.StartTime;
            sprintToUpdate.EndTime = sprintBacklog.EndTime;
            sprintToUpdate.Description = sprintBacklog.Description;
            await backlogRepository.UpdateBacklog(sprintToUpdate);
            return await GetBacklogById(sprintBacklog.Id);
        }

        public async Task<int> DeleteSprint(int sprintId)
        {
            var sprint = backlogRepository.GetBacklogById(sprintId);
            return await backlogRepository.DeleteBacklog(sprint);
        }
    }
}
