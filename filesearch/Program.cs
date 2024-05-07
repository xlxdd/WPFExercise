internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("欢迎使用文件搜索工具！");
        Console.WriteLine();

        Console.Write("请输入文件名的关键字：");
        string? keyword = Console.ReadLine();

        DriveInfo[] allDrives = DriveInfo.GetDrives();
        var allFiles = new List<string>();
        foreach (DriveInfo d in allDrives)
        {
            if (d.IsReady == true)
            {
                var tmpFiles = EnumerateDirectories(d.Name, "*", SearchOption.AllDirectories);
                allFiles.AddRange(tmpFiles);
            }
        }
        // 获取计算机上的所有文件

        // 搜索包含关键字的文件
        int resultCount = 0;
        foreach (string filePath in allFiles)
        {
            string fileName = Path.GetFileName(filePath);
            if (fileName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"找到匹配的文件：{filePath}:{fileName}");
                resultCount++;
            }
        }

        if (resultCount == 0)
        {
            Console.WriteLine("未找到匹配的文件。");
        }
        else
        {
            Console.WriteLine($"共找到 {resultCount} 个匹配的文件。");
        }

        Console.WriteLine();
        Console.WriteLine("感谢使用文件搜索工具！");
    }
    public static IEnumerable<string> EnumerateDirectories(string parentDirectory, string searchPattern, SearchOption searchOpt)
    {
        try
        {
            var directories = Enumerable.Empty<string>();
            if (searchOpt == SearchOption.AllDirectories)
            {
                directories = Directory.EnumerateDirectories(parentDirectory)
                    .SelectMany(x => EnumerateDirectories(x, searchPattern, searchOpt));
            }
            return directories.Concat(Directory.EnumerateDirectories(parentDirectory, searchPattern));
        }
        catch (UnauthorizedAccessException)
        {
            return Enumerable.Empty<string>();
        }
    }
}