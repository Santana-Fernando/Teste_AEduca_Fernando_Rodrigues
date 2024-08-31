using System.ComponentModel.DataAnnotations;

namespace Domain.Students.Entidades
{
    public class Student
    {
        [Key]
        public int RA { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
    }
}
