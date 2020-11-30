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
    /// Interaction logic for rFacturas.xaml
    /// </summary>
    public partial class rFacturas : Window
    {
        Usuarios usuario;
        Facturas factura;
        List<ProductoDetalle> detalle;
        public rFacturas(Usuarios user)
        {
            InitializeComponent();
            usuario = user;
            factura = new Facturas(){ UsuarioId = usuario.UsuarioId };
            DataContext = factura;

            detalle = new List<ProductoDetalle>();

            ClienteCombobox.ItemsSource = ClientesBLL.GetList(c => true);
            ClienteCombobox.SelectedValuePath = "ClienteId";
            ClienteCombobox.DisplayMemberPath = "Nombres";


            VendedorCombobox.ItemsSource = VendedoresBLL.GetList(v => true);
            VendedorCombobox.SelectedValuePath = "VendedorId";
            VendedorCombobox.DisplayMemberPath = "Nombres";

            ProductoCombobox.ItemsSource =ProductosBLL.GetList(p => true);
            ProductoCombobox.SelectedValuePath = "ProductoId";
            ProductoCombobox.DisplayMemberPath = "Descripcion";
        }

        private void Limpiar()
        {
            factura = new Facturas() { UsuarioId = usuario.UsuarioId };
            DataContext = factura;
            detalle = new List<ProductoDetalle>();
            Actualizar();
        }

        private void Actualizar()
        {
            DataContext = null;
            DataContext = factura;

            ProductosDataGrid.ItemsSource = null;
            ProductosDataGrid.ItemsSource = detalle;

            ProductoCombobox.SelectedIndex = -1;
            PrecioTextbox.Clear();
            CantidadTextbox.Clear();
        }

        private void RemoverBoton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductosDataGrid.Items.Count > 0 && ProductosDataGrid.SelectedIndex <= ProductosDataGrid.Items.Count - 1)
            {
                ProductoDetalle d = (ProductoDetalle)ProductosDataGrid.SelectedItem;

                FacturasDetalle detalle = factura.FacturasDetalle.Where(p => p.ProductoId == d.ProductoId).First();

                factura.Total -= detalle.Cantidad * detalle.Precio;
                factura.FacturasDetalle.Remove(detalle);

                this.detalle.Remove(d);

                Actualizar();
            }
        }

        private void BuscarBoton_Click(object sender, RoutedEventArgs e)
        {
            Facturas anterior = FacturasBLL.Buscar(Utilities.ToInt(FacturaIdTextbox.Text));

            if (anterior != null)
            {
                Limpiar();

                factura = anterior;

                foreach (FacturasDetalle f in factura.FacturasDetalle)
                {
                    this.detalle.Add(new ProductoDetalle(f));
                }

                Actualizar();
            }
            else
            {
                MessageBox.Show("Orden no encontrada.", "Registro de facturas");
            }
        }

        private void AgregarBoton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarDetalle())
                return;

            FacturasDetalle nuevoDetalle = new FacturasDetalle()
            {
                FacturaId = Utilities.ToInt(FacturaIdTextbox.Text),
                ProductoId = Utilities.ToInt(ProductoCombobox.SelectedValue.ToString()),
                Cantidad = Utilities.ToFloat(CantidadTextbox.Text),
                Precio = Utilities.ToDouble(PrecioTextbox.Text)
            };

            foreach (FacturasDetalle d in factura.FacturasDetalle)
            {
                if (d.ProductoId == nuevoDetalle.ProductoId)
                {
                    factura.Total -= d.Cantidad * d.Precio; // Falta logica aqui
                    factura.FacturasDetalle.Remove(d);

                    this.detalle.RemoveAll(de => de.ProductoId == d.ProductoId);
                    break;
                }
            }

            factura.FacturasDetalle.Add(nuevoDetalle);
            factura.Total += nuevoDetalle.Cantidad * nuevoDetalle.Precio;

            this.detalle.Add(new ProductoDetalle(nuevoDetalle));

            Actualizar();
        }

        private void NuevoBoton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void GuardarBoton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarFactura())
                return;

            bool paso = FacturasBLL.Guardar(factura);

            if (paso)
            {
                Limpiar();
                MessageBox.Show("Factura guardada con éxito.", "Registro de facturas", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No fue posible guardar.", "Registro de facturas", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminarBoton_Click(object sender, RoutedEventArgs e)
        {
            int id = Utilities.ToInt(FacturaIdTextbox.Text);

            Limpiar();

            if (FacturasBLL.Eliminar(id))
                MessageBox.Show("Factura eliminada.", "Registro de facturas", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("No se pudo eliminar la factura", "Registro de facturas", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool ValidarDetalle()
        {
            if(ProductoCombobox.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un producto", "Registro de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (Utilities.ToFloat(CantidadTextbox.Text) == 0)
            {
                MessageBox.Show("Introduzca una cantidad válida que sea mayor a cero", "Registro de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private bool ValidarFactura()
        {
            if(!DateTime.TryParse(FechaDatePicker.Text, out _))
            {
                MessageBox.Show("Introduzca una fecha válida", "Registro de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if(VendedorCombobox.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un vendedor", "Registro de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if(factura.FacturasDetalle.Count == 0)
            {
                MessageBox.Show("Ingrese por lo menos un producto en la factura", "Registro de facturas",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void ProductoCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductoCombobox.SelectedIndex != -1)
                PrecioTextbox.Text = ((Productos)ProductoCombobox.SelectedItem).Precio.ToString("N2");
        }

    }

    public class ProductoDetalle
    {
        public int ProductoId { get; set; }
        public int FacturaId { get; set; }
        public string Descripcion { get; set; }
        public float Cantidad { get; set; }
        public double Precio { get; set; }
        public double Subtotal { get; set; }
        public double ITBIS { get; set; }
        public double Total { get; set; }
        public double Descuento { get; set; }

        public ProductoDetalle(FacturasDetalle detalle)
        {
            Productos p = ProductosBLL.Buscar(detalle.ProductoId);

            ProductoId = detalle.ProductoId;
            FacturaId = detalle.FacturaId;
            Descripcion = p.Descripcion;
            Cantidad = detalle.Cantidad;
            Precio = detalle.Precio;
            Subtotal = Cantidad * Precio;
            ITBIS = Subtotal * p.PorcentajeITBIS;
            Total = Subtotal + ITBIS;
            Descuento = detalle.Descuento;
        }
    }
}
