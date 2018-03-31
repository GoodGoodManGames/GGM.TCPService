using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using SuperSocket.SocketBase;

namespace GGM.TCPService.Router
{
    using Attribute;

    internal class DefaultRouter<TSession, TPacketInfo> : IRouter<TSession, TPacketInfo>
            where TPacketInfo : PacketInfo
            where TSession : AppSession<TSession, TPacketInfo>, new()
    {
        private readonly Dictionary<uint, Func<AppSession<TSession, TPacketInfo>, PacketInfo, Task>> _routeMap = new Dictionary<uint, Func<AppSession<TSession, TPacketInfo>, PacketInfo, Task>>();

        public void RegisterController(object controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            // TODO : 아직 Routin부분이 미완성입니다. 현재 제작중에 있습니다.
            var methodInfos = controller.GetType().GetMethods();
            foreach (var methodInfo in methodInfos)
            {   
                var routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>();
                if (routeAttribute == null)
                    return;
                _routeMap[routeAttribute.Route] =
                    Delegate.CreateDelegate(typeof(Func<AppSession<TSession, TPacketInfo>, PacketInfo, Task>), controller, methodInfo.Name) as Func<AppSession<TSession, TPacketInfo>, PacketInfo, Task>;
            }
        }
        public Task Route(TPacketInfo packetInfo, AppSession<TSession, TPacketInfo> appSession)
        {
            Func<AppSession<TSession, TPacketInfo>, PacketInfo, Task> process;
            if (!_routeMap.TryGetValue(packetInfo.Route, out process))
                throw new Exception("Not registered route.");

            Console.WriteLine(appSession.SessionID);
            return process(appSession, packetInfo);
        }
    }
}