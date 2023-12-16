
using DLEmployeeManagementSystem;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.DataAccess.DataMembers;

namespace BLEmployeeManagementSystem 
{
    public class BLEmployee
    {
       
        public static List<CTEmployee> EmployeeList()
        {
            //Get the all employee list
            var oResult = DLEmployee.EmployeeList().ToList();
            return oResult.ToList();
            
        }
        public static CSQLResult EmployeeSave(int iId, string sFirstName, string sMiddleName, string sLastName)
        {
            //Save the employee
            CSQLResult oResult = new CSQLResult();
            try
            {
                oResult = DLEmployee.EmployeeSave(iId, sFirstName, sMiddleName, sLastName);
            }
            catch (Exception ex)
            {
                oResult.Message = ex.Message;
            }
            return oResult;
        }

        public static CTEmployee EmployeeGet(int iId)
        {
            //Get the employee
            var oResult = DLEmployee.EmployeeGet(iId);
            return oResult;
        }
        public static CSQLResult EmployeeDelete(int iId)
        {
            //Save the employee
            CSQLResult oResult = new CSQLResult();
            try
            {
                oResult = DLEmployee.EmployeeDelete(iId);
            }
            catch (Exception ex)
            {
                oResult.Message = ex.Message;
            }
            return oResult;
        }
    }
}