Imports System.Globalization

Public Class HeightConverter
  Implements IValueConverter

  Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
    Dim Width = CDbl(value)
    Return Width / -1.5
  End Function

  Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
    Dim width = CDbl(value)
    Return width * -1.5
  End Function
End Class
