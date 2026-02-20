using System.ComponentModel.DataAnnotations;

namespace tl2_tp8_2025_michdeaver.ViewModels
{
    public class ProductoViewModel
    {
        public int IdProducto {get; set;}
        
        [StringLength(250, ErrorMessage = "La descripcion tiene un maximo de 250 caracteres")]
        [Display(Name = "Descripcion del producto")]
        public string Descripcion {get; set;}
        
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El precio tiene que ser positivo")]
        [Display(Name = "Precio")]
        public int Precio {get; set;}
    }
}