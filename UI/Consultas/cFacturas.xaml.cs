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
            List<object> listado = new List<object>();

            string criterio = CriterioTextBox.Text.Trim();

            if (!ValidarFechas())
                return;

            DateTime? desde = DesdeDatePicker.SelectedDate;
            DateTime? hasta = HastaDatePicker.SelectedDate != null ? ((DateTime)HastaDatePicker.SelectedDate).AddHours(24) : HastaDatePicker.SelectedDate;

            if (desde == null || hasta == null)
            {
                if (desde != null)
                    hasta = DateTime.Now.AddDays(1);
                else if (hasta != null)
                    desde = new DateTime(1, 1, 1);
            }

            if (criterio.Length > 0)
            {
                switch (FiltroCombobox.SelectedIndex)
                {
                    case 0:
                        listado = FacturasBLL.GetList("FacturaId", criterio, desde, hasta);
                        break;

                    case 1:
                        listado = FacturasBLL.GetList("Cliente", criterio, desde, hasta);
                        break;

                    case 2:
                        listado = FacturasBLL.GetList("Vendedor", criterio, desde, hasta);
                        break;

                    case 3:
                        listado = FacturasBLL.GetList("Usuario", criterio, desde, hasta);
                        break;

                }
            }
            else
            {
                listado = FacturasBLL.GetList("", "", desde, hasta);
            }

            FacturasDataGrid.ItemsSource = null;
            FacturasDataGrid.ItemsSource = listado;
        }

        private bool ValidarFechas()
        {
            if (DesdeDatePicker.Text.Length != 0 && !DateTime.TryParse(DesdeDatePicker.Text, out _))
            {
                MessageBox.Show("Introduzca una fecha inicial válida", "Consulta de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (HastaDatePicker.Text.Length != 0 && !DateTime.TryParse(HastaDatePicker.Text, out _))
            {
                MessageBox.Show("Introduzca una fecha final válida", "Consulta de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (DesdeDatePicker.SelectedDate > HastaDatePicker.SelectedDate)
            {
                MessageBox.Show("La fecha inicial no puede ser mayor a la fecha final", "Consulta de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void FiltroCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CriterioTextBox.AutoCompleteSource = null;

            switch (FiltroCombobox.SelectedIndex)
            {
                case 1:
                    CriterioTextBox.AutoCompleteSource = ClientesBLL.GetList(c => true);
                    CriterioTextBox.SearchItemPath = "Nombres";
                    break;
                case 2:
                    CriterioTextBox.AutoCompleteSource = VendedoresBLL.GetList(c => true);
                    CriterioTextBox.SearchItemPath = "Nombres";
                    break;
                case 3:
                    CriterioTextBox.AutoCompleteSource = UsuariosBLL.GetList(c => true);
                    CriterioTextBox.SearchItemPath = "Nombres";
                    break;

            }

        }
    }
}
