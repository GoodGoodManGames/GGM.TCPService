using SuperSocket.SocketBase.Protocol;

namespace GGM.TCPService
{
    public abstract class PacketInfo : BinaryRequestInfo
    {
        protected PacketInfo(int router, byte[] body) : base(null, body)
        {
            Route = router;
        }

        public int Route { get; }
    }
}