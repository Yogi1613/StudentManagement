using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Model;
using StudentManagement.Service;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Teacher")]
    public class StudentController : ControllerBase
    {
        

        private readonly StudentService _studentService;
        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public IEnumerable<Student> GetAll()
        {
            var AllStudentsData = _studentService.GetAllStudents();
            return AllStudentsData;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var result = _studentService.GetStudentById(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{name:alpha}")]   //or we can also use  "byname/{name}, byemail/{email}
        public IActionResult Get(string name)
        {
            var result = _studentService.GetStudentByName(name);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentService.AddStudent(student);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public IActionResult Update(Student student)
        {
            if (_studentService.GetStudentById(student.Id) == null)
            {
                return NotFound();
            }
            
            _studentService.UpdateStudent(student);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _studentService.GetStudentById(id);
            if (result == null)
            {
                return NotFound();
            }

            _studentService.DeleteStudentById(id);
            return Ok();
        }
        [HttpDelete("{name:alpha}")]
        public IActionResult Delete(string name)
        {
            var result = _studentService.GetStudentByName(name);
            if (result == null)
            {
                return NotFound();
            }

            _studentService.DeleteStudentByName(name);
            return Ok();
        }
    }
}
