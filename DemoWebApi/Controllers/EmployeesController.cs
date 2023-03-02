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
        public IEnumerable<Employees> Get()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }

        }
        public HttpResponseMessage Get(int id)
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
        public HttpResponseMessage Post([FromBody] Employees employees)
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

        public HttpResponseMessage Deleted(int id)
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
        public HttpResponseMessage Put(int id, [FromBody] Employees employees)
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

