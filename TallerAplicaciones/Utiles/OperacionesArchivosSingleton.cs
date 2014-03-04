using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace uy.edu.ort.taller.aplicaciones.utiles
{
    public class OperacionesArchivosSingleton
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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



   
        //public bool CreateDiskSharedSpace(string login)
        //{
        //    string dir = Settings.GetInstance().GetProperty("upload.directory", @"c:/shared") + "/" + login;
        //    if (!Directory.Exists(dir))
        //    {
        //        try
        //        {
        //            Directory.CreateDirectory(dir);
        //            return true;
        //        }
        //        catch (Exception)
        //        {

        //            return false;
        //        }

        //    }
        //    return true;
        //}

        private string BasePath()
        {
            return Settings.GetInstance().GetProperty("upload.directory", @"c:\uploads");
        }

     
        

    }
}