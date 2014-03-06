using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.interfaces
{
    public interface IAtributo
    {
        void AltaAtributo(Atributo atributo);

        void AltaAtributoCombo(AtributoCombo atributoCombo);

        void AltaAtributoSimple(AtributoSimple atributoSimple);

        List<Atributo> GetAtributos();

        //List<ValorPredefinido> GetValoresPredefinidos();
    }
}
