using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace DotNetFlow
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    [MulticastAttributeUsage(PersistMetaData = true)]
    public class ehandlerAttribute : OnExceptionAspect
    {
        public string _channel;
        public Type _exceptionType;
        public string _handlerName;

        public ehandlerAttribute(string channel, Type exceptionType, string handlerName)
        {
            _channel = channel;
            _exceptionType = exceptionType;
            _handlerName = handlerName;
        }
    }
}