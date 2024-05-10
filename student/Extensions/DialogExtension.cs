using data.Models;
using Prism.Events;
using Prism.Services.Dialogs;
using student.Common;
using student.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace student.Extensions;

public static class DialogExtension
{
    /// <summary>
    /// 询问窗口
    /// </summary>
    /// <param name="dialogHost">指定的DialogHost会话主机</param>
    /// <param name="title">标题</param>
    /// <param name="content">询问内容</param>
    /// <param name="dialogHostName">会话主机名称(唯一)</param>
    /// <returns></returns>
    public static async Task<IDialogResult> Question(this IDialogHostService dialogHost,
        string title, string content, string dialogHostName = "Root"
        )
    {
        DialogParameters param = new DialogParameters();
        param.Add("Title", title);
        param.Add("Content", content);
        param.Add("dialogHostName", dialogHostName);
        var dialogResult = await dialogHost.ShowDialog("MsgView", param, dialogHostName);
        return dialogResult;
    }
    public static async Task<IDialogResult> Warn(this IDialogHostService dialogHost,
        string title, string content, string dialogHostName = "Root"
        )
    {
        DialogParameters param = new DialogParameters();
        param.Add("Title", title);
        param.Add("Content", content);
        param.Add("dialogHostName", dialogHostName);
        var dialogResult = await dialogHost.ShowDialog("WarnView", param, dialogHostName);
        return dialogResult;
    }
    public static async Task<IDialogResult> Summar(this IDialogHostService dialogHost,
        List<GradeDto> grades,
        byte summaryType,
        string dialogHostName = "Root"
        )
    {
        DialogParameters param = new DialogParameters();
        param.Add("Grades", grades);
        param.Add("SummaryType", summaryType);
        param.Add("dialogHostName", dialogHostName);
        return await dialogHost.ShowDialog("SummaryView", param, dialogHostName);
    }
    public static void UpdateLoading(this IEventAggregator aggregator, UpdateModel model)
    {
        aggregator.GetEvent<UpdateLoadingEvent>().Publish(model);
    }
    public static void Register(this IEventAggregator aggregator, Action<UpdateModel> model)
    {
        aggregator.GetEvent<UpdateLoadingEvent>().Subscribe(model);
    }
    public static void RegisterMessage(this IEventAggregator aggregator,
        Action<MessageModel> action, string filterName = "Main")
    {
        aggregator.GetEvent<MessageEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
    }
    public static void SendMessage(this IEventAggregator aggregator, string message, string filterName = "Main")
    {
        aggregator.GetEvent<MessageEvent>().Publish(new MessageModel()
        {
            Filter = filterName,
            Message = message,
        });
    }
}
