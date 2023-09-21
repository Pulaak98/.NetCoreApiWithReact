using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Model;
using WebApi.Repository;

namespace WebApi.BLL
{
    public class StudentManager
    {
            private readonly StudentRepository _studentRepository;

            public StudentManager(StudentRepository studentRepository)
            {
                _studentRepository = studentRepository;
            }

            public async Task<RegistrationResponse> RegisterAsync(StudentRegister request)
            {
                

                if (await _studentRepository.GetStudentByStudentIdAsync(request.StudentId) != null)
                {
                    return new RegistrationResponse { Success = false, ErrorMessage = "Username already exists." };
                }

                var student = new Student
                {
                    StudentId = request.StudentId,
                    StudentName=request.StudentName,
                    Password = request.Password 
                };

                await _studentRepository.CreateStudentAsync(student);

                return new RegistrationResponse { Success = true };
            }

            public async Task<LoginResponse> LoginAsync(StudentLogin request)
            {
                var student = await _studentRepository.GetStudentByStudentIdAsync(request.StudentId);

                if (student == null || student.Password != request.Password)
                {
                    return new LoginResponse { Success = false, ErrorMessage = "Invalid username or password." };
                }

                
                string token = GenerateJwtToken(student);

                return new LoginResponse { Success = true, Token = token };
            }

        private string GenerateJwtToken(Student student)
        {
            string secretKey = "1234512345123451234512345123451234512345"; 
            string issuer = "WebApi"; 
            string audience = "Swagger.com";

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, student.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, student.StudentId),
       
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
