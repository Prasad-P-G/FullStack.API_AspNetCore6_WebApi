using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly FullStackDbContext fullStackDbContext;

        public EmployeesController(FullStackDbContext fullStackDbContext)
        {
            this.fullStackDbContext = fullStackDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await this.fullStackDbContext.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await this.fullStackDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await this.fullStackDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
                this.fullStackDbContext.Remove(employee);
                await this.fullStackDbContext.SaveChangesAsync();
                return Ok(employee);

            }
            return BadRequest();
        }
        
        /// <summary>
        /// for updating , we can handle with Employee id as Route parameter
        /// using [FromRoute] or by handling [FromQuery] and the other parameter employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            var result = await this.fullStackDbContext.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (result != null)
            {
                result.Name = employee.Name;
                result.Phone = employee.Phone;
                result.Email = employee.Email;
                result.Salary = employee.Salary;
                result.Department = employee.Department;
                await this.fullStackDbContext.SaveChangesAsync();
                return Ok(result);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();
            await this.fullStackDbContext.Employees.AddAsync(employeeRequest);
            await this.fullStackDbContext.SaveChangesAsync();
            return Ok(employeeRequest);

        }

        [HttpGet]
        [Route("Search/{name}")]

        public IEnumerable<Employee> Search(string name)
        {
            var result = this.fullStackDbContext.Employees.Where(e=>e.Name == name);
            return result;
        }
    }
}
