using System.Text.RegularExpressions;

var contact = new Dictionary<string, string>();
bool flag = false;
// 正则表达式模式
string pattern = @"^1[3-9]\d{9}$";
while (!flag)
{
    Console.Clear();
    Console.WriteLine("######通讯录######");
    Console.WriteLine("#################");
    Console.WriteLine("功能选择：");
    Console.WriteLine("1:添加");
    Console.WriteLine("2:查找");
    Console.WriteLine("3:删除");
    Console.WriteLine("4:退出");
    Console.WriteLine("#################");
    //foreach (var key in contact.Keys)
    //{
    //    Console.WriteLine($"{key} : {contact[key]}");
    //}
    string? k = Console.ReadLine();
    int opt;
    while (!int.TryParse(k, out opt))
    {
        Console.WriteLine("非法输入，请重试：");
        k = Console.ReadLine()!.ToString();
    }
    string? name;
    string? phone;
    switch (opt)
    {
        case 1:
            Console.WriteLine("添加联系人：");
            Console.WriteLine("输入名称：");
            name = Console.ReadLine();
            Console.WriteLine("输入电话：");
            phone = Console.ReadLine();
            if (name == null || phone == null)
            {
                Console.WriteLine("非法输入，请重试");
                break;
            }
            if (!Regex.IsMatch(phone, pattern))
            {
                Console.WriteLine("无效的手机号码格式，请重试");
                Console.ReadKey();
                break;
            }
            if (!contact.TryAdd(name, phone))
            {
                Console.WriteLine("添加失败，请重试");
                Console.ReadKey();
                break;
            }
            Console.WriteLine("添加成功！");
            Console.ReadKey();
            break;
        case 2:
            Console.WriteLine("查找联系人：");
            Console.WriteLine("输入名称：");
            name = Console.ReadLine();
            if (name == null)
            {
                Console.WriteLine("非法输入，请重试");
                break;
            }
            if (!contact.ContainsKey(name))
            {
                Console.WriteLine("查无此人！");
                Console.ReadKey();
                break;
            }
            Console.WriteLine($"{name}的电话是：{contact[name]}");
            Console.ReadKey();
            break;
        case 3:
            Console.WriteLine("删除联系人：");
            Console.WriteLine("输入名称：");
            name = Console.ReadLine();
            if (name == null)
            {
                Console.WriteLine("非法输入，请重试");
                break;
            }
            if (!contact.ContainsKey(name))
            {
                Console.WriteLine("查无此人！");
                Console.ReadKey();
                break;
            }
            contact.Remove(name);
            Console.WriteLine("删除成功");
            Console.ReadKey();
            break;
        case 4:
            flag = true;
            break;
        default:
            Console.WriteLine("非法输入，请重试！");
            break;
    }
}