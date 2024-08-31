using Domain.Students.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Students.Interfaces
{
    public interface IStudent
    {
        void Add(Student aluno);
        Task<IEnumerable<Student>> GetList();
        Task<Student> GetById(int ra);
        Task<Student> GetByCPF(string cpf);
        void Update(Student aluno);
        void Remove(Student aluno);
    }
}
