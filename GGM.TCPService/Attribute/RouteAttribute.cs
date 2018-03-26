using System;

namespace GGM.TCPService.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteAttribute : System.Attribute
    {
        public RouteAttribute(uint route)
        {
            Route = route;
        }

        public uint Route { get; }
    }
}