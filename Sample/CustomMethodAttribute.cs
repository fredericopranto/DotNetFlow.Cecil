using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class CustomMethodAttribute : Attribute
    {
        public CustomMethodAttribute()
        {
            Console.WriteLine("Construtor CustomClassAttribute\n");
        }

        public CustomMethodAttribute(string arg)
        {
            Console.WriteLine("Construtor CustomMethodAttribute(string arg)\n");
        }
    }
}