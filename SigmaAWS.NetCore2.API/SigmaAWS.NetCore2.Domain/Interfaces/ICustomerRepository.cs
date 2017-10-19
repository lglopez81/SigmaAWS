using System.Collections.Generic;
using System.Threading.Tasks;
using SigmaAWS.NetCore2.Domain.Models;

namespace SigmaAWS.NetCore2.Domain.Interfaces
{
    public interface ICustomerRepository : IRepository<Customers>
    {
        Task<List<Customers>> GetCustomersAsync();
        Task<PagingResult<Customers>> GetCustomersPageAsync(int skip, int take);
        Task<Customers> GetCustomerAsync(int id);

        Task<Customers> InsertCustomerAsync(Customers customers);
        Task<bool> UpdateCustomerAsync(Customers customers);
        Task<bool> DeleteCustomerAsync(int id);
    }
}