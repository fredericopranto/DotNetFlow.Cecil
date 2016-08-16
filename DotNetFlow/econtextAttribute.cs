using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using System.IO;
using Mono.Cecil;

namespace DotNetFlow
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    [MulticastAttributeUsage(PersistMetaData = true)]
    public class econtextAttribute : OnExceptionAspect
    {
        public enum SiteTypeEnum { HandlingSite, RaisingSite };

        public string SiteName { get; set; }
        public SiteTypeEnum SiteType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteType"></param>
        /// <param name="siteName"></param>
        public econtextAttribute(SiteTypeEnum siteType, string siteName)
        {
            Console.WriteLine("Contrutor econtextAttribute(SiteType siteType, string siteName) \n");
            SiteType = siteType;
            SiteName = siteName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine(">>>> econtextAttribute.OnException executando...\n");

            args.FlowBehavior = FlowBehavior.Continue;
            string msg = string.Format("{0} had an error @ {1}: {2}\n{3}",
            args.Method.Name, DateTime.Now,
            args.Exception.Message, args.Exception.StackTrace);

            object[] customAttributes = args.Method.GetCustomAttributes(true);

            econtextAttribute econtext = (econtextAttribute)customAttributes.Where(m => m.GetType().Equals(typeof(econtextAttribute))).FirstOrDefault();
            List<Object> echannels = customAttributes.Where(m => m.GetType().Equals(typeof(echannelAttribute))).ToList();
            List<Object> ehandlers = customAttributes.Where(m => m.GetType().Equals(typeof(ehandlerAttribute))).ToList();

            if (econtext != null)
            {
                string raisingSite = econtext.SiteName; // Qual o nome do raising site do método que lançou a exceção?

                foreach (echannelAttribute echannel in echannels) // Quais são os canais do raising site?
                {
                    if (echannel._siteName.Equals(raisingSite))
                    {
                        if (echannel._exceptionType.Equals(args.Exception.GetType()))
                        {
                            string channelId = echannel._channel; // Desse canais, qual que vai tratar o tipo de exceção?

                            foreach (ehandlerAttribute ehandler in ehandlers) // Qual o tratador para este tipo de exceção?
                            {
                                if (ehandler._channel.Equals(channelId))    
                                {
                                    string handlerName = ehandler._handlerName;
                                    MethodDefinition method = GetMethodInfoByHandler(handlerName);
                                    InvokeMethod(method.DeclaringType.FullName, method.Name); //Então, execute o tratador.
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static string InvokeMethod(string typeName, string methodName)
        {
            Type calledType = null;
            string Locator = Path.GetDirectoryName(
                            new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + @"\..\..\..\Sample\bin\Debug\Sample.exe";
            var assembly = AssemblyDefinition.ReadAssembly(Locator);

            foreach (var module in assembly.Modules)
            {
                foreach (var type in module.Types)
                {
                    if (type.FullName.Equals(typeName))
                    {
                        calledType = Type.GetType(type.FullName + ", " + assembly.FullName);
                        break;
                    }
                }
            }

            try
            {
                return (String)calledType.InvokeMember(
                                methodName,
                                BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                                null,
                                null,
                                null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>Instruction
        /// 
        /// <param name="handlerName"></param>
        /// <returns></returns>
        private MethodDefinition GetMethodInfoByHandler(string handlerName)
        {
            string Locator = Path.GetDirectoryName(
                            new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + @"\..\..\..\Sample\bin\Debug\Sample.exe";
            var assembly = AssemblyDefinition.ReadAssembly(Locator);

            var controller = assembly.Modules[0].Types.ToArray().First(t => t.FullName.Equals("DotNetFlow.HandlerExceptionLocator"));

            foreach (MethodDefinition item in controller.Methods.ToArray())
            {
                if (item.CustomAttributes[0].ConstructorArguments[1].Value.Equals(handlerName))
                {
                    return item;
                }
            }

            return null;
        }
    }
}