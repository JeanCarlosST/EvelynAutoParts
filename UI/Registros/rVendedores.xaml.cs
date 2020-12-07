using BLL;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.Registros
{
    /// <summary>
    /// Interaction logic for rVendedores.xaml
    /// </summary>
    public partial class rVendedores : Window
    {
        Vendedores vendedor;
        Usuarios usuario;
        public rVendedores(Usuarios user)
        {
            InitializeComponent();
            this.usuario = user;
            Limpiar();
        }
        
        private void Limpiar()
        {
            this.vendedor = new Vendedores() { UsuarioId = usuario.UsuarioId };
            vendedor.UsuarioId = 1;
            DataContext = vendedor;
        }

        private void BuscarBoton_Click(object sender, RoutedEventArgs e)
        {
            var vendedor = VendedoresBLL.Buscar(Utilities.ToInt(VendedorIdTextbox.Text));

            if (vendedor != null)
                this.vendedor = vendedor;
            else
            {
                this.vendedor = new Vendedores();
                MessageBox.Show("No se encontró ningún vendedor", "Registro de vendedores",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.DataContext = this.vendedor;
        }

        private void NuevoBoton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void GuardarBoton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validar())
                return;

            var found = VendedoresBLL.Guardar(vendedor);

            if (found)
            {
                MessageBox.Show("Vendedor guardado con éxito", "Registro de vendedores",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Limpiar();

            }
            else
                MessageBox.Show("No se pudo guardar correctamente", "Registro de vendedores",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EliminarBoton_Click(object sender, RoutedEventArgs e)
        {
            if (VendedoresBLL.Eliminar(Utilities.ToInt(VendedorIdTextbox.Text)))
            {
                Limpiar();
                MessageBox.Show("Vendedor borrado con éxito", "Registro de vendedores",
                                MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
                MessageBox.Show("No se puedo borrar correctamente", "Registro de vendedores",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool Validar()
        {
            bool esValido = true;

            if (!ValidarApellidos())
                esValido = false;

            if (!ValidarNombres())
                esValido = false;

            return esValido;
        }

        private void NombresTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!ValidarNombres() && NombresTextbox.Text.Length > 0)
                {
                    AdvertenciaNombresLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    NombresTextbox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaNombresLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                NombresTextbox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void ApellidosTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!ValidarApellidos() && ApellidosTextbox.Text.Length > 0)
                {
                    AdvertenciaApellidosLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    ApellidosTextbox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaApellidosLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                ApellidosTextbox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        public bool ValidarNombres()
        {
            bool esValido = true;

            if (NombresTextbox.Text.Any(Char.IsDigit) || NombresTextbox.Text.Any(Char.IsPunctuation) || NombresTextbox.Text.Any(Char.IsSymbol))
            {
                esValido = false;
                NombresTextbox.BorderBrush = new SolidColorBrush(Colors.Red);
                AdvertenciaNombresLabel.Text = "El nombre solo debe contener letras";
            }
            else if(NombresTextbox.Text.Length < 1)
            {
                esValido = false;
                AdvertenciaNombresLabel.Text = "Debe ingresar un nombre";
                AdvertenciaNombresLabel.Visibility = Visibility.Visible;
            }
            else if (NombresTextbox.Text.Length < 3)
            {
                esValido = false;
                AdvertenciaNombresLabel.Text = "El nombre debe contener más de 2 caracteres";
                AdvertenciaNombresLabel.Visibility = Visibility.Visible;
            }

            return esValido;
        }

        public bool ValidarApellidos()
        {
            bool esValido = true;

            if (ApellidosTextbox.Text.Any(Char.IsDigit) || ApellidosTextbox.Text.Any(Char.IsPunctuation) || ApellidosTextbox.Text.Any(Char.IsSymbol))
            {
                esValido = false;
                ApellidosTextbox.BorderBrush = new SolidColorBrush(Colors.Red);
                AdvertenciaApellidosLabel.Text = "El apellido solo debe contener letras";
            }
            else if (ApellidosTextbox.Text.Length < 1)
            {
                esValido = false;
                AdvertenciaApellidosLabel.Text = "Debe ingresar un apellido";
                AdvertenciaApellidosLabel.Visibility = Visibility.Visible;
            }
            else if (ApellidosTextbox.Text.Length < 3)
            {
                esValido = false;
                AdvertenciaApellidosLabel.Text = "El apellido debe contener más de 2 caracteres";
                AdvertenciaApellidosLabel.Visibility = Visibility.Visible;
            }

            return esValido;
        }
    }
}
