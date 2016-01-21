Imports System.ComponentModel
Imports System.Timers

Public Class BaseViewModel
  Implements INotifyPropertyChanged

  Private Shared _errorMessage As String = String.Empty
  Public Shared Property ErrorMessage() As String
    Get
      Return _errorMessage
    End Get
    Set
      _errorMessage = Value
      NotifyStaticPropertyChanged(NameOf(ErrorMessage))
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

  Public Shared Sub TimerHelper(duration As Integer, timerDuration As Func(Of Task))
    Dim timer = New Timer(duration)
    AddHandler timer.Elapsed, Sub() timerDuration.Invoke()
    timer.Enabled = True
  End Sub
End Class
