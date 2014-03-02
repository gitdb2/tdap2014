using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                ret = db.Empresas.First(e => e.EmpresaDistribuidoraID == id);
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
    }
}
