using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entidades
{
    public class Facturas
    {
        [Key]
        public int FacturaId { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [Required]
        public int ClienteId { get; set; }
        
        [Required]
        public int VendedorId { get; set; }
        
        [Required]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double Total { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double ITBIS { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double Balance { get; set; }

        [ForeignKey("FacturaId")]
        public virtual List<FacturasDetalle> FacturasDetalle { get; set; }

        [ForeignKey("FacturaId")]
        public virtual List<CobrosDetalle> CobrosDetalle { get; set; }

        public Facturas()
        {
            Fecha = DateTime.Now;
            FacturasDetalle = new List<FacturasDetalle>();
            CobrosDetalle = new List<CobrosDetalle>();
        }
    }
    public class FacturasDetalle
    {
        [Key]
        public int FacturaDetalleId { get; set; }
        
        [Required]
        public int FacturaId { get; set; }
        
        [Required]
        public int ProductoId { get; set; }
        
        [Required]
        public float Cantidad { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double Precio { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double ITBIS { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double Total { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double Descuento { get; set; }
    }
}
