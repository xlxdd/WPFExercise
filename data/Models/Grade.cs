using data.Models;

namespace EntityFramework.Models
{
    public partial class Grade : EntityBase
    {
        public int? GradeId { get; set; }
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
        private decimal? score;

        public decimal? Score
        {
            get { return score; }
            set { score = value; OnPropertyChanged(); }
        }
        public DateTime? ExamDate { get; set; }
    }
}
