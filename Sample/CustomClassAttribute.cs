using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class CustomClassAttribute : Attribute
    {
        public CustomClassAttribute()
        {
            Console.WriteLine("Construtor CustomClassAttribute\n");
        }

        public CustomClassAttribute(string arg)
        {
            Console.WriteLine("CustomClassAttribute(string arg)\n");
        }
    }
}