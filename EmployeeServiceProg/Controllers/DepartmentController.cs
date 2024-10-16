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
    public class DepartmentController : ControllerBase
    {
        #region Services 

        private readonly IDepartmentRepository _departmentRepository;

        #endregion

        #region Constructors

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        #endregion

        #region Public Methods
        [HttpGet("department/all")]
        public ActionResult<IList<DepartmentDto>> GetAllDepartmnents()
        {
            return Ok(_departmentRepository.GetAll().Select(d =>
                new DepartmentDto
                {
                    Id = d.Id,
                    Description = d.Description,
                }).ToList());
        }

        [HttpPost("department/create")]
        public ActionResult<int> CreateDepartments([FromQuery] string description)
        {
            return Ok(_departmentRepository.Create(new Department
            {
                Description = description
            }));
        }

        [HttpDelete("department/delete")]
        public ActionResult<bool> DeleteDepartments([FromQuery] Guid id)
        {
            return Ok(_departmentRepository.Delete(id));
        }

        [HttpPut("department/update")]
        public ActionResult<bool> UpdateDepartments([FromBody] DepartmentDto depDto)
        {
            return Ok(_departmentRepository.Update(new Department
            {
                Id = depDto.Id,
                Description = depDto.Description
            }));
        }

        [HttpGet("department/{id}")]
        public ActionResult<DepartmentDto> GetDepartmentsById(Guid id)
        {
            var department = _departmentRepository.GetById(id);
            if (department == null)
            {
                return NotFound($"Department with Id {id} not found.");
            }

            return Ok(new DepartmentDto
            {
                Id = department.Id,
                Description = department.Description
            });
        }
        #endregion

    }
}
