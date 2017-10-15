using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SigmaAWS.NetCore2.DAL.Context;
//using SigmaAWS.NetCore2.DAL.Context;
using SigmaAWS.NetCore2.Domain.Interfaces;
using SigmaAWS.NetCore2.Domain.Models;

namespace SigmaAWS.NetCore2.DAL.Repositories
{
    public class StateRepository : Repository<States>, IStateRepository
    {
        private readonly CustomersContext _context;
        private readonly ILogger _logger;

        public StateRepository(CustomersContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("StateRepository");
        }

        public async Task<List<States>> GetStatesAsync()
        {
            return await _context.States.OrderBy(s => s.Abbreviation).ToListAsync();
        }
    }
}
