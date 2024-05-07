using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using student.Extensions;

namespace student.ViewModels;

public class NavigationViewModel : BindableBase, INavigationAware
{
    private readonly IContainerProvider _containerProvider;
    public readonly IEventAggregator _aggregator;
    public NavigationViewModel(IContainerProvider containerProvider)
    {
        _containerProvider = containerProvider;
        _aggregator = _containerProvider.Resolve<IEventAggregator>();
    }
    public virtual bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext)
    {

    }

    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {

    }
    public void UpdateLoading(bool IsOpen)
    {
        _aggregator.UpdateLoading(new Events.UpdateModel() { IsOpen = IsOpen });
    }
}
