using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace uy.edu.ort.taller.aplicaciones.utiles
{
    public class OperacionesArchivosSingleton
    {

        #region Singleton
        private static OperacionesArchivosSingleton instance = new OperacionesArchivosSingleton();
        private OperacionesArchivosSingleton()
        {
        
        }
        public static OperacionesArchivosSingleton GetInstance()
        {
            return instance;
        }
        #endregion
   
        private string BasePath()
        {
            return Settings.GetInstance().GetProperty("upload.directory", @"c:\uploads");
        }

    }
}