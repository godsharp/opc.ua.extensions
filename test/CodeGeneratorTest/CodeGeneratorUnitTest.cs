using System;
using System.IO;
using System.Xml;
using CommonTest;
using GodSharp.Extensions.Opc.Ua.CodeGenerator;
using Opc.Ua;
using Xunit;
using Xunit.Abstractions;
// ReSharper disable UseNameOfInsteadOfTypeOf

namespace CodeGeneratorTest
{
    public class CodeGeneratorUnitTest:UnitTestBase
    {
        public CodeGeneratorUnitTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
        
        [Fact]
        public void ComplexObjectCodeGeneratorUnitTest()
        {
            FileCodeGenerator generator = new();
            generator.Execute(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "..\\",
                    "..\\",
                    "..\\",
                    "test",
                    "CodeGeneratorTest"
                )
            );
        }
        
        [Fact]
        public void GenerateObjectTypes()
        {
            var types = @$" public const string Boolean=""{typeof(bool).Name}""; 
 public const string SByte=""{typeof(sbyte).Name}""; 
 public const string Int16=""{typeof(short).Name}""; 
 public const string Int32=""{typeof(int).Name}""; 
 public const string Int64=""{typeof(long).Name}""; 
 public const string Byte=""{typeof(byte).Name}""; 
 public const string UInt16=""{typeof(ushort).Name}""; 
 public const string UInt32=""{typeof(uint).Name}""; 
 public const string UInt64=""{typeof(ulong).Name}""; 
 public const string Float=""{typeof(float).Name}""; 
 public const string Double=""{typeof(double).Name}""; 
 public const string String=""{typeof(string).Name}""; 
 public const string DateTime=""{typeof(DateTime).Name}""; 
 public const string Guid=""{typeof(Guid).Name}""; 
 public const string Uuid=""{typeof(Uuid).Name}""; 
 public const string XmlElement=""{typeof(XmlElement).Name}""; 
 public const string Variant=""{typeof(Variant).Name}""; 
 public const string DataValue=""{typeof(DataValue).Name}""; 
 public const string NodeId=""{typeof(NodeId).Name}""; 
 public const string ExpandedNodeId=""{typeof(ExpandedNodeId).Name}""; 
 public const string ExtensionObject=""{typeof(ExtensionObject).Name}""; 
 public const string QualifiedName=""{typeof(QualifiedName).Name}""; 
 public const string LocalizedText=""{typeof(LocalizedText).Name}""; 
 public const string StatusCode=""{typeof(StatusCode).Name}""; 
 public const string DiagnosticInfo=""{typeof(DiagnosticInfo).Name}""; 
 public const string BooleanArray=""{typeof(bool[]).Name}""; 
 public const string SByteArray=""{typeof(sbyte[]).Name}""; 
 public const string Int16Array=""{typeof(short[]).Name}""; 
 public const string Int32Array=""{typeof(int[]).Name}""; 
 public const string Int64Array=""{typeof(long[]).Name}""; 
 public const string ByteArray=""{typeof(byte[]).Name}""; 
 public const string UInt16Array=""{typeof(ushort[]).Name}""; 
 public const string UInt32Array=""{typeof(uint[]).Name}""; 
 public const string UInt64Array=""{typeof(ulong[]).Name}""; 
 public const string FloatArray=""{typeof(float[]).Name}""; 
 public const string DoubleArray=""{typeof(double[]).Name}""; 
 public const string StringArray=""{typeof(string[]).Name}""; 
 public const string DateTimeArray=""{typeof(DateTime[]).Name}""; 
 public const string GuidArray=""{typeof(Guid[]).Name}"";
 public const string UuidArray=""{typeof(Uuid[]).Name}""; 
 public const string XmlElementArray=""{typeof(XmlElement[]).Name}""; 
 public const string VariantArray=""{typeof(Variant[]).Name}""; 
 public const string DataValueArray=""{typeof(DataValue[]).Name}""; 
 public const string NodeIdArray=""{typeof(NodeId[]).Name}""; 
 public const string ExpandedNodeIdArray=""{typeof(ExpandedNodeId[]).Name}""; 
 public const string ExtensionObjectArray=""{typeof(ExtensionObject[]).Name}""; 
 public const string QualifiedNameArray=""{typeof(QualifiedName[]).Name}""; 
 public const string LocalizedTextArray=""{typeof(LocalizedText[]).Name}""; 
 public const string StatusCodeArray=""{typeof(StatusCode[]).Name}""; 
 public const string DiagnosticInfoArray=""{typeof(DiagnosticInfo[]).Name}""; ";

            Output.WriteLine(types);
        }
    }
}