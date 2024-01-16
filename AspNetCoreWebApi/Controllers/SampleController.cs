//using AspNetCoreWebApi.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace AspNetCoreWebApi.Controllers
//{
    
//    [ApiController]
//    public class EmployeesController : ControllerBase
//    {


//        [Route("employees/all")]
//        public string GetAllEmployees()
//        {
//            return "All Employees";
//        }

//        [Route("employees/{id:int}")]
//        public string GetEmployeeById(int id) {
        
//        return "Employee by Id: " + id.ToString();
        
//        }

//        [Route("employees/{name:alpha}")]
//        public string GetEmployeeDetails(string name)
//        {
//            return "Employee Details by Id: " + name;
//        }
//    }
//}
