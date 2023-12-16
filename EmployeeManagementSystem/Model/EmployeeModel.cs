using EmployeeManagementSystem.DataAccess.DataMembers;
using System.Xml.Linq;

namespace EmployeeManagementSystem.Model
{
    public class EmployeeModel
    {
        public EmployeeModel() { }

        public EmployeeModel(CTEmployee objEmployee)
        {
            Id = objEmployee.Id;
            FirstName = objEmployee.FirstName;
            MiddleName = objEmployee.MiddleName;
            LastName = objEmployee.LastName;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
