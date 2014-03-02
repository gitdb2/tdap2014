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
            using (var db = new Persistencia())
            {
                var usuario = db.Usuarios.SingleOrDefault(u => u.Login == loginUsuario);
                perfil.Usuario = usuario;
                db.PerfilesUsuario.Add(perfil);
                db.SaveChanges();
            }
        }

        public void ModificarPerfilUsuario(PerfilUsuario perfilModificado)
        {
            using (var db = new Persistencia())
            {
                var perfilActual = db.PerfilesUsuario.Include("Usuario").SingleOrDefault(u => u.PerfilUsuarioID == perfilModificado.PerfilUsuarioID);
                if (perfilActual != null)
                {
                    perfilActual.Nombre = perfilModificado.Nombre;
                    perfilActual.Apellido = perfilModificado.Apellido;
                    perfilActual.Email = perfilModificado.Email;
                }
                db.SaveChanges();
            }
        }

        public void BajaPerfilUsuario(int idPerfil)
        {
            using (var db = new Persistencia())
            {
                var perfilUsuario = db.PerfilesUsuario.Include("Usuario").SingleOrDefault(u => u.PerfilUsuarioID == idPerfil);
                if (perfilUsuario != null)
                {
                    perfilUsuario.Usuario.Activo = false;
                    perfilUsuario.Activo = false;
                }
                db.SaveChanges();
            }
        }

        public List<PerfilUsuario> ListarUsuarios(bool incluirInactivos)
        {
            using (var db = new Persistencia())
            {
                if (incluirInactivos)
                {
                    return db.PerfilesUsuario.Include("Usuario").ToList();
                }
                else
                {
                    return (db.PerfilesUsuario.Include("Usuario").Where(p => p.Activo)).ToList();
                }
            }
        }

        public PerfilUsuario ObtenerPerfil(int idPerfil)
        {
            using (var db = new Persistencia())
            {
                return db.PerfilesUsuario.Include("Usuario").SingleOrDefault(u => u.PerfilUsuarioID == idPerfil);
            }
        }

        public Usuario ObtenerUsuario(int usuarioId)
        {
            using (var db = new Persistencia())
            {
                return db.Usuarios.SingleOrDefault(u => u.UsuarioID == usuarioId);
            }
        }
    }
}



