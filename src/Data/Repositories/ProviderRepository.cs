using Business.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(BusinessDbContext Context) : base(Context)
        {
        }

        public async Task<Provider> GetProviderAndAddress(Guid id)
        {
            return await  Context
                .Providers
                .Include(p => p.Address)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Provider> GetProviderAndAddressAndProduct(Guid id)
        {
            return await Context
                .Providers
                .Include(p => p.Address)
                .Include(p => p.Products)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
