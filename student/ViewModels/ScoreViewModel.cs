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
using System.Collections.ObjectModel;
using System.Linq;

namespace student.ViewModels;

public class ScoreViewModel : NavigationViewModel
{
    private readonly IDialogHostService dialogHost;
    public ScoreViewModel(IContainerProvider provider) : base(provider)
    {
        AddCommand = new DelegateCommand(AddGrade);
        UpdateCommand = new DelegateCommand<GradeDto>(UpdateGradeAsync);
        DeleteCommand = new DelegateCommand<GradeDto>(DeleteGradeAsync);
        dialogHost = provider.Resolve<IDialogHostService>();
    }
    #region bindings
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
    private int? addCourseId;
    public int? AddCourseId
    {
        get { return addCourseId; }
        set { addCourseId = value; RaisePropertyChanged(); }
    }
    private string? grade;
    public string? Grade
    {
        get { return grade; }
        set { grade = value; RaisePropertyChanged(); }
    }
    private string? searchText;

    public string? SearchText
    {
        get { return searchText; }
        set { searchText = value; RaisePropertyChanged(); }
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
    private ObservableCollection<GradeDto> gradeDtos;

    public ObservableCollection<GradeDto> GradeDtos
    {
        get { return gradeDtos; }
        set { gradeDtos = value; RaisePropertyChanged(); }
    }
    #endregion
    #region commands
    public DelegateCommand<GradeDto> DeleteCommand { get; private set; }
    public DelegateCommand<GradeDto> UpdateCommand { get; private set; }
    public DelegateCommand AddCommand { get; private set; }
    public DelegateCommand SearchCommand { get; private set; }
    #endregion
    #region functions
    private async void InitData()
    {
        try
        {
            UpdateLoading(true);
            AddCourseId = null;
            SelectedCourseId = null;
            SelectedStudent = null;
            using var context = new stu_infoContext();
            Students = new ObservableCollection<Student>(await context.Students.AsNoTracking().Where(s => s.IsDeleted == false).ToListAsync());
            Courses = new ObservableCollection<Course>(await context.Courses.AsNoTracking().Where(c => c.IsDeleted == false).ToListAsync());
            var gras = await context.Grades.AsNoTracking().Where(s => s.IsDeleted == false)
                .Join(context.Courses,
                    grade => grade.CourseId, // 选课关系表的课程ID
                    course => course.CourseId, // 课程表的课程ID
                    (grade, course) => new { Grade = grade, CoureCode = course.CourseCode, CourseName = course.CourseName } // 结果选择器
                )
                .Join(context.Students,
                    sc => sc.Grade.StudentId, // 上一步结果的学生ID
                    student => student.StudentId, // 学生表的学生ID
                    (sc, student) => new GradeDto { Gra = sc.Grade, StudentNumber = student.StudentNumber, StudentName = student.Name, CourseCode = sc.CoureCode, CourseName = sc.CourseName } // 结果选择器
                ).ToListAsync();
            GradeDtos = new ObservableCollection<GradeDto>(gras);
        }
        finally { UpdateLoading(false); }
    }
    private async void AddGrade()
    {
        decimal scoreNum;
        if (SelectedStudent == null || AddCourseId == null)
        {
            SendMessage("请选中");
            return;
        }
        //如果输入不符合要求
        if (!(decimal.TryParse(Grade, out scoreNum) && scoreNum >= 0 && scoreNum <= 100 && scoreNum % 0.5m == 0))
        {
            SendMessage("请输入合法数据（0-100，小数支持0.5）");
            return;
        }
        try
        {
            UpdateLoading(true);
            using var context = new stu_infoContext();
            //查询该学生该课程是否选课
            var choosen = await context.Selects.AsNoTracking().AnyAsync(s => s.StudentId == SelectedStudent.StudentId && s.CourseId == AddCourseId && s.IsDeleted == false);
            if (!choosen) { SendMessage("该学生并未选择该课程"); return; }
            //查讯该学生该课程是否已有成绩记录
            var exist = await context.Grades.AsNoTracking().AnyAsync(s => s.StudentId == SelectedStudent.StudentId && s.CourseId == AddCourseId && s.IsDeleted == false);
            if (exist) throw new Exception();
            context.Grades.Add(new Grade { StudentId = SelectedStudent.StudentId, CourseId = AddCourseId, Score = scoreNum, ExamDate = DateTime.Today, IsDeleted = false });
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
    private async void DeleteGradeAsync(GradeDto gra)
    {
        try
        {
            var res = await dialogHost.Question("温馨提示", "确认删除该条成绩信息?");
            if (res.Result != ButtonResult.OK) return;
            UpdateLoading(true);
            using var context = new stu_infoContext();
            var graa = context.Grades.FirstOrDefault(s => s.GradeId == gra.Gra.GradeId);
            if (graa != null)
            {
                graa.IsDeleted = !graa.IsDeleted;
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
    private async void UpdateGradeAsync(GradeDto gra)
    {
        decimal? scoreNum = gra.Gra.Score;
        //如果输入不符合要求
        if (scoreNum == null && scoreNum >= 0 && scoreNum <= 100 && scoreNum % 0.5m == 0)
        {
            SendMessage("请输入合法数据（0-100，小数支持0.5）");
            return;
        }
        try
        {
            var res = await dialogHost.Question("温馨提示", $"确认更新学生{gra.StudentName}的成绩?");
            if (res.Result != ButtonResult.OK) return;
            UpdateLoading(true);
            using var context = new stu_infoContext();
            context.Grades.Update(gra.Gra);
            context.SaveChanges();
            SendMessage("操作成功！");
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
    #endregion
}
