using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio.Exceptions;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorEmpresaDistribuidora :IEmpresaDistribuidora
    {
        
        #region singleton
        private static ManejadorEmpresaDistribuidora instance = new ManejadorEmpresaDistribuidora();

        private ManejadorEmpresaDistribuidora() { }

        public static ManejadorEmpresaDistribuidora GetInstance()
        {
            return instance;
        }
        #endregion
        

        public void AltaEmpresa(EmpresaDistribuidora empresa)
        {
            using (var db = new Persistencia())
            {
                db.Empresas.Add(empresa);
                db.SaveChanges();
            }
        }

        public EmpresaDistribuidora GetEmpresaDistribuidora(int id)
        {
            EmpresaDistribuidora ret = null;
            using (var db = new Persistencia())
            {
                ret = db.Empresas.FirstOrDefault(e => e.EmpresaDistribuidoraID == id);
            }
            return ret;
        }

      

        public List<EmpresaDistribuidora> ListarEmpresasDistribuidoras(ActivoEnum activoEnum)
        {
            List<EmpresaDistribuidora> ret = null;
            using (var db = new Persistencia())
            {
                if (!db.Empresas.Any())
                {
                    ret = new List<EmpresaDistribuidora>();
                }
                else
                {
                    if (activoEnum == ActivoEnum.Todos)
                    {
                        ret = db.Empresas.Include("Ejecutivo").ToList();
                    }
                    else
                    {
                        ret = db.Empresas.Include("Ejecutivo")
                            .Where(e => e.Activo == (activoEnum == ActivoEnum.Activo)).ToList();
                    }
                   
                }
            }
            return ret;
        }

        public List<EmpresaDistribuidora> ListarEmpresasDistribuidoras()
        {
            List<EmpresaDistribuidora> ret = null;
            using (var db = new Persistencia())
            {
                if (!db.Empresas.Any())
                {
                    ret = new List<EmpresaDistribuidora>();
                }
                else
                {
                    ret = db.Empresas.Include("Ejecutivo").Where(e => e.Activo).ToList(); 
                }
            }
            return ret;
        }

        public List<int> GetEmpresasDeEjecutivo(int idDistrib)
        {
         
            using (var db = new Persistencia())
            {
                return db.Empresas
                    .Include("Ejecutivo")
                    .Where(e => e.Activo && e.Ejecutivo.PerfilUsuarioID == idDistrib)
                    .Select(e=> e.EmpresaDistribuidoraID).ToList(); 
            }
           
        }

        public bool Baja(int idEmpresa)
        {
            bool ok = false;
            using (var db = new Persistencia())
            {
             var empresa=  db.Empresas.SingleOrDefault(e=> e.EmpresaDistribuidoraID == idEmpresa);

                if (empresa != null)
                {
                    empresa.Activo = false;

                    db.SaveChanges();
                    ok = true;
                }
                else
                {
                    throw new CustomException("No se pudo encontrar esa Empresa") { Key = "idEmpresa" };
                }

            }
            return ok;
        }

        public void Modificar(EmpresaDistribuidora empresa)
        {
            using (var db = new Persistencia())
            {
                var empresaDB = db.Empresas.Find(empresa.EmpresaDistribuidoraID);
                if (empresaDB != null)
                {
                    empresaDB.Activo = empresa.Activo;
                    empresaDB.Nombre = empresa.Nombre;

                    db.SaveChanges();
                }
                else
                {
                    throw new CustomException("No se pudo encontrar esa empresa") { Key = "idEmpresa" };
                }

            }

        }

    }
}
