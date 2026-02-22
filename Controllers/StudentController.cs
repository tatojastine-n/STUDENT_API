using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public StudentController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/student
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        // GET: api/student/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { Message = $"Student with ID {id} was not found." });
            }

            return Ok(student);
        }

        // POST: api/student
        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        // DELETE: api/student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { Message = $"Student with ID {id} does not exist and cannot be deleted." });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Student with ID {id} has been deleted successfully." });
        }
    }
}