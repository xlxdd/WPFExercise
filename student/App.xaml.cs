using Prism.DryIoc;
using Prism.Ioc;
using student.Common;
using student.ViewModels;
using student.Views;
using System.Windows;

namespace student;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override Window CreateShell()
    {
        return Container.Resolve<MainView>();
    }
    protected override void OnInitialized()
    {
        var service = App.Current.MainWindow.DataContext as IConfigureService;
        if (service != null)
        {
            service.Configure();
        }
        base.OnInitialized();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<IDialogHostService, DialogHostService>();
        containerRegistry.RegisterForNavigation<StudentView, StudentViewModel>();
        containerRegistry.RegisterForNavigation<CourseView, CourseViewModel>();
        containerRegistry.RegisterForNavigation<SelectView, SelectViewModel>();
        containerRegistry.RegisterForNavigation<ScoreView, ScoreViewModel>();
        containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
        containerRegistry.RegisterForNavigation<WarnView, WarnViewModel>();
    }
}
