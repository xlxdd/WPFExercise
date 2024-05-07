using data.Models;

namespace EntityFramework.Models
{
    public partial class Student : EntityBase
    {
        public int? StudentId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        private string? name;

        public string? Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 学号
        /// </summary>
        private int? studentNunber;

        public int? StudentNumber
        {
            get { return studentNunber; }
            set { studentNunber = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 入学日期
        /// </summary>
        private DateTime adminssionDate;

        public DateTime AdmissionDate
        {
            get { return adminssionDate; }
            set { adminssionDate = value; OnPropertyChanged(); }
        }

    }
}
