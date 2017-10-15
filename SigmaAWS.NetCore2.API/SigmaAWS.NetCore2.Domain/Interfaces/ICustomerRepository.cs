using System.Collections.Generic;
using System.Threading.Tasks;
using SigmaAWS.NetCore2.Domain.Models;

namespace SigmaAWS.NetCore2.Domain.Interfaces
{
    public interface ICustomerRepository : IRepository<Customers>
    {
        Task<List<Customers>> GetCustomersAsync();
    }
}