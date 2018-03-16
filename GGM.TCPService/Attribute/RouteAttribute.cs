using System;

namespace GGM.TCPService.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteAttribute : System.Attribute
    {
        public RouteAttribute(int route)
        {
            Route = route;
        }

        public int Route { get; }
    }
}