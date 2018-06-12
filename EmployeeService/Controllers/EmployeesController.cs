using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {

        //public IEnumerable<Employee> Get()
        [HttpGet]
        [BasicAuthentication]
        public HttpResponseMessage LoadAllEmployees()
        {

            string userName = Thread.CurrentPrincipal.Identity.Name;

            using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
            {
                // version 1
                //switch (gender.ToLower())
                //{
                //    case "all":
                //        return Request.CreateResponse(HttpStatusCode.OK, employeeDBEntities.Employees.ToList());
                //    case "male":
                //        return Request.CreateResponse(HttpStatusCode.OK, employeeDBEntities.Employees.Where(x =>x.Gender.ToLower()=="male").ToList());
                //    case "female":
                //        return Request.CreateResponse(HttpStatusCode.OK, employeeDBEntities.Employees.Where(x => x.Gender.ToLower() == "female").ToList());
                //     default:
                //        return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "value for gender must be all, male / female");
                //}


                switch (userName.ToLower())
                {
                 
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, employeeDBEntities.Employees.Where(x => x.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, employeeDBEntities.Employees.Where(x => x.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }

        }

        [HttpGet]
        public HttpResponseMessage LoadEmployeeById(int id)
        {
            using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
            {
                var entity = employeeDBEntities.Employees.Where(x => x.ID == id).FirstOrDefault();

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee not found");
                }
            }

        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
                {
                    employeeDBEntities.Employees.Add(employee);
                    employeeDBEntities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        public HttpResponseMessage Delete(int id)
        {
            using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
            {
                try
                {
                    var entity = employeeDBEntities.Employees.Where(x => x.ID == id).FirstOrDefault();
                    if (entity == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Employee with ID =" + id.ToString()+"not found");
                    }
                    else
                    {
                        employeeDBEntities.Employees.Remove(entity);
                        employeeDBEntities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);

                    }
                } catch(Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
              
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
                {
                    var entity = employeeDBEntities.Employees.Where(x => x.ID == id).FirstOrDefault();
                    if (entity == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Employee with ID =" + id.ToString() + "not found");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;
                        employeeDBEntities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);

                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
