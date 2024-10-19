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
            var departments = _departmentRepository.GetAll().Select(d =>
                new DepartmentDto
                {
                    Id = d.Id,
                    Description = d.Description,
                }).ToList();

            if (!departments.Any()) // Если список пуст
            {
                return NotFound("No departments found.");
            }

            return Ok(departments);
        }

        [HttpPost("department/create")]
        public ActionResult<int> CreateDepartments([FromQuery] string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return BadRequest("Description cannot be null or empty.");
            }

            try
            {
                var result = _departmentRepository.Create(new Department
                {
                    Description = description
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpDelete("department/delete")]
        public ActionResult<bool> DeleteDepartments([FromQuery] Guid id)
        {
            var existingDepartment = _departmentRepository.GetById(id);
            if (existingDepartment == null)
            {
                return NotFound($"Department with Id {id} not found.");
            }

            var result = _departmentRepository.Delete(id);
            if (!result)
            {
                return BadRequest("Failed to delete the department.");
            }

            return Ok(result);
        }

        [HttpPut("department/update")]
        public ActionResult<bool> UpdateDepartments([FromBody] DepartmentDto depDto)
        {
            if (depDto == null || string.IsNullOrEmpty(depDto.Description))
            {
                return BadRequest("Department data is invalid.");
            }

            var existingDepartment = _departmentRepository.GetById(depDto.Id);
            if (existingDepartment == null)
            {
                return NotFound($"Department with Id {depDto.Id} not found.");
            }

            var result = _departmentRepository.Update(new Department
            {
                Id = depDto.Id,
                Description = depDto.Description
            });

            if (!result)
            {
                return BadRequest("Failed to update the department.");
            }

            return Ok(result);
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
