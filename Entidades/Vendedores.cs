using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entidades
{
    public class Vendedores
    {
        [Key]
        public int VendedorId { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [Required]
        public string Nombres { get; set; }
        
        [Required]
        public string Apellidos { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double Comision { get; set; }

        [ForeignKey("VendedorId")]
        public virtual List<Facturas> Facturas { get; set; }

        [ForeignKey("VendedorId")]
        public virtual List<Cobros> Cobros { get; set; }

        public Vendedores()
        {
            Facturas = new List<Facturas>();
            Cobros = new List<Cobros>();
        }
    }
}
