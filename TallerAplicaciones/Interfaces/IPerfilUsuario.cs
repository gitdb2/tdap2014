using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.interfaces
{
    public interface IPerfilUsuario
    {

         void AltaPerfilUsuario(PerfilUsuario perfil, string loginUsuario);

         void ModificarPerfilUsuario(PerfilUsuario perfil);
        
         void BajaPerfilUsuario(PerfilUsuario perfil);

         List<PerfilUsuario> ListarUsuarios();

    }
}
