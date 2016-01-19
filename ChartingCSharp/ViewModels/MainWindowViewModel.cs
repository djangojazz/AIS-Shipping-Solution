namespace ChartingCSharp.ViewModels
{
  using System;
  using System.Threading.Tasks;
  using System.Timers;
  using System.Windows.Input;
  using Business;
  using Microsoft.Maps.MapControl.WPF;
  using System.Linq;
  using System.Windows;
  public class MainWindowViewModel : BaseViewModel
  {
    private Visibility _pinVisible;
    private string _address;
    private string _locationAddress;
    private double _latitude;
    private double _longitude;
    private GeocodeService.GeocodeResult _geocodeResult;
    private Location _location;

    #region Public Properties
    public Location Location
    {
      get { return _location; }
      set
      {
        _location = value;
      }
    }

    public Visibility PinVisible
    {
      get { return _pinVisible; }
      set
      {
        _pinVisible = value;
        OnPropertyChanged("PinHidden");
      }
    }
    
    public string Address
    {
      get { return _address; }
      set
      {
        _address = value;
        OnPropertyChanged("Address");
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

    public double Latitude
    {
      get { return _latitude; }
      set
      {
        _latitude = value;
        OnPropertyChanged("Latitude");
      }
    }

    public double Longitude
    {
      get { return _longitude; }
      set
      {
        _longitude = value;
        OnPropertyChanged("Longitude");
      }
    }

    public GeocodeService.GeocodeResult GeocodeResult
    {
      get { return _geocodeResult; }
      set
      {
        _geocodeResult = value;
        OnPropertyChanged("GeocodeResult");
      }
    }
    #endregion

    
    public ICommand GeocodeAddressCommand { get; private set; }

    public MainWindowViewModel()
    {
      //PinVisible = Visibility.Hidden;
      Address = "7560 SW Lara St., Portland OR";
      TimerSetupWithRefresh(2000);
      GeocodeAddressCommand = new DelegateCommand<string>(GeocodeAddress);
      //AddMapLayer();
    }

    private void TimerSetupWithRefresh(int refreshDuration)
    {
      Timer timer = new Timer(refreshDuration);
      timer.Elapsed += async (sender, e) => await RefreshShips();
      timer.Enabled = true;
    }

    private async Task RefreshShips()
    {
      LocationAddress = "Now it is: " + DateTime.Now;
    }

    private void GeocodeAddress(string input)
    {
      using (GeocodeService.GeocodeServiceClient client = new GeocodeService.GeocodeServiceClient("CustomBinding_IGeocodeService"))
      {
        GeocodeService.GeocodeRequest request = new GeocodeService.GeocodeRequest();
        request.Credentials = new Credentials() { ApplicationId = (App.Current.Resources["BingCredentials"] as ApplicationIdCredentialsProvider).ApplicationId };
        request.Query = Address;
        GeocodeResult = client.Geocode(request).Results[0];

        LocationAddress = GeocodeResult.Address.FormattedAddress;
        Latitude = GeocodeResult.Locations[0].Latitude;
        Longitude = GeocodeResult.Locations[0].Longitude;
        Location = new Location(Latitude, Longitude);
        PinVisible = Visibility.Visible;
      }
    }

    private void AddMapLayer()
    {
      Pushpin pin = new Pushpin();
      pin.Location = GeocodeResult.Locations.Select(x => new Location(x.Latitude, x.Longitude)).FirstOrDefault();
      pin.ToolTip = Address;
    }
  }
}
