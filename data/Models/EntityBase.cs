using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace data.Models;

public class EntityBase : INotifyPropertyChanged
{
    private bool isDeleted;

    public bool IsDeleted
    {
        get { return isDeleted; }
        set { isDeleted = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// 实现通知更新
    /// </summary>
    /// <param name="propertyName"></param>
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
