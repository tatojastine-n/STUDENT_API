using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _configuration;

        public EnrollmentController(ApplicationDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetEnrollments()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();

            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent(Enrollment enrollment)
        {

            var course = await _context.Courses.FindAsync(enrollment.CourseId);
            if (course != null)
            {
                var maxEnrollments = int.Parse(_configuration["MaxEnrollmentsPerCourse"]);
                var enrolledCount = await _context.Enrollments
                    .Where(e => e.CourseId == enrollment.CourseId)
                    .CountAsync();

                if (enrolledCount >= maxEnrollments)
                {
                    return BadRequest("This course has reached its maximum enrollment.");
                }
            }

            enrollment.EnrollmentDate = DateTime.Now;  
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnrollments), new { id = enrollment.EnrollmentId }, enrollment);
        }
    }
}