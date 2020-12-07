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

        private void BuscarBoton_Click(object sender, RoutedEventArgs e)
        {
            Contexto context = new Contexto();

            var encontrado = ClientesBLL.Buscar(Convert.ToInt32(ClienteIdTextBox.Text));

            if (encontrado != null)
                this.cliente = encontrado;
            else
            {
                this.cliente = new Clientes();
                MessageBox.Show("Cliente no encontrado", "Registro de clientes");
            }

            Cargar();
        }

        private void NuevoBoton_Click(object sender, RoutedEventArgs e)
        {
            Nuevo();
        }

        private void GuardarBoton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validar())
                return;

            cliente.Cedula = CedulaMask.Value.ToString();
            cliente.Telefono = TelefonoMask.Value.ToString();
            

            if(CelularMask.Value != null)
                cliente.Celular = CelularMask.Value.ToString();

            if (EmailMask.Value != null)
                cliente.Email = EmailMask.Value.ToString();

            var paso = ClientesBLL.Guardar(cliente);
            if (paso)
            {
                OcultarLabel();
                Nuevo();
                MessageBox.Show("Cliente guardado con éxito.", "Registro de clientes",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("No fue posible guardar", "Registro de clientes",
                               MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void EliminarBoton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientesBLL.Eliminar(Convert.ToInt32(ClienteIdTextBox.Text)))
            {
                Nuevo();
                MessageBox.Show("Cliente eliminado", "Registro de clientes",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("No se pudo eliminar el cliente", "Registro de clientes",
                                MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (!ValidarNombres())
                esValido = false;

            if (!ValidarApellidos())
                esValido = false;

            if(!ValidarCedula())
                esValido = false;
            else if (ClientesBLL.Existe(3, convertInt(), CedulaMask.Value.ToString()))
            {
                esValido = false;
                AdvertenciaCedulaLabel.Text = "El número de cédula se encuentra registrado";
                AdvertenciaCedulaLabel.Visibility = Visibility.Visible;
                SystemSounds.Beep.Play();
            }

            if (!ValidarTelefono())
                esValido = false;
            else if (ClientesBLL.Existe(4, convertInt(), TelefonoMask.Value.ToString()))
            {
                esValido = false;
                AdvertenciaTelefonoLabel.Text = "El número de teléfono se encuentra registrado";
                AdvertenciaTelefonoLabel.Visibility = Visibility.Visible;
                SystemSounds.Beep.Play();
            }
            if(CelularMask.Text.Length >= 11) { 
                if (!ValidarCelular())
                    esValido = false;
                else if (ClientesBLL.Existe(2, convertInt(), CelularMask.Value.ToString()))
                {
                    esValido = false;
                    AdvertenciaCelularLabel.Text = "El número de celular se encuentra registrado";
                    AdvertenciaCelularLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
            }

            if (!ValidarEmail())
                esValido = false;

            if (DireccionTextBox.Text.Length < 2)
            {
                esValido = false;
                AdvertenciaDireccionLabel.Visibility = Visibility.Visible;
                SystemSounds.Beep.Play();
            }

            if (TelefonoMask.Text.Length < 1)
            {
                esValido = false;
                MessageBox.Show("Debe ingresar un número de teléfono", "Registro de clientes", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return esValido;
        }

        private void NombresTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!ValidarNombres() && NombresTextBox.Text.Length > 0)
                {  
                    AdvertenciaNombreLabel.Visibility = Visibility.Visible;
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
                if (!ValidarApellidos() && ApellidosTextBox.Text.Length > 0)
                {
                    AdvertenciaApellidoLabel.Visibility = Visibility.Visible;
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

        public bool ValidarNombres()
        {
            bool esValido = true;
            if (NombresTextBox.Text.Any(Char.IsDigit) || NombresTextBox.Text.Any(Char.IsPunctuation) || NombresTextBox.Text.Any(Char.IsSymbol))
            {
                esValido = false;
                NombresTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                AdvertenciaNombreLabel.Text = "El nombre solo debe contener letras";
            }
            else if (NombresTextBox.Text.Length < 3)
            {
                esValido = false;
                AdvertenciaNombreLabel.Text = "El nombre debe contener más de 2 caracteres";
                AdvertenciaNombreLabel.Visibility = Visibility.Visible;
            }

            return esValido;
        }

        public bool ValidarApellidos()
        {
            bool esValido = true;
            if (ApellidosTextBox.Text.Any(Char.IsDigit) || ApellidosTextBox.Text.Any(Char.IsPunctuation) || ApellidosTextBox.Text.Any(Char.IsSymbol))
            {
                esValido = false;
                ApellidosTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                AdvertenciaApellidoLabel.Text = "El apellido solo debe contener letras";
            }
            else if (ApellidosTextBox.Text.Length < 3)
            {
                esValido = false;
                AdvertenciaApellidoLabel.Text = "El apellido debe contener más de 2 caracteres";
                AdvertenciaApellidoLabel.Visibility = Visibility.Visible;
            }

            return esValido;
        }

        public bool ValidarCelular()
        {
            bool esValido = true;
            if (CelularMask.HasError || CelularMask.Text.Length < 11)
            {
                esValido = false;
                AdvertenciaCelularLabel.Text = "Debe completar el número celular";
                AdvertenciaCelularLabel.Visibility = Visibility.Visible;
            }
            

            return esValido;
        }

        public bool ValidarCedula()
        {
            bool esValido = true;
            if (CedulaMask.HasError || CedulaMask.Text.Length < 11)
            {
                esValido = false;
                AdvertenciaCedulaLabel.Text = "Debe completar el numero de cédula";
                AdvertenciaCedulaLabel.Visibility = Visibility.Visible;
            }

            return esValido;
        }

        public bool ValidarTelefono()
        {
            bool esValido = true;
            if (TelefonoMask.HasError || TelefonoMask.Text.Length < 11)
            {
                esValido = false;
                AdvertenciaTelefonoLabel.Text = "Debe completar el número de teléfono";
                AdvertenciaTelefonoLabel.Visibility = Visibility.Visible;
            }
            

            return esValido;
        }
        public bool ValidarEmail()
        {
            bool esValido = true;
            if (EmailMask.Text.Length >= 1 && EmailMask.Text.Length < 5)
            {
                esValido = false;
                AdvertenciaEmailLabel.Text = "Debe completar el email";
                AdvertenciaEmailLabel.Visibility = Visibility.Visible;

                if (ClientesBLL.Existe(1, convertInt(), EmailMask.Text))
                {
                    esValido = false;
                    AdvertenciaEmailLabel.Text = "El email se encuentra registrado";
                    AdvertenciaEmailLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
            }

            return esValido;
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

        private void TelefonoMask_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            { 
                TelefonoMask.BorderBrush = new SolidColorBrush(Colors.Black);
                AdvertenciaTelefonoLabel.Visibility = Visibility.Hidden;
            }
            catch
            {
                TelefonoMask.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void CelularMask_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CelularMask.BorderBrush = new SolidColorBrush(Colors.Black);
                AdvertenciaCelularLabel.Visibility = Visibility.Hidden;
            }
            catch
            {
                CelularMask.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void CedulaMask_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
             
                CedulaMask.BorderBrush = new SolidColorBrush(Colors.Black);
                AdvertenciaCedulaLabel.Visibility = Visibility.Hidden;
                
            }
            catch
            {
                CedulaMask.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void EmailMask_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!ValidarEmail())
                {
                    AdvertenciaEmailLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
                else
                {
                    EmailMask.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaEmailLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                    EmailMask.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void DireccionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                DireccionTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
                AdvertenciaDireccionLabel.Visibility = Visibility.Hidden;

            }
            catch
            {
                DireccionTextBox.Foreground = SystemColors.ControlTextBrush;
            }
        }
    }
}
