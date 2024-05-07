Console.WriteLine("欢迎使用任务提醒应用！");
Console.WriteLine();

// 获取任务和截止日期
Console.Write("请输入任务：");
string? task = Console.ReadLine();

Console.Write("请输入截止日期（格式：YYYY-MM-DD）：");
if (DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
{
    // 计算剩余天数
    TimeSpan remainingTime = deadline - DateTime.Today;
    int remainingDays = (int)remainingTime.TotalDays;

    if (remainingDays > 0)
    {
        Console.WriteLine($"你的任务是：{task}");
        Console.WriteLine($"截止日期是：{deadline.ToShortDateString()}");
        Console.WriteLine($"距离截止日期还有 {remainingDays} 天。");
    }
    else if (remainingDays == 0)
    {
        Console.WriteLine($"今天是截止日期！请尽快完成任务：{task}");
    }
    else
    {
        Console.WriteLine($"截止日期已过！请确保已完成任务：{task}");
    }
}
else
{
    Console.WriteLine("无效的日期格式。请使用 YYYY-MM-DD 格式输入截止日期。");
}

Console.WriteLine();
Console.WriteLine("感谢使用任务提醒应用！");
Console.ReadKey();