namespace data.Models;

public class User : EntityBase
{
    public int Id { get; set; }

    private string userName;

    public string UserName
    {
        get { return userName; }
        set { userName = value; OnPropertyChanged(); }
    }

    private string account;

    public string Account
    {
        get { return account; }
        set { account = value; OnPropertyChanged(); }
    }

    private string passWord;

    public string PassWord
    {
        get { return passWord; }
        set { passWord = value; OnPropertyChanged(); }
    }
}
