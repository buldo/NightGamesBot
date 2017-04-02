namespace Buldo.Ngb.Bot.Routing
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    internal class RouteAttribute : Attribute
    {
        public RouteAttribute(string path)
        {
            Path = path.ToLower();
        }

        public string Path { get; }
    }
}
