using EmployeeService.Data;

namespace EmployeeServiceProg.Services.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee, int>
    {
    }
}
