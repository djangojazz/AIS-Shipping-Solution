Imports Microsoft.Maps.MapControl.WPF
Imports System.Windows
Imports System.Windows.Input

Class ChartingDashboard
  Public Sub New()
    InitializeComponent()
    DataContext = New ChartingDashboardViewModel
  End Sub
End Class
