using BlazorStudent.Server.Model;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorStudent.Server.Services
{
    public interface IStudent
    {
        Task<List<Students>> StudentList();
    }
    public class StudentServices : IStudent
    {
        private readonly IConfiguration _configuration;

        public StudentServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("StudentContext").Value;
            return connection;
        }

        public async Task<List<Students>> StudentList()
        {
            var connectionString = this.GetConnection();
            List<Students> lst = new List<Students>();
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    await con.OpenAsync();
                    var com = new MySqlCommand("SELECt `Id`,`Fname`,`Mi`,`Lname`,`Birthday`,`Address`,`Email` FROM tblstudinfo", 
                        con)
                    {
                        CommandType = CommandType.Text
                    };
                    var rdr = (MySqlDataReader)await com.ExecuteReaderAsync();

                    while (rdr.Read())
                    {
                        lst.Add(new Students
                        {
                            Id = rdr["Id"].ToString(),
                            FirstName = rdr["Fname"].ToString(),
                            Mi = rdr["Mi"].ToString(),
                            LastName = rdr["Lname"].ToString(),
                            Birthdate = (DateTime)rdr["Birthday"],
                            Address = rdr["Address"].ToString(),
                            Email = rdr["Email"].ToString()
                        });
                    }
                    return lst.ToList();
                }
                finally { con.Close(); }

            }
        }
    }
}
