using EmployeeServiceProg.Services.Interfaces;
using EmployeeServiceProgProto;
using Grpc.Core;
using static EmployeeServiceProgProto.EmployeesService;

namespace EmployeeServiceProg.Services.Impl.Services
{
    public class EmployeesService : EmployeesServiceBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public override Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request, ServerCallContext context)
        {
            // Преобразуем строку DepartmentId в Guid
            if (!Guid.TryParse(request.DepartmentId, out var departmentId))
            {
                // Обработка ошибки, если строка не является валидным GUID
                throw new ArgumentException("Invalid GUID format", nameof(request.DepartmentId));
            }

            var id = _employeeRepository.Create(new EmployeeService.Data.Employee
            {
                DepartmentId = departmentId,
                EmployeeTypeId = request.EmployeeTypeId,
                Name = request.Name,
                Surname = request.Surname,
                Patronymic = request.Patronymic,
                Salary = request.Salary
            });
            CreateEmployeeResponse response = new CreateEmployeeResponse();
            response.Id = id;
            return Task.FromResult(response);
        }

        public override Task<DeleteEmloyeeResponse> DeleteEmloyee(DeleteEmloyeeRequest request, ServerCallContext context)
        {
            _employeeRepository.Delete(request.Id);
            return Task.FromResult(new DeleteEmloyeeResponse());
        }

        public override Task<GetAllEmployeeResponse> GetAllEmployee(GetAllEmployeeRequest request, ServerCallContext context)
        {
            GetAllEmployeeResponse response = new GetAllEmployeeResponse();
            response.Employee.AddRange(_employeeRepository.GetAll().Select(et => new Employee
            {
                Id = et.Id,
                DepartmentId = et.DepartmentId.ToString(),
                EmployeeTypeId = et.EmployeeTypeId,
                Name = et.Name,
                Surname = et.Surname,
                Patronymic = et.Patronymic,
                Salary = (int)et.Salary
            }).ToList());
            return Task.FromResult(response);
        }
    }
}
