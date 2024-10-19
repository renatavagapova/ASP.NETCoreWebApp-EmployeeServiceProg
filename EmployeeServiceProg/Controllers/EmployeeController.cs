using EmployeeServiceProg.Models.Dto;
using EmployeeServiceProg.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EmployeeService.Data;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeServiceProg.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        #region Services 

        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Constructors

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Public Methods
        [HttpGet("employee/all")]
        public ActionResult<IList<EmployeeDto>> GetAllEmployee()
        {
            var employees = _employeeRepository.GetAll().Select(et =>
                new EmployeeDto
                {
                    Id = et.Id,
                    DepartmentId = et.DepartmentId,
                    EmployeeTypeId = et.EmployeeTypeId,
                    Name = et.Name,
                    Surname = et.Surname,
                    Patronymic = et.Patronymic,
                    Salary = et.Salary
                }).ToList();

            if (!employees.Any()) // если список сотрудников пуст
            {
                return NotFound("No employees found.");
            }

            return Ok(employees);
        }

        [HttpPost("eployee/create")]
        public ActionResult<int> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null || string.IsNullOrEmpty(employeeDto.Name) || string.IsNullOrEmpty(employeeDto.Surname))
            {
                return BadRequest("Employee data is invalid.");
            }

            try
            {
                var result = _employeeRepository.Create(new Employee
                {
                    DepartmentId = employeeDto.DepartmentId,
                    EmployeeTypeId = employeeDto.EmployeeTypeId,
                    Name = employeeDto.Name,
                    Surname = employeeDto.Surname,
                    Patronymic = employeeDto.Patronymic,
                    Salary = employeeDto.Salary
                });

                if (result == 0) 
                {
                    return BadRequest("Failed to create the employee.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("employee/delete")]
        public ActionResult<bool> DeleteEmloyee([FromQuery] int id)
        {
            var existingEmployee = _employeeRepository.GetById(id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with Id {id} not found.");
            }

            var result = _employeeRepository.Delete(id);
            if (!result)
            {
                return BadRequest("Failed to delete the employee.");
            }

            return Ok(result);
        }

        [HttpPut("employee/update")]
        public ActionResult<bool> UpdateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null || string.IsNullOrEmpty(employeeDto.Name) || string.IsNullOrEmpty(employeeDto.Surname))
            {
                return BadRequest("Employee data is invalid.");
            }

            var existingEmployee = _employeeRepository.GetById(employeeDto.Id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with Id {employeeDto.Id} not found.");
            }

            var result = _employeeRepository.Update(new Employee
            {
                Id = employeeDto.Id,
                DepartmentId = employeeDto.DepartmentId,
                EmployeeTypeId = employeeDto.EmployeeTypeId,
                Name = employeeDto.Name,
                Surname = employeeDto.Surname,
                Patronymic = employeeDto.Patronymic,
                Salary = employeeDto.Salary
            });

            if (!result)
            {
                return BadRequest("Failed to update the employee.");
            }

            return Ok(result);
        }

        [HttpGet("employee/{id}")]
        public ActionResult<EmployeeDto> GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee == null)
            {
                return NotFound($"Employee with Id {id} not found.");
            }

            return Ok(new EmployeeDto
            {
                Id = employee.Id,
                DepartmentId = employee.DepartmentId,
                EmployeeTypeId = employee.EmployeeTypeId,
                Name = employee.Name,
                Surname = employee.Surname,
                Patronymic = employee.Patronymic,
                Salary = employee.Salary
            });
        }
        #endregion
    }
}
