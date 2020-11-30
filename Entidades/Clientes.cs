using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entidades
{
    public class Clientes
    {
        [Key]
        public int ClienteId { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [Required]
        public string Nombres { get; set; }
        
        [Required]
        public string Apellidos { get; set; }
        
        [Required]
        public string Direccion { get; set; }
        
        [Required]
        public string Cedula { get; set; }
        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        

        [ForeignKey("ClienteId")]
        public virtual List<Facturas> Facturas { get; set; }

        [ForeignKey("ClienteId")]
        public virtual List<Cobros> Cobros { get; set; }

        public Clientes()
        {
            Facturas = new List<Facturas>();
            Cobros = new List<Cobros>();
        }
    }
}
