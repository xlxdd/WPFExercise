using Prism.Events;
using Prism.Services.Dialogs;
using student.Common;
using student.Events;
using System;
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
    public static void UpdateLoading(this IEventAggregator aggregator, UpdateModel model)
    {
        aggregator.GetEvent<UpdateLoadingEvent>().Publish(model);
    }
    public static void Register(this IEventAggregator aggregator, Action<UpdateModel> model)
    {
        aggregator.GetEvent<UpdateLoadingEvent>().Subscribe(model);
    }
}
