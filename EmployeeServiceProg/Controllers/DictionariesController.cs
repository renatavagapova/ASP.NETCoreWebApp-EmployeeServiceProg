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
            return Ok(_employeeTypeRepository.GetAll().Select(et =>
                new EmployeeTypeDto
                {
                    Id = et.Id,
                    Description = et.Description,
                }).ToList());
        }

        [HttpPost("eployee-types/create")]
        public ActionResult<int> CreateEmployeeType([FromQuery] string description)
        {
            return Ok(_employeeTypeRepository.Create(new EmployeeType
            {
                Description = description
            }));
        }

        [HttpDelete("employee-types/delete")]
        public ActionResult<bool> DeleteEmloyeeType([FromQuery] int id)
        {
            return Ok(_employeeTypeRepository.Delete(id));
        }

        [HttpPut("employee-types/update")]
        public ActionResult<bool> UpdateEmployeeType([FromBody] EmployeeTypeDto employeeTypeDto)
        {
            return Ok(_employeeTypeRepository.Update(new EmployeeType
            {
                Id = employeeTypeDto.Id,
                Description = employeeTypeDto.Description
            }));
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
