using Fanex.MailDelivery.Core.Models;
using Fanex.COProfit.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.COProfit.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecuteController : ControllerBase
    {
        private readonly MainService service;

        public ExecuteController(MainService service)
        {
            this.service = service;
        }

        [HttpGet]
        public JobMessage Get()
        {
            return service.Execute();
        }
    }
}