
using System.ComponentModel;

namespace OpcUaTest
{
    [DefaultValue(Unknow)]
    public enum ServerState
    {
        Running,
        Failed,
        NoConfiguration,
        Suspended,
        Shutdown,
        Test,
        CommunicationFault,
        Unknow
    }
}