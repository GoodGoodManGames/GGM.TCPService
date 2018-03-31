using System.Threading.Tasks;
using SuperSocket.SocketBase;

namespace GGM.TCPService.Router
{
    public interface IRouter<TSession, TPacketInfo> 
                where TPacketInfo : PacketInfo 
                where TSession : AppSession<TSession, TPacketInfo>, new()
    {
        void RegisterController(object controller);
        Task Route(TPacketInfo packetInfo, AppSession<TSession, TPacketInfo> appSession);
    }
}