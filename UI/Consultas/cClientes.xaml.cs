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
    /// Interaction logic for cClientes.xaml
    /// </summary>
    public partial class cClientes : Window
    {
        public cClientes()
        {
            InitializeComponent();
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            var listado = new List<Clientes>();

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
                switch (FiltroComboBox.SelectedIndex)
                {
                    case 0:
                        listado = ClientesBLL.GetList(c => c.ClienteId == Utilities.ToInt(criterio));
                        break;

                    case 1:
                        listado = ClientesBLL.GetList(c => c.Nombres.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 2:
                        listado = ClientesBLL.GetList(c => c.Apellidos.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 3:
                        listado = ClientesBLL.GetList(c => c.Direccion.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 4:
                        listado = ClientesBLL.GetList(c => c.Celular.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 5:
                        listado = ClientesBLL.GetList(c => c.Telefono.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 6:
                        listado = ClientesBLL.GetList(c => c.Cedula.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 7:
                        listado = ClientesBLL.GetList(c => c.Email.ToLower().Contains(criterio.ToLower()));
                        break;
                }
            }
            else
            {
                if (desde == null && hasta == null)
                    listado = ClientesBLL.GetList(c => true);
                else
                    listado = ClientesBLL.GetList(c => c.Fecha >= desde && c.Fecha <= hasta);
            }

            ClientesDataGrid.ItemsSource = null;
            ClientesDataGrid.ItemsSource = listado;
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



        private void FiltroComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listado = new List<Clientes>();
            listado = ClientesBLL.GetList(c => true);

            CriterioTextBox.AutoCompleteSource = listado;

            switch (FiltroComboBox.SelectedIndex)
            {
                case 0:
                    CriterioTextBox.SearchItemPath = "ClienteId";
                    break;
                case 1:
                    CriterioTextBox.SearchItemPath = "Nombres";
                    break;
                case 2:
                    CriterioTextBox.SearchItemPath = "Apellidos";
                    break;
                case 3:
                    CriterioTextBox.SearchItemPath = "Direccion";
                    break;
                case 4:
                    CriterioTextBox.SearchItemPath = "Telefono";
                    break;
                case 5:
                    CriterioTextBox.SearchItemPath = "Celular";
                    break;
                case 6:
                    CriterioTextBox.SearchItemPath = "Cedula";
                    break;
                case 7:
                    var lista = new List<Clientes>();
                    foreach (var item in listado)
                    {
                        if (item.Email != null)
                            lista.Add(item);
                    }
                    CriterioTextBox.AutoCompleteSource = lista;
                    CriterioTextBox.SearchItemPath = "Email";
                    break;

            }
        }
    }
}
