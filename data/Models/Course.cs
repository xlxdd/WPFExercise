using data.Models;

namespace EntityFramework.Models
{
    public partial class Course : EntityBase
    {
        private int? courseId;

        public int? CourseId
        {
            get { return courseId; }
            set { courseId = value; OnPropertyChanged(); }
        }
        private string? courseName;

        public string? CourseName
        {
            get { return courseName; }
            set { courseName = value; OnPropertyChanged(); }
        }
        private int? courseCode;

        public int? CourseCode
        {
            get { return courseCode; }
            set { courseCode = value; OnPropertyChanged(); }
        }
        private string? teacher;
        public string? Teacher
        {
            get { return teacher; }
            set { teacher = value; OnPropertyChanged(); }
        }
    }
}
