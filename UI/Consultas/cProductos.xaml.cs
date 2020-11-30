using BLL;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.Consultas
{
    /// <summary>
    /// Interaction logic for cProductos.xaml
    /// </summary>
    public partial class cProductos : Window
    {
        public cProductos()
        {
            InitializeComponent();
        }

        private void BuscarBoton_Click(object sender, RoutedEventArgs e)
        {
            var listado = new List<Productos>();

            string criterio = CriterioTextBox.Text.Trim();
            if (criterio.Length > 0)
            {
                switch (FiltroComboBox.SelectedIndex)
                {
                    case 0:
                        listado = ProductosBLL.GetList(p => p.ProductoId == Utilities.ToInt(CriterioTextBox.Text));
                        break;

                    case 1:
                        listado = ProductosBLL.GetList(p => p.Descripcion.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 2:
                        listado = ProductosBLL.GetList(p => p.Precio < Utilities.ToDouble(criterio));
                        break;

                    case 3:
                        listado = ProductosBLL.GetList(p => p.Costo < Utilities.ToDouble(criterio));
                        break;

                }
            }
            else
            {
                listado = ProductosBLL.GetList(c => true);
            }

            ProductosDataGrid.ItemsSource = null;
            ProductosDataGrid.ItemsSource = listado;
        }
    }
}
