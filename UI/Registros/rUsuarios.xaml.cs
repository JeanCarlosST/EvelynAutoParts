﻿using BLL;
using DAL;
using Entidades;
using System;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.Registros
{
    /// <summary>
    /// Interaction logic for rUsuarios.xaml
    /// </summary>
    public partial class rUsuarios : Window
    {
        private Usuarios usuario;

        public rUsuarios()
        {
            InitializeComponent();
            Limpiar();
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            Contexto context = new Contexto();

            var encontrado = UsuariosBLL.Buscar(Convert.ToInt32(UsuarioIdTextBox.Text));

            if (encontrado != null)
            {
                this.usuario = encontrado;
                ClaveStackPanel.Visibility = Visibility.Hidden;
                ConfirmarClaveStackPanel.Visibility = Visibility.Hidden;
                CambiarClaveButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.usuario = new Usuarios();
                MessageBox.Show("No encontrado", "Error", MessageBoxButton.OK);
            }

            this.DataContext = usuario;
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            this.usuario = new Usuarios();
            usuario.FechaCreacion = DateTime.Now;
            this.DataContext = usuario;
            UsuarioIdTextBox.Text = null;
            ClavePasswordBox.Password = null;
            ConfirmarClavePasswordBox.Password = null;
            ClaveStackPanel.Visibility = Visibility.Visible;
            ConfirmarClaveStackPanel.Visibility = Visibility.Visible;
            CambiarClaveButton.Visibility = Visibility.Hidden;
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validar())
                return;

            usuario.Clave = getHashSha256(ClavePasswordBox.Password.Trim());

            var paso = UsuariosBLL.Guardar(usuario);
            if (paso)
            {
                Limpiar();
                MessageBox.Show("Guardado con Exito", "Exito!!", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Error al guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsuariosBLL.Eliminar(Convert.ToInt32(UsuarioIdTextBox.Text)))
            {
                Limpiar();
                MessageBox.Show("Eliminado", "Exito", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Error al eliminar", "Error", MessageBoxButton.OK);

        }

        private void ConfirmarClaveTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ConfirmarClavePasswordBox.Password != ClavePasswordBox.Password)
                {
                    ConfirmarClavePasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    AdvertenciaConfirmarClaveLabel.Content = "Las claves no coinciden";
                    AdvertenciaConfirmarClaveLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    ConfirmarClavePasswordBox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaConfirmarClaveLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                ConfirmarClavePasswordBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void ClaveTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClavePasswordBox.Password.Any(Char.IsPunctuation))
                {
                    ClavePasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    AdvertenciaClaveLabel.Content = "La clave solo debe contener numeros, letras y simbolos";
                    AdvertenciaClaveLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
                else
                {
                    ClavePasswordBox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaClaveLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                ClavePasswordBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void NombreusuarioTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (NombreUsuarioTextBox.Text.Any(Char.IsPunctuation) || NombreUsuarioTextBox.Text.Any(Char.IsSymbol))
                {
                    NombreUsuarioTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    AdvertenciaNombreUsuarioLabel.Content = "El nombre usuario solo debe tener numeros y letras.";
                    AdvertenciaNombreUsuarioLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
                else if (NombreUsuarioTextBox.Text.Any(Char.IsWhiteSpace))
                {
                    NombreUsuarioTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    AdvertenciaNombreUsuarioLabel.Content = "No debe tener espacios en blanco.";
                    AdvertenciaNombreUsuarioLabel.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                }
                else
                {
                    NombreUsuarioTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
                    AdvertenciaNombreUsuarioLabel.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                NombreUsuarioTextBox.Foreground = SystemColors.ControlTextBrush;
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
            else if (!NombresTextBox.Text.Any(char.IsLetter))
            {
                esValido = false;
                AdvertenciaApellidoLabel.Content = "El nombre solo debe contener letras";
                AdvertenciaApellidoLabel.Visibility = Visibility.Visible;
            }

            if (ApellidosTextBox.Text.Length < 3)
            {
                esValido = false;
                AdvertenciaApellidoLabel.Content = "El apellido debe contener más de 2 caracteres";
                AdvertenciaApellidoLabel.Visibility = Visibility.Visible;
            }
            else if (!ApellidosTextBox.Text.Any(char.IsLetter))
            {
                esValido = false;
                AdvertenciaApellidoLabel.Content = "El apellido solo debe contener letras";
                AdvertenciaApellidoLabel.Visibility = Visibility.Visible;
            }

            if (NombreUsuarioTextBox.Text.Length < 6)
            {
                esValido = false;
                AdvertenciaNombreUsuarioLabel.Content = "El nombre de usuario debe tener minimo 6 caracteres";
                AdvertenciaNombreUsuarioLabel.Visibility = Visibility.Visible;
            }
            else if (!NombreUsuarioTextBox.Text.Any(char.IsLetterOrDigit))
            {
                esValido = false;
                AdvertenciaNombreUsuarioLabel.Content = "El nombre de usuario solo debe contener numeros y letras";
                AdvertenciaNombreUsuarioLabel.Visibility = Visibility.Visible;
            }
            else if (NombreUsuarioTextBox.Text.Any(Char.IsWhiteSpace))
            {
                esValido = false;
                AdvertenciaNombreUsuarioLabel.Content = "No debe tener espacios en blanco.";
                AdvertenciaNombreUsuarioLabel.Visibility = Visibility.Visible;
            }
            else if (NombreUsuarioTextBox.Text.All(char.IsDigit))
            {
                AdvertenciaNombreUsuarioLabel.Content = "Debe incluir letras";
                AdvertenciaNombreUsuarioLabel.Visibility = Visibility.Visible;
            }
            else if (UsuariosBLL.Existe(convertInt(), usuario.NombreUsuario))
            {
                esValido = false;
                AdvertenciaNombreUsuarioLabel.Content = "Este usuario se encuentra registrado";
                AdvertenciaNombreUsuarioLabel.Visibility = Visibility.Visible;
            }

            if (ClavePasswordBox.Password.Length < 6)
            {
                esValido = false;
                AdvertenciaClaveLabel.Content = "La clave debe tener minimo 6 caracteres";
                AdvertenciaClaveLabel.Visibility = Visibility.Visible;
            }
            else if (ClavePasswordBox.Password.Any(Char.IsPunctuation))
            {
                esValido = false;
                AdvertenciaClaveLabel.Content = "La clave solo debe contener numeros, letras y simbolos";
                AdvertenciaClaveLabel.Visibility = Visibility.Visible;
            }


            return esValido;
        }
        public int convertInt()
        {
            int numero;
            int.TryParse(UsuarioIdTextBox.Text, out numero);
            return numero;
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

        private void CambiarClaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(usuario.UsuarioId.ToString());
            CambiarClave cambio = new CambiarClave(usuario.UsuarioId);
            cambio.Show();
        }
    }
}
