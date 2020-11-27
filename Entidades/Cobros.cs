using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entidades
{
    public class Cobros
    {
        [Key]
        public int CobroId { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [Required]
        public int ClienteId { get; set; }
        
        [Required]
        public int VendedorId { get; set; }
        
        [Required]
        public DateTime Fecha { get; set; }
        
        [Column(TypeName="money")]
        [Required]
        public double Total { get; set; }

        [ForeignKey("CobroId")]
        public virtual List<CobrosDetalle> Detalle { get; set; }
    }
    public class CobrosDetalle
    {
        [Key]
        public int CobroDetalleId { get; set; }
        
        [Required]
        public int CobroId { get; set; }
        
        [Required]
        public int FacturaId { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double Monto { get; set; }
    }
}
