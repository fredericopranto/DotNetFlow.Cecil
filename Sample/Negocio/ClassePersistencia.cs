using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Sample
{
    public class ClassePersistencia
    {
        public void CadastrarUsuario(Usuario usr)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand("select * from usuarios where nome = " + usr.Nome);
            cmd.ExecuteNonQuery();
        }

        internal void ListarUsuarios()
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand("select * from usuarios");
            cmd.ExecuteNonQuery();
        }
    }
}
