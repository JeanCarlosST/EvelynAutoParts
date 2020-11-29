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

            if (CriterioTextBox.Text.Trim().Length > 0)
            {
                switch (FiltroComboBox.SelectedIndex)
                {
                    case 0:
                        listado = UsuariosBLL.GetList(u => u.UsuarioId == Convert.ToInt32(CriterioTextBox.Text));
                        break;

                    case 1:
                        listado = UsuariosBLL.GetList(u => u.Nombres == CriterioTextBox.Text);
                        break;
                    case 2:
                        listado = UsuariosBLL.GetList(u => u.Apellidos == CriterioTextBox.Text);
                        break;
                    case 3:
                        listado = UsuariosBLL.GetList(u => u.NombreUsuario == CriterioTextBox.Text);
                        break;
                    case 4:
                        if (DesdeDatePicker.SelectedDate != null)
                            listado = UsuariosBLL.GetList(u => u.FechaCreacion.Date >= DesdeDatePicker.SelectedDate);

                        if (HastaDatePicker.SelectedDate != null)
                            listado = UsuariosBLL.GetList(u => u.FechaCreacion.Date <= HastaDatePicker.SelectedDate);
                        break;
                }
            }
            else
            {
                listado = UsuariosBLL.GetList(u => true);
            }

            DatosDataGrid.ItemsSource = null;
            DatosDataGrid.ItemsSource = listado;
        }

        private void FiltroComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listado = new List<Usuarios>();
            listado = UsuariosBLL.GetList(u => true);

            CriterioTextBox.AutoCompleteSource = listado;

            CriterioGrid.Visibility = Visibility.Visible;
            DesdeStackPanel.Visibility = Visibility.Hidden;
            HastatackPanel.Visibility = Visibility.Hidden;

            switch (FiltroComboBox.SelectedIndex)
            {
                case 0:
                    CriterioTextBox.SearchItemPath = "IdUsuario";
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
                case 4:
                    CriterioGrid.Visibility = Visibility.Hidden;
                    DesdeStackPanel.Visibility = Visibility.Visible;
                    HastatackPanel.Visibility = Visibility.Visible;

                    break;
            }
        }
    }
}
