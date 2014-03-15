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

        private ManejadorAtributo()
        {
        }

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
                var atribCombos = db.Atributos.OfType<AtributoCombo>().Include("Valores").ToList();

                var atribSimple = db.Atributos.OfType<AtributoSimple>().Cast<Atributo>().ToList();

                return atribCombos.Union(atribSimple).ToList();

            }
        }

     


        public List<Atributo> GetAtributosActivos()
        {
            using (var db = new Persistencia())
            {
                var atribCombos = db.Atributos
                                    .OfType<AtributoCombo>()
                                    .Include("Valores")
                                    .Where(a => a.Activo == true).ToList();

                var atribSimple = db.Atributos
                                    .OfType<AtributoSimple>()
                                    .Cast<Atributo>()
                                    .Where(a => a.Activo == true).ToList();

                return atribCombos.Union(atribSimple).ToList();

            }
        }




        public Atributo GetAtributo(int idAtributo)
        {
            using (var db = new Persistencia())
            {
                var atributo = db.Atributos.SingleOrDefault(a => a.AtributoID == idAtributo);
                                            
                return atributo;

            }   
        }

        public AtributoCombo GetAtributoCombo(int idAtributo)
        {
            using (var db = new Persistencia())
            {
                var atributoCombo = db.Atributos.OfType<AtributoCombo>().Include("Valores").SingleOrDefault(a => a.AtributoID == idAtributo);

                return atributoCombo;

            }
            
        }


        public void EditarAtributoSimple(int idAtributo,bool activo, string nuevoNombre)
        {
            using (var db = new Persistencia())
            {
                var atributo = db.Atributos.SingleOrDefault(a => a.AtributoID == idAtributo);
                atributo.Activo = activo;
                atributo.Nombre = nuevoNombre;
                db.SaveChanges();
            }
        }

        public void EditarAtributoCombo(int idAtributo, bool activo, string nuevoNombre, bool dataCombo, List<int> listaABorrar, List<String> valoresNuevos)
        {
            using (var db = new Persistencia())
            {
                var atributo = db.Atributos.OfType<AtributoCombo>().Include("Valores").SingleOrDefault(a => a.AtributoID == idAtributo);
                atributo.Activo = activo;
                atributo.Nombre = nuevoNombre;
                List<ValorPredefinido> listaValores = atributo.Valores;
                if ((listaValores != null) && (listaValores.Any()))
                {
                    foreach (var valor in listaValores)
                    {
                        if ((listaABorrar != null) && (listaABorrar.Contains(valor.ValorPredefinidoID)))
                        {
                            valor.Activo = false;
                        }
                        else
                        {
                            valor.Activo = true;
                        }
                    }
                }
                if ((valoresNuevos != null ) && (valoresNuevos.Any()))
                {
                    foreach (var valor in valoresNuevos)
                    {
                        listaValores.Add(new ValorPredefinido(valor, true));
                    }
                }
                db.SaveChanges();
            }
        }

        public ValorPredefinido GetValorPredefinido(int idValor)
        {
            using (var db = new Persistencia())
            {
                return db.ValoresPredefinidos.SingleOrDefault(a => a.ValorPredefinidoID == idValor);
               
            }
        }
    }
}
