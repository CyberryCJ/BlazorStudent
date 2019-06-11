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
        Task SaveData(Students students);


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

        public async Task SaveData(Students students)
        {
            var connectionString = this.GetConnection();
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    await con.OpenAsync();
                    var com = new MySqlCommand("INSERT INTO tblstudinfo () VALUES (@id,@fname,@mi,@lname,@bdate,@address,@email,@user,@pwd)", con)
                    {
                        CommandType = CommandType.Text
                    };
                    com.Parameters.AddWithValue("@id", students.Id);
                    com.Parameters.AddWithValue("@fname", students.FirstName);
                    com.Parameters.AddWithValue("@mi", students.Mi);
                    com.Parameters.AddWithValue("@lname", students.LastName);
                    com.Parameters.AddWithValue("@bdate", students.Birthdate);
                    com.Parameters.AddWithValue("@address", students.Address);
                    com.Parameters.AddWithValue("@email", students.Email);
                    com.Parameters.AddWithValue("@user", students.UserName);
                    com.Parameters.AddWithValue("@pwd", students.Password);
                    await com.ExecuteNonQueryAsync();
                }
                finally { con.Close(); }

            }
        }
       

    }
}
