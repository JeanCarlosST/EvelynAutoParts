using DAL;
using Entidades;
using BLL;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Media;

namespace UI.Registros
{
    /// <summary>
    /// Interaction logic for rClientes.xaml
    /// </summary>
    public partial class rClientes : Window
    {
        private Clientes cliente;
        Usuarios usuario;
        public rClientes(Usuarios user)
        {
            InitializeComponent();
            usuario = user;
            Nuevo();

        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            Contexto context = new Contexto();

            var encontrado = ClientesBLL.Buscar(Convert.ToInt32(ClienteIdTextBox.Text));

            if (encontrado != null)
                this.cliente = encontrado;
            else
            {
                this.cliente = new Clientes();
                MessageBox.Show("No encontrado", "Error", MessageBoxButton.OK);
            }

            Cargar();
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Nuevo();
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validar())
                return;

            cliente.Cedula = CedulaMask.Value.ToString();
            cliente.Celular = CelularMask.Value.ToString();
            cliente.Telefono = TelefonoMask.Value.ToString();
            cliente.Email = EmailMask.Value.ToString();

            var paso = ClientesBLL.Guardar(cliente);
            if (paso)
            {
                OcultarLabel();
                Nuevo();
                MessageBox.Show("Guardado con Exito", "Exito!!", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Error al guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientesBLL.Eliminar(Convert.ToInt32(ClienteIdTextBox.Text)))
            {
                Nuevo();
                MessageBox.Show("Eliminado", "Exito", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Error al eliminar", "Error", MessageBoxButton.OK);

        }

        private void Cargar()
        {
            CedulaMask.Value = cliente.Cedula;
            CelularMask.Value = cliente.Celular;
            TelefonoMask.Value = cliente.Telefono;
            EmailMask.Value = cliente.Email;
            this.DataContext = cliente;

        }

        private void Nuevo()
        {
            this.cliente = new Clientes(){ UsuarioId = usuario.UsuarioId };
            this.cliente.Fecha = DateTime.Now;
            ClienteIdTextBox.Text = null;
            Cargar();
        }

        private bool Validar()
        {
            Contexto contexto = new Contexto();
            bool esValido = true;

            if (NombresTextBox.Text.Length < 3)
            {
                esValido = false;
                AdvertenciaNombreLabel.Content = "El nombre debe contener más de 2 caracteres";
                AdvertenciaNombreLabel.Visibility = Visibility.Visible;
            }

            if (ApellidosTextBox.Text.Length < 3)
            {
                esValido = false;
                AdvertenciaApellidoLabel.Content = "El apellido debe contener más de 2 caracteres";
                AdvertenciaApellidoLabel.Visibility = Visibility.Visible;
            }

            if (CelularMask.HasError || CelularMask.Text.Length < 2)
            {
                esValido = false;
                AdvertenciaCelularLabel.Content = "Debe completar el numero celular";
                AdvertenciaCelularLabel.Visibility = Visibility.Visible;
            }
            else if (ClientesBLL.Existe(2, convertInt(), CelularMask.Value.ToString()))
            {
                esValido = false;
                AdvertenciaCelularLabel.Content = "El numero de celular se encuentra registrado";
                AdvertenciaCelularLabel.Visibility = Visibility.Visible;
                SystemSounds.Beep.Play();

            }

            if (CedulaMask.HasError || CedulaMask.Text.Length < 2)
            {
                esValido = false;
                AdvertenciaCedulaLabel.Content = "Debe completar el numero de cedula";
                AdvertenciaCedulaLabel.Visibility = Visibility.Visible;
            }
            else if (ClientesBLL.Existe(3, convertInt(), CedulaMask.Value.ToString()))
            {
                esValido = false;
                AdvertenciaCedulaLabel.Content = "El numero de cedula se encuentra registrado";
                AdvertenciaCedulaLabel.Visibility = Visibility.Visible;
                SystemSounds.Beep.Play();
            }

            if (TelefonoMask.HasError || TelefonoMask.Text.Length < 2)
            {
                esValido = false;
                AdvertenciaTelefonoLabel.Content = "Debe completar el numero de Telefono";
                AdvertenciaTelefonoLabel.Visibility = Visibility.Visible;
            }
            else if (ClientesBLL.Existe(4, convertInt(), TelefonoMask.Value.ToString()))
            {
                esValido = false;
                AdvertenciaTelefonoLabel.Content = "El numero de telefono se encuentra registrado";
                AdvertenciaTelefonoLabel.Visibility = Visibility.Visible;
                SystemSounds.Beep.Play();
            }

            if (EmailMask.Text.Length > 2 && EmailMask.Text.Length < 5)
            {
                esValido = false;
                AdvertenciaEmailLabel.Content = "Debe completar el email";
                AdvertenciaEmailLabel.Visibility = Visibility.Visible;

                if (ClientesBLL.Existe(1, convertInt(), EmailMask.Text))
                {
                    esValido = false;
                    AdvertenciaEmailLabel.Content = "El email se encuentra registrado";
                    AdvertenciaEmailLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
            }           

            if (DireccionTextBox.Text.Length < 2)
            {
                esValido = false;
                AdvertenciaDireccionLabel.Visibility = Visibility.Visible;
                SystemSounds.Beep.Play();
            }

            if (CelularMask.Text.Length < 1 || TelefonoMask.Text.Length < 1)
            {
                esValido = false;
                MessageBox.Show("Debe ingresar un numero de telefono o celular", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return esValido;
        }

        private void NombresTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (NombresTextBox.Text.Any(Char.IsDigit) || NombresTextBox.Text.Any(Char.IsPunctuation) || NombresTextBox.Text.Any(Char.IsSymbol))
                {
                    NombresTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    AdvertenciaNombreLabel.Content = "El nombre solo debe contener letras";
                    AdvertenciaNombreLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
                else
                {
                    NombresTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaNombreLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                NombresTextBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void ApellidosTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (ApellidosTextBox.Text.Any(Char.IsDigit) || ApellidosTextBox.Text.Any(Char.IsPunctuation) || ApellidosTextBox.Text.Any(Char.IsSymbol))
                {
                    ApellidosTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    AdvertenciaApellidoLabel.Content = "El apellido solo debe contener letras";
                    AdvertenciaApellidoLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
                else
                {
                    ApellidosTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaApellidoLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                ApellidosTextBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void OcultarLabel()
        {
            AdvertenciaApellidoLabel.Visibility = Visibility.Hidden;
            AdvertenciaCedulaLabel.Visibility = Visibility.Hidden;
            AdvertenciaNombreLabel.Visibility = Visibility.Hidden;
            AdvertenciaTelefonoLabel.Visibility = Visibility.Hidden;
            AdvertenciaEmailLabel.Visibility = Visibility.Hidden;
            AdvertenciaDireccionLabel.Visibility = Visibility.Hidden;
            AdvertenciaCelularLabel.Visibility = Visibility.Hidden;
        }

        public int convertInt()
        {
            int numero;
            int.TryParse(ClienteIdTextBox.Text, out numero);
            return numero;
        }
    }
}
