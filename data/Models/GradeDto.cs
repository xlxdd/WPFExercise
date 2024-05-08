using EntityFramework.Models;

namespace data.Models;

public class GradeDto
{
    public Grade? Gra { get; set; }
    public int? StudentNumber { get; set; }
    public string? StudentName { get; set; }
    public int? CourseCode { get; set; }
    public string? CourseName { get; set; }
}
