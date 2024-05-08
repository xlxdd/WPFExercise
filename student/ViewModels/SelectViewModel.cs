using data;
using data.Models;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using student.Common;
using student.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace student.ViewModels;

public class SelectViewModel : NavigationViewModel
{
    private readonly IDialogHostService dialogHost;
    public SelectViewModel(IContainerProvider provider) : base(provider)
    {
        DeleteCommand = new DelegateCommand<SelectDto>(DeleteSelectAsync);
        AddCommand = new DelegateCommand(AddSelect);
        dialogHost = provider.Resolve<IDialogHostService>();
    }
    private Student? selectedStudent;
    public Student? SelectedStudent
    {
        get { return selectedStudent; }
        set { selectedStudent = value; RaisePropertyChanged(); }
    }
    private int? selectedCourseId;
    public int? SelectedCourseId
    {
        get { return selectedCourseId; }
        set { selectedCourseId = value; RaisePropertyChanged(); }
    }
    private ObservableCollection<Student> students;
    public ObservableCollection<Student> Students
    {
        get { return students; }
        set { students = value; RaisePropertyChanged(); }
    }
    private ObservableCollection<Course> courses;
    public ObservableCollection<Course> Courses
    {
        get { return courses; }
        set { courses = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<SelectDto> selectDtos;

    public ObservableCollection<SelectDto> SelectDtos
    {
        get { return selectDtos; }
        set { selectDtos = value; RaisePropertyChanged(); }
    }
    public DelegateCommand<SelectDto> DeleteCommand { get; private set; }
    public DelegateCommand AddCommand { get; private set; }
    private async void InitData()
    {
        try
        {
            UpdateLoading(true);
            SelectedCourseId = null;
            SelectedStudent = null;
            using var context = new stu_infoContext();
            Students = new ObservableCollection<Student>(await context.Students.AsNoTracking().Where(s => s.IsDeleted == false).ToListAsync());
            Courses = new ObservableCollection<Course>(await context.Courses.AsNoTracking().Where(c => c.IsDeleted == false).ToListAsync());
            var sels = await context.Selects.AsNoTracking().Where(s => s.IsDeleted == false)
                .Join(context.Courses,
                    select => select.CourseId, // 选课关系表的课程ID
                    course => course.CourseId, // 课程表的课程ID
                    (select, course) => new { Select = select, CoureCode = course.CourseCode, CourseName = course.CourseName } // 结果选择器
                )
                .Join(context.Students,
                    sc => sc.Select.StudentId, // 上一步结果的学生ID
                    student => student.StudentId, // 学生表的学生ID
                    (sc, student) => new SelectDto { Sel = sc.Select, StudentNumber = student.StudentNumber, StudentName = student.Name, CourseCode = sc.CoureCode, CourseName = sc.CourseName } // 结果选择器
                ).ToListAsync();
            SelectDtos = new ObservableCollection<SelectDto>(sels);
        }
        finally { UpdateLoading(false); }
    }
    private async void DeleteSelectAsync(SelectDto sel)
    {
        try
        {
            var res = await dialogHost.Question("温馨提示", "确认删除该条选课信息?");
            if (res.Result != ButtonResult.OK) return;
            UpdateLoading(true);
            using var context = new stu_infoContext();
            var sell = context.Selects.FirstOrDefault(s => s.SelectId == sel.Sel.SelectId);
            if (sell != null)
            {
                sell.IsDeleted = !sell.IsDeleted;
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
    private async void AddSelect()
    {
        if (SelectedStudent == null || SelectedCourseId == null)
        {
            SendMessage("请选中");
            return;
        }
        try
        {
            UpdateLoading(true);
            using var context = new stu_infoContext();
            var exist = await context.Selects.AsNoTracking().AnyAsync(s => s.StudentId == SelectedStudent.StudentId && s.CourseId == SelectedCourseId && s.IsDeleted == false);
            if (exist) throw new Exception();
            context.Selects.Add(new Select { StudentId = SelectedStudent.StudentId, CourseId = SelectedCourseId, SelectDate = DateTime.Today, IsDeleted = false });
            context.SaveChanges();
            SendMessage("操作成功！");
            InitData();
        }
        catch
        {
            UpdateLoading(false);
            await dialogHost.Warn("警告", "操作失败");
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
