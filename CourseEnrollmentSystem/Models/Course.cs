namespace CourseEnrollmentSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = "";
        public string Instructor { get; set; } = "";
        public int Credits { get; set; }
    }
}
