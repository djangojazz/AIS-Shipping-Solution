Imports Microsoft.Maps.MapControl.WPF
Imports System
Imports System.Linq
Imports System.Windows

Public Class MapInteractivity
#Region "GeocodeResult2"

  Public Shared ReadOnly GeocodeResultProperty As DependencyProperty = DependencyProperty.RegisterAttached("GeocodeResult",
    GetType(GeocodeService.GeocodeResult),
    GetType(MapInteractivity),
    New UIPropertyMetadata(Nothing, AddressOf OnGeocodeResultChanged))

  Public Shared Function GetGeocodeResult(target As Map) As GeocodeService.GeocodeResult
    Return DirectCast(target.GetValue(GeocodeResultProperty), GeocodeService.GeocodeResult)
  End Function

  Public Shared Sub SetGeocodeResult(target As Map, value As GeocodeService.GeocodeResult)
    target.SetValue(GeocodeResultProperty, value)
  End Sub

  Private Shared Sub OnGeocodeResultChanged(o As DependencyObject, e As DependencyPropertyChangedEventArgs)
    OnGeocodeResultChanged(DirectCast(o, Map), DirectCast(e.OldValue, GeocodeService.GeocodeResult), DirectCast(e.NewValue, GeocodeService.GeocodeResult))
  End Sub

  Private Shared Sub OnGeocodeResultChanged(map As Map, oldValue As GeocodeService.GeocodeResult, newValue As GeocodeService.GeocodeResult)
    Dim location As Location = newValue.Locations.[Select](Function(x) New Location(x.Latitude, x.Longitude)).First()

    map.SetView(location, map.ZoomLevel)
  End Sub

#End Region

#Region "GeocodeResultLayer"
  Public Shared ReadOnly GeocodeResultLayerProperty As DependencyProperty = DependencyProperty.RegisterAttached("GeocodeResultLayer", GetType(MapLayer), GetType(MapInteractivity), New UIPropertyMetadata(Nothing, AddressOf OnGeocodeResultLayerChanged))

  Public Shared Function GetGeocodeResultLayer(target As DependencyObject) As MapLayer
    Return DirectCast(target.GetValue(GeocodeResultLayerProperty), MapLayer)
  End Function

  Public Shared Sub SetGeocodeResultLayer(target As DependencyObject, value As MapLayer)
    target.SetValue(GeocodeResultLayerProperty, value)
  End Sub

  Private Shared Sub OnGeocodeResultLayerChanged(o As DependencyObject, e As DependencyPropertyChangedEventArgs)
    OnGeocodeResultLayerChanged(DirectCast(o, Map), DirectCast(e.OldValue, MapLayer), DirectCast(e.NewValue, MapLayer))
  End Sub

  Private Shared Sub OnGeocodeResultLayerChanged(map As Map, oldValue As MapLayer, newValue As MapLayer)
    map.Children.Add(newValue)
  End Sub
#End Region
End Class