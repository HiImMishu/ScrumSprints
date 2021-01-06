using Project.Models;
using Project.Models.DTO;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service
{
    public class ItemService
    {
        private readonly IItemRepository itemRepository;
        private readonly IBacklogRepository backlogRepository;
        private readonly IUserRepository userRepository;

        public ItemService(ItemRepository itemRepository, BacklogRepository backlogRepository, UserRepository userRepository)
        {
            this.itemRepository = itemRepository;
            this.backlogRepository = backlogRepository;
            this.userRepository = userRepository;
        }

        public async Task<ItemDTO> SaveItem(Item item)
        {
            item.ItemId = 0;
            await itemRepository.SaveItem(item);
            return await GetItemById(item.ItemId);
        }

        public async Task<ItemDTO> GetItemById(int itemId)
        {
            var item = await itemRepository.GetItemById(itemId);

            return new ItemDTO()
            {
                ItemId = item.ItemId,
                Description = item.Description,
                AddedAt = item.AddedAt,
                Status = item.status,
                Sprint = (item.SprintId == null) ? null : getBacklogById((int)item.SprintId),
                ModifiedBy = (item.ModifiedBy == null) ? null : GetExecutiveUser((int)item.ModifiedBy)
            };
        }

        private BacklogDTO getBacklogById(int backlogId)
        {
            var backlog = backlogRepository.GetBacklogById(backlogId);
            return new BacklogDTO()
            {
                Id = backlog.Id,
                Description = backlog.Description,
                StartTime = backlog.StartTime,
                EndTime = backlog.EndTime
            };
        }

        private SimpleUserDTO GetExecutiveUser(int userId)
        {
            var user = userRepository.GetUserById(userId);

            return new SimpleUserDTO()
            {
                Id = user.id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        public async Task<ItemDTO> UpdateItem(Item item)
        {
            if (item.SprintId == -1)
            {
                item.SprintId = null;
            }

            if (item.SprintId != null && backlogRepository.GetBacklogById((int)item.SprintId) == null)
            {
                return null;
            }

            var itemToUpdate = await itemRepository.GetItemById(item.ItemId);
            itemToUpdate.Description = item.Description;
            itemToUpdate.status = item.status;
            itemToUpdate.SprintId = item.SprintId;
            itemToUpdate.ModifiedBy = item.ModifiedBy;

            await itemRepository.UpdateItem(itemToUpdate);
            return await GetItemById(itemToUpdate.ItemId);
        }

        public async Task<int> DeleteItem(int itemId)
        {
            var item = await itemRepository.GetItemById(itemId);
            return await itemRepository.DeleteItem(item);
        }
    }
}