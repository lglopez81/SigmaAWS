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

        public async Task<States> GetStateAsync(int id)
        {
            return await _context.States.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<States> GetStateByNameAsync(string name)
        {
            return await _context.States.SingleOrDefaultAsync(c => c.Name == name);
        }

        public async Task<States> InsertStateAsync(States state)
        {
            _context.Add(state);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
                _logger.LogError($"Error in {nameof(InsertStateAsync)}: " + exp.Message);
            }

            return state;
        }

        public async Task<bool> DeleteStateAsync(int id)
        {
           var state = await _context.States
                .SingleOrDefaultAsync(c => c.Id == id);
            _context.Remove(state);
            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _logger.LogError($"Error in {nameof(DeleteStateAsync)}: " + exp.Message);
            }
            return false;
        }
    }
}
