Imports Microsoft.Maps.MapControl.WPF
Imports System
Imports System.Linq
Imports System.Windows

Public Class MapInteractivity
#Region "GeocodeResult"

  'Public Shared ReadOnly GeocodeResultProperty As DependencyProperty = DependencyProperty.RegisterAttached("GeocodeResult",
  '  GetType(GeocodeService.GeocodeResult),
  '  GetType(MapInteractivity),
  '  New UIPropertyMetadata(Nothing, AddressOf OnGeocodeResultChanged))

  'Public Shared Function GetGeocodeResult(target As Map) As GeocodeService.GeocodeResult
  '  Return DirectCast(target.GetValue(GeocodeResultProperty), GeocodeService.GeocodeResult)
  'End Function

  'Public Shared Sub SetGeocodeResult(target As Map, value As GeocodeService.GeocodeResult)
  '  target.SetValue(GeocodeResultProperty, value)
  'End Sub

  'Private Shared Sub OnGeocodeResultChanged(o As DependencyObject, e As DependencyPropertyChangedEventArgs)
  '  OnGeocodeResultChanged(DirectCast(o, Map), DirectCast(e.OldValue, GeocodeService.GeocodeResult), DirectCast(e.NewValue, GeocodeService.GeocodeResult))
  'End Sub

  'Private Shared Sub OnGeocodeResultChanged(map As Map, oldValue As GeocodeService.GeocodeResult, newValue As GeocodeService.GeocodeResult)
  '  Dim location As Location = newValue.Locations.[Select](Function(x) New Location(x.Latitude, x.Longitude)).First()

  '  map.SetView(location, map.ZoomLevel)
  'End Sub
#End Region

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
    OnLocationRectangleChanged(DirectCast(o, Map), DirectCast(e.OldValue, LocationRect), DirectCast(e.NewValue, LocationRect))
  End Sub

  Private Shared Sub OnLocationRectangleChanged(map As Map, oldValue As LocationRect, newValue As LocationRect)
    map.SetView(newValue)
  End Sub
#End Region

End Class