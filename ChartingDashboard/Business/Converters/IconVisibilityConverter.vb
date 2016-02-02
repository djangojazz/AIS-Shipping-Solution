Imports System.Globalization

Public Class IconVisibilityConverter
  Implements IValueConverter

  Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
    Dim shipCount = CInt(value)
    Return If(shipCount > 1, Visibility.Visible, Visibility.Collapsed)
  End Function

  Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
    Return New NotImplementedException
  End Function
End Class
