double opNum1 = 0;
double opNum2 = 0;
double res = 0;
string? op = "";
string? tmp = "";
while (true)
{
    Console.Clear();
    Console.WriteLine("#########计算器#########");
    Console.WriteLine("########################");
    Console.WriteLine("加法  --  +");
    Console.WriteLine("减法  --  -");
    Console.WriteLine("乘法  --  *");
    Console.WriteLine("除法  --  /");
    Console.WriteLine("退出  --  q");
    Console.WriteLine("########################");
    Console.WriteLine("请输入操作符：");
    tmp = Console.ReadLine();
    if (tmp == "q") break;
    op = tmp;
    if (op != "+" && op != "-" && op != "*" && op != "/")
    {
        Console.WriteLine("非法输入，请按任意键重试");
        Console.ReadKey();
        continue;
    }
    Console.WriteLine("请输入第一个操作数：");
    tmp = Console.ReadLine();
    if (!Double.TryParse(tmp, out opNum1))
    {
        Console.WriteLine("非法输入，请按任意键重试");
        Console.ReadKey();
        continue;
    }
    Console.WriteLine("请输入第二个操作数：");
    tmp = Console.ReadLine();
    if (!Double.TryParse(tmp, out opNum2))
    {
        Console.WriteLine("非法输入，请按任意键重试");
        Console.ReadKey();
        continue;
    }
    switch (op)
    {
        case "+":
            res = opNum1 + opNum2;
            break;
        case "-":
            res = opNum1 - opNum2;
            break;
        case "*":
            res = opNum1 * opNum2;
            break;
        case "/":
            if (opNum2 == 0)
            {
                //其实除数为0不会报错
                Console.WriteLine("除数不能为0，请按任意键重试");
                Console.ReadKey();
                continue;
            }
            res = opNum1 / opNum2;
            break;
        default: break;

    }
    Console.WriteLine($"{opNum1}{op}{opNum2}={res}");
    Console.WriteLine("按任意键继续");
    Console.ReadKey();
}
