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
                MessageBox.Show("Cobro no encontrado", "Registro de cobros");
            }
        }

        private void EliminarBoton_Click(object sender, RoutedEventArgs e)
        {
            Cobros existe = CobrosBLL.Buscar(this.cobros.CobroId);

            if (existe == null)
            {
                MessageBox.Show("No se pudo eliminar el cobro", "Registro de cobros",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                CobrosBLL.Eliminar(this.cobros.CobroId);
                MessageBox.Show("Cobro eliminado", "Registro de cobros",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Limpiar();
            }
        }
        private void GuardarBoton_Click(object sender, RoutedEventArgs e)
        {
            bool paso = false;
            if (!Validar())
                return;
            
            paso = CobrosBLL.Guardar(cobros);
            if (paso)
            {
                Limpiar();
                MessageBox.Show("Cobro guardada con éxito", "Registro de cobros",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("No fue posible guardar", "Registro de cobros",
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
            if (!ValidarCobro() && !ValidarMonto())
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
                if (!ValidarMonto())
                    MontoTextBox.BorderBrush = new SolidColorBrush(Colors.Red);

                else
                    AdvertenciaMonto.Text = "";
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
            if(MontoTextBox.Text.Length < 1)
            {
                esValido = false;
                AdvertenciaMonto.Text = "Debe ingresar un monto";
                AdvertenciaMonto.Visibility = Visibility.Visible;
            }

            else if (Convert.ToDouble(MontoTextBox.Text) > Convert.ToDouble(BalanceTextBox.Text))
            {
                esValido = false;
                AdvertenciaMonto.Text = "No puede ingresar un monto mayor al balance";
                AdvertenciaMonto.Visibility = Visibility.Visible;
            }
            else if (Convert.ToDouble(MontoTextBox.Text) <= 0)
            {
                esValido = false;
                AdvertenciaMonto.Text = "No puede ingresar un monto menor o igual a 0";
                AdvertenciaMonto.Visibility = Visibility.Visible;
            }

            if (BalanceTextBox.Text == "0" && BalanceTextBox.Text.Length > 0)
            {
                esValido = false;
                AdvertenciaMonto.Text = "No puede ingresar un monto ya que el balance es 0";
                AdvertenciaMonto.Visibility = Visibility.Visible;
            }
            else if (!MontoTextBox.Text.Any(Char.IsDigit))
            {
                esValido = false;
                AdvertenciaMonto.Text = "Solo puede ingresar dígitos";
                AdvertenciaMonto.Visibility = Visibility.Visible;
            }
            else if (MontoTextBox.Text.Length < 1)
            {
                esValido = false;
                AdvertenciaMonto.Text = "Ingrese un monto";
                AdvertenciaMonto.Visibility = Visibility.Visible;
            }
            return esValido;
        }


        public bool Validar()
        {
            bool esValido = true;
            if(cobros.Detalle.Count < 1)
            {
                MessageBox.Show("Debe ingresar minimo un cobro", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                esValido = false;
            }

            if (ClientesComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Debe elegir un cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                esValido = false;
            }
            return esValido;
        }

        public bool ValidarCobro()
        {
            bool esValido = true;

            if(ClientesComboBox.SelectedIndex < 0)
            {
                AdvertenciaCliente.Text = "Primero debe seleccionar un cliente";
                esValido = false;
            }
            else if (FacturasComboBox.SelectedIndex < 0)
            {
                AdvertenciaFacturas.Text = "Debe seleccionar una factura";
                esValido = false;
            }
            return esValido;
        }

        private void ClientesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientesComboBox.SelectedValue != null)
            {
                FacturasComboBox.ItemsSource = FacturasBLL.GetList(f => f.ClienteId == Convert.ToInt32(ClientesComboBox.SelectedValue));
                FacturasComboBox.SelectedValuePath = "FacturaId";
                FacturasComboBox.DisplayMemberPath = "FacturaId";
            }
            else
                FacturasComboBox.ItemsSource = null;
            
        }
    }
}
