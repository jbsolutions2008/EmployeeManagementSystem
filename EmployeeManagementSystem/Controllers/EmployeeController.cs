
using BLEmployeeManagementSystem;
using EmployeeManagementSystem.DataAccess.DataMembers;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : ControllerBase
    {
        
        [HttpGet]
        [Route("api/employeesList")]
        public IActionResult EmployeeList()
        {
            APIResponse apiResponse = new APIResponse();
            List<EmployeeModel> objEmployee = new List<EmployeeModel>();
            try
            {
                //Get the Employee List and return the json
                objEmployee = BLEmployee.EmployeeList().Select(s => new EmployeeModel { Id = s.Id, FirstName = s.FirstName, MiddleName = s.MiddleName, LastName = s.LastName }).ToList();
                return Ok(objEmployee);
            }
            catch (Exception ex)
            {
                apiResponse.statusCode = 401;
                apiResponse.message = ex.Message;
                return BadRequest(apiResponse);
            }
            
        }

        [HttpGet]
        [Route("api/employeesGet")]
        public IActionResult EmployeeGet(int id)
        {
            APIResponse apiResponse = new APIResponse();
            try
            {
                //Get the Employee and return the json
                CTEmployee objEmployee = BLEmployee.EmployeeGet(id);
                CSQLResult oResult = new CSQLResult();
                if (!objEmployee.Success)
                {
                    oResult.Success = false;
                    oResult.Message = objEmployee.Message;
                    return Ok(oResult);
                }
                else
                {
                    EmployeeModel objEmployeeModel = new EmployeeModel(objEmployee);
                    return Ok(objEmployeeModel);
                }
            }
            catch (Exception ex)
            {
                apiResponse.statusCode = 401;
                apiResponse.message = ex.Message;
                return BadRequest(apiResponse);
            }
        }

        [HttpPost]
        [Route("api/employeesSave")]
        public IActionResult EmployeeSave(string firstname, string middlename, string lastname)
        {
            APIResponse apiResponse = new APIResponse();
            if(firstname == "" && middlename == "" && lastname == "")
            {
                return NotFound("Params are missing!!");
            }
            try
            {
                EmployeeModel employee = new EmployeeModel();
                employee.FirstName = firstname;
                employee.MiddleName = middlename;
                employee.LastName = lastname;

                //Save the Employee and return the json
                CSQLResult oResult = BLEmployee.EmployeeSave(0, employee.FirstName, employee.MiddleName, employee.LastName);
            
                if (oResult.Success)
                {
                    return Ok(oResult);
                }
                else
                {
                    return Ok(oResult);
                }
            }
            catch(Exception ex)
            {
                apiResponse.statusCode = 401;
                apiResponse.message = ex.Message;
                return BadRequest(apiResponse);
            }
        }
        [HttpPut]
        [Route("api/employeesUpdate")]
        public IActionResult EmployeeUpdate(int id, string firstname, string middlename, string lastname)
        {
            APIResponse apiResponse = new APIResponse();
            try
            {
                EmployeeModel employee = new EmployeeModel();
                employee.FirstName = firstname;
                employee.MiddleName = middlename;
                employee.LastName = lastname;

                CSQLResult oResult = BLEmployee.EmployeeSave(id, employee.FirstName, employee.MiddleName, employee.LastName);

                if (oResult.Success)
                {
                    return Ok(oResult);
                }
                else
                {
                    return Ok(oResult);
                }
            }
            catch (Exception ex)
            {
                apiResponse.statusCode = 401;
                apiResponse.message = ex.Message;
                return BadRequest(apiResponse);
            }
        }
        [HttpDelete]
        [Route("api/employeesDelete")]
        public IActionResult EmployeeDelete(int id)
        {
            APIResponse apiResponse = new APIResponse();

            try
            {
                CSQLResult oResult = BLEmployee.EmployeeDelete(id);
                return Ok(oResult);
            }
            catch (Exception ex)
            {
                apiResponse.statusCode = 401;
                apiResponse.message = ex.Message;
                return BadRequest(apiResponse);
            }
            
        }
    }
}
