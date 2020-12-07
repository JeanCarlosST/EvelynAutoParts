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
                MessageBox.Show("No se encontró ningún producto", "Registro de productos",
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
            bool esValido = true;
            DescripcionTextbox.Text = DescripcionTextbox.Text.Trim();

            if(DescripcionTextbox.Text.Length == 0)
            {
                esValido = false;
                AdvertenciaDescripcionLabel.Content = "Introduzca una descripción";
                AdvertenciaDescripcionLabel.Visibility = Visibility.Visible;
            }
            if (Utilities.ToDouble(InventarioTextbox.Text) <= 0)
            {
                esValido = false;
                AdvertenciaInventarioLabel.Text = "Introduzca una cantidad para el inventario válido mayor a 0";
                AdvertenciaInventarioLabel.Visibility = Visibility.Visible;
            }
            if (PorcentajeITBISCombobox.SelectedIndex < 0)
            {
                esValido = false;
                AdvertenciaItbisLabel.Text = "Seleccione un porcentaje de ITBIS";
                AdvertenciaItbisLabel.Visibility = Visibility.Visible;
            }
            if (!ValidarGanancia())
                esValido = false;

            if (!ValidarMaxDescuento())
                esValido = false;

            if (!ValidarPrecio())
                esValido = false;

            if (!ValidarCosto())
                esValido = false;

            return esValido;
        }

        public bool ValidarGanancia()
        {
            bool esValido = true;
            if (MargenGananciaTextbox.Text.Length == 0)
            {
                esValido = false;
                AdvertenciaGananciaLabel.Text = "Introduzca un margen de ganancia";
                AdvertenciaGananciaLabel.Visibility = Visibility.Visible;
            }
            else if (Utilities.ToDouble(MargenGananciaTextbox.Text) <= 0)
            {
                esValido = false;
                AdvertenciaGananciaLabel.Text = "Introduzca un margen de ganancia válido mayor a 0";
                AdvertenciaGananciaLabel.Visibility = Visibility.Visible;
            }
            else if (Utilities.ToFloat(MargenGananciaTextbox.Text) > 1.0 && Utilities.ToFloat(MargenGananciaTextbox.Text) < 0.0)
            {
                esValido = false;
                AdvertenciaGananciaLabel.Text =  "Margen de ganancia no puede solo puede aceptar número del 1 al 0";
                AdvertenciaGananciaLabel.Visibility = Visibility.Visible;
            }
            else if (!MargenGananciaTextbox.Text.Any(Char.IsDigit))
            {
                esValido = false;
                AdvertenciaGananciaLabel.Text = "Solo debe introducir digitos";
                AdvertenciaGananciaLabel.Visibility = Visibility.Visible;
            }

            return esValido;
        }
        public bool ValidarMaxDescuento()
        {
            bool esValido = true;
            if (Utilities.ToDouble(MaxDescuentoTextbox.Text) <= 0)
            {
                esValido = false;
                AdvertenciaDescuentoLabel.Text = "Introduzca un porcentaje valido";
                AdvertenciaDescuentoLabel.Visibility = Visibility.Visible;
            }
            else if (MaxDescuentoTextbox.Text.Any(char.IsLetter))
            {
                esValido = false;
                AdvertenciaDescuentoLabel.Text = "Introduzca un porcentaje valido";
                AdvertenciaDescuentoLabel.Visibility = Visibility.Visible;
            }
            else if (Utilities.ToFloat(MaxDescuentoTextbox.Text) > 1.0 && Utilities.ToFloat(MaxDescuentoTextbox.Text) < 0.0)
            {
                esValido = false;
                AdvertenciaDescuentoLabel.Text =  "Máximo descuento no puede solo puede aceptar número del 1 al 0";
                AdvertenciaDescuentoLabel.Visibility = Visibility.Visible;
            }
            else if (Utilities.ToFloat(MaxDescuentoTextbox.Text) > Utilities.ToFloat(MargenGananciaTextbox.Text))
            {
                esValido = false;
                AdvertenciaDescuentoLabel.Text = "Máximo descuento no puede ser mayor a margen de ganancia";
                AdvertenciaDescuentoLabel.Visibility = Visibility.Visible;
            }

            return esValido;
        }
        public bool ValidarCosto()
        {
            bool esValido = true;
            if (CostoTextbox.Text.Length == 0)
            {
                esValido = false;
                AdvertenciaCostoLabel.Text = "Introduzca un costo";
                AdvertenciaCostoLabel.Visibility = Visibility.Visible;
            }
            else if (Utilities.ToDouble(CostoTextbox.Text) <= 0)
            {
                esValido = false;
                AdvertenciaCostoLabel.Text = "Introduzca un costo válido mayor a 0";
                AdvertenciaCostoLabel.Visibility = Visibility.Visible;
            }
            else if (!CostoTextbox.Text.Any(Char.IsDigit))
            {
                esValido = false;
                AdvertenciaCostoLabel.Text = "Solo debe introducir digitos";
                AdvertenciaCostoLabel.Visibility = Visibility.Visible;
            }

            return esValido;
        }

        public bool ValidarPrecio()
        {
            bool esValido = true;
            if (PrecioTextbox.Text.Length == 0)
            {
                esValido = false;
                AdvertenciaPrecioLabel.Text = "Introduzca un precio";
                AdvertenciaPrecioLabel.Visibility = Visibility.Visible;
            }
            else if (Utilities.ToDouble(PrecioTextbox.Text) <= 0)
            {
                esValido = false;
                AdvertenciaPrecioLabel.Text = "Introduzca un precio válido mayor a 0";
                AdvertenciaPrecioLabel.Visibility = Visibility.Visible;
            }
            else if (!PrecioTextbox.Text.Any(Char.IsDigit))
            {
                esValido = false;
                AdvertenciaPrecioLabel.Text = "Solo debe introducir digitos";
                AdvertenciaPrecioLabel.Visibility = Visibility.Visible;
            }

            return esValido;
        }

        private void InventarioTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!InventarioTextbox.Text.Any(Char.IsDigit) && InventarioTextbox.Text.Length > 0)
                {
                    AdvertenciaInventarioLabel.Text = "Solo debe ingresar digitos";
                    AdvertenciaInventarioLabel.Visibility = Visibility.Visible;
                    InventarioTextbox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    InventarioTextbox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaInventarioLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                InventarioTextbox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void PrecioTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!ValidarPrecio() && PrecioTextbox.Text.Length > 1)
                {
                    AdvertenciaPrecioLabel.Visibility = Visibility.Visible;
                    PrecioTextbox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    PrecioTextbox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaPrecioLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                PrecioTextbox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void CostoTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!ValidarCosto() && CostoTextbox.Text.Length > 1)
                {
                    AdvertenciaCostoLabel.Visibility = Visibility.Visible;
                    CostoTextbox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    CostoTextbox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaCostoLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                CostoTextbox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void MaxDescuentoTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!ValidarMaxDescuento() && MaxDescuentoTextbox.Text.Length > 1)
                {
                    AdvertenciaDescuentoLabel.Visibility = Visibility.Visible;
                    MaxDescuentoTextbox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    MaxDescuentoTextbox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaDescuentoLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                MaxDescuentoTextbox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void MargenGananciaTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!ValidarGanancia() && MargenGananciaTextbox.Text.Length > 1)
                {
                    AdvertenciaGananciaLabel.Visibility = Visibility.Visible;
                    MargenGananciaTextbox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    MargenGananciaTextbox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaGananciaLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                MargenGananciaTextbox.Foreground = SystemColors.ControlTextBrush;
            }
        }
    }
}
