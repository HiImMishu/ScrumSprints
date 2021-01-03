using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly ProjectContext _context;

        public ItemRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetItemsByProductId(int productId)
        {
            return await _context.Item.Where(i => i.ProductId == productId).ToListAsync();
        }

        public async Task<int> SaveItem(Item item)
        {
            _context.Item.Add(item);
            return await _context.SaveChangesAsync();
        }

        public async Task<Item> GetItemById(int itemId)
        {
            return await _context.Item.FindAsync(itemId);
        }

        public async Task<int> UpdateItem(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteItem(Item item)
        {
            _context.Item.Remove(item);
            return await _context.SaveChangesAsync();
        }
    }

    public interface IItemRepository
    {
        Task<List<Item>> GetItemsByProductId(int productId);
        Task<Item> GetItemById(int itemId);
        Task<int> SaveItem(Item item);
        Task<int> UpdateItem(Item item);
        Task<int> DeleteItem(Item item);
    }
}
