Imports Microsoft.Maps.MapControl.WPF

Public Class ShipModel
  Public Property MMSI As Integer
  Public Property ShipName As String
  Public Property ShipType As ShipType
  Public Property Location As Location
  Public Property BoatHale As Double
  Public Property ExpectedVolume As Double
  Public Property Catches As List(Of CatchType)
End Class