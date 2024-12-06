using Microsoft.AspNetCore.Mvc;

namespace Fanex.COProfit.Service.Controllers;

using Olympus.Diagnostic;

[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{
    [HttpGet]
    public string Index()
    {
        return "OK";
    }

    [Route("gc")]
    [HttpGet]
    public string Gc()
    {
        return GarbageCollector.CollectExtremely(); ;
    }
}