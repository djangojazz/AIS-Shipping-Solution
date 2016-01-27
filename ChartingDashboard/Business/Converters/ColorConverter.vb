Imports System.Globalization

Public Class ColorConverter
  Implements IValueConverter

  Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
    Dim x = value
    Dim shipType = DirectCast(value, ShipType)

    If (shipType = ShipType.Owned) Then
      Return CType(Application.Current.Resources("brush.Foreground.BoatGradientOwned"), LinearGradientBrush)
    ElseIf (shipType = ShipType.Contractor) Then
      Return CType(Application.Current.Resources("brush.Foreground.BoatGradientContractor"), LinearGradientBrush)
    Else
      Return CType(Application.Current.Resources("brush.Foreground.BoatGradientOther"), LinearGradientBrush)
    End If
  End Function

  Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
    Return New NotImplementedException
  End Function
End Class
