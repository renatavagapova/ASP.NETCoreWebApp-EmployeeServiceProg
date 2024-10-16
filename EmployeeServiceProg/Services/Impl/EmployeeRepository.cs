using EmployeeService.Data;
using EmployeeServiceProg.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeServiceProg.Services.Impl
{
    public class EmployeeRepository : IEmployeeRepository
    {
        #region Services
        private readonly EmployeeServiceDbContext _context;
        #endregion

        #region Constructors
        public EmployeeRepository(EmployeeServiceDbContext context)
        {
            _context = context;
        }
        #endregion

        public int Create(Employee data)
        {
            // Проверяем существование DepartmentId
            var departmentExists = _context.Departments.Any(d => d.Id == data.DepartmentId);
            if (!departmentExists)
            {
                throw new Exception("Департамент не найден. Укажите существующий DepartmentId.");
            }

            // Проверяем существование EmployeeTypeId
            var employeeTypeExists = _context.EmployeeTypes.Any(et => et.Id == data.EmployeeTypeId);
            if (!employeeTypeExists)
            {
                throw new Exception("Тип сотрудника не найден. Укажите существующий EmployeeTypeId.");
            }

            _context.Employees.Add(data);
            _context.SaveChanges();
            return data.Id;
        }

        public bool Delete(int id)
        {
            Employee employee = GetById(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IList<Employee> GetAll()
        {
            return _context.Employees
                       .Include(e => e.EmployeeType)  // Загрузка связанных данных EmployeeType
                       .Include(e => e.Department)    // Загрузка связанных данных Department
                       .ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees
                       .Include(e => e.EmployeeType)  // Загрузка связанных данных EmployeeType
                       .Include(e => e.Department)    // Загрузка связанных данных Department
                       .FirstOrDefault(x => x.Id == id);
        }

        public bool Update(Employee data)
        {
            Employee employee = GetById(data.Id);
            // Проверяем существование DepartmentId
            var departmentExists = _context.Departments.Any(d => d.Id == data.DepartmentId);
            if (!departmentExists)
            {
                throw new Exception("Департамент не найден. Укажите существующий DepartmentId.");
            }

            // Проверяем существование EmployeeTypeId
            var employeeTypeExists = _context.EmployeeTypes.Any(et => et.Id == data.EmployeeTypeId);
            if (!employeeTypeExists)
            {
                throw new Exception("Тип сотрудника не найден. Укажите существующий EmployeeTypeId.");
            }
            if (employee != null)
            {
                employee.EmployeeTypeId = data.EmployeeTypeId;
                employee.DepartmentId = data.DepartmentId;
                employee.Surname = data.Surname;
                employee.Name = data.Name;
                employee.Patronymic = data.Patronymic;
                employee.Salary = data.Salary;
                var res = _context.SaveChanges();
                return res > 0 ? true : false;
            }
            return false;
        }
    }
}
