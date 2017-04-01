using System;
using System.Collections.Generic;
using System.Text;

namespace Buldo.Ngb.Bot.Routing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal class RouteAttribute : Attribute
    {
        public RouteAttribute(string path)
        {
            Path = path.ToLower();
        }

        public string Path { get; }
    }
}
