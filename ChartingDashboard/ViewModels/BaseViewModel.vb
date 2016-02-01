Imports System.ComponentModel
Imports System.Timers

Public Class BaseViewModel

  Private Shared _errorMessage As String = String.Empty
  Public Shared Property ErrorMessage As String
    Get
      Return _errorMessage
    End Get
    Set
      _errorMessage = Value
      NotifyStaticPropertyChanged(NameOf(ErrorMessage))
    End Set
  End Property

  Public Shared Event StaticPropertyChanged As EventHandler(Of PropertyChangedEventArgs)

  Private Shared Sub NotifyStaticPropertyChanged(propertyName As String)
    RaiseEvent StaticPropertyChanged(Nothing, New PropertyChangedEventArgs(propertyName))
  End Sub

  Public Shared Function TimerHelper(duration As Double, timerDuration As Action) As Timer
    Dim timer = New Timer(duration)
    AddHandler timer.Elapsed, Sub() timerDuration.Invoke()
    timer.Enabled = True
    Return timer
  End Function

End Class
