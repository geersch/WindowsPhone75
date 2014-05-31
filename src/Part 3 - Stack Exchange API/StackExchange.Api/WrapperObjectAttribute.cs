using System;

namespace StackExchange.Api
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WrapperObjectAttribute : Attribute
    {
        public WrapperObjectAttribute(string wrapperObject)
        {
            this.WrapperObject = wrapperObject;
        }

        public string WrapperObject { get; set; }
    }
}
