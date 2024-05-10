using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using student.Common;
using student.Common.Consts;
using student.Common.Models;
using System.Collections.ObjectModel;

namespace student.ViewModels;

class MainViewModel : BindableBase, IConfigureService
{
    private string userName;

    public string UserName
    {
        get { return userName; }
        set { userName = value; RaisePropertyChanged(); }
    }

    private readonly IRegionManager _regionManager;
    private IRegionNavigationJournal _journal;
    public MainViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        MenuBars = new ObservableCollection<MenuBar>();
        NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
        GoBackCommand = new DelegateCommand(() =>
        {
            if (null != _journal && _journal.CanGoBack) _journal.GoBack();
        });
        GoForwardCommand = new DelegateCommand(() =>
        {
            if (null != _journal && _journal.CanGoForward) _journal.GoForward();
        });
    }

    private void Navigate(MenuBar bar)
    {
        if (null == bar || string.IsNullOrWhiteSpace(bar.NameSpace)) { return; }
        _regionManager.Regions[PrismRegionNames.MainViewRegionName].RequestNavigate(bar.NameSpace, back =>
        {
            _journal = back.Context.NavigationService.Journal;
        });
    }

    public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
    public DelegateCommand GoBackCommand { get; private set; }
    public DelegateCommand GoForwardCommand { get; private set; }
    private ObservableCollection<MenuBar> menuBars;

    public ObservableCollection<MenuBar> MenuBars
    {
        get { return menuBars; }
        set { menuBars = value; RaisePropertyChanged(); }
    }
    private void CreateMenu()
    {
        MenuBars.Add(new MenuBar { Icon = "AccountSchool", Title = "学生管理", NameSpace = "StudentView" });
        MenuBars.Add(new MenuBar { Icon = "Book", Title = "课程管理", NameSpace = "CourseView" });
        MenuBars.Add(new MenuBar { Icon = "React", Title = "选课管理", NameSpace = "SelectView" });
        MenuBars.Add(new MenuBar { Icon = "NoteText", Title = "成绩管理", NameSpace = "ScoreView" });
        MenuBars.Add(new MenuBar { Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });
    }

    public void Configure()
    {
        CreateMenu();
        UserName = AppSessions.UserName;
        _regionManager.Regions[PrismRegionNames.MainViewRegionName].RequestNavigate("StudentView");
    }
}
