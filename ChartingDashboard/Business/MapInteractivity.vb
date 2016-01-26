Imports Microsoft.Maps.MapControl.WPF
Imports System
Imports System.Linq
Imports System.Windows

Public Class MapInteractivity
  Inherits BaseViewModel

  Sub New()
    ErrorMessage = "Bad"
  End Sub

  'May never need
  '#Region "GeocodeResult"
  '  Public Shared ReadOnly GeocodeResultProperty As DependencyProperty = DependencyProperty.RegisterAttached("GeocodeResult",
  '    GetType(GeocodeService.GeocodeResult),
  '    GetType(MapInteractivity),
  '    New UIPropertyMetadata(Nothing, AddressOf OnGeocodeResultChanged))

  '  Public Shared Function GetGeocodeResult(target As Map) As GeocodeService.GeocodeResult
  '    Return DirectCast(target.GetValue(GeocodeResultProperty), GeocodeService.GeocodeResult)
  '  End Function

  '  Public Shared Sub SetGeocodeResult(target As Map, value As GeocodeService.GeocodeResult)
  '    target.SetValue(GeocodeResultProperty, value)
  '  End Sub

  '  Private Shared Sub OnGeocodeResultChanged(o As DependencyObject, e As DependencyPropertyChangedEventArgs)
  '    OnGeocodeResultChanged(DirectCast(o, Map), DirectCast(e.OldValue, GeocodeService.GeocodeResult), DirectCast(e.NewValue, GeocodeService.GeocodeResult))
  '  End Sub

  '  Private Shared Sub OnGeocodeResultChanged(map As Map, oldValue As GeocodeService.GeocodeResult, newValue As GeocodeService.GeocodeResult)
  '    Try
  '      Dim location As Location = newValue.Locations.[Select](Function(x) New Location(x.Latitude, x.Longitude)).First()
  '      map.SetView(location, map.ZoomLevel)
  '    Catch ex As Exception
  '      ErrorMessage = $"Map could not be properly Set as {DateTime.Now}!"
  '    End Try
  '  End Sub
  '#End Region

#Region "LocationRectangle"

  Public Shared ReadOnly LocationRectangleProperty As DependencyProperty = DependencyProperty.RegisterAttached("LocationRectangle",
    GetType(LocationRect),
    GetType(MapInteractivity),
    New UIPropertyMetadata(Nothing, AddressOf OnLocationRectangleChanged))

  Public Shared Function GetLocationRectangle(target As Map) As LocationRect
    Return DirectCast(target.GetValue(LocationRectangleProperty), LocationRect)
  End Function

  Public Shared Sub SetLocationRectangle(target As Map, value As LocationRect)
    target.SetValue(LocationRectangleProperty, value)
  End Sub

  Private Shared Sub OnLocationRectangleChanged(o As DependencyObject, e As DependencyPropertyChangedEventArgs)
    Dim map = DirectCast(o, Map)
    Dim rectangle = DirectCast(e.NewValue, LocationRect)
    Try
      'Throw New Exception
      ErrorMessage = String.Empty
      map.SetView(rectangle)
    Catch ex As Exception
      ErrorMessage = $"Map could not be properly Set as {DateTime.Now}!"
    End Try
  End Sub
#End Region
End Class