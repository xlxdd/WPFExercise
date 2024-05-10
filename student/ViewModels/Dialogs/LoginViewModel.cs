using data;
using data.Models;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using student.Common;
using student.Extensions;
using System;

namespace student.ViewModels.Dialogs;

public class LoginViewModel : BindableBase, IDialogAware
{
    public LoginViewModel(IEventAggregator aggregator)
    {
        UserDto = new ResgiterUserDto();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        this.aggregator = aggregator;
    }

    public string Title { get; set; } = "Student";

    public event Action<IDialogResult> RequestClose;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        LoginOut();
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
    }

    #region Login

    private int selectIndex;

    public int SelectIndex
    {
        get { return selectIndex; }
        set { selectIndex = value; RaisePropertyChanged(); }
    }


    public DelegateCommand<string> ExecuteCommand { get; private set; }


    private string userName;

    public string UserName
    {
        get { return userName; }
        set { userName = value; RaisePropertyChanged(); }
    }

    private string passWord;
    private readonly IEventAggregator aggregator;

    public string PassWord
    {
        get { return passWord; }
        set { passWord = value; RaisePropertyChanged(); }
    }

    private void Execute(string obj)
    {
        switch (obj)
        {
            case "Login": Login(); break;
            case "LoginOut": LoginOut(); break;
            case "Resgiter": Resgiter(); break;
            case "ResgiterPage": SelectIndex = 1; break;
            case "Return": SelectIndex = 0; break;
        }
    }

    private ResgiterUserDto userDto;

    public ResgiterUserDto UserDto
    {
        get { return userDto; }
        set { userDto = value; RaisePropertyChanged(); }
    }

    async void Login()
    {
        if (string.IsNullOrWhiteSpace(UserName) ||
            string.IsNullOrWhiteSpace(PassWord))
        {
            return;
        }
        try
        {
            using var context = new stu_infoContext();
            var loginResult = await context.Users.FirstOrDefaultAsync(u => u.Account == UserName && u.PassWord == PassWord);

            if (loginResult != null)
            {
                AppSessions.UserName = loginResult.UserName;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                aggregator.SendMessage("登陆失败", "Login");
            }
        }
        catch
        {
            //登录失败提示...
            aggregator.SendMessage("登陆失败", "Login");
        }
    }

    private async void Resgiter()
    {
        if (string.IsNullOrWhiteSpace(UserDto.Account) ||
            string.IsNullOrWhiteSpace(UserDto.UserName) ||
            string.IsNullOrWhiteSpace(UserDto.PassWord) ||
            string.IsNullOrWhiteSpace(UserDto.NewPassWord))
        {
            aggregator.SendMessage("请输入完整的注册信息！", "Login");
            return;
        }

        if (UserDto.PassWord != UserDto.NewPassWord)
        {
            aggregator.SendMessage("密码不一致,请重新输入！", "Login");
            return;
        }
        try
        {
            using var context = new stu_infoContext();
            await context.Users.AddAsync(new User { UserName = UserDto.UserName, Account = UserDto.Account, PassWord = UserDto.PassWord, IsDeleted = false });
            var res = await context.SaveChangesAsync();
            if (res != 0)
            {
                aggregator.SendMessage("注册成功", "Login");
                //注册成功,返回登录页页面
                SelectIndex = 0;
            }
        }
        catch
        {
            aggregator.SendMessage("注册失败", "Login");
        }
    }

    void LoginOut()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.No));
    }

    #endregion
}
