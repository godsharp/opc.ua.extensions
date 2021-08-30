using Opc.Ua;
// ReSharper disable UnusedMember.Global

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public interface IEncoding
    {
        void Write<T>(IEncoder encoder, T field, string name);
        void Read<T>(IDecoder decoder, ref T field, string name);
    }

    public class OpcUaEncoding : IEncoding
    {
        private readonly IEncodingFactory _factory;

        public OpcUaEncoding(IEncodingFactory factory)
        {
            _factory = factory;
        }

        public void Write<T>(IEncoder encoder, T field, string name)
        {
            _factory.GetEncoding<T>().Write(encoder, field, name);
        }

        public void Read<T>(IDecoder decoder, ref T field, string name)
        {
            _factory.GetEncoding<T>().Read(decoder, ref field, name);
        }
    }

    public interface IEncoding<T>
    {
        void Write(IEncoder encoder, T field, string name);
        void Read(IDecoder decoder, ref T field, string name);
    }

    public abstract class Encoding<T> : IEncoding<T>
    {
        public virtual void Write(IEncoder encoder, T field, string name)
        {
            OnWrite(encoder, field, name);
        }

        // ReSharper disable once RedundantAssignment
        public virtual void Read(IDecoder decoder, ref T field, string name)
        {
            field = OnRead(decoder, name);
        }

        protected abstract void OnWrite(IEncoder encoder, T field, string name);
        protected abstract T OnRead(IDecoder decoder, string name);
    }
}
