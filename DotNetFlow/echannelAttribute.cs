using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace DotNetFlow
{
    /// <summary>
    /// An explicit exception channel (channel, for short) is an abstract duct through which exceptions 
    /// flow from a raising site to a handling site.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    [MulticastAttributeUsage(PersistMetaData = true)]
    public class echannelAttribute : OnExceptionAspect
    {
        public enum SiteType { HandlingSite, RaisingSite };

        public string _channel;
        public Type _exceptionType;
        public SiteType _siteType;
        public string _siteName;

        public echannelAttribute(string channel, Type exceptionType, string siteName, SiteType siteType = SiteType.RaisingSite)
        {
            _channel = channel;
            _exceptionType = exceptionType;
            _siteType = siteType;
            _siteName = siteName;
        }
    }
}
