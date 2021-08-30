using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Opc.Ua;
// ReSharper disable RedundantAssignment

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class ComplexObjectEncoding<T> : IEncoding<T> where T : ComplexObject
    {
        private static Func<T> New = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

        public void Read(IDecoder decoder, ref T field, string name)
        {
            var t = New();
            t.Decode(decoder);
            field = t;
        }

        public void Write(IEncoder encoder, T field, string name)
        {
            field?.Encode(encoder);
        }
    }

    public sealed class ComplexObjectArrayEncoding<T> : IEncoding<T[]> where T : ComplexObject
    {
        private static Func<T> New = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
        
        public void Read(IDecoder decoder, ref T[] field, string name)
        {
            var length = decoder.ReadInt32(null);

            if (length < 0)
            {
                field = null;
                return;
            }
            
            if (decoder.Context.MaxArrayLength > 0 && decoder.Context.MaxArrayLength < length)
            {
                throw ServiceResultException.Create(
                    StatusCodes.BadEncodingLimitsExceeded,
                    "MaxArrayLength {0} < {1}",
                    decoder.Context.MaxArrayLength,
                    length);
            }

            var list = new List<T>();
            for (var i = 0; i < length; i++)
            {
                var t = New();
                t.Decode(decoder);
                list.Add(t);
            }

            field = list.ToArray();
        }

        public void Write(IEncoder encoder, T[] field, string name)
        {
            var length = field?.Length ?? 0;
            encoder.WriteInt32(null, length);
            if (length < 1)
            {
                return;
            }

            foreach (var item in field)
            {
                item?.Encode(encoder);
            }
        }
    }
}