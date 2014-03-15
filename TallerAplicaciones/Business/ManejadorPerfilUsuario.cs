using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        /// <summary>
        /// retorna una lista de distribuidores, incluyendo la referencia a la empresa a al que pertenences, y 
        /// cuyo ejecutivo de cuenta tiene ide idEjecutivo.
        /// </summary>
        /// <param name="idEjecutivo"></param>
        /// <returns></returns>
        public List<Distribuidor> GetDistribuidoresConEmpresasDeEjecutivo(int idEjecutivo)
        {
            using (var db = new Persistencia())
            {
                try
                {
                    var distribuidores = db.PerfilesUsuario.OfType<Distribuidor>()
                                .Include(d => d.Empresa)
                                .Include(d => d.Usuario)
                               .Where(d => d.Empresa.Ejecutivo.PerfilUsuarioID == idEjecutivo)
                               .OrderBy(d=> d.Empresa.EmpresaDistribuidoraID)
                              .ToList();
                    return distribuidores;
                }
                catch (Exception)
                {
                    
                   return new List<Distribuidor>();
                }
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

        public void AltaPerfilUsuario(Distribuidor perfil, int idEmpresa, string login)
        {
            using (var db = new Persistencia())
            {
                using (var scope = new TransactionScope())
                {
                    Usuario usuario = db.Usuarios.SingleOrDefault(u => u.Login == login);

                    perfil.Usuario = usuario;
                    db.PerfilesUsuario.Add(perfil);

                    var empresa = ManejadorEmpresaDistribuidora.GetInstance().GetEmpresaDistribuidora(idEmpresa);
                    db.Empresas.Attach(empresa);

                    perfil.Empresa = empresa;

                    db.SaveChanges();
                    scope.Complete();
                } 
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

        public PerfilUsuario GetPerfilUsuarioByLogin(string login)
        {
            using (var db = new Persistencia())
            {
                var ret = db.PerfilesUsuario.Include("Usuario").FirstOrDefault(p => p.Usuario.Login == login);
                return ret;
            }
        }

        public Usuario ObtenerUsuario(string login)
        {
            using (var db = new Persistencia())
            {
                return db.Usuarios.FirstOrDefault(u => u.Login == login);
            }
        }

        public Usuario ObtenerUsuarioDistribuidor(string login)
        {
            Usuario res = null;
            using (var db = new Persistencia())
            {
                var dist = db.PerfilesUsuario.OfType<Distribuidor>()
                    .Include(x => x.Usuario)
                    .SingleOrDefault(p => p.Usuario.Login == login);
                if (dist != null)
                {
                    res = dist.Usuario;
                }
            }
            return res;
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

                EjecutivoDeCuenta dbejec = db.PerfilesUsuario.Include("Usuario")
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
                dbejec.Usuario.Activo = perfil.Activo;

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

        public List<Distribuidor> GetDistribuidores()
        {
            using (var db = new Persistencia())
            {
                try
                {
                    var distribuidores = db.PerfilesUsuario.OfType<Distribuidor>()
                                .Include(d => d.Empresa)
                                .Include(d => d.Usuario)
                                .ToList();
                    return distribuidores;
                }
                catch (Exception)
                {

                    return new List<Distribuidor>();
                }
            }
        }

        public List<EjecutivoDeCuenta> GetEjecutivos()
        {
            using (var db = new Persistencia())
            {
                try
                {
                    var distribuidores = db.PerfilesUsuario.OfType<EjecutivoDeCuenta>()
                                .Include(d => d.Usuario)
                                .ToList();
                    return distribuidores;
                }
                catch (Exception)
                {

                    return new List<EjecutivoDeCuenta>();
                }
            }
        }

        public List<Distribuidor> ObtenerDistribuidoresDeEmpresa(int empresaDistribuidoraId)
        {
            return GetDistribuidores().Where(d => d.Empresa.EmpresaDistribuidoraID == empresaDistribuidoraId).ToList();
        }

        public PerfilUsuario ObtenerPerfilUsuarioSegunRol(int rolId)
        {
            switch (rolId)
            {
                case (int) UserRole.Administrador:
                    return new Administrador();
                
                case (int) UserRole.EjecutivoDeCuenta:
                    return new EjecutivoDeCuenta();

                case (int) UserRole.Distribuidor:
                    return new Distribuidor();
                
                default:
                    throw new ArgumentException("Tipo de usuario invalido");
            }
        }

    }
}



