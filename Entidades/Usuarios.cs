using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    }
}
