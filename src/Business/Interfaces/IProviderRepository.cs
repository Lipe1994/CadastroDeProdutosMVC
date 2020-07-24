using Models;
using System;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProviderRepository : IRepository<Provider>
    {
        Task<Provider> GetProviderAndAddress(Guid id);
        Task<Provider> GetProviderAndAddressAndProduct(Guid id);
    }
}