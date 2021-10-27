using ClientAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase {

        private readonly IClientService _clientService;

        [HttpGet(nameof(GetClientById))]
        public async Task<IActionResult> GetClientById(string id, CancellationToken cancellationToken) {
            try {
                return Ok(await _clientService.GetClientById(id, cancellationToken));
            }catch(Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost(nameof(AppendEvents))]
        public IActionResult AppendEvents(string id, CancellationToken cancellationToken) {
            try {
                _clientService.PushEventsToClientStream(id, cancellationToken);
                return Ok();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        public ClientController(IClientService clientService) {
            _clientService = clientService;
        }
    }
}
