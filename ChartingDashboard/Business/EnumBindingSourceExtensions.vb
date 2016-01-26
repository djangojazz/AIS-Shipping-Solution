Imports System.Windows.Markup

Public Class EnumBindingSourceExtension
  Inherits MarkupExtension

  Private _enumType As Type
  Public Property EnumType() As Type
    Get
      Return Me._enumType
    End Get
    Set
      If Value <> Me._enumType Then
        If Value IsNot Nothing Then
          Dim enumType1 As Type = If(Nullable.GetUnderlyingType(Value), Value)

          If Not enumType1.IsEnum Then
            Throw New ArgumentException("Type must be for an Enum.")
          End If
        End If

        Me._enumType = Value
      End If
    End Set
  End Property

  Public Sub New()
  End Sub

  Public Sub New(enumType As Type)
    Me.EnumType = enumType
  End Sub

  Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
    If Me._enumType Is Nothing Then
      Throw New InvalidOperationException("The EnumType must be specified.")
    End If

    Dim actualEnumType As Type = If(Nullable.GetUnderlyingType(Me._enumType), Me._enumType)
    Dim enumValues As Array = [Enum].GetValues(actualEnumType)

    If actualEnumType = Me._enumType Then
      Return enumValues
    End If

    Dim tempArray As Array = Array.CreateInstance(actualEnumType, enumValues.Length + 1)
    enumValues.CopyTo(tempArray, 1)
    Return tempArray
  End Function
End Class
