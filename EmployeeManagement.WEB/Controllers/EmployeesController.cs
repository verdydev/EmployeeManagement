using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.WEB.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public EmployeesController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _baseApiUrl = config["ApiBaseUrl"]; 
        }

        // GET: EmployeesController
        public async Task<IActionResult> Index()
        {
            var apiUrl = $"{_baseApiUrl}/employees"; 

            List<Employee> employees = new List<Employee>();

            try
            {
                employees = await _httpClient.GetFromJsonAsync<List<Employee>>(apiUrl);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Failed to get data: " + ex.Message;
            }

            return View(employees);
        }

        // GET: EmployeesController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var apiUrl = $"{_baseApiUrl}/employees/{id}"; 

            Employee employee = new Employee();

            try
            {
                employee = await _httpClient.GetFromJsonAsync<Employee>(apiUrl);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Failed to get data: " + ex.Message;
            }

            ViewBag.Page = "Details";
            return View("CreateEdit", employee);
        }

        // GET: EmployeesController/Create
        public async Task<ActionResult> Create()
        {
            var apiGetPositions = $"{_baseApiUrl}/positions";
            Employee employee = new Employee();
            List<string>? positions = new List<string>();

            try
            {
                positions = await _httpClient.GetFromJsonAsync<List<string>>(apiGetPositions);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Failed to get data: " + ex.Message;
            }

            ViewBag.Page = "Create";
            ViewBag.Positions = new SelectList(positions);

            return View("CreateEdit",employee);
        }

        // POST: EmployeesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            var apiUrl = $"{_baseApiUrl}/employees";
            var apiGetPositions = $"{_baseApiUrl}/positions";
            var employee = new Employee();
            var positions = new List<string>();

            try
            {
                employee = new Employee
                {
                    ID = Guid.NewGuid().ToString(), 
                    Name = collection["Name"],
                    Email = collection["Email"],
                    Position = collection["Position"],
                    Salary = decimal.Parse(collection["Salary"])
                };

                var response = await _httpClient.PostAsJsonAsync(apiUrl, employee);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Status"] = "success";
                    TempData["Message"] = "Employee has been create!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    positions = await _httpClient.GetFromJsonAsync<List<string>>(apiGetPositions);
                    ViewBag.Error = "Failed to add new employee";
                    ViewBag.Positions = new SelectList(positions);
                    return View(employee); 
                }
            }
            catch (Exception ex)
            {
                positions = await _httpClient.GetFromJsonAsync<List<string>>(apiGetPositions);
                ViewBag.Error = $"Something when wrong: {ex.Message}";
                ViewBag.Positions = new SelectList(positions);
                return View(employee); 
            }
        }

        // GET: EmployeesController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var apiUrl = $"{_baseApiUrl}/employees/{id}"; 
            var apiGetPositions = $"{_baseApiUrl}/positions";

            Employee? employee = new Employee();
            List<string>? positions = new List<string>();

            try
            {
                employee = await _httpClient.GetFromJsonAsync<Employee>(apiUrl);
                positions = await _httpClient.GetFromJsonAsync<List<string>>(apiGetPositions);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Failed to get data: " + ex.Message;
            }

            ViewBag.Page = "Edit";
            ViewBag.Positions = new SelectList(positions);
            return View("CreateEdit",employee);
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
        {
            var apiUrl = $"{_baseApiUrl}/employees/{id}";
            var apiGetPositions = $"{_baseApiUrl}/positions";
            var employee = new Employee();
            var positions = new List<string>();
            try
            {
                employee = new Employee
                {
                    ID = collection["ID"],
                    Name = collection["Name"],
                    Email = collection["Email"],
                    Position = collection["Position"],
                    Salary = decimal.Parse(collection["Salary"])
                };

                var response = await _httpClient.PutAsJsonAsync(apiUrl, employee);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Status"] = "success";
                    TempData["Message"] = "Employee has been updated!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    positions = await _httpClient.GetFromJsonAsync<List<string>>(apiGetPositions);
                    ViewBag.Error =  $"Failed to update employee";
                    ViewBag.Positions = new SelectList(positions);
                    return View("CreateEdit", employee);
                }
            }
            catch (Exception ex)
            {
                positions = await _httpClient.GetFromJsonAsync<List<string>>(apiGetPositions);
                ViewBag.Error = $"Something when wrong: {ex.Message}";
                ViewBag.Positions = new SelectList(positions);
                return View("CreateEdit", employee);
            }
        }

        // GET: EmployeesController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var apiUrl = $"{_baseApiUrl}/employees/{id}";

            try
            {
                var response = await _httpClient.DeleteAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Status"] = "error";
                    TempData["Message"] = $"failed remove employee: {response.StatusCode}";
                }
                else
                {
                    TempData["Status"] = "success";
                    TempData["Message"] = "Employee has been deleted!";
                }
            }
            catch (Exception ex)
            {
                TempData["Status"] = "error";
                TempData["Message"] = $"failed remove employee: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
