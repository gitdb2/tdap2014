using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Browser;
using uy.edu.ort.taller.aplicaciones.clientedistribuidores.ApiDistribuidores;

namespace uy.edu.ort.taller.aplicaciones.clientedistribuidores.Logica
{
    public class PlayList
    {

        private Dictionary<ArchivoDTO, int> Elementos { get; set; }

        public PlayList()
        {
            Elementos = new Dictionary<ArchivoDTO, int>();
        }

        public void CargarPlayList(ObservableCollection<ArchivoDTO> origen)
        {
            if (origen.Any())
            {
                foreach (var item in origen)
                {
                    item.Url = GenerarPrefijoUrlArchivo() + item.Url;
                    Elementos.Add(item, 0);
                }
            }
        }

        private string GenerarPrefijoUrlArchivo()
        {
            var host = HtmlPage.Document.DocumentUri.Host;
            var port = HtmlPage.Document.DocumentUri.Port;
            return "http://" + host + ":" + port;
        }

        public ArchivoDTO SiguienteElemento()
        {
            var siguiente = ElementoPlayListMenosMostrado();
            Elementos[siguiente]++;
            return siguiente;
        }
        
        private ArchivoDTO ElementoPlayListMenosMostrado()
        {
            int minVeces = Int16.MaxValue;
            ArchivoDTO elementoMinVecesMostrado = null;
            if (Elementos != null)
            {
                foreach (var par in Elementos)
                {
                    if (par.Value <= minVeces)
                    {
                        minVeces = par.Value;
                        elementoMinVecesMostrado = par.Key;
                    }
                }
            }
            return elementoMinVecesMostrado;
        }

    }
}
