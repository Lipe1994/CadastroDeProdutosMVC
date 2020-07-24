using Business.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(BusinessDbContext Context) : base(Context)
        {
        }

        public async  Task<Address> GetAddressByProvider(Guid providerId)
        {
            return await Context.Adresses.Where(a => a.ProviderId == providerId).FirstOrDefaultAsync(); 
        }
    }
}
