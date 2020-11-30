using System;
using System.Collections.Generic;
using System.Text;
using BLL;
using Entidades;
using DAL;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace UI.Registros
{
    /// <summary>
    /// Interaction logic for rCobros.xaml
    /// </summary>
    public partial class rCobros : Window
    {
        Usuarios usuario;
        Cobros cobros = new Cobros();
        public rCobros(Usuarios user)
        {
            InitializeComponent();
            usuario = user;
            cobros = new Cobros();
            ClientesComboBox.ItemsSource = ClientesBLL.GetList(c => true);
            ClientesComboBox.SelectedValuePath = "ClienteId";
            ClientesComboBox.DisplayMemberPath = "Cedula";

            FacturasComboBox.ItemsSource = FacturasBLL.GetList();
            FacturasComboBox.SelectedValuePath = "FacturaId";
            FacturasComboBox.DisplayMemberPath = "FacturaId";

            Limpiar();
        }


        private void Limpiar()
        {
            this.cobros = new Cobros() { UsuarioId = usuario.UsuarioId };
            this.cobros.Fecha = DateTime.Now;
            this.DataContext = cobros;
            TotalTextBox.Text = "0";

        }
        private void BuscarBoton_Click(object sender, RoutedEventArgs e)
        {
            Cobros encontrado = CobrosBLL.Buscar(Convert.ToInt32(CobroIdTextBox.Text));

            if (encontrado != null)
            {
                this.cobros = encontrado;
                this.DataContext = null;
                this.DataContext = cobros;
            }
            else
            {
                Limpiar();
                MessageBox.Show("El numero de cobro no existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminarBoton_Click(object sender, RoutedEventArgs e)
        {
            Cobros existe = CobrosBLL.Buscar(this.cobros.CobroId);

            if (existe == null)
            {
                MessageBox.Show("No existe cobro en la base de datos", "Fallo",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                CobrosBLL.Eliminar(this.cobros.CobroId);
                MessageBox.Show("Eliminado", "Exito",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Limpiar();
            }
        }
        private void GuardarBoton_Click(object sender, RoutedEventArgs e)
        {
            bool paso = false;
            if (Validar())
                paso = CobrosBLL.Guardar(cobros);

            if (paso)
            {
                Limpiar();
                MessageBox.Show("Guardado!", "Exito",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Fallo al guardar", "Fallo",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool Existe()
        {
            Cobros esValido = CobrosBLL.Buscar(cobros.CobroId);

            return (esValido != null);
        }
        private void AgregarBoton_Click(object sender, RoutedEventArgs e)
        {
            Contexto context = new Contexto();
            if (!ValidarCobro())
                return;

            cobros.Total += Convert.ToDouble(MontoTextBox.Text);
            cobros.Detalle.Add(new CobrosDetalle(cobros.CobroId, Convert.ToInt32(FacturasComboBox.SelectedValue), Convert.ToDouble(MontoTextBox.Text)));
            MontoTextBox.Text = "";
            this.DataContext = null;
            this.DataContext = cobros;
        }

        private void RemoverBoton_Click(object sender, RoutedEventArgs e)
        {
            Contexto contexto = new Contexto();
            if (CobrosDataGrid.Items.Count >= 1 && CobrosDataGrid.SelectedIndex <= CobrosDataGrid.Items.Count - 1)
            {
                CobrosDetalle cobro = (CobrosDetalle)CobrosDataGrid.SelectedValue;
                cobros.Total -= cobro.Monto;
                
                cobros.Detalle.RemoveAt(CobrosDataGrid.SelectedIndex);
                this.DataContext = null;
                this.DataContext = cobros;
            }
        }

        private void NuevoBoton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void FacturasComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Facturas facturas = FacturasBLL.Buscar(Convert.ToInt32(FacturasComboBox.SelectedValue));
    
            if (FacturasComboBox.SelectedValue != null)
                BalanceTextBox.Text = Convert.ToString(facturas.Balance);
            else
                BalanceTextBox.Text = "";
        }

        private void MontoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
             try
             {
                if(!ValidarMonto())
                    MontoTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                
                else
                     MontoTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
                
             }
             catch
             {
                 MontoTextBox.Foreground = SystemColors.ControlTextBrush;
             }
        }

        public bool ValidarMonto()
        {
            bool esValido = true;
            if (BalanceTextBox.Text == "0")
            {
                esValido = false;
                AdvertenciaMonto.Content = "No puede ingresar un monto ya que el balance es 0";
                AdvertenciaMonto.Visibility = Visibility.Visible;
            }
            else if (!MontoTextBox.Text.Any(Char.IsDigit))
            {
                esValido = false;
                AdvertenciaMonto.Content = "Solo puede ingresar digitos";
                AdvertenciaMonto.Visibility = Visibility.Visible;
            }
            else if (MontoTextBox.Text.Length < 1)
            {
                esValido = false;
                AdvertenciaMonto.Content = "Ingrese un monto";
                AdvertenciaMonto.Visibility = Visibility.Visible;
            }
            return esValido;
        }


        public bool Validar()
        {
            bool esValido = true;
            if (ClientesComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Debe elegir un Cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                esValido = false;
            }
            return esValido;
        }

        public bool ValidarCobro()
        {
            bool esValido = true;

            if(ClientesComboBox.SelectedIndex < 0)
            {
                AdvertenciaCliente.Content = "Primero debe seleccionar un Cliente";
                esValido = false;
            }
            else if (FacturasComboBox.SelectedIndex < 0)
            {
                AdvertenciaFacturas.Content = "Debe seleccionar una factura";
                esValido = false;
            }
            return esValido;
        }
    }
}
