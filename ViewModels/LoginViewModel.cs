using System.ComponentModel.DataAnnotations;

namespace tl2_tp8_2025_michdeaver.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo usuario es obligatorio")]
        public string Username {get; set;}

        [Display(Name = "Contrasenia")]
        [Required(ErrorMessage = "El campo contrasenia es obligatorio")]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        public string ErrorMessaje {get; set;}
    }
}