using Microsoft.Extensions.Primitives;

namespace StudentManagement.Model.DTOs
{
    public class RegisterTeacherDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
