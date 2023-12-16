using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.DataAccess.DataMembers
{
    public class CTEmployee : CSQLResult
    {
        public CTEmployee()
        {
            Id = 0;
            FirstName = "";
            MiddleName = "";
            LastName = "";
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
    public class CSQLResult
    {
        /* Store the SQL output */
        public CSQLResult()
        {
            Success = false;
            Message = "";
        }
        public Boolean Success { get; set; }
        public string Message { get; set; }

    }
}