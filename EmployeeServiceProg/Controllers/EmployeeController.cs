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
            return Ok(_employeeRepository.GetAll().Select(et =>
                new EmployeeDto
                {
                    Id = et.Id,
                    DepartmentId = et.DepartmentId,
                    EmployeeTypeId = et.EmployeeTypeId,
                    Name = et.Name,
                    Surname = et.Surname,
                    Patronymic = et.Patronymic,
                    Salary=et.Salary
                }).ToList());
        }

        [HttpPost("eployee/create")]
        public ActionResult<int> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            return Ok(_employeeRepository.Create(new Employee
            {
                DepartmentId = employeeDto.DepartmentId,
                EmployeeTypeId = employeeDto.EmployeeTypeId,
                Name = employeeDto.Name,
                Surname = employeeDto.Surname,
                Patronymic = employeeDto.Patronymic,
                Salary = employeeDto.Salary
            }));
        }

        [HttpDelete("employee/delete")]
        public ActionResult<bool> DeleteEmloyee([FromQuery] int id)
        {
            return Ok(_employeeRepository.Delete(id));
        }

        [HttpPut("employee/update")]
        public ActionResult<bool> UpdateEmployee([FromBody] EmployeeDto employeeDto)
        {
            return Ok(_employeeRepository.Update(new Employee
            {
                Id = employeeDto.Id,
                DepartmentId = employeeDto.DepartmentId,
                EmployeeTypeId = employeeDto.EmployeeTypeId,
                Name = employeeDto.Name,
                Surname = employeeDto.Surname,
                Patronymic = employeeDto.Patronymic,
                Salary = employeeDto.Salary
            }));
        }

        [HttpGet("employee/{id}")]
        public ActionResult<EmployeeDto> GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetById(id);

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
