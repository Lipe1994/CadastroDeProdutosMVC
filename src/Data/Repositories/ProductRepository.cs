using Business.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BusinessDbContext Context) : base(Context)
        {
        }

        public async Task<Product> GetProductAndProvider(Guid id)
        {
            return await Context.Products
                .AsNoTracking()
                .Include(f => f.Provider)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsAndProviders()
        {
            return await Context.Products
                .AsNoTracking()
                .Include(f => f.Provider)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByProvider(Guid ProviderId)
        {
            return await Context
                .Products
                .AsNoTracking()
                .Where(p => p.ProviderId == ProviderId)
                .ToListAsync();
        }
    }
}
