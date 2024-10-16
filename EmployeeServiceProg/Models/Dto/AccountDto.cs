namespace EmployeeServiceProg.Models.Dto
{
    public class AccountDto
    {
        public int AccountId { get; set; }

        public string Email { get; set; }

        public bool Locked { get; set; }

        public string Name { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }
    }
}
