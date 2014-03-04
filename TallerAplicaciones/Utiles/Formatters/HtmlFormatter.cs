using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.utiles.Formatters
{
    public class HtmlFormatter : IVisitorHtmlFormatter
    {
        public string Html { get; set; }


        public void Visit(Foto elem)
        {
            Html = "<a href='#' data='" + elem.Url + "' onclick=\"openImage($(this))\"><img width=\"100px\" src=\"" + elem.Url + "\" alt=\"" + elem.Nombre + "\" /></a>";
        }


        public void Visit(Video elem)
        {
            Html = "<a href='#' data=\"" + elem.Url + "\" onclick=\"openVideo($(this))\"><img width=\"32px\" src=\"/Images/play_icon.png\" alt=\"" + elem.Nombre + "\" /></a>";
  
        }
    }
}
