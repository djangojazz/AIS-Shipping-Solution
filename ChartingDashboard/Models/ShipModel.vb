Imports Microsoft.Maps.MapControl.WPF

Public Class ShipModel
  Public Property MMSI As Integer
  Public Property ShipName As String
  Public Property ShipType As ShipType
  Public Property Location As Location

  Public ReadOnly Property BoatGradient As LinearGradientBrush
    Get
      Return CType(Application.Current.Resources("brush.Foreground.BoatGradientOwned"), LinearGradientBrush)
    End Get
  End Property


End Class
