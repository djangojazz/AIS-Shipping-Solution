Imports System.ComponentModel

Public Class BaseViewModel
  Implements INotifyPropertyChanged

  Public Shared Event ErrorMessageChanged As EventHandler

  Private Shared _errorMessage As String = String.Empty
  Public Shared Property ErrorMessage() As String
    Get
      Return _errorMessage
    End Get
    Set
      _errorMessage = Value
      RaiseEvent ErrorMessageChanged(Nothing, EventArgs.Empty)
    End Set
  End Property

  Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

  Public Sub OnPropertyChanged(propertyName As String)
    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
  End Sub

  Public Shared Event StaticPropertyChanged As EventHandler(Of PropertyChangedEventArgs)

  Private Shared Sub NotifyStaticPropertyChanged(propertyName As String)
    RaiseEvent StaticPropertyChanged(Nothing, New PropertyChangedEventArgs(propertyName))
  End Sub
End Class
