using SuperSocket.SocketBase.Protocol;

namespace GGM.TCPService
{
    public abstract class PacketInfo : BinaryRequestInfo
    {
        protected PacketInfo(uint router, uint resultCode, byte[] body) : base(null, body)
        {
            Route = router;
            ResultCode = resultCode;
        }

        public uint Route { get; }
        public uint ResultCode { get; }
    }
}