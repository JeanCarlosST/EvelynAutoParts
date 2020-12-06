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
    /// Interaction logic for cVendedores.xaml
    /// </summary>
    public partial class cVendedores : Window
    {
        public cVendedores()
        {
            InitializeComponent();
        }

        private void BuscarBoton_Click(object sender, RoutedEventArgs e)
        {
            var listado = new List<Vendedores>();

            string criterio = CriterioTextBox.Text.Trim();
            if (criterio.Length > 0)
            {
                switch (FiltroComboBox.SelectedIndex)
                {
                    case 0:
                        listado = VendedoresBLL.GetList(v => v.VendedorId == Utilities.ToInt(CriterioTextBox.Text));
                        break;

                    case 1:
                        listado = VendedoresBLL.GetList(v => v.Nombres.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 2:
                        listado = VendedoresBLL.GetList(v => v.Apellidos.ToLower().Contains(criterio.ToLower()));
                        break;

                }
            }
            else
            {
                listado = VendedoresBLL.GetList(c => true);
            }

            VendedoresDataGrid.ItemsSource = null;
            VendedoresDataGrid.ItemsSource = listado;
        }

        private void FiltroComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listado = new List<Vendedores>();
            listado = VendedoresBLL.GetList(c => true);

            CriterioTextBox.AutoCompleteSource = listado;

            /*CriterioStackPanel.Visibility = Visibility.Visible;
            FechasGrid.Visibility = Visibility.Hidden;*/


            switch (FiltroComboBox.SelectedIndex)
            {
                case 0:
                    CriterioTextBox.SearchItemPath = "VendedorId";
                    break;
                case 1:
                    CriterioTextBox.SearchItemPath = "Nombres";
                    break;
                case 2:
                    CriterioTextBox.SearchItemPath = "Apellidos";
                    break;
                /*case 3:
                    CriterioStackPanel.Visibility = Visibility.Hidden;
                    FechasGrid.Visibility = Visibility.Visible;
                    break;*/

            }
        }
    }
}
