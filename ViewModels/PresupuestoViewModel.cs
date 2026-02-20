using System.ComponentModel.DataAnnotations;

namespace tl2_tp8_2025_michdeaver.ViewModels
{
    public class PresupuestoViewModel
    {
        public int IdPresupuesto {get; set;}

        [Display(Name = "Nombre Destinatario")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreDestinatario {get; set;}

        [Display(Name = "Fecha de Creacion")]
        [Required(ErrorMessage = "La fecha de creacion es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion {get; set;}
    }
}