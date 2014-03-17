using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace Utiles
{
    public class HtmlFormatterAtributo: IVisitorHtmlAtributo
    {

       public string Html { get; set; }

       public void Visit(AtributoCombo elem)
       {
           Html = "<SELECT "+(elem.EsMultiseleccion()? "multiple='multiple'" : "")+" NAME=\"selCombo\" class='atributoCombo'\">";

           foreach (var item in elem.Valores)
           {
               Html += "<OPTION VALUE=\"" + item.Valor + "\">" + item.Valor + "</OPTION>\" ";
           }
           Html += "</SELECT> ";
       }

       public void Visit(AtributoSimple elem)
       {
           Html = "<label>" + "Texto simple" + "</label>";
       }   

    }
}

