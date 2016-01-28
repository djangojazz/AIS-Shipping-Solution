Imports System.Globalization

Public Class ColorConverter
  Implements IValueConverter

  Public Property OwnerColor As Brush = CType(Application.Current.Resources("brush.Foreground.BoatGradientOwned"), LinearGradientBrush)

  Public Property ContractorBrush As Brush = CType(Application.Current.Resources("brush.Foreground.BoatGradientContractor"), LinearGradientBrush)

  Public Property OtherBrush As Brush = CType(Application.Current.Resources("brush.Foreground.BoatGradientOther"), LinearGradientBrush)


  Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
    Dim shipType = DirectCast(value, ShipType)

    Select Case shipType
      Case ShipType.Owned : Return OwnerColor
      Case ShipType.Contractor : Return ContractorBrush
      Case Else : Return OtherBrush
    End Select
  End Function

  Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
    Return New NotImplementedException
  End Function
End Class
