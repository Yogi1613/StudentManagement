using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Model.DTOs;
using StudentManagement.Service;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherService _teacherService;
        public TeacherController(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterTeacherDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _teacherService.Register(dto);
            if(result== "Already Exists")
            {
                return BadRequest(new { message = result });
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginTeacherDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var token = _teacherService.Login(dto);

            if(token == "Invalid Credentials")
            {
                return Unauthorized(new {message = token});
            }
            return Ok(new { token });
        }
    }
}
