Imports System.Runtime.CompilerServices
Imports System.Xml
Imports System.Xml.Serialization

Public Module Serializers
  <Extension>
  Public Function SerializeToXml(Of T)(valueToSerialize As T) As String
    Dim ns = New XmlSerializerNamespaces()
    ns.Add("", "")
    Dim sw As New IO.StringWriter()

    Using writer As XmlWriter = XmlWriter.Create(sw, New XmlWriterSettings() With {.OmitXmlDeclaration = True})
      Dim xmler = New XmlSerializer(valueToSerialize.GetType())
      xmler.Serialize(writer, valueToSerialize, ns)
    End Using

    Return sw.ToString()
  End Function
End Module
