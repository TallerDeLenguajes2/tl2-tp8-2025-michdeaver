using System.ComponentModel.DataAnnotations; // for validaciones
using Microsoft.AspNetCore.Mvc.Rendering; // for selectList

namespace tl2_tp8_2025_michdeaver.ViewModels
{
    public class AgregarProductoViewModel
    {
        public int IdPresupuesto {get; set;}

        [Display(Name = "Producto a agregar")]
        public int IdProducto {get; set;}

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantiad tiene que ser positiva")]
        public int Cantidad {get; set;}

        public SelectList ListaProductos {get; set;}
    }
}