using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Data
{
    [Table("Employees")]
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Department))]
        public Guid DepartmentId { get; set; }

        [ForeignKey(nameof(EmployeeType))]
        public int EmployeeTypeId { get; set; }

        [Column, StringLength(225)]
        public string Name { get; set; }

        [Column, StringLength(225)]
        public string Surname { get; set; }

        [Column, StringLength(225)]
        public string Patronymic { get; set; }

        [Column(TypeName ="money")]
        public decimal Salary { get; set; }

        public EmployeeType EmployeeType { get; set; }
        public Department Department { get; set; }

    }
}
