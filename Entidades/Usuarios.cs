using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Entidades
{
    public class Usuarios
    {
        [Key]
        public int UsuarioId { get; set; }
        
        [Required]
        public string Nombres { get; set; }
        
        [Required]
        public string Apellidos { get; set; }
        
        [Required]
        public string NombreUsuario { get; set; }
        
        [Required]
        public string Clave { get; set; }
        
        [Required]
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual List<Cobros> Cobros { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual List<Vendedores> Vendedores { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual List<Clientes> Clientes { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual List<Facturas> Facturas { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual List<Productos> Productos { get; set; }

        public Usuarios()
        {
            FechaCreacion = DateTime.Now;
            Cobros = new List<Cobros>();
            Vendedores = new List<Vendedores>();
            Clientes = new List<Clientes>();
            Facturas = new List<Facturas>();
            Productos = new List<Productos>();
        }

        public Usuarios(int id, string nombreUsuario, string clave, string nombres, string apellidos)
        {
            UsuarioId = id;
            NombreUsuario = nombreUsuario;
            Clave = getHashSha256(clave);
            Nombres = nombres;
            Apellidos = apellidos;
            FechaCreacion = DateTime.Now;
            Cobros = new List<Cobros>();
            Vendedores = new List<Vendedores>();
            Clientes = new List<Clientes>();
            Facturas = new List<Facturas>();
            Productos = new List<Productos>();
        }

        public static string getHashSha256(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
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
