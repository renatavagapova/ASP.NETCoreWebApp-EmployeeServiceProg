using EmployeeService.Data;

namespace EmployeeServiceProg.Services.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department, Guid>
    {
    }
}
