using Fanex.MailDelivery.Core.Models;
using Fanex.COProfit.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.COProfit.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class ExecuteController(MainService service) : ControllerBase
{
    [HttpGet]
    public JobMessage Get()
    {
        return service.Execute();
    }
}