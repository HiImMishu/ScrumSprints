using Project.Models;
using Project.Models.DTO;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IUserRepository userRepository;
        private readonly IItemRepository itemRepository;
        private readonly IBacklogRepository backlogRepository;

        public ProductService(ProductRepository productRepository, TeamRepository teamRepository, UserRepository userRepository, ItemRepository itemRepository, BacklogRepository backlogRepository)
        {
            this.productRepository = productRepository;
            this.teamRepository = teamRepository;
            this.userRepository = userRepository;
            this.itemRepository = itemRepository;
            this.backlogRepository = backlogRepository;
        }

        public async Task<ProductDTO> GetProductById(int productId)
        {
            var product = await productRepository.GetProductById(productId);
            Team team = null;
            User user;

            if (product == null)
            {
                return null;
            }    

            user = await userRepository.GetUserByIdAsync(product.OwnerId);
            var items = await itemRepository.GetItemsByProductId(productId);
            var itemsDTO = items.Select(i => new ItemDTO()
            {
                ItemId = i.ItemId,
                Description = i.Description,
                AddedAt = i.AddedAt,
                Status = i.status,
                Sprint = (i.SprintId == null) ? null : GetBacklog((int)i.SprintId),
                ModifiedBy = (i.ModifiedBy == null) ? null : GetExecutiveUser((int)i.ModifiedBy)
            }).ToList();

            var backlogs = await backlogRepository.GetProductBacklogs(productId);
            var backlogsDTO = backlogs.Select(b => new BacklogDTO()
            {
                Id = b.Id,
                Description = b.Description,
                StartTime = b.StartTime,
                EndTime = b.EndTime
            }).ToList();

            if (product.DevTeam != null)
            {
                team = await teamRepository.GetTeamById((int)product.DevTeam);                
            }

            return new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Owner = new SimpleUserDTO() { Id = user.id, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName },
                DevTeam = (team == null) ? null : new SimpleTeamDTO() { Id = team.Id, Name = team.Name, CreatedAt = team.CreatedAt },
                Backlogs = backlogsDTO,
                ProductItems = itemsDTO
            };
        }

        private BacklogDTO GetBacklog(int backlogId)
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

        public async Task<ProductDTO> SaveProduct(Product product)
        {
            if (product.DevTeam == -1)
            {
                product.DevTeam = null;
            }
            await productRepository.SaveProduct(product);
            return await GetProductById(product.Id);
        }

        public async Task<ProductDTO> UpdateProduct(ProductUpdateDTO updateDTO)
        {
            var product = await productRepository.GetProductById(updateDTO.Id);
            var team = await teamRepository.GetTeamById(updateDTO.DevTeam);

            if (team == null)
            {
                throw new ArgumentException("Team not found.");
            }

            product.Name = updateDTO.Name;
            product.DevTeam = updateDTO.DevTeam;

            await productRepository.UpdateProduct(product);
            return await GetProductById(product.Id);
        }

        public async Task<int> DeleteProduct(int productId)
        {
            var product = await productRepository.GetProductById(productId);
            return await productRepository.DeleteProduct(product);
        }
    }
}
