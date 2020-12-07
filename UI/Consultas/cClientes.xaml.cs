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

            if (CriterioTextBox.Text.Trim().Length > 0)
            {
                switch (FiltroComboBox.SelectedIndex)
                {
                    case 0:
                        listado = ClientesBLL.GetList(c => c.ClienteId == Convert.ToInt32(CriterioTextBox.Text));
                        break;

                    case 1:
                        listado = ClientesBLL.GetList(c => c.Nombres == CriterioTextBox.Text);
                        break;
                    case 2:
                        listado = ClientesBLL.GetList(c => c.Apellidos == CriterioTextBox.Text);
                        break;

                    case 3:
                        listado = ClientesBLL.GetList(c => c.Direccion == CriterioTextBox.Text);
                        break;

                    case 4:
                        listado = ClientesBLL.GetList(c => c.Celular == CriterioTextBox.Text);
                        break;

                    case 5:
                        listado = ClientesBLL.GetList(c => c.Telefono == CriterioTextBox.Text);
                        break;

                    case 6:
                        listado = ClientesBLL.GetList(c => c.Cedula == CriterioTextBox.Text);
                        break;

                    case 7:
                        listado = ClientesBLL.GetList(c => c.Email == CriterioTextBox.Text);
                        break;

                    case 8:
                        if (DesdeDatePicker.SelectedDate != null)
                            listado = ClientesBLL.GetList(c => c.Fecha.Date >= DesdeDatePicker.SelectedDate);

                        if (HastaDatePicker.SelectedDate != null)
                            listado = ClientesBLL.GetList(c => c.Fecha.Date <= HastaDatePicker.SelectedDate);
                        break;

                }
            }
            else
            {
                listado = ClientesBLL.GetList(c => true);
            }

            ClientesDataGrid.ItemsSource = null;
            ClientesDataGrid.ItemsSource = listado;
        }

        private void FiltroComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listado = new List<Clientes>();
            listado = ClientesBLL.GetList(c => true);

            CriterioTextBox.AutoCompleteSource = listado;

            switch (FiltroComboBox.SelectedIndex)
            {
                case 0:
                    CriterioTextBox.SearchItemPath = "IdCliente";
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
                    CriterioTextBox.SearchItemPath = "Email";
                    break;

            }
        }
    }
}
