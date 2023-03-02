using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace DemoWebApi.Controllers
{
    public class EmployeesController : ApiController
    {

        public HttpResponseMessage Get(string gender = "All")
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e =>e.Gender.ToLower() == "female").ToList());

                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value for gender must be all,male or female  " +  gender + "  is invalid");

                        
                }
            }

        }
        [HttpGet]
        public HttpResponseMessage LoadAllemployessById(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Employee with ID=  " + id.ToString() + "Not Found");
                }
            }

        }
        [HttpPost]
        public HttpResponseMessage CreateEmployee([FromBody] Employees employees)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {

                    entities.Employees.Add(employees);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employees);
                    message.Headers.Location = new Uri(Request.RequestUri + employees.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }


        }
        [HttpDelete]
        public HttpResponseMessage DeleteEmployeeById(int id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {

                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Employee with ID=  " + id.ToString() + "Not Found to delete");

                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);


                    }

                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
        [HttpPut]
        public HttpResponseMessage UpdateEmployeeById(int id, [FromBody] Employees employees)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Employee with ID=  " + id.ToString() + "Not Found to delete");

                    }
                    else
                    {
                        entity.FirstName = employees.FirstName;
                        entity.LastName = employees.LastName;
                        entity.Gender = employees.Gender;
                        entity.Salary = employees.Salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);


                    }



                }
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }



        }
    }


}

