Public Class MainViewModel
  Public Property Text() As String
    Get
      Return m_Text
    End Get
    Set(value As String)
      m_Text = value
    End Set
  End Property
  Private m_Text As String

  Public Sub New()
    Text = "Hello There"
  End Sub
End Class
