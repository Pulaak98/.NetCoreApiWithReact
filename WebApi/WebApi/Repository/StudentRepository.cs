using Microsoft.EntityFrameworkCore;
using WebApi.Database_Context;
using WebApi.Model;

namespace WebApi.Repository
{
    public class StudentRepository
    {
        private readonly WebApiDbContext _context;

        public StudentRepository(WebApiDbContext context)
        {
            _context = context;
        }

        public async Task<Student> GetStudentByStudentIdAsync(string studentId)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task CreateStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }
    }
}
