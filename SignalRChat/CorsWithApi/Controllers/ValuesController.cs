using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorsWithApi.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CorsWithApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ValuesController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Task.WaitAll(
            _hubContext.Clients.All.SendAsync("ReceiveMessage", "Api", "Message")
            );
            //await _hub.Clients.All.SendAsync("show_data", data);

            return new string[] { "value1", "value2" };
        }
    }
}
