using data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using student.Common;
using student.Extensions;
using System.Collections.ObjectModel;
using System.Linq;

namespace student.ViewModels;

public class CourseViewModel : NavigationViewModel
{
    private readonly IDialogHostService dialogHost;
    public CourseViewModel(IContainerProvider provider) : base(provider)
    {
        NewCourse = new Course();
        DeleteCommand = new DelegateCommand<Course>(DeleteCourseAsync);
        UpdateCommand = new DelegateCommand<Course>(UpdateCourseAsync);
        AddCommand = new DelegateCommand(AddCourse);
        dialogHost = provider.Resolve<IDialogHostService>();
    }
    private Course newCourse;

    public Course NewCourse
    {
        get { return newCourse; }
        set { newCourse = value; RaisePropertyChanged(); }
    }


    private ObservableCollection<Course> courses;

    public ObservableCollection<Course> Courses
    {
        get { return courses; }
        set { courses = value; RaisePropertyChanged(); }
    }
    public DelegateCommand<Course> DeleteCommand { get; private set; }
    public DelegateCommand<Course> UpdateCommand { get; private set; }
    public DelegateCommand AddCommand { get; private set; }
    private async void InitData()
    {
        try
        {
            UpdateLoading(true);
            NewCourse.CourseId = null;
            using var context = new stu_infoContext();
            var cous = await context.Courses.AsNoTracking().Where(c => c.IsDeleted == false).ToListAsync();
            Courses = new ObservableCollection<Course>(cous);
        }
        finally { UpdateLoading(false); }
    }
    private async void DeleteCourseAsync(Course course)
    {
        try
        {
            var res = await dialogHost.Question("温馨提示", $"确认删除课程{course.CourseName}?");
            if (res.Result != ButtonResult.OK) return;
            UpdateLoading(true);
            using var context = new stu_infoContext();
            var cou = context.Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
            if (cou != null)
            {
                cou.IsDeleted = !cou.IsDeleted;
                context.SaveChanges();
            }
            SendMessage("操作成功！");
        }
        finally
        {
            UpdateLoading(false);
        }
        InitData();
    }
    private async void UpdateCourseAsync(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.CourseName) || course.CourseCode == null || string.IsNullOrWhiteSpace(course.Teacher))
        {
            SendMessage("非法输入");
            return;
        }
        try
        {
            var res = await dialogHost.Question("温馨提示", $"确认更新课程{course.CourseName}的信息?");
            if (res.Result != ButtonResult.OK) return;
            UpdateLoading(true);
            using var context = new stu_infoContext();
            context.Courses.Update(course);
            context.SaveChanges();
            SendMessage("操作成功！");
        }
        finally
        {
            UpdateLoading(false);
        }
    }
    private async void AddCourse()
    {
        if (string.IsNullOrWhiteSpace(NewCourse.CourseName) || NewCourse.CourseCode == null || string.IsNullOrWhiteSpace(NewCourse.Teacher))
        {
            SendMessage("非法输入");
            return;
        }
        try
        {
            UpdateLoading(true);
            using var context = new stu_infoContext();
            context.Courses.Add(NewCourse);
            context.SaveChanges();
            SendMessage("操作成功");
        }
        catch
        {
            UpdateLoading(false);
            await dialogHost.Warn("警告", "操作失败，课程代码不能重复");
        }
        finally
        {
            UpdateLoading(false);
        }
        InitData();
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        InitData();
    }
}
