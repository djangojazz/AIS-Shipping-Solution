Public Module ExtensionMethods

  <Runtime.CompilerServices.Extension>
  Public Function ToRadians(val As Double) As Double
    Return (Math.PI / 180) * val
  End Function
End Module
