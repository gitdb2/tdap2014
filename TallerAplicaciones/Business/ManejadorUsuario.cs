using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorUsuario : IPerfilUsuario
    {
        #region singleton
        private static ManejadorUsuario instance = new ManejadorUsuario();

        private ManejadorUsuario() { }

        public static ManejadorUsuario GetInstance()
        {
            return instance;
        }
        #endregion

        public void AltaPerfilUsuario(PerfilUsuario perfil)
        {
            using (Persistencia db = new Persistencia())
            {
                db.PerfilesUsuario.Add(perfil);
                db.SaveChanges();
            }
        }

        public void ModificarPerfilUsuario(PerfilUsuario perfil)
        {
            throw new NotImplementedException();
        }

        public void BajaPerfilUsuario(PerfilUsuario perfil)
        {
            throw new NotImplementedException();
        }

        public List<PerfilUsuario> ListarUsuarios()
        {
            throw new NotImplementedException();
        }

        public Usuario BuscarUsuario(int usuarioId)
        {
            using (Persistencia db = new Persistencia())
            {
                return db.Usuarios.Find(usuarioId);
            }
        }

    }
}
