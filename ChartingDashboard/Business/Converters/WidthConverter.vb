﻿Imports System.Globalization

Public Class WidthConverter
  Implements IValueConverter

  Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
    Dim Width = CDbl(value)
    Return Width / -2
  End Function

  Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
    Dim width = CDbl(value)
    Return width * -2
  End Function
End Class
