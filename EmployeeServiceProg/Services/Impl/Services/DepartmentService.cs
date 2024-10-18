using EmployeeServiceProg.Services.Interfaces;
using EmployeeServiceProgProto;
using Grpc.Core;
using static EmployeeServiceProgProto.DepartmentService;

namespace EmployeeServiceProg.Services.Impl.Services
{
    public class DepartmentService : DepartmentServiceBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public override Task<CreateDepartmentResponse> CreateDepartment(CreateDepartmentRequest request, ServerCallContext context)
        {
            var id = _departmentRepository.Create(new EmployeeService.Data.Department
            {
                Description = request.Description
            });
            CreateDepartmentResponse response = new CreateDepartmentResponse();
            response.Id = id.ToString();
            return Task.FromResult(response);
        }

        public override Task<DeleteDepartmentResponse> DeleteDepartment(DeleteDepartmentRequest request, ServerCallContext context)
        {
            // Преобразуем строку в Guid
            if (!Guid.TryParse(request.Id, out var departmentId))
            {
                // Обработка ошибки, если строка не является валидным GUID
                throw new ArgumentException("Invalid GUID format", nameof(request.Id));
            }
            _departmentRepository.Delete(departmentId);
            return Task.FromResult(new DeleteDepartmentResponse());
        }

        public override Task<GetAllDepartmentResponse> GetAllDepartment(GetAllDepartmentRequest request, ServerCallContext context)
        {
            GetAllDepartmentResponse response = new GetAllDepartmentResponse();
            response.Department.AddRange(_departmentRepository.GetAll().Select(et => new EmployeeServiceProgProto.Department
            {
                Id = et.Id.ToString(),
                Description = et.Description,
            }).ToList());
            return Task.FromResult(response);
        }
    }
}
