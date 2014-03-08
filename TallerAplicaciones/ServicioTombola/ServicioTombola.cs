using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioTombola
{
    public class ServicioTombola : IServicioTombola
    {


        public Guid RealizarApuesta(int[] numeros) 
        {
            int[] valores = new int[100];
            if (numeros != null && numeros.Length >= 3 && numeros.Length <= 7)
            {
                foreach (int n in numeros)
                {
                    if (n < 0 || n > 99)
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.NewGuid();
            }
            return Guid.Empty;
        }


        public double CorrespondePremio(Guid identificadorApuesta) 
        {
            int monto = (int)(new Random().NextDouble() * 10000);
            if (monto > 1000)
            {
                return 0;
            }
            return monto;
        }


        public void EfectuarPago(Guid identificadorApuesta) 
        {
            // No hago nada
        }

    }
}