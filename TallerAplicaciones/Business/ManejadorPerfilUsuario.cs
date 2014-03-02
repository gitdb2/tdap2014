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
            using (var db = new Persistencia())
            {
                if (incluirInactivos)
                {
                    return db.PerfilesUsuario.Include("Usuario").ToList();
                }
                else
                {
                    return (db.PerfilesUsuario.Include("Usuario").Where(p => p.Activo == false)).ToList();
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
    }
}



