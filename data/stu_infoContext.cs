using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace data
{
    public class stu_infoContext : DbContext
    {

        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Select> Selects { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt.UseMySql("host=127.0.0.1;port=3306;database=stu_info;userid=root;password=111111", ServerVersion.Parse("8.0.35-mysql"));
        }
    }
}
