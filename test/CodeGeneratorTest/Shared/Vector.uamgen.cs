//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by MSBuild generator.
//     Source: Vector.cs
// </auto-generated>
//------------------------------------------------------------------------------

using GodSharp.Extensions.Opc.Ua.Types;
using Opc.Ua;
using static GodSharp.Extensions.Opc.Ua.Types.Encodings.EncodingFactory;

namespace CodeGeneratorSourceGeneratorTest
{
	public partial class UaAnsiCServerVector : ComplexObject 
	{
		public override void Encode(IEncoder encoder)
		{
			base.Encode(encoder);
			Encoding.Write(encoder, X, nameof(X));
			Encoding.Write(encoder, Y, nameof(Y));
			Encoding.Write(encoder, Z, nameof(Z));
		}

		public override void Decode(IDecoder decoder)
		{
			base.Decode(decoder);
			Encoding.Read(decoder, ref X, nameof(X));
			Encoding.Read(decoder, ref Y, nameof(Y));
			Encoding.Read(decoder, ref Z, nameof(Z));
		}
	}

	public partial class ProsysVector : ComplexObject 
	{
		public override void Encode(IEncoder encoder)
		{
			base.Encode(encoder);
			Encoding.Write(encoder, X, nameof(X));
			Encoding.Write(encoder, Y, nameof(Y));
			Encoding.Write(encoder, Z, nameof(Z));
		}

		public override void Decode(IDecoder decoder)
		{
			base.Decode(decoder);
			Encoding.Read(decoder, ref X, nameof(X));
			Encoding.Read(decoder, ref Y, nameof(Y));
			Encoding.Read(decoder, ref Z, nameof(Z));
		}
	}
}
