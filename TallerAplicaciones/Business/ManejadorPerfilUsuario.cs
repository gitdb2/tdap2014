using System;
using System.Collections.Generic;
using System.Linq;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;
using System.Transactions;

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
                    perfilActual.Activo = perfilModificado.Activo;
                    perfilActual.Usuario.Activo = perfilModificado.Activo;
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

        public List<PerfilUsuario> ListarUsuarios()
        {
            using (var db = new Persistencia())
            {
                return db.PerfilesUsuario.Include("Usuario").ToList();
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

 		public void AltaPerfilUsuario(EjecutivoDeCuenta perfil, string login, List<int> idEmpresas)
        {  
            using (var db = new Persistencia())
            {
                using (var scope = new TransactionScope())
                {
                    Usuario usuario = db.Usuarios.SingleOrDefault(u => u.Login == login);

                    perfil.Usuario = usuario;
                    db.PerfilesUsuario.Add(perfil);

                    if (idEmpresas.Count > 0)
                    {
                        List<EmpresaDistribuidora> empresasList = 
                                    (from x in db.Empresas where idEmpresas.Contains(x.EmpresaDistribuidoraID)
                                      select x).ToList();

                        foreach (var empresaDistribuidora in empresasList)
                        {
                            empresaDistribuidora.Ejecutivo = perfil;
                        }

                    }

                    db.SaveChanges();
                    scope.Complete();
                }          
            }
        }

    }
}



