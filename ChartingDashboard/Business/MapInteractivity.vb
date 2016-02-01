Imports Microsoft.Maps.MapControl.WPF
Imports System
Imports System.Linq
Imports System.Windows

Public Class MapInteractivity
  Inherits BaseViewModel

  Sub New()
    ErrorMessage = "Bad"
  End Sub

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