using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.interfaces
{
    public interface IUsuario
    {
         void AltaUsuario(Usuario usuario);
         void ModificarUsuario(Usuario usuario);
         void BajaUsuario(Usuario usuario);

         List<Usuario> ListarUsuarios();
         Usuario BuscarUsuario(int usuarioId);

         bool Login(string login, string password);
    }
}
