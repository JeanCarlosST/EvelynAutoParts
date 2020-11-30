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
            if(NombresTextbox.Text.Length == 0)
            {
                MessageBox.Show("Introduzca un nombre", "Registro de vendedores",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (NombresTextbox.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Introduzca un nombre que no contenga dígitos", "Registro de vendedores",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (ApellidosTextbox.Text.Length == 0)
            {
                MessageBox.Show("Introduzca un apellido", "Registro de vendedores",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (ApellidosTextbox.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Introduzca un apellido que no contenga dígitos", "Registro de vendedores",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
