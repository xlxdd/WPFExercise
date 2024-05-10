using Prism.Events;

namespace student.Events;

public class MessageModel
{
    public string Filter { get; set; }
    public string Message { get; set; }
}
public class MessageEvent : PubSubEvent<MessageModel>
{
}
