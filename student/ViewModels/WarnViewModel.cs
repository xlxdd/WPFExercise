using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using student.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace student.ViewModels;

public class WarnViewModel:BindableBase, IDialogHostAware
{
    public WarnViewModel()
    {
        SaveCommand = new DelegateCommand(Save);
    }

    private string title;

    public string Title
    {
        get { return title; }
        set { title = value; RaisePropertyChanged(); }
    }

    private string content;

    public string Content
    {
        get { return content; }
        set { content = value; RaisePropertyChanged(); }
    }
    private void Save()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
        {
            DialogParameters param = new DialogParameters();
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
        }
    }

    public string DialogHostName { get; set; } = "Root";
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public void OnDialogOpend(IDialogParameters parameters)
    {
        if (parameters.ContainsKey("Title"))
            Title = parameters.GetValue<string>("Title");

        if (parameters.ContainsKey("Content"))
            Content = parameters.GetValue<string>("Content");
    }
}
