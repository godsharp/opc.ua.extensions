using Opc.Ua;

using System.Xml;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class XmlElementEncoding : Encoding<XmlElement>
    {
        protected override XmlElement OnRead(IDecoder decoder, string name) => decoder.ReadXmlElement(name);

        protected override void OnWrite(IEncoder encoder, XmlElement field, string name) => encoder.WriteXmlElement(name, field);
    }

    public sealed class XmlElementArrayEncoding : Encoding<XmlElement[]>
    {
        protected override XmlElement[] OnRead(IDecoder decoder, string name) => decoder.ReadXmlElementArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, XmlElement[] field, string name) => encoder.WriteXmlElementArray(name, field);
    }
}
