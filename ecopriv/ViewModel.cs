using System.ComponentModel;
using System.Collections.ObjectModel;
using ecopriv.Models; // Ensure this namespace is correct and accessible

public class ViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Sum> _numbers;

    public ObservableCollection<Sum> Numbers
    {
        get => _numbers;
        set
        {
            if (_numbers != value)
            {
                _numbers = value;
                OnPropertyChanged(nameof(Numbers));
            }
        }
    }

    public ViewModel()  // Removed the unused parameter 'test'
    {
        Numbers = new ObservableCollection<Sum>
        {
            new Sum("mat", 10000, "kostnad"),
            new Sum("Hushåll", 2000, "kostnad"),
            new Sum("Buss", 1500, "kostnad")
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
