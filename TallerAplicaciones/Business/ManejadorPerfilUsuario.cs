using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public Distribuidor FindDistribuidor(int idDistribuidor)
        {
            using (var db = new Persistencia())
            {
                return db.PerfilesUsuario.OfType<Distribuidor>()
                    .Include(p => p.Empresa)
                    .Include(p => p.Usuario)
                    .SingleOrDefault(p => p.PerfilUsuarioID == idDistribuidor);
               
            }
        }


        public EmpresaDistribuidora GetEmpresaDistribuidoraPerfil(int idPerfil)
        {
            using (var db = new Persistencia())
            {
                var empresa = db.PerfilesUsuario.OfType<Distribuidor>()
                    .Where(p => p.PerfilUsuarioID == idPerfil)
                    .Select(p => p.Empresa)
                    .SingleOrDefault();
                return empresa;
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


        public void UpdateCompany(int idPerfil, int idNewCompany)
        {
            using (var db = new Persistencia())
            {
                var perfil = db.PerfilesUsuario.OfType<Distribuidor>().SingleOrDefault(p => p.PerfilUsuarioID == idPerfil);
                var empresa = ManejadorEmpresaDistribuidora.GetInstance().GetEmpresaDistribuidora(idNewCompany);
                db.Empresas.Attach(empresa);

                if (perfil != null && empresa != null)
                {
                  
                    perfil.Empresa = empresa;
                    db.SaveChanges();
                }
            }
        }

        public EjecutivoDeCuenta FindEjecutivo(int idDistrib)
        {
            using (var db = new Persistencia())
            {
                return db.PerfilesUsuario.OfType<EjecutivoDeCuenta>()
                    .Include(p => p.Usuario)
                    .SingleOrDefault(p => p.PerfilUsuarioID == idDistrib);
               
            }
        }

        public void ModificarPerfilUsuario(EjecutivoDeCuenta perfil, List<int> idEmpresas)
        {
            using (var db = new Persistencia())
            {
               

                EjecutivoDeCuenta dbejec = db.PerfilesUsuario
                                             .OfType<EjecutivoDeCuenta>()
                                             .Single(p => p.PerfilUsuarioID == perfil.PerfilUsuarioID);
                if (dbejec != null)
                {

                    List<EmpresaDistribuidora> empresasDeEjec =
                   db.Empresas
                       .Include(e => e.Ejecutivo)
                       .Where(e => e.Ejecutivo.PerfilUsuarioID == perfil.PerfilUsuarioID)
                       .ToList();
                    foreach (var empresa in empresasDeEjec)
                    {
                        empresa.Ejecutivo = null;
                    }
                List<EmpresaDistribuidora> empresasSelecccionadas =
                    db.Empresas
                        .Include(e => e.Ejecutivo)
                        .Where(e => idEmpresas.Contains(e.EmpresaDistribuidoraID))
                        .ToList();

                foreach (var empresa in empresasSelecccionadas)
                {
                    empresa.Ejecutivo = dbejec;
                }


                dbejec.Nombre = perfil.Nombre;
                dbejec.Apellido = perfil.Apellido;
                dbejec.Email = perfil.Email;
                dbejec.Activo = perfil.Activo;
                }

               

                db.SaveChanges();
                
            }
        }


    }
}



