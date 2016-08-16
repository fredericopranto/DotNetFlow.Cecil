using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    public class ClasseAdaptador
    {
        public void CadastrarUsuario(Usuario usr)
        {
            ClasseNegocio neg = new ClasseNegocio();
            neg.CadastrarUsuario(usr);
        }

        public void ListarUsuarios()
        {
            ClasseNegocio neg = new ClasseNegocio();
            neg.ListarUsuarios();
        }
    }
}
