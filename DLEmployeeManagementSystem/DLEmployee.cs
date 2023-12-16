using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.DataAccess.DataMembers;
using System.Data;

namespace DLEmployeeManagementSystem
{
    public class DLEmployee
    {
        /* Create obj for DLCommon and DLEmployee */
        public static DLCommon objCommon = new DLCommon();
        public static List<CTEmployee> EmployeeList()
        {
            //Get the all employee list from db
            List<CTEmployee> oResult = new List<CTEmployee>();
            try
            {
                DataSet dsEmployee = objCommon.GetDataSet("tblEmployee", "usp_EmployeeList");
                oResult = dsEmployee.Tables[0].AsEnumerable().Select(row => new CTEmployee
                {
                    Id = row.Field<int?>(0).GetValueOrDefault(),
                    FirstName = row.Field<string>(1),
                    MiddleName = row.Field<string>(2),
                    LastName = row.Field<string>(3),
                    
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oResult;
        }
        public static CSQLResult EmployeeSave(int iId, string sFirstName, string sMiddleName, string sLastName)
        {
            //Save the employee in db
            CSQLResult oResult = new CSQLResult();

            try
            {
                DataSet dsEmployee = objCommon.GetDataSet("tblEmployee", "usp_EmployeeSave " + iId + ", '" + sFirstName + "', '" + sMiddleName + "','" + sLastName + "'");
                using (DataTable dtEmployee = dsEmployee.Tables["tblEmployee"])
                {
                    if (dtEmployee != null && dtEmployee.Rows.Count > 0)
                    {
                        oResult.Success = Convert.ToBoolean(dtEmployee.Rows[0]["Success"]);
                        oResult.Message = dtEmployee.Rows[0]["ErrorDescription"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                oResult.Success = false;
                oResult.Message = ex.Message;
            }
            return oResult;
        }
        public static CTEmployee EmployeeGet(int iId)
        {
            //Get the employee from db
            CTEmployee oResult = new CTEmployee();
            try
            {
                DataSet dsEmployee = objCommon.GetDataSet("tblEmployee", "usp_GetEmployee " + iId);

                using (DataTable dtEmployee = dsEmployee.Tables["tblEmployee"])
                {
                    if (dtEmployee.Rows.Count > 0)
                    {
                        oResult = new CTEmployee()
                        {
                            Id = Convert.ToInt32(dtEmployee.Rows[0]["Id"]),
                            FirstName = dtEmployee.Rows[0]["FirstName"].ToString(),
                            MiddleName = dtEmployee.Rows[0]["MiddleName"].ToString(),
                            LastName = dtEmployee.Rows[0]["LastName"].ToString()
                        };
                    }
                    else
                    {
                        oResult = new CTEmployee()
                        {
                            Message = "No record found.",
                            Success = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oResult;
        }
        public static CSQLResult EmployeeDelete(int iId)
        {
            //Delete the employee in db
            CSQLResult oResult = new CSQLResult();

            try
            {
                DataSet dsEmployee = objCommon.GetDataSet("tblEmployee", "usp_DeleteEmployee " + iId + "");
                using (DataTable dtEmployee = dsEmployee.Tables["tblEmployee"])
                {
                    if (dtEmployee != null && dtEmployee.Rows.Count > 0)
                    {
                        oResult.Success = Convert.ToBoolean(dtEmployee.Rows[0]["Success"]);
                        oResult.Message = dtEmployee.Rows[0]["ErrorDescription"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                oResult.Success = false;
                oResult.Message = ex.Message;
            }
            return oResult;
        }
    }
}