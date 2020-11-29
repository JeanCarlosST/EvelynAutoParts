using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for CambiarClave.xaml
    /// </summary>
    public partial class CambiarClave : Window
    {
        private int usuario;
        public CambiarClave(int id)
        {
            InitializeComponent();
            usuario = id;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Validar())
                return;

            var paso = UsuariosBLL.Buscar(usuario).Clave = GetHashSha256(ClaveActualPasswordBox.Password.Trim());
            if (paso != null)
            {
                //OcultarLabel();
                this.Close();
                MessageBox.Show("Clave Cambiada con exitos!!", "Exito!!", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Error al guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private bool Validar()
        {
            Contexto contexto = new Contexto();
            bool esValido = true;

            if (UsuariosBLL.Buscar(usuario).Clave != GetHashSha256(ClaveActualPasswordBox.Password.Trim()))
            {
                esValido = false;
                AdvertenciaClaveLabel.Visibility = Visibility.Visible;
            }
            if (NuevaClavePasswordBox.Password.Length < 6)
            {
                esValido = false;
                AdvertenciaClaveLabel.Content = "La clave debe tener minimo 6 caracteres";
                AdvertenciaClaveLabel.Visibility = Visibility.Visible;
            }
            else if (NuevaClavePasswordBox.Password.Any(Char.IsPunctuation))
            {
                esValido = false;
                AdvertenciaClaveLabel.Content = "La clave solo debe contener numeros, letras y simbolos";
                AdvertenciaClaveLabel.Visibility = Visibility.Visible;
            }
            else if (NuevaClavePasswordBox.Password.Length < 6)
            {
                esValido = false;
                AdvertenciaClaveLabel.Content = "La clave debe tener minimo 6 caracteres";
                AdvertenciaClaveLabel.Visibility = Visibility.Visible;
            }
            if (NuevaClavePasswordBox.Password != ConfirmarNuevaClavePasswordBox.Password)
            {
                esValido = false;
                MessageBox.Show("Las claves no coinciden", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return esValido;
        }

        public static string getHashSha256(string clave)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(clave);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

        private void NuevaClavePasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NuevaClavePasswordBox.Password.Any(Char.IsPunctuation))
                {
                    NuevaClavePasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    AdvertenciaClaveLabel.Content = "La clave solo debe contener numeros, letras y simbolos";
                    AdvertenciaClaveLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
                else if (NuevaClavePasswordBox.Password.Any(Char.IsPunctuation))
                {
                    NuevaClavePasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    AdvertenciaClaveLabel.Content = "La clave debe tener minimo 6 caracteres";
                    AdvertenciaClaveLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
                else
                {
                    NuevaClavePasswordBox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaClaveLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                NuevaClavePasswordBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void ConfirmarNuevaClavePasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ConfirmarNuevaClavePasswordBox.Password != NuevaClavePasswordBox.Password)
                {
                    ConfirmarNuevaClavePasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    AdvertenciaConfirmarNuevaClaveLabel.Content = "Las claves no coinciden";
                    AdvertenciaConfirmarNuevaClaveLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    ConfirmarNuevaClavePasswordBox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaConfirmarNuevaClaveLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                ConfirmarNuevaClavePasswordBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        public static string GetHashSha256(string clave)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(clave);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}