namespace StudentAPI.Models
{
    public class Course
    {
        public int CourseId { get; set; }  // Unique ID for the course
        public string CourseName { get; set; }  // Name of the course
        public string Instructor { get; set; }  // Instructor name
        public int Credits { get; set; }  // Number of credits the course is worth
        public string Description { get; set; }  // Course description
    }
}