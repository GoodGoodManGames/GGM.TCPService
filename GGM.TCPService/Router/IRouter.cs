using System.Threading.Tasks;
using SuperSocket.SocketBase;

namespace GGM.TCPService.Router
{
    public interface IRouter
    {
        void RegisterController(object controller);
        Task Route(PacketInfo packetInfo, AppSession appSession);
    }
}