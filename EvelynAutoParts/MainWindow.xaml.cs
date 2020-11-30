using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.Consultas;
using UI.Registros;

namespace EvelynAutoParts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Usuarios usuario;
        public MainWindow(Usuarios usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        //----------------------------------Registros--------------------------------------

        private void rClientesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new rClientes(usuario).Show();
        }

        private void rCobrosMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new rCobros(usuario).Show();
        }

        private void rFacturasMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new rFacturas(usuario).Show();
        }

        private void rProductosMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new rProductos(usuario).Show();
        }

        private void rUsuariosMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new rUsuarios().Show();
        }

        private void rVendedoresMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new rVendedores(usuario).Show();
        }

        //----------------------------------Consultas--------------------------------------

        private void cCobrosMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new cCobros().Show();
        }

        private void cClientesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new cClientes().Show();
        }

        private void cFacturasMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new cFacturas().Show();
        }
        private void cProductosMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new cProductos().Show();
        }

        private void cUsuariosMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new cUsuarios().Show();
        }

        private void cVendedoresMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new cVendedores().Show();
        }
    }
}
