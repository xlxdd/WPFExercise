Console.WriteLine("请依次输入学生成绩，#表示结束");
int count = 0;
var scores = new List<double>();
while (true)
{
    count++;
    string? tmp;
    double score;
    tmp = Console.ReadLine();
    if ("#" == tmp)
    {
        Console.WriteLine($"共录入了{count - 1}个学生成绩");
        Console.WriteLine($"平均分：{scores.Average()}");
        Console.WriteLine($"最高分：{scores.Max()}");
        Console.ReadKey();
        break;
    }
    while (!double.TryParse(tmp, out score))
    {
        Console.WriteLine("非法输入，请重新输入");
        tmp = Console.ReadLine();
    }
    scores.Add(score);
}
