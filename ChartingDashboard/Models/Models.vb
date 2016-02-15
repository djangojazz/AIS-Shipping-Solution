Imports DataAccess
Imports Microsoft.Maps.MapControl.WPF

Public Class ShipDb
  Public Property ShipId As Integer
  Public Property MMSI As Integer
  Public Property ShipName As String
  Public Property Latitude As Double
  Public Property Longitude As Double
  Public Property ShipTypeId As Integer
  Public Property ShipVolumeId As Integer
  Public Property BoatHale As Double
  Public Property ExpectedVolume As Double
  Public Property CatchTypeID As Integer
End Class

Public Class ShipGroupingModel
  Public Property Location As Location
  Public Property ShipType As ShipType
  Public Property Ships As IList(Of ShipModel)
End Class

Public Class ShipModel
  Public Property MMSI As Integer
  Public Property ShipName As String
  Public Property ShipType As ShipType
  Public Property Location As Location
  Public Property Volumes As IList(Of ShipVolume)
End Class

Public Class ShipVolume
  Public ReadOnly Property Threshold As Double
    Get
      Return (BoatHale / ExpectedVolume) * 100
    End Get
  End Property

  Public Property BoatHale As Double
  Public Property ExpectedVolume As Double
  Public Property CatchType As CatchType

  Public ReadOnly Property ThresholdColor As Brush
    Get
      Return ReturnColorForThreshold()
    End Get
  End Property

  Private Function ReturnColorForThreshold() As Brush
    If (Threshold >= 90) Then
      Return DirectCast(New BrushConverter().ConvertFrom("#71BC4B"), SolidColorBrush)
    ElseIf (Threshold >= 80) Then
      Return DirectCast(New BrushConverter().ConvertFrom("#03A093"), SolidColorBrush)
    Else
      Return Brushes.Gray
    End If
  End Function
End Class