using data;

using var context = new stu_infoContext();
var stus = context.Students.ToList();
foreach (var stu in stus)
{
    Console.WriteLine(stu);
}
