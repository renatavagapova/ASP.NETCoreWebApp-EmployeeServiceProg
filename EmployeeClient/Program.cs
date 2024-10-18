using Grpc.Net.Client;
using static EmployeeServiceProgProto.DepartmentService;
using static EmployeeServiceProgProto.DictionareService;

namespace EmployeeClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            DictionareServiceClient dictionareServiceClient = new DictionareServiceClient(channel);
            DepartmentServiceClient departmentServiceClient = new DepartmentServiceClient(channel);

            //#region For EmployeeType
            //Console.Write("Укажите тип сотрудника: ");

            //var responseEmpType = dictionareServiceClient.CreateEmployeeType(new EmployeeServiceProgProto.CreateEmployeeTypeRequest
            //{
            //    Description = Console.ReadLine()
            //});

            //if (responseEmpType != null)
            //{
            //    Console.WriteLine($"Тип сотрудника успешно добавлен: #{responseEmpType.Id}");
            //}

            //var getAllEmployeeTypesResponse = dictionareServiceClient.GetAllEmployeeTypes(new EmployeeServiceProgProto.GetAllEmployeeTypesRequest());
            //foreach (var employeeType in getAllEmployeeTypesResponse.EmployeeTypes)
            //{
            //    Console.WriteLine($"#{employeeType.Id} / {employeeType.Description}");
            //}
            //#endregion

            #region For Department
            Console.Write("Укажите отдел: ");

            var responseDepart = departmentServiceClient.CreateDepartment(new EmployeeServiceProgProto.CreateDepartmentRequest
            {
                Description = Console.ReadLine()
            });

            if (responseDepart != null)
            {
                Console.WriteLine($"Отдел успешно добавлен: #{responseDepart.Id}");
            }

            var getAllDepartmentResponse = departmentServiceClient.GetAllDepartment(new EmployeeServiceProgProto.GetAllDepartmentRequest());
            foreach (var department in getAllDepartmentResponse.Department)
            {
                Console.WriteLine($"#{department.Id} / {department.Description}");
            }
            #endregion
            Console.ReadLine();
        }


    }
}
