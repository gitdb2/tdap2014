using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace TallerAplicaciones.Models
{

     public class PedidoCreateModel
    {

        //para alta
        [Display(Name = "Id del Pedido")]
        public int PedidoID { get; set; }

        [Required]
        [Display(Name = "Aprobado")]
        public bool Aprobado { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }
        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
        [Required]
        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Required]
        [Display(Name = "Ejecutivo de cuenta")]
        public int EjecutivoId { get; set; }

        [Required]
        [Display(Name = "Distribuidor")]
        public int DistribuidorID { get; set; }

        /// <summary>
        /// lista de ids de productos seleccionados
        /// </summary>
        ///
        [Required]
        [Display(Name = "Productos seleccionados")]
        public List<int> Productos { get; set; }
        /// <summary>
        /// lista de cantidades de productos seleccionados
        /// la posicion en la lista se corresponde con la de Productos
        /// </summary>
        [Required]
        [Display(Name = "Cantidades para los productos seleccionados")]
        public List<int> Cantidades { get; set; }
        

        /// <summary>
        /// Lista de distribuidores disponibles para asociar al pedido
        /// </summary>
      //  [Display(Name = "Distribuidor")]
        public List<Distribuidor> DistribuidoresDisponibles { get; set; }

        /// <summary>
        /// Lista de productos disponibles para las seleccion
        /// </summary>
      //    [Display(Name = "Productos")]
        public List<Producto> ProductosDisponibles { get; set; }

        // para modificacion
     ///   [Display(Name = "Ejecutivo")]
        public EjecutivoDeCuenta EjecutivoDeCuenta { get; set; }
      

    }



    public class PedidoCreatePOSTModel
    {

        //para alta
        [Display(Name = "Id del Pedido")]
        public int PedidoID { get; set; }

        [Required]
        [Display(Name = "Aprobado")]
        public bool Aprobado { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }
        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
        [Required]
        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Required]
        [Display(Name = "Ejecutivo de cuenta")]
        public int EjecutivoId { get; set; }

        [Required]
        [Display(Name = "Distribuidor")]
        public int DistribuidorID { get; set; }

        /// <summary>
        /// lista de ids de productos seleccionados
        /// </summary>
        ///
        [Required]
        [Display(Name = "Productos seleccionados")]
        public List<int> Productos { get; set; }
        /// <summary>
        /// lista de cantidades de productos seleccionados
        /// la posicion en la lista se corresponde con la de Productos
        /// </summary>
        [Required]
        [Display(Name = "Cantidades para los productos seleccionados")]
        public List<int> Cantidades { get; set; }
        


      

    }


    public class PedidoEditModelPost
    {
        [Required]
        [Display(Name = "Id del Pedido")]
        public int PedidoID { get; set; }

        [Required]
        [Display(Name = "Aprobado")]
        public bool Aprobado { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }
        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
        [Required]
        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Required]
        [Display(Name = "Ejecutivo de cuenta")]
        public int EjecutivoId { get; set; }

        [Required]
        [Display(Name = "Distribuidor")]
        public int DistribuidorID { get; set; }
    }

    public class PedidoEditModel : PedidoEditModelPost
    {

        public PedidoEditModel()
        {
            DistribuidoresDisponibles = new List<Distribuidor>();
            ProductosDisponibles = new List<Producto>();
          
        }

        public List<Distribuidor> DistribuidoresDisponibles { get; set; }
        
        public List<Producto> ProductosDisponibles { get; set; }

        public EjecutivoDeCuenta EjecutivoDeCuenta { get; set; }

        public Pedido Pedido { get; set; }
        
    }
    

    public class PedidoListModel
    {
        public List<Pedido> Pedidos { get; set; } 
    }

    public class DeletePedidoModel
    {
        public int PedidoID { get; set; }
    }

}