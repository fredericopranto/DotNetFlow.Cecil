using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1) Chamando metodo da Visao....\n");

            ClasseVisao vis = new ClasseVisao();
            vis.CadastrarUsuario();

            Console.WriteLine("2) Metodo da Visao Executado.\n");

            Console.WriteLine("3) Lendo atributos ClasseVisao.CadastrarUsuario().\n");
            Console.ReadLine();

            var customMethodAttributes = Type.GetType("Sample.ClasseVisao").GetMethod("CadastrarUsuario").GetCustomAttributes(true);

            foreach (var item in customMethodAttributes)
            {
                Console.WriteLine("Lendo parametros cutomizados....\n");
                Console.WriteLine(item.ToString() + "\n");
            }

            Console.WriteLine("-----");
            Console.ReadLine();

        }
    }
}
