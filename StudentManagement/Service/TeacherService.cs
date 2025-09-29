using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Model;
using StudentManagement.Model.DTOs;
using StudentManagement.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Service
{
    public class TeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<Teacher> _passwordHasher;

        public TeacherService(ITeacherRepository teacherRepository, IConfiguration config, IPasswordHasher<Teacher> passwordHasher)
        {
            _teacherRepository = teacherRepository;
            _config = config;
            _passwordHasher = passwordHasher;
        }
        public string Register(RegisterTeacherDto dto)
        {
            var teacherFromDb = _teacherRepository.GetByEmail(dto.Email);
            if (teacherFromDb != null)
            {
                return "Already Exists";
            }
            Teacher teacher = new Teacher
            {
                Name = dto.Name,
                Email = dto.Email
            };
            teacher.PasswordHash = _passwordHasher.HashPassword(teacher, dto.Password);

            _teacherRepository.Add(teacher);
            _teacherRepository.Save();
            return "Success";
        }

        public string  Login(LoginTeacherDto dto)
        {
            Teacher teacher = _teacherRepository.GetByEmail(dto.Email);
            if (teacher == null)
            {
                return "Invalid Credentials";
            }
            var result = _passwordHasher.VerifyHashedPassword(teacher, teacher.PasswordHash, dto.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                return "Invalid Credentials";
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,teacher.Email),
                new Claim(ClaimTypes.Name, teacher.Name), 
                new Claim("teacherId", teacher.Id.ToString()),
                new Claim(ClaimTypes.Role, "Teacher")

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
