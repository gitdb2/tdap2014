using System;
using System.Collections.Generic;
using System.Linq;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorPerfilUsuario : IPerfilUsuario
    {
        #region singleton
        private static ManejadorPerfilUsuario instance = new ManejadorPerfilUsuario();

        private ManejadorPerfilUsuario() { }

        public static ManejadorPerfilUsuario GetInstance()
        {
            return instance;
        }
        #endregion

        public void AltaPerfilUsuario(PerfilUsuario perfil, string loginUsuario)
        {
            using (Persistencia db = new Persistencia())
            {
                Usuario usuario = db.Usuarios.SingleOrDefault(u => u.Login == loginUsuario);
                perfil.Usuario = usuario;
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

        public List<PerfilUsuario> ListarUsuarios(bool incluirInactivos)
        {
            using (Persistencia db = new Persistencia())
            {
                if (incluirInactivos)
                {
                    return db.PerfilesUsuario.ToList();
                }
                else
                {
                    return (db.PerfilesUsuario.Where(p => p.Activo == false)).ToList();
                }
            }
        }

    }
}



