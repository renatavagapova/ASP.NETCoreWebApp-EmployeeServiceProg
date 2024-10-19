using EmployeeService.Data;
using EmployeeServiceProg.Models.Dto;
using EmployeeServiceProg.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeServiceProg.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        #region Services
        private readonly IEmployeeTypeRepository _employeeTypeRepository;
        #endregion

        #region Constructors
        public DictionariesController(IEmployeeTypeRepository employeeTypeRepository)
        {
            _employeeTypeRepository = employeeTypeRepository;
        }
        #endregion

        #region Public Methods
        [HttpGet("employee-types/all")]
        public ActionResult<IList<EmployeeTypeDto>> GetAllEmployeeTypes()
        {
            var employeeTypes = _employeeTypeRepository.GetAll().Select(et =>
                new EmployeeTypeDto
                {
                    Id = et.Id,
                    Description = et.Description,
                }).ToList();

            // Проверка на пустой список
            if (employeeTypes == null || !employeeTypes.Any()) 
            {
                return NotFound("No employee types found.");
            }

            return Ok(employeeTypes);
        }

        [HttpPost("eployee-types/create")]
        public ActionResult<int> CreateEmployeeType([FromQuery] string description)
        {
            var createdId = _employeeTypeRepository.Create(new EmployeeType
            {
                Description = description
            });

            if (createdId == 0) // Если создание не удалось, возвращаем BadRequest
            {
                return BadRequest("Failed to create employee type.");
            }

            return Ok(createdId);
        }

        [HttpDelete("employee-types/delete")]
        public ActionResult<bool> DeleteEmloyeeType([FromQuery] int id)
        {
            var isDeleted = _employeeTypeRepository.Delete(id);

            if (!isDeleted)
            {
                return NotFound($"Failed to delete EmployeeType with Id {id}.");
            }
            return Ok(isDeleted);
        }

        [HttpPut("employee-types/update")]
        public ActionResult<bool> UpdateEmployeeType([FromBody] EmployeeTypeDto employeeTypeDto)
        {
            var isUpdated = _employeeTypeRepository.Update(new EmployeeType
            {
                Id = employeeTypeDto.Id,
                Description = employeeTypeDto.Description
            });

            if (!isUpdated)
            {
                return NotFound($"Failed to update EmployeeType with Id {employeeTypeDto.Id}.");
            }

            return Ok(true);
        }

        [HttpGet("employee-types/{id}")]
        public ActionResult<EmployeeTypeDto> GetEmployeeTypeById(int id)
        {
            var employeeType = _employeeTypeRepository.GetById(id);
            if (employeeType == null)
            {
                return NotFound($"EmployeeType with Id {id} not found.");
            }

            return Ok(new EmployeeTypeDto
            {
                Id = employeeType.Id,
                Description = employeeType.Description
            });
        }

        #endregion
    }
}
