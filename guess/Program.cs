Console.WriteLine("随机生成了一个1-1000的整数，输入你的猜测:");
var rnd = new Random();
int randomInt = rnd.Next(1, 1001); // 生成 1 到 1000 之间的随机整数
//Console.WriteLine($"这个数是{randomInt}");
while (true)
{
    string? tmp = Console.ReadLine();
    int guess;
    while (!int.TryParse(tmp, out guess))
    {
        Console.WriteLine("非法输入，请重试");
        tmp = Console.ReadLine();
    }
    if (guess > randomInt)
    {
        Console.WriteLine("猜大了，请继续。");
    }
    else if (guess < randomInt)
    {
        Console.WriteLine("猜小了，请继续。");
    }
    else
    {
        Console.WriteLine("恭喜你猜对了！");
        Console.ReadKey();
        break;
    }
}