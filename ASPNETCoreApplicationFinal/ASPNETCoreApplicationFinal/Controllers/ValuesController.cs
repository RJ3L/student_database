using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPNETCoreApplicationFinal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using Dapper;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace ASPNETCoreApplicationFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetAllValues")]
        public string GetValues()
        {
            Console.WriteLine("Check");
            try
            {
                IDbConnection con = new MySqlConnection(_configuration.GetConnectionString("ValueAppCon").ToString());

                string sql = "SELECT * FROM Student";
                List<Student> studentlist = con.Query<Student>(sql).ToList();
                Response response = new Response();
                if (studentlist.Count > 0)
                {
                    return JsonConvert.SerializeObject(studentlist);
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No data found";
                    return JsonConvert.SerializeObject(studentlist);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        [HttpGet]
        [Route("SingleFetch/{surname}")]
        public string GetStudent(string surname)
        {
            IDbConnection con = new MySqlConnection(_configuration.GetConnectionString("ValueAppCon").ToString());
            string sql = "SELECT * FROM Student WHERE Surname = @surname";
            List<Student> student = con.Query<Student>(sql, new { Surname = surname }).ToList();
            return JsonConvert.SerializeObject(student);
        }

        [HttpGet]
        [Route("SingleFetchID/{Id}")]
        public string GetStudent(int Id)
        {
            IDbConnection con = new MySqlConnection(_configuration.GetConnectionString("ValueAppCon").ToString());
            string sql = "SELECT * FROM Student WHERE ID = @Id";
            List<Student> student = con.Query<Student>(sql, new { ID = Id }).ToList();
            return JsonConvert.SerializeObject(student);
        }
        [HttpPost]
        [Route("InsertValue")]
        public string InsertValues([FromBody] Student student)
        {
            Console.WriteLine("Check create");
            Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
            if (validateEmailRegex.IsMatch(student.Email))
            {
                try
                {
                    if (student.ContactNumber.Length == 10 && student.ContactNumber[0] == '9')
                    {
                        if (student.Age >= 18)
                        {
                            var studentData = new { student.Surname, student.FirstName, student.MiddleName, student.Age, student.Birthday, student.ContactNumber, student.Email, student.Remarks };
                            IDbConnection con = new MySqlConnection(_configuration.GetConnectionString("ValueAppCon").ToString());
                            string sql = "INSERT INTO Student (Surname, FirstName, MiddleName, Age, Birthday, ContactNumber, Email, Remarks) VALUES (@Surname, @FirstName, @MiddleName, @Age, @Birthday, @ContactNumber, @Email, @Remarks)";
                            Response response = new Response();
                            Console.WriteLine(studentData);
                            Console.WriteLine(response);
                            int affectedRows = con.Execute(sql, studentData);
                            if (affectedRows > 0)
                            {
                                return "Success";
                            }
                            else
                            {
                                response.StatusCode = 100;
                                response.ErrorMessage = "No data found";
                            }
                        }
                        else
                        {
                            return "Invalid age";
                        }
                    }
                    {
                        return "Invalid Contact Number";
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    return ex.Message;
                }
            }
            else
            {
                return "Invalid Email";
            }
        }

        [HttpPut]
        [Route("Updatevalue")]
        public string UpdateValues([FromBody] Student student)
        {
            Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
            if (validateEmailRegex.IsMatch(student.Email))
            {
                try
                {
                    if (student.ContactNumber.Length == 10 && student.ContactNumber[0] == '9')
                    {
                        if (student.Age >= 18)
                        {
                            IDbConnection con = new MySqlConnection(_configuration.GetConnectionString("ValueAppCon").ToString());
                            string sql = "UPDATE Student SET Surname = @Surname, FirstName = @FirstName, MiddleName = @MiddleName, Age = @Age, Birthday = @Birthday, ContactNumber = @ContactNumber, Email = @Email, Remarks = @Remarks WHERE ID = @ID";
                            Response response = new Response();
                            int affectedRows = con.Execute(sql, student);
                            if (affectedRows > 0)
                            {
                                return "Success";
                            }
                            else
                            {
                                response.StatusCode = 100;
                                response.ErrorMessage = "No data found";
                            }
                        }
                        else
                        {
                            return "Invalid age";
                        }
                    }
                    {
                        return "Invalid Contact Number";
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    return ex.Message;
                }
            }
            else
            {
                return "Invalid Email";
            }
        }
        [HttpDelete]
        [Route("DeleteValue/{_Id}")]
        public string DeleteValues(int _Id)
        {
            try
            {
                var StudentData = new { Id = _Id };
                IDbConnection con = new MySqlConnection(_configuration.GetConnectionString("ValueAppCon").ToString());
                string sql = "DELETE FROM Student WHERE ID = @Id";
                Response response = new Response();
                int affectedRows = con.Execute(sql, StudentData);
                if (affectedRows > 0)
                {
                    return "Success";
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No data found";
                    return "Error   ";
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return ex.Message;
            }
        }
    }
}