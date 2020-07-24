using Models;
using System;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> GetAddressByProvider(Guid fornecedorId);
    }
}