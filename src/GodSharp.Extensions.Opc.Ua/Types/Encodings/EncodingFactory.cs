using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GodSharp.Extensions.Opc.Ua.Utilities;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedType.Global

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public interface IEncodingFactory
    {
        void Register<TType, TEncoding>() where TType : IEncoding<TType>, new();
        void Register<TType, TEncoding>(TType encoding) where TType : IEncoding<TType>;
        void Register(Assembly[] assemblies);
        void Register(Type type);
        void RegisterTypeNamespace(params TypeNamespace[] typeNamespaces);
        TypeNamespace GetTypeNamespace(Type type);
        TypeNamespace GetTypeNamespace<T>();
        IEncoding<T> GetEncoding<T>();
    }

    public class EncodingFactory : IEncodingFactory
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public static IEncoding Encoding { get; protected set; }
        public static IEncodingFactory Instance { get; }
        private static Type EncodingBaseType { get; } = typeof(IEncoding<>);
        private static Type EnumerableType { get; } = typeof(IEnumerable<>);
        private static Type ComplexObjectType { get; } = typeof(ComplexObject);
        private static Type ComplexObjectEncodingType { get; } = typeof(ComplexObjectEncoding<>);
        private static Type ComplexObjectArrayEncodingType { get; } = typeof(ComplexObjectArrayEncoding<>);

        private readonly ConcurrentDictionary<Type, dynamic> _encoding;
        private readonly ConcurrentDictionary<Type, TypeNamespace> _namespaces;

        static EncodingFactory()
        {
            Instance = new EncodingFactory();
            Instance.Register(new[] {typeof(IEncodingFactory).Assembly, Assembly.GetEntryAssembly()});
            Encoding = new OpcUaEncoding(Instance);
        }

        public static void RegisterEncoding(IEncoding encoding)
        {
            if (encoding == null) return;
            Encoding = encoding;
        }

        public EncodingFactory()
        {
            _encoding = new ConcurrentDictionary<Type, dynamic>();
            _namespaces = new ConcurrentDictionary<Type, TypeNamespace>();
        }

        public IEncoding<T> GetEncoding<T>()
        {
            var type = typeof(T);
            if (_encoding.TryGetValue(type, out var obj) && obj is IEncoding<T> encoding)
            {
                return encoding;
            }
            
            if (type.IsSubclassOf(ComplexObjectType))
            {
                dynamic tmp = InstanceHelper.Generic(ComplexObjectEncodingType, type);
                _encoding.TryAdd(type, tmp);
                return tmp as IEncoding<T>;
            }

            Type tt = null;
            if (type.IsArray)
            {
                tt = type.GetElementType();
            }
            else if (type.IsGenericType && type.GetInterfaces().Any(x=> x.GetGenericTypeDefinition()== EnumerableType))
            {
                tt = type.GenericTypeArguments?.FirstOrDefault(x=>x.IsSubclassOf(ComplexObjectType));
            }

            if (tt != null)
            { 
                dynamic tmp = InstanceHelper.Generic(ComplexObjectArrayEncodingType, tt);
                _encoding.TryAdd(type, tmp);
            }
            
            if (_encoding.TryGetValue(type, out dynamic obj2) && obj2 is IEncoding<T> encoding2)
            {
                return encoding2;
            }

            throw new NotSupportedException($"the type {typeof(T)} not support.");
        }

        public void Register<TType, TEncoding>() where TType : IEncoding<TType>, new()
        {
            _encoding.TryAdd(typeof(TType), new TType());
        }

        public void Register<TType, TEncoding>(TType encoding) where TType : IEncoding<TType>
        {
            if (encoding == null) return;
            _encoding.TryAdd(typeof(TType), encoding);
        }

        public void Register(Type type)
        {
            if (type.IsAbstract || !type.IsClass || !type.IsPublic) return;
            var implement = type.GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == EncodingBaseType)
                ?.GetGenericArguments()
                .FirstOrDefault();
            if (implement == null || string.IsNullOrWhiteSpace(implement.AssemblyQualifiedName)) return;

            dynamic encoding = InstanceHelper.Instance(type);
            if (encoding == null) return;

            _encoding.TryAdd(implement, encoding);
        }

        public void Register(Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0) return;

            var types = assemblies
                .Where(x=>x!=null)
                .SelectMany(m => m
                    .GetTypes()
                    .Where(x =>
                        x.IsClass &&
                        !x.IsAbstract &&
                        x.GetInterfaces()
                            ?.Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == EncodingBaseType)==true
                    )
                )
                .Distinct()
                .ToArray();

            foreach (var t in types) Register(t);
        }

        public void RegisterTypeNamespace(params TypeNamespace[] typeNamespaces)
        {
            if (typeNamespaces.Length == 0) return;
            foreach (var typeNamespace in typeNamespaces)
            {
                RegisterTypeNamespace(typeNamespace);
            }
        }

        private void RegisterTypeNamespace(TypeNamespace typeNamespace)
        {
            var type = Type.GetType(typeNamespace.Type);
            if(type==null) return;
            _namespaces.TryAdd(type, typeNamespace);
        }

        public TypeNamespace GetTypeNamespace<T>()
        {
            return GetTypeNamespace(typeof(T));
        }

        public TypeNamespace GetTypeNamespace(Type type)
        {
            return _namespaces.TryGetValue(type, out var typeNamespace) ? typeNamespace : null;
        }
    }
}
