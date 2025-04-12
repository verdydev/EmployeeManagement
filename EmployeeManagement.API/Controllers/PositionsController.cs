using EmployeeManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/positions")]
    public class PositionsController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeetService;
        public PositionsController(ILogger<EmployeeController> logger, IEmployeeService employeetService)
        {
            _logger = logger;
            _employeetService = employeetService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<string> positions = await _employeetService.GetPositions();
            return Ok(positions);
        }
    }
}
