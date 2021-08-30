using System;
using System.Runtime.InteropServices;

namespace GodSharp.Extensions.Opc.Ua.Utilities
{
    public class StructConverter
    {
        /// <summary>
        /// convert struct to byte array.
        /// </summary>
        /// <param name="t">the type</param>
        /// <typeparam name="T">the type instance</typeparam>
        /// <returns>byte array</returns>
        public static byte[] GetBytes<T>(T t)
        {
            var size = Marshal.SizeOf<T>();
            var bytes = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(t, ptr, true);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);
            return bytes;
        }

        // public static byte[] GetBytes(object obj)
        // {
        //     var size = Marshal.SizeOf(obj);
        //     var bytes = new byte[size];
        //     var ptr = Marshal.AllocHGlobal(size);
        //     Marshal.StructureToPtr(obj, ptr, true);
        //     Marshal.Copy(ptr, bytes, 0, size);
        //     Marshal.FreeHGlobal(ptr);
        //     return bytes;
        // }

        /// <summary>
        /// convert byte array to object
        /// </summary>
        /// <param name="bytes">the byte array</param>
        /// <param name="type">the type</param>
        /// <returns>a object</returns>
        public static object GetStruct(byte[] bytes,Type type)
        {
            if (bytes == null) return null;
            var size = Marshal.SizeOf(type);
            if (size > bytes.Length) return null;
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, ptr, size);
            var obj = Marshal.PtrToStructure(ptr, type);
            Marshal.FreeHGlobal(ptr);
            return obj;
        }

        /// <summary>
        /// convert byte array to struct
        /// </summary>
        /// <param name="bytes">the byte array</param>
        /// <typeparam name="T">the type</typeparam>
        /// <returns>a <seealso cref="T"/></returns>
        public static T GetStruct<T>(byte[] bytes)
        {
            if (bytes == null) return default;
            var type = typeof(T);
            var size = Marshal.SizeOf(type);
            if (size > bytes.Length) return default;
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, ptr, size);
            var t = Marshal.PtrToStructure<T>(ptr);
            Marshal.FreeHGlobal(ptr);
            return t;
        }
    }
}
