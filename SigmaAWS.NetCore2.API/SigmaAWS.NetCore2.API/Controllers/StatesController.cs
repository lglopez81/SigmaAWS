using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SigmaAWS.NetCore2.Domain.Interfaces;
using SigmaAWS.NetCore2.Domain.Models;

namespace SigmaAWS.NetCore2.API.Controllers
{
    [Route("api/states")]
    public class StatesApiController : Controller
    {
        readonly IStateRepository _stateRepository;
        readonly ILogger _logger;

        public StatesApiController(IStateRepository stateRepo, ILoggerFactory loggerFactory) {
            _stateRepository = stateRepo;
            _logger = loggerFactory.CreateLogger(nameof(StatesApiController));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<States>), 200)]
        public async Task<ActionResult> States()
        {
            try
            {
                var states = await _stateRepository.GetStatesAsync();
                return Ok(states);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest();
            }
        }

    }
}
