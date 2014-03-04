using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorAtributo: IAtributo
    {

        #region singleton
        private static ManejadorAtributo instance = new ManejadorAtributo();

        private ManejadorAtributo() { }

        public static ManejadorAtributo GetInstance()
        {
            return instance;
        }
        #endregion

        public void AltaAtgributo(dominio.Atributo atributo)
        {
            using (var db = new Persistencia())
            {
                
            }
        }

        private object Persistencia()
        {
            throw new NotImplementedException();
        }
    }
}
