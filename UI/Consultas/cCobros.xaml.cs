using BLL;
using DAL;
using Entidades;
using System;
using System.Collections;
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
    /// Interaction logic for cCobros.xaml
    /// </summary>
    public partial class cCobros : Window
    {
        public cCobros()
        {
            InitializeComponent();
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            var listado = new List<Cobros>();

            if (CriterioTextBox.Text.Trim().Length > 0)
            {
                switch (FiltroComboBox.SelectedIndex)
                {
                    case 0:
                        listado = CobrosBLL.GetList(c => c.CobroId == Convert.ToInt32(CriterioTextBox.Text));
                        break;

                    /*case 1:
                        listado = CobrosBLL.GetList(u => u.  == CriterioTextBox.Text);
                        break;*/
                    case 2:

                        var cliente = ClientesBLL.Buscar(CriterioTextBox.Text);
                        listado = CobrosBLL.GetList(c => c.ClienteId == cliente.ClienteId);
                        break;

                    case 3:
                        listado = CobrosBLL.GetList(c => c.Total == Convert.ToDouble(CriterioTextBox.Text));
                        break;
                    case 4:
                        if (DesdeDatePicker.SelectedDate != null)
                            listado = CobrosBLL.GetList(c => c.Fecha.Date >= DesdeDatePicker.SelectedDate);

                        if (HastaDatePicker.SelectedDate != null)
                            listado = CobrosBLL.GetList(c => c.Fecha.Date <= HastaDatePicker.SelectedDate);
                        break;

                }
            }
            else
            {
                listado = CobrosBLL.GetList(c => true);
            }

            DatosDataGrid.ItemsSource = null;
            DatosDataGrid.ItemsSource = listado;

        }

        private void FiltroComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listado = new List<Cobros>();
            listado = CobrosBLL.GetList(c => true); ;

            CriterioTextBox.AutoCompleteSource = listado;

            CriterioGrid.Visibility = Visibility.Visible;
            DesdeStackPanel.Visibility = Visibility.Hidden;
            HastatackPanel.Visibility = Visibility.Hidden;


            switch (FiltroComboBox.SelectedIndex)
            {
                case 0:
                    CriterioTextBox.SearchItemPath = "CobroId";
                    break;
                case 1:
                    CriterioTextBox.AutoCompleteSource = UsuariosBLL.GetList(c => true);
                    CriterioTextBox.SearchItemPath = "Nombres";
                    break;
                case 2:
                    CriterioTextBox.AutoCompleteSource = ClientesBLL.GetList(c => true);
                    CriterioTextBox.SearchItemPath = "Cedula";
                    break;
                case 3:
                    CriterioTextBox.SearchItemPath = "Total";
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
