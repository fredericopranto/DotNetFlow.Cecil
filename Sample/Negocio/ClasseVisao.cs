using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Reflection;
using DotNetFlow;

namespace Sample
{
    public class ClasseVisao
    {
        //[econtext(econtextAttribute.SiteTypeEnum.RaisingSite, "rsite1")]
        //[echannel("eec1", typeof(System.InvalidOperationException), "rsite1")]
        //[ehandler("eec1", typeof(System.InvalidOperationException), "hsite1")]
        public void CadastrarUsuario()
        {
            Usuario usr = new Usuario();
            ClasseAdaptador adp = new ClasseAdaptador();
            //adp.CadastrarUsuario(usr);
        }

        public void ListarUsuarios()
        {
            Usuario usr = new Usuario();
            ClasseAdaptador adp = new ClasseAdaptador();
            //adp.ListarUsuarios();
        }
    }

    public class Usuario
    {
        public string Nome { get; set; }
    }
}