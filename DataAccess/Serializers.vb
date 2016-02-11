Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization

Module Serializers
  <Extension>
  Public Function SerializeToXml(Of T)(valueToSerialize As T) As String
    Dim x As New XmlSerializer(valueToSerialize.GetType())
    Dim ns = New XmlSerializerNamespaces()
    ns.Add("", "")
    Dim sw As New IO.StringWriter()
    x.Serialize(sw, valueToSerialize, ns)

    Return sw.ToString()
  End Function
End Module
