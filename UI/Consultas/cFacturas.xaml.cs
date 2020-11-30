using BLL;
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
    /// Interaction logic for cFacturas.xaml
    /// </summary>
    public partial class cFacturas : Window
    {
        public cFacturas()
        {
            InitializeComponent();
        }

        private void BuscarBoton_Click(object sender, RoutedEventArgs e)
        {
            var listado = FacturasBLL.GetList();

            string criterio = CriterioTextbox.Text.Trim();

            if (criterio.Length > 0)
            {
                switch (FiltroCombobox.SelectedIndex)
                {
                    case 0:
                        listado.RemoveAll(f => Utilities.ToInt(f.GetType().GetProperty("FacturaId").GetValue(f).ToString()) != Utilities.ToInt(criterio));
                        break;

                    case 1:
                        listado.RemoveAll(f => !f.GetType().GetProperty("Cliente").GetValue(f).ToString().ToLower().Contains(criterio.ToLower()));
                        break;

                    case 2:
                        listado.RemoveAll(f => !f.GetType().GetProperty("Vendedor").GetValue(f).ToString().ToLower().Contains(criterio.ToLower()));
                        break;

                }
            }

            FacturasDataGrid.ItemsSource = null;
            FacturasDataGrid.ItemsSource = listado;
        }

        private bool ValidarFechas()
        {
            if(!DateTime.TryParse(DesdeDatePicker.Text, out _))
            {
                MessageBox.Show("Introduzca una fecha inicial válida", "Registro de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (!DateTime.TryParse(HastaDatePicker.Text, out _))
            {
                MessageBox.Show("Introduzca una fecha final válida", "Registro de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void FiltroCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(FiltroCombobox.SelectedIndex == 3)
            {
                CriterioStackPanel.Visibility = Visibility.Hidden;
                FechasGrid.Visibility = Visibility.Visible;
            }
            else
            {
                CriterioStackPanel.Visibility = Visibility.Visible;
                FechasGrid.Visibility = Visibility.Hidden;
            }
                
        }
    }
}
