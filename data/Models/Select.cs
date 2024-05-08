using data.Models;

namespace EntityFramework.Models
{
    public partial class Select : EntityBase
    {
        public int? SelectId { get; set; }
        private int? studentId;

        public int? StudentId
        {
            get { return studentId; }
            set { studentId = value; OnPropertyChanged(); }
        }
        private int? courseId;

        public int? CourseId
        {
            get { return courseId; }
            set { courseId = value; OnPropertyChanged(); }
        }
        public DateTime SelectDate { get; set; }
    }
}
