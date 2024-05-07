using data.Models;

namespace EntityFramework.Models
{
    public partial class Grade : EntityBase
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Score { get; set; }
        public DateTime ExamDate { get; set; }
    }
}
