using data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using student.Common;
using student.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace student.ViewModels;

class StudentViewModel : NavigationViewModel
{
    private readonly IDialogHostService dialogHost;
    public StudentViewModel(IContainerProvider provider) : base(provider)
    {
        NewStudent = new Student() { AdmissionDate = DateTime.Today, IsDeleted = false };
        DeleteCommand = new DelegateCommand<Student>(DeleteStudentAsync);
        UpdateCommand = new DelegateCommand<Student>(UpdateStudentAsync);
        AddCommand = new DelegateCommand(AddStudent);
        dialogHost = provider.Resolve<IDialogHostService>();
    }
    private Student newStudent;

    public Student NewStudent
    {
        get { return newStudent; }
        set { newStudent = value; RaisePropertyChanged(); }
    }


    private ObservableCollection<Student> students;

    public ObservableCollection<Student> Students
    {
        get { return students; }
        set { students = value; RaisePropertyChanged(); }
    }
    public DelegateCommand<Student> DeleteCommand { get; private set; }
    public DelegateCommand<Student> UpdateCommand { get; private set; }
    public DelegateCommand AddCommand { get; private set; }
    private async void InitData()
    {
        UpdateLoading(true);
        NewStudent.StudentId = null;
        using var context = new stu_infoContext();
        var stus = await context.Students.AsNoTracking().Where(s=>s.IsDeleted==false).ToListAsync();
        Students = new ObservableCollection<Student>(stus);
        UpdateLoading(false);
    }
    private async void DeleteStudentAsync(Student student)
    {
        try
        {
            var res = await dialogHost.Question("温馨提示", $"确认删除学生{student.Name}?");
            if (res.Result != ButtonResult.OK) return;
            UpdateLoading(true);
            using var context = new stu_infoContext();
            var stu = context.Students.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (stu != null)
            {
                stu.IsDeleted = !stu.IsDeleted;
                student.IsDeleted = stu.IsDeleted;
                context.SaveChanges();
            }
        }
        finally
        {
            UpdateLoading(false);
        }
        InitData();
    }
    private async void UpdateStudentAsync(Student student)
    {
        try
        {
            var res = await dialogHost.Question("温馨提示", $"确认更新学生{student.Name}的信息?");
            if (res.Result != ButtonResult.OK) return;
            UpdateLoading(true);
            using var context = new stu_infoContext();
            context.Students.Update(student);
            context.SaveChanges();
        }
        finally
        {
            UpdateLoading(false);
        }
    }
    private async void AddStudent()
    {
        try
        {
            UpdateLoading(true);
            using var context = new stu_infoContext();
            context.Students.Add(NewStudent);
            context.SaveChanges();
            InitData();
        }
        catch
        {
            UpdateLoading(false);
            await dialogHost.Warn("警告", "操作失败，学号不能重复");
        }
        finally
        {
            UpdateLoading(false);
        }
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        InitData();
    }
}
