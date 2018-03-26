using System;
using System.Threading.Tasks;
using DemoService.Controller;
using GGM.Context.Attribute;
using GGM.TCPService;
using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace DemoService.Service
{
    public class DemoService : TCPService<TestSession, TestPacketInfo>
    {
        [AutoWired]
        public DemoService(DemoController controller)
            : base(new DefaultReceiveFilterFactory<ReceiveFilter, TestPacketInfo>(), controller)
        {
        }

        public override Task Boot(string[] arguments)
        {
            Console.WriteLine("Start Server");
            return base.Boot(arguments);
        }
        protected override void OnConnected(TestSession session)
        {
            Console.WriteLine($"{session.SessionID} connected.");
        }

        protected override void OnDisconnected(TestSession session, CloseReason closeReason)
        {
            Console.WriteLine($"{session.SessionID} disconnected. : {closeReason}");
        }
    }

    public class TestSession : AppSession<TestSession, TestPacketInfo>
    {
    }

    public class TestPacketInfo : PacketInfo
    {
        public TestPacketInfo(uint router, uint resultCode, byte[] body) : base(router, resultCode, body)
        {
        }
    }

    public class ReceiveFilter : FixedHeaderReceiveFilter<TestPacketInfo>
    {
        public ReceiveFilter() : base(12)
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return BitConverter.ToInt32(header, offset + 0);
        }

        protected override TestPacketInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return new TestPacketInfo(
                BitConverter.ToUInt32(header.Array, 4)
              , BitConverter.ToUInt32(header.Array, 8)
              , bodyBuffer.CloneRange(offset, length)
            );
        }
    }
}