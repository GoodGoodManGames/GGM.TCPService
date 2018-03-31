using System;
using System.Text;
using System.Threading.Tasks;
using DemoService.Service;
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
        public Task First(AppSession<TestSession, TestPacketInfo> appSession, PacketInfo packetInfo)
        {
            Console.WriteLine($"1 번 라우트 ResultCode {packetInfo.ResultCode}, Body: {Encoding.Default.GetString(packetInfo.Body)}");
            return Task.FromResult(true);
        }

        [Route(2)]
        public Task Second(AppSession<TestSession, TestPacketInfo> appSession, PacketInfo packetInfo)
        {
            Console.WriteLine($"2 번 라우트 ResultCode {packetInfo.ResultCode}, Body: {Encoding.Default.GetString(packetInfo.Body)},");
            var a = Encoding.Default.GetBytes("Message");
            appSession.Send(a, 0, a.Length);
            return Task.FromResult(true);
        }
    }
}