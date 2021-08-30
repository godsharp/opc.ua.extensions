using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class DiagnosticInfoEncoding : Encoding<DiagnosticInfo>
    {
        protected override DiagnosticInfo OnRead(IDecoder decoder, string name) => decoder.ReadDiagnosticInfo(name);

        protected override void OnWrite(IEncoder encoder, DiagnosticInfo field, string name) => encoder.WriteDiagnosticInfo(name, field);
    }

    public sealed class DiagnosticInfoArrayEncoding : Encoding<DiagnosticInfo[]>
    {
        protected override DiagnosticInfo[] OnRead(IDecoder decoder, string name) => decoder.ReadDiagnosticInfoArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, DiagnosticInfo[] field, string name) => encoder.WriteDiagnosticInfoArray(name, field);
    }
}
