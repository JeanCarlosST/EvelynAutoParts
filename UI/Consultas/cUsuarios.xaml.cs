using BLL;
using Entidades;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UI.Consultas
{
    /// <summary>
    /// Interaction logic for cUsuarios.xaml
    /// </summary>
    public partial class cUsuarios : Window
    {
        public cUsuarios()
        {
            InitializeComponent();
        }
        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            var listado = new List<Usuarios>();

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
                        listado = UsuariosBLL.GetList(u => u.UsuarioId == Utilities.ToInt(criterio));
                        break;

                    case 1:
                        listado = UsuariosBLL.GetList(u => u.Nombres.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 2:
                        listado = UsuariosBLL.GetList(u => u.Apellidos.ToLower().Contains(criterio.ToLower()));
                        break;

                    case 3:
                        listado = UsuariosBLL.GetList(u => u.NombreUsuario.ToLower().Contains(criterio.ToLower()));
                        break;
                }
            }
            else
            {
                if (desde == null && hasta == null)
                    listado = UsuariosBLL.GetList(u => true);
                else
                    listado = UsuariosBLL.GetList(u => u.FechaCreacion >= desde && u.FechaCreacion <= hasta);
            }

            UsuariosDataGrid.ItemsSource = null;
            UsuariosDataGrid.ItemsSource = listado;
        }

        private bool ValidarFechas()
        {
            if (DesdeDatePicker.Text.Length != 0 && !DateTime.TryParse(DesdeDatePicker.Text, out _))
            {
                MessageBox.Show("Introduzca una fecha inicial válida", "Consulta de usuarios",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (HastaDatePicker.Text.Length != 0 && !DateTime.TryParse(HastaDatePicker.Text, out _))
            {
                MessageBox.Show("Introduzca una fecha final válida", "Consulta de usuarios",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (DesdeDatePicker.SelectedDate > HastaDatePicker.SelectedDate)
            {
                MessageBox.Show("La fecha inicial no puede ser mayor a la fecha final", "Consulta de usuarios",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void FiltroComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listado = new List<Usuarios>();
            listado = UsuariosBLL.GetList(u => true);

            CriterioTextBox.AutoCompleteSource = listado;

            switch (FiltroComboBox.SelectedIndex)
            {
                case 0:
                    CriterioTextBox.SearchItemPath = "UsuarioId";
                    break;
                case 1:
                    CriterioTextBox.SearchItemPath = "Nombres";
                    break;
                case 2:
                    CriterioTextBox.SearchItemPath = "Apellidos";
                    break;
                case 3:
                    CriterioTextBox.SearchItemPath = "NombreUsuario";
                    break;
            }
        }
    }
}
