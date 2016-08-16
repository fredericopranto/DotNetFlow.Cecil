using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace Sample
{
    public class ClasseNegocio
    {
        public void CadastrarUsuario(Usuario usr)
        {
            ClassePersistencia per = new ClassePersistencia();
            per.CadastrarUsuario(usr);
        }

        public void ListarUsuarios()
        {
            ClassePersistencia per = new ClassePersistencia();
            per.ListarUsuarios();
        }
    }
}