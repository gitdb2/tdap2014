using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.utiles;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorArchivos :IArchivo
    {
        
        #region singleton
        private static ManejadorArchivos instance = new ManejadorArchivos();

        private ManejadorArchivos() { }

        public static ManejadorArchivos GetInstance()
        {
            return instance;
        }
        #endregion




        public Foto SubirFoto()
        {
            throw new NotImplementedException();
        }
    }
}
