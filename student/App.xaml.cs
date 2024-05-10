using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using student.Common;
using student.ViewModels;
using student.ViewModels.Dialogs;
using student.Views;
using student.Views.Dialogs;
using System;
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
        Current.MainWindow.Hide();
        var dialog = Container.Resolve<IDialogService>();

        dialog.ShowDialog("LoginView", callback =>
        {
            if (callback.Result != ButtonResult.OK)
            {
                Environment.Exit(0);
                return;
            }
            Current.MainWindow.Show();
        });
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
        containerRegistry.RegisterDialog<LoginView, LoginViewModel>();
        containerRegistry.RegisterForNavigation<StudentView, StudentViewModel>();
        containerRegistry.RegisterForNavigation<CourseView, CourseViewModel>();
        containerRegistry.RegisterForNavigation<SelectView, SelectViewModel>();
        containerRegistry.RegisterForNavigation<ScoreView, ScoreViewModel>();
        containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
        containerRegistry.RegisterForNavigation<WarnView, WarnViewModel>();
        containerRegistry.RegisterForNavigation<SummaryView, SummaryViewModel>();
    }
}
