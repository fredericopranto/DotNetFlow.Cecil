//#define FLOWERROR

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Reflection;
using System.IO;
using Mono.Cecil.Cil;

namespace DotNetFlow
{
    static class Program
    {
        static void Main(string[] args)
        {
            TypeDefinition ControllerExceptionDefinition;
            ArrayList ListOfExceptionDefinitionMethods;

            try
            {

                //DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();

                //assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + @"\..\..\..\Sample\bin\Debug";);
                //var readerParameters = new ReaderParameters { AssemblyResolver = assemblyResolver };

                //AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly("Sample.exe", readerParameters);

                //foreach (var moduleDefinition in assembly.Modules)
                //{
                //    foreach (var type in moduleDefinition.GetTypes())
                //    {
                //        foreach (var method in type.Methods)
                //        {
                //            if (!HasAttribute("MyCustomAttribute", method.CustomAttributes))
                //            {
                //              ILProcessor ilProcessor = method.Body.GetILProcessor();
                //              ilProcessor.InsertBefore(method.Body.Instructions.First(), ilProcessor.Create(OpCodes.Call, threadCheckerMethod));
                //            }
                //        }
                //    }
                //}

                Console.WriteLine("Iniciando a Introspecção...\n");

                AssemblyDefinition assembly = GetAssembly();

                ValidateExceptionDefinition(assembly, out ControllerExceptionDefinition);

                PopulateDefinitionMethods(ref ControllerExceptionDefinition, out ListOfExceptionDefinitionMethods);

                // Busca por cada exception annotatios e adicionar ao módulo no assembly
                // Ao final gera uma nova assembly no formado "[assembly_name]_aop.exe" no mesmo diretório so [assembly_name].exe original
                
                foreach (var ExceptionMethodDefinition in ListOfExceptionDefinitionMethods)
                {
                    var classFullName = ExceptionMethodDefinition.ToString().Substring(0, ExceptionMethodDefinition.ToString().LastIndexOf('.'));

                    foreach (var module in assembly.Modules)
                    {
                        foreach (var type in module.Types)
                        {
                            if (type.FullName.ToUpper().Equals(classFullName.ToUpper()))
                            {
                                if (type.HasMethods)
                                {
                                    foreach (var method in type.Methods)
                                    {
                                        foreach (string searchMethod in GetMethodsFromClass(type.FullName, ListOfExceptionDefinitionMethods))
                                        {
                                            if (method.Name.ToUpper().Equals(searchMethod.ToUpper()))
                                            {
                                                MethodReference attributeConstructor = module.Import(typeof(econtextAttribute).GetConstructors()[0]);
                                                method.CustomAttributes.Add(
                                                    new CustomAttribute(attributeConstructor)
                                                    {
                                                        ConstructorArguments =
                                                {
                                                    new CustomAttributeArgument(attributeConstructor.Parameters[0].ParameterType, econtextAttribute.SiteTypeEnum.RaisingSite),
                                                    new CustomAttributeArgument(attributeConstructor.Parameters[1].ParameterType, "rsite1")
                                                }
                                                    });

                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

//                if (false) //TODO: Verificar o lançamento de erros de execeção
//                {
//#if (FLOWERROR)
//#error ParametersError.Error.TYPE_DEFINITION_NOT_FOUND_IN_ASSEMBLY.ToString()
//#endif
//                    throw new Exception(ParametersError.Error.TYPE_DEFINITION_NOT_FOUND_IN_ASSEMBLY.ToString());
//                }

                //TODO: Recuperar do app.config
                string Locator = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + @"\..\..\..\Sample\bin\Debug\Sample.exe";
                assembly.Write(Locator.Replace(".exe", "_aop.exe"));
                Console.WriteLine("Assembly gerado.\n");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static AssemblyDefinition GetAssembly()
        {
            // Identificando o caminho da aplicação que vai ser injetado as exception annotations
            //TODO: Recuperar do app.config
            string Locator = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + @"\..\..\..\Sample\bin\Debug\Sample.exe";
            //string Locator = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + @"\..\..\..\Projetos.SmartIRC4Net\bin\Debug\Projetos.SmartIRC4Net.dll";
            return AssemblyDefinition.ReadAssembly(Locator);
        }

        private static void PopulateDefinitionMethods(ref TypeDefinition ControllerExceptionDefinition, out ArrayList ExceptionDefinitionMethods)
        {
            ExceptionDefinitionMethods = new ArrayList();
            var ExceptionDefinitionDirtMethods = ControllerExceptionDefinition.Fields.ToArray();

            foreach (var item in ExceptionDefinitionDirtMethods)
                ExceptionDefinitionMethods.Add(Between(item.FullName, "<", ">").Replace('_', '.'));
        }

        private static void ValidateExceptionDefinition(AssemblyDefinition assembly, out TypeDefinition ControllerExceptionDefinition)
        {
            try
            {
                ControllerExceptionDefinition = assembly.Modules[0].Types.ToArray().First(t => t.FullName.Equals("DotNetFlow.ControllerExceptionDefinition"));
            }
            catch (Exception)
            {
                throw new Exception(ParametersError.Error.CONTROLLER_EXCEPCTION_DEFINITION_CLASS_NOT_FOUND.ToString());
            }
        }

        /// <summary>
        /// Get string value between [first] a and [last] b.
        /// </summary>
        public static string Between(string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        /// <summary>
        /// Get string value between [first] a and [last] b.
        /// </summary>
        public static ArrayList GetMethodsFromClass(string classFullName, ArrayList list)
        {
            ArrayList retorno = new ArrayList();
            foreach (string item in list)
                if (item.Contains(classFullName))
                    if (item.ToString().Substring(item.ToString().LastIndexOf('.') + 1).Equals(item.ToString().Substring(item.ToString().LastIndexOf('.'))))
                        retorno.Add(".ctor");
                    else
                        retorno.Add(item.ToString().Substring(item.ToString().LastIndexOf('.') + 1));
            return retorno;
        }

        private static bool HasAttribute(string attributeName, IEnumerable<CustomAttribute> customAttributes)
        {
            return GetAttributeByName(attributeName, customAttributes) != null;
        }

        private static CustomAttribute GetAttributeByName(string attributeName, IEnumerable<CustomAttribute> customAttributes)
        {
            foreach (var attribute in customAttributes)
                if (attribute.AttributeType.FullName == attributeName)
                    return attribute;
            return null;
        }

        #region "Deprecated"

        //AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(Locator);
        //ModuleDefinition module = assembly.MainModule;
        //TypeDefinition type = assembly.MainModule.GetType("Sample.ClasseVisao");
        //MethodDefinition targetMethod = module.GetMemberReferences().me type.Methods.OfType<MethodDefinition>()
        //    .Where(m => m.Name == "CadastrarUsuario" && m.Parameters.Count == 0).Single();

        //MethodReference attrConstructor = module.Import(typeof(eTesteAttribute).GetConstructor(new[] { typeof(string) }));
        //targetMethod.CustomAttributes.Add(new CustomAttribute(attrConstructor)
        //    {
        //        ConstructorArguments =
        //        {
        //            new CustomAttributeArgument(module.TypeSystem.String, "opa")
        //        }
        //    });

        //MethodReference attributeConstructor = module.Import(typeof(econtextAttribute).GetConstructor(new[] { typeof(DotNetFlow.econtextAttribute.SiteType), typeof(string) }));
        //attribute.ConstructorArguments.Add(new CustomAttributeArgument(typeof(DotNetFlow.econtextAttribute.SiteType), DotNetFlow.econtextAttribute.SiteType.RaisingSite));
        //attribute.ConstructorArguments.Add(new CustomAttributeArgument(module.TypeSystem.String, "rsite1"));

        //module.Write(Locator);



        //void Initialization()
        //{
        //    //var assembly = AssemblyDefinition.ReadAssembly(Assembly.GetExecutingAssembly().Location + "/../Library/ScriptAssemblies/Assembly-CSharp.dll");

        //    //foreach (var module in assembly.Modules)
        //    //{
        //    //    foreach (var type in module.Types)
        //    //    {
        //    //        if (type.HasMethods)
        //    //        {
        //    //            MethodReference injectTestMethod = type.Methods[0];

        //    //            foreach (var method in type.Methods)
        //    //            {
        //    //                if (method.Name == "CadastrarUsuario")
        //    //                {
        //    //                    injectTestMethod = method;
        //    //                }
        //    //            }

        //    //            foreach (var method in type.Methods)
        //    //            {
        //    //                if (HasAttribute("NRTime", method.CustomAttributes))
        //    //                {
        //    //                    ILProcessor ilProcessor = method.Body.GetILProcessor();
        //    //                    ilProcessor.InsertBefore(method.Body.Instructions[0], ilProcessor.Create(OpCodes.Call, injectTestMethod));
        //    //                    ilProcessor.InsertBefore(method.Body.Instructions[0], ilProcessor.Create(OpCodes.Ldarg_0));
        //    //                }
        //    //            }
        //    //        }
        //    //    }

        //    //    module.Write(Assembly.GetExecutingAssembly().Location + "/../Library/ScriptAssemblies/Assembly-CSharp.dll");
        //    //}


        //    //AssemblyDefinition opa = AssemblyDefinition.ReadAssembly(Assembly.GetExecutingAssembly().Location);
        //    //TypeDefinition type = opa.MainModule.GetType("Sample.ClasseVisao");
        //    //MethodDefinition targetMethod = type.Methods.OfType<MethodDefinition>()
        //    //    .Where(m => m.Name == "CadastrarUsuario" && m.Parameters.Count == 0).Single();

        //    //ModuleDefinition module = ModuleDefinition.ReadModule(Assembly.GetExecutingAssembly().Location);
        //    //MethodReference attributeConstructor = module.Import(typeof(econtextAttribute).GetConstructor(new [] { typeof(DotNetFlow.econtextAttribute.SiteType), typeof(string) }));
        //    //CustomAttribute attribute = new CustomAttribute(attributeConstructor);
        //    ////attribute.ConstructorArguments.Add(new CustomAttributeArgument(typeof(DotNetFlow.econtextAttribute.SiteType), DotNetFlow.econtextAttribute.SiteType.RaisingSite));
        //    //attribute.ConstructorArguments.Add(new CustomAttributeArgument(module.TypeSystem.String, "rsite1"));

        //    //targetMethod.CustomAttributes.Add(attribute);

        //    //module.Write(Assembly.GetExecutingAssembly().Location + "/opa.dll");
        //}

        //private static bool HasAttribute(string attributeName, IEnumerable<CustomAttribute> customAttributes)
        //{
        //    return GetAttributeByName(attributeName, customAttributes) != null;
        //}

        //private static CustomAttribute GetAttributeByName(string attributeName, IEnumerable<CustomAttribute> customAttributes)
        //{
        //    foreach (var attribute in customAttributes)
        //        if (attribute.AttributeType.FullName == attributeName)
        //            return attribute;
        //    return null;
        //}

        //[econtext(econtextAttribute.SiteType.RaisingSite, "rsite1")]
        //[echannel("eec1", typeof(SqlException), "rsite1")]
        //[ehandler("eec1", typeof(SqlException), "hsite1")]
        //[echannel("eec2", typeof(IOException), "rsite1")]
        //[ehandler("eec2", typeof(IOException), "hsite2")]
        //[echannel("eec3", typeof(InvalidOperationException), "rsite1")]
        //[ehandler("eec3", typeof(InvalidOperationException), "hsite3")]

        #endregion
    }
}
