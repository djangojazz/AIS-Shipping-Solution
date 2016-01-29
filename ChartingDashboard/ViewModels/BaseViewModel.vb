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

  Private Shared _dimension As Double

  Public Shared Property Dimension As Double
    Get
      Return _dimension
    End Get
    Set(value As Double)
      _dimension = value
      NotifyStaticPropertyChanged(NameOf(Dimension))
    End Set
  End Property

  Private Shared _buffer As Double

  Public Shared Property Buffer As Double
    Get
      Return _buffer
    End Get
    Set(value As Double)
      _buffer = value
      NotifyStaticPropertyChanged(NameOf(Buffer))
    End Set
  End Property

  Public Shared Event StaticPropertyChanged As EventHandler(Of PropertyChangedEventArgs)

  Private Shared Sub NotifyStaticPropertyChanged(propertyName As String)
    RaiseEvent StaticPropertyChanged(Nothing, New PropertyChangedEventArgs(propertyName))
  End Sub

  Public Shared Sub TimerHelper(duration As Integer, timerDuration As Action)
    Dim timer = New Timer(duration)
    AddHandler timer.Elapsed, Sub() timerDuration.Invoke()
    timer.Enabled = True
  End Sub
End Class
