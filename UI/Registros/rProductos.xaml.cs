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
    /// Interaction logic for rProductos.xaml
    /// </summary>
    public partial class rProductos : Window
    {
        Productos producto;
        Usuarios usuario;
        public rProductos(Usuarios user)
        {
            InitializeComponent();
            this.usuario = user;
            Limpiar();
        }

        private void Limpiar()
        {
            this.producto = new Productos() { UsuarioId = usuario.UsuarioId };
            this.producto.UsuarioId = 1;
            DataContext = producto;
        }

        private void BuscarBoton_Click(object sender, RoutedEventArgs e)
        {
            var producto = ProductosBLL.Buscar(Utilities.ToInt(ProductoIdTextbox.Text));

            if (producto != null)
                this.producto = producto;
            else
            {
                this.producto = new Productos();
                MessageBox.Show("No se encontró ningún producto", "Registro de productoes",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.DataContext = this.producto;
        }

        private void NuevoBoton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void GuardarBoton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validar())
                return;

            switch (PorcentajeITBISCombobox.SelectedIndex) 
            {
                case 0:
                    producto.PorcentajeITBIS = 0;
                    break;
                case 1:
                    producto.PorcentajeITBIS = 0.12f;
                    break;
                case 2:
                    producto.PorcentajeITBIS = 0.18f;
                    break;

            }

            var found = ProductosBLL.Guardar(producto);

            if (found)
            {
                MessageBox.Show("Producto guardado con éxito", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Limpiar();

            }
            else
                MessageBox.Show("No se pudo guardar correctamente", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EliminarBoton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductosBLL.Eliminar(Utilities.ToInt(ProductoIdTextbox.Text)))
            {
                Limpiar();
                MessageBox.Show("Producto borrado con éxito", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
                MessageBox.Show("No se puedo borrar correctamente", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool Validar()
        {
            DescripcionTextbox.Text = DescripcionTextbox.Text.Trim();

            if(DescripcionTextbox.Text.Length == 0)
            {
                MessageBox.Show("Introduzca una descripción", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (InventarioTextbox.Text.Length == 0)
            {
                MessageBox.Show("Introduzca una cantidad para el inventario", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (PrecioTextbox.Text.Length == 0)
            {
                MessageBox.Show("Introduzca un precio", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (Utilities.ToDouble(PrecioTextbox.Text) == 0)
            {
                MessageBox.Show("Introduzca un precio válido mayor a 0", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (CostoTextbox.Text.Length == 0)
            {
                MessageBox.Show("Introduzca un costo", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (Utilities.ToDouble(CostoTextbox.Text) == 0)
            {
                MessageBox.Show("Introduzca un costo válido mayor a 0", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (PorcentajeITBISCombobox.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un porcentaje de ITBIS", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (MargenGananciaTextbox.Text.Length == 0)
            {
                MessageBox.Show("Introduzca un margen de ganancia", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (Utilities.ToDouble(MargenGananciaTextbox.Text) == 0)
            {
                MessageBox.Show("Introduzca un margen de ganancia válido mayor a 0", "Registro de productos",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
