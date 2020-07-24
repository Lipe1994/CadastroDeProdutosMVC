using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByProvider(Guid fornecedorId);
        Task<IEnumerable<Product>> GetProductsAndProviders();
        Task<Product> GetProductAndProvider(Guid id);
    }
}