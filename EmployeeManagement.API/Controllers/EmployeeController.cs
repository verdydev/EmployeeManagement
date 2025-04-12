using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeetService;
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeetService)
        {
            _logger = logger;
            _employeetService = employeetService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<Employee> reimbursements = await _employeetService.GetAllAsync();
            return Ok(reimbursements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var employee = await _employeetService.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Employee data can't be empty.");

            var id = await _employeetService.AddAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Employee employee)
        {
            if (employee == null || id != employee.ID)
                return BadRequest("Employee data can't be empty");

            var updated = await _employeetService.UpdateAsync(employee);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("parameter can't be empty");

            var deleted = await _employeetService.DeleteByIdAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
