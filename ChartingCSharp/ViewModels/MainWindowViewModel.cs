namespace ChartingCSharp.ViewModels
{
  using System;
  using System.ComponentModel;
  using System.Threading.Tasks;
  using System.Timers;
  using System.Windows.Input;

  public class MainWindowViewModel : INotifyPropertyChanged
  {
    private string _address;
    private string _locationName;
    private string _locationAddress;
    private string _latitude;
    private string _longitude;

    #region Public Properties
    public string Address
    {
      get { return _address; }
      set
      {
        _address = value;
        OnPropertyChanged("Address");
      }
    }
    public string LocationName
    {
      get { return _locationName; }
      set
      {
        _locationName = value;
        OnPropertyChanged("LocationName");
      }
    }

    public string LocationAddress
    {
      get { return _locationAddress; }
      set
      {
        _locationAddress = value;
        OnPropertyChanged("LocationAddress");
      }
    }

    public string Latitude
    {
      get { return _latitude; }
      set
      {
        _latitude = value;
        OnPropertyChanged("Latitude");
      }
    }

    public string Longitude
    {
      get { return _longitude; }
      set
      {
        _longitude = value;
        OnPropertyChanged("Longitude");
      }
    }
    #endregion

    public ICommand GeocodeAddressCommand { get; private set; }

    public MainWindowViewModel()
    {
      Address = "7560 SW Lara St., Portland OR";
      TimerSetupWithRefresh(5000);
      LocationName = "Here I am";
    }

    private void TimerSetupWithRefresh(int refreshDuration)
    {
      Timer timer = new Timer(refreshDuration);
      timer.Elapsed += async (sender, e) => await RefreshShips();
      timer.Enabled = true;
    }

    private async Task RefreshShips()
    {
      LocationName = "Now it is: " + DateTime.Now;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
