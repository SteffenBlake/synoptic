using System;

namespace SynopticS
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CommandActionAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}