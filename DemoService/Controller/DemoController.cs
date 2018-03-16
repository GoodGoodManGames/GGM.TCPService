using System;
using System.Text;
using System.Threading.Tasks;
using GGM.Context;
using GGM.Context.Attribute;
using GGM.TCPService;
using GGM.TCPService.Attribute;
using SuperSocket.SocketBase;

namespace DemoService.Controller
{
    [Managed(ManagedType.Singleton)]
    public class DemoController
    {
        [Route(1)]
        public Task First(AppSession appSession, PacketInfo packetInfo)
        {
            Console.WriteLine($"1 번 라우트, Body: {Encoding.Default.GetString(packetInfo.Body)}");
            return Task.FromResult(true);
        }

        [Route(2)]
        public Task Second(AppSession appSession, PacketInfo packetInfo)
        {
            Console.WriteLine($"2 번 라우트, Body: {Encoding.Default.GetString(packetInfo.Body)}");
            return Task.FromResult(true);
        }
    }
}