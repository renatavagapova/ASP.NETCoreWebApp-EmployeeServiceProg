using EmployeeServiceProg.Models.Dto;
using EmployeeServiceProg.Services.Interfaces;
using EmployeeServiceProgProto;
using Grpc.Core;
using static EmployeeServiceProgProto.DictionareService;

namespace EmployeeServiceProg.Services.Impl.Services
{
    public class DictionariesService : DictionareServiceBase
    {
        private readonly IEmployeeTypeRepository _employeeTypeRepository;

        public DictionariesService(IEmployeeTypeRepository employeeTypeRepository)
        {
            _employeeTypeRepository = employeeTypeRepository;
        }

        public override Task<CreateEmployeeTypeResponse> CreateEmployeeType(CreateEmployeeTypeRequest request, ServerCallContext context)
        {
            var id = _employeeTypeRepository.Create(new EmployeeService.Data.EmployeeType
            {
                Description = request.Description
            });
            CreateEmployeeTypeResponse response = new CreateEmployeeTypeResponse();
            response.Id = id;
            return Task.FromResult(response);
        }

        public override Task<DeleteEmloyeeTypeResponse> DeleteEmloyeeType(DeleteEmloyeeTypeRequest request, ServerCallContext context)
        {
            _employeeTypeRepository.Delete(request.Id);
            return Task.FromResult(new DeleteEmloyeeTypeResponse());
        }

        public override Task<GetAllEmployeeTypesResponse> GetAllEmployeeTypes(GetAllEmployeeTypesRequest request, ServerCallContext context)
        {
            GetAllEmployeeTypesResponse response = new GetAllEmployeeTypesResponse();
            response.EmployeeTypes.AddRange(_employeeTypeRepository.GetAll().Select(et => new EmployeeType
            {
                Id = et.Id,
                Description = et.Description,
            }).ToList());
            return Task.FromResult(response);
        }
    }
}
