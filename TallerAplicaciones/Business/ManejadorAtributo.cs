using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorAtributo : IAtributo
    {

        #region singleton
        private static ManejadorAtributo instance = new ManejadorAtributo();

        private ManejadorAtributo() { }

        public static ManejadorAtributo GetInstance()
        {
            return instance;
        }
        #endregion

        public void AltaAtributo(dominio.Atributo atributo)
        {
            using (var db = new Persistencia())
            {
                db.Atributos.Add(atributo);
                db.SaveChanges();
            }
        }

        public void AltaAtributoCombo(dominio.AtributoCombo atributo)
        {
            using (var db = new Persistencia())
            {
                db.Atributos.Add(atributo);
                db.SaveChanges();
            }
        }

        public void AltaAtributoSimple(dominio.AtributoSimple atributo)
        {
            using (var db = new Persistencia())
            {
                db.Atributos.Add(atributo);
                db.SaveChanges();
            }
        }

        public List<Atributo> GetAtributos()
        {
            using (var db = new Persistencia())
            {


                var atribCombos = db.Atributos
                    .OfType<AtributoCombo>().Include("Valores").ToList();

                var atribSimple = db.Atributos.OfType<AtributoSimple>().Cast<Atributo>().ToList();

                return atribCombos.Union(atribSimple).ToList();



            }
        }

    }
}
