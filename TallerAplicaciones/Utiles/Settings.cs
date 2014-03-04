using System;

namespace uy.edu.ort.taller.aplicaciones.utiles
{
    public class Settings
    {
        static private Settings instance = new Settings();
        private Properties config = new Properties("settings.properties");
        public static Settings GetInstance()
        {
            return instance;
        }
        private Settings()
        {
            // Properties config = new Properties("settings.properties");
            //get value whith default value
            //  com_port.Text = config.get("com_port", "1");

        }

        public String GetProperty(String key)
        {
            return config.Get(key);
        }

        public String GetProperty(String key, String defval)
        {
            return config.Get(key, defval);
        }


    }
}