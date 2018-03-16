using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using SuperSocket.SocketBase;

namespace GGM.TCPService.Router
{
    using Attribute;

    internal class DefaultRouter : IRouter
    {
        private readonly Dictionary<int, Func<AppSession, PacketInfo, Task>> _routeMap =
            new Dictionary<int, Func<AppSession, PacketInfo, Task>>();

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
                    Delegate.CreateDelegate(typeof(Func<AppSession, PacketInfo, Task>), controller, methodInfo.Name) as Func<AppSession, PacketInfo, Task>;
            }
        }

        public Task Route(PacketInfo packetInfo, AppSession appSession)
        {
            Func<AppSession, PacketInfo, Task> process;
            if (!_routeMap.TryGetValue(packetInfo.Route, out process))
                throw new Exception("Not registered route.");

            return process(appSession, packetInfo);
        }
    }
}