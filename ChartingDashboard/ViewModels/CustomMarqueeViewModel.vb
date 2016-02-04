<NotifyPropertyChanged>
Public Class CustomMarqueeViewModel
  Inherits BaseViewModel

  Public Sub New(secondsToScroll As Integer)
    MarqueeTimeSpan = New Duration(New TimeSpan(0, 0, secondsToScroll))
  End Sub

  Public Property MarqueeText As String
  Public Property MarqueeTimeSpan As Duration
End Class