using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using DistribuidoresApp.Temp;

namespace DistribuidoresApp.Views
{
    public partial class DetalleProducto : Page
    {
        public ProductoFake Producto { get; set; }

        public DetalleProducto()
        {
            InitializeComponent();
            Producto = new ProductoFake()
            {
                Nombre = "Prod1", Codigo = "AAAXG", Descripcion = "Desc prod1", ProductoFakeId = 1
            };
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
