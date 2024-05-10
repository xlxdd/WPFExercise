using data.Models;
using LiveCharts;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using student.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace student.ViewModels.Dialogs;

public class SummaryViewModel : BindableBase, IDialogHostAware
{
    public SummaryViewModel()
    {
        CancelCommand = new DelegateCommand(Cancel);
    }
    public ObservableCollection<GradeDto> Model { get; set; }
    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public DelegateCommand PrintCommand { get; set; }
    public Func<ChartPoint, string> PointLabel { get; set; }
    private string name;

    public string Name
    {
        get { return name; }
        set { name = value; RaisePropertyChanged(); }
    }
    private string sum;

    public string Sum
    {
        get { return sum; }
        set { sum = value; RaisePropertyChanged(); }
    }
    private string min;

    public string Min
    {
        get { return min; }
        set { min = value; RaisePropertyChanged(); }
    }
    private string max;

    public string Max
    {
        get { return max; }
        set { max = value; RaisePropertyChanged(); }
    }
    private string avg;

    public string Avg
    {
        get { return avg; }
        set { avg = value; RaisePropertyChanged(); }
    }
    private ChartValues<int> fail;

    public ChartValues<int> Fail
    {
        get { return fail; }
        set { fail = value; RaisePropertyChanged(); }
    }
    private ChartValues<int> pass;

    public ChartValues<int> Pass
    {
        get { return pass; }
        set { pass = value; RaisePropertyChanged(); }
    }
    private ChartValues<int> excellent;

    public ChartValues<int> Excellent
    {
        get { return excellent; }
        set { excellent = value; RaisePropertyChanged(); }
    }
    /// <summary>
    /// 取消
    /// </summary>
    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No)); //取消返回NO告诉操作结束
    }
    public void OnDialogOpend(IDialogParameters parameters)
    {
        PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        if (parameters.ContainsKey("Grades") && parameters.ContainsKey("SummaryType"))
        {
            Model = new ObservableCollection<GradeDto>(parameters.GetValue<List<GradeDto>>("Grades"));
            var summaryType = parameters.GetValue<byte>("SummaryType");
            if (summaryType == 0)
            {
                Name = Model[0].CourseName;
            }
            else
            {
                Name = Model[0].StudentName;
            }
            Sum = Model.Count.ToString();
            Min = Model.Min(m => m.Gra.Score).ToString();
            Max = Model.Max(m => m.Gra.Score).ToString();
            Avg = Model.Min(m => m.Gra.Score).ToString();
            Fail = new ChartValues<int>() { Model.Count(m => m.Gra.Score < 60m) };
            Pass = new ChartValues<int>() { Model.Count(m => (m.Gra.Score < 90m && m.Gra.Score >= 60m)) };
            Excellent = new ChartValues<int>() { Model.Count(m => m.Gra.Score >= 90m) };
        }
    }
}
