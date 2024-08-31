using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Students.ViewModel
{
    public class StudentView
    {
        public int RA { get; set; }

        [Required(ErrorMessage = "The field Nome is required")]
        [MinLength(10)]
        [MaxLength(100)]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "The field E-mail is required")]
        [MinLength(10)]
        [MaxLength(100)]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field CPF is required")]
        [MinLength(11)]
        [MaxLength(11)]
        public string CPF { get; set; }

    }
}
