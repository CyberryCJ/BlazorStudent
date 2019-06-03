using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorStudent.Server.Model;
using BlazorStudent.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorStudent.Server.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        readonly IStudent _iStudent;

        public StudentController(IStudent istudent)
        {
            _iStudent = istudent;
        }
        [HttpGet("[action]")]
        public  async Task<List<Students>> StudentList()
        {
            var student = await _iStudent.StudentList();
            return student;
        }
    }
}