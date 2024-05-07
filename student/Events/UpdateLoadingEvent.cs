using Prism.Events;

namespace student.Events;
public class UpdateModel
{
    public bool IsOpen { get; set; }
}

class UpdateLoadingEvent : PubSubEvent<UpdateModel>
{
}
