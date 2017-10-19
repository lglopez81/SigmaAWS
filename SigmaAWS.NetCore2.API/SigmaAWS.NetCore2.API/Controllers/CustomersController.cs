using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SigmaAWS.NetCore2.API.Infrastucture;
using SigmaAWS.NetCore2.Domain.Interfaces;
using SigmaAWS.NetCore2.Domain.Models;

namespace SigmaAWS.NetCore2.API.Controllers
{
    [Route("api/customers")]
    public class CustomersApiController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger;

        public CustomersApiController(ICustomerRepository customerRepo, ILoggerFactory loggerFactory) {
            _customerRepository = customerRepo;
            _logger = loggerFactory.CreateLogger(nameof(CustomersApiController));
        }

        // GET api/customers
        [HttpGet]
        [ProducesResponseType(typeof(List<Customers>), 200)]
        public async Task<ActionResult> Customers()
        {
            try
            {
                var customers = await _customerRepository.GetCustomersAsync();
                return Ok(customers);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest();
            }
        }

        // GET api/customers/page/10/10
        [HttpGet("page/{skip}/{take}")]
        [NoCache]
        [ProducesResponseType(typeof(List<Customers>), 200)]
        public async Task<ActionResult> CustomersPage(int skip, int take)
        {
            try
            {
                var pagingResult = await _customerRepository.GetCustomersPageAsync(skip, take);
                Response.Headers.Add("X-InlineCount", pagingResult.TotalRecords.ToString());
                return Ok(pagingResult.Records);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest();
            }
        }

        // GET api/customers/5
        [HttpGet("{id}", Name = "GetCustomerRoute")]
        [NoCache]
        [ProducesResponseType(typeof(Customers), 200)]
        public async Task<ActionResult> Customers(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerAsync(id);
                return Ok(customer);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest();
            }
        }

        // POST api/customers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCustomer([FromBody]Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var newCustomer = await _customerRepository.InsertCustomerAsync(customers);
                if (newCustomer == null)
                {
                    return BadRequest();
                }
                return CreatedAtRoute("GetCustomerRoute", new { id = newCustomer.Id });
                //return CreatedAtRoute("GetCustomerRoute", new { id = newCustomer.Id },
                //        new ApiResponse { Status = true, Customers = newCustomer });
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest();
            }
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody]Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var status = await _customerRepository.UpdateCustomerAsync(customers);
                if (!status)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest();
            }
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                var status = await _customerRepository.DeleteCustomerAsync(id);
                if (!status)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest();
            }
        }

    }
}
