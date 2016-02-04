Imports System.Globalization

Public Class NegatingConverter
  Implements IValueConverter

  Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
    If TypeOf value Is Double Then
      Return -CDbl(value)
    End If
    Return value
  End Function

  Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
    If TypeOf value Is Double Then
      Return +CDbl(value)
    End If
    Return value
  End Function
End Class
