using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SigmaAWS.NetCore2.DAL.Context;
using SigmaAWS.NetCore2.Domain.Interfaces;
using SigmaAWS.NetCore2.Domain.Models;

namespace SigmaAWS.NetCore2.DAL.Repositories
{
    public class CustomerRepository : Repository<Customers>, ICustomerRepository
    {

        private readonly CustomersContext _context;
        private readonly ILogger _logger;

        public CustomerRepository(CustomersContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("CustomerRepository");
        }

        public async Task<List<Customers>> GetCustomersAsync()
        {
            var response = await _context.Customers.OrderBy(c => c.LastName)
                                 //.Include(c => c.State)
                                 .ToListAsync();
            return response;
        }

        public async Task<PagingResult<Customers>> GetCustomersPageAsync(int skip, int take)
        {
            var totalRecords = await _context.Customers.CountAsync();
            var customers = await _context.Customers
                                 .OrderBy(c => c.LastName)
                                 .Include(c => c.State)
                                 .Include(c => c.Orders)
                                 .Skip(skip)
                                 .Take(take)
                                 .ToListAsync();
            return new PagingResult<Customers>(customers, totalRecords);
        }

        public async Task<Customers> GetCustomerAsync(int id)
        {
            return await _context.Customers
                                 .Include(c => c.State)
                                 .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customers> InsertCustomerAsync(Customers customer)
        {
            _context.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
                _logger.LogError($"Error in {nameof(InsertCustomerAsync)}: " + exp.Message);
            }

            return customer;
        }

        public async Task<bool> UpdateCustomerAsync(Customers customer)
        {
            //Will update all properties of the Customer
            _context.Customers.Attach(customer);
            _context.Entry(customer).State = EntityState.Modified;
            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
                _logger.LogError($"Error in {nameof(UpdateCustomerAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            //Extra hop to the database but keeps it nice and simple for this demo
            //Including orders since there's a foreign-key constraint and we need
            //to remove the orders in addition to the customer
            var customer = await _context.Customers
                                .Include(c => c.Orders)
                                .SingleOrDefaultAsync(c => c.Id == id);
            _context.Remove(customer);
            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _logger.LogError($"Error in {nameof(DeleteCustomerAsync)}: " + exp.Message);
            }
            return false;
        }

    }
}