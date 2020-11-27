using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entidades
{
    public class Productos
    {
        [Key]
        public int ProductoId { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [Required]
        public string Descripcion { get; set; }
        
        [Required]
        public float Inventario { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double Precio { get; set; }
        
        [Column(TypeName = "money")]
        [Required]
        public double Costo { get; set; }
       
        [Required]
        public float PorcentajeITBIS { get; set; }
        
        [Required]
        public float MargenGanancia { get; set; }

        [ForeignKey("ProductoId")]
        public virtual List<FacturasDetalle> FacturasDetalle { get; set; }
    }
}
