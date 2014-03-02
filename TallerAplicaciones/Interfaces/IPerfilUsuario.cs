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

        void ModificarPerfilUsuario(PerfilUsuario perfilModificado);
        
        void BajaPerfilUsuario(int idPerfil);

        PerfilUsuario ObtenerPerfil(int idPerfil);

        List<PerfilUsuario> ListarUsuarios(bool incluirInactivos);

        Usuario ObtenerUsuario(int usuarioId);

    }
}
