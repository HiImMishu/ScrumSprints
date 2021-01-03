using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProjectContext _context;

        public ProductRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductByUserIdAsync(int userId)
        {
            return await _context.Product.Where(p => p.OwnerId == userId).ToListAsync();
        }

        public async Task<Product> GetProductByTeamId(int teamId)
        {
            return await _context.Product.Where(p => p.DevTeam == teamId).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Product.Where(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<int> SaveProduct(Product product)
        {
            _context.Product.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteProduct(Product product)
        {
            _context.Product.Remove(product);
            return await _context.SaveChangesAsync();
        }
    }

    public interface IProductRepository
    {
        Task<List<Product>> GetProductByUserIdAsync(int userId);
        Task<Product> GetProductByTeamId(int teamId);
        Task<Product> GetProductById(int productId);
        Task<int> SaveProduct(Product product);
        Task<int> UpdateProduct(Product product);
        Task<int> DeleteProduct(Product product);
    }
}
