using Opc.Ua;

using System;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class DateTimeEncoding : Encoding<DateTime>
    {
        protected override DateTime OnRead(IDecoder decoder, string name) => decoder.ReadDateTime(name);

        protected override void OnWrite(IEncoder encoder, DateTime field, string name) => encoder.WriteDateTime(name, field);
    }

    public sealed class DateTimeArrayEncoding : Encoding<DateTime[]>
    {
        protected override DateTime[] OnRead(IDecoder decoder, string name) => decoder.ReadDateTimeArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, DateTime[] field, string name) => encoder.WriteDateTimeArray(name, field);
    }
}
