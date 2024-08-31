using Domain.Students.Interfaces;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Students.Entidades;

namespace Infra.Data.Repository.Students
{
    public class StudentRepository : IStudent
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Student tarefa)
        {
            _context.Add(tarefa);
            _context.SaveChanges();
        }

        public async Task<Student> GetById(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<IEnumerable<Student>> GetList()
        {
            return await _context.Students.ToListAsync();
        }

        public void Update(Student tarefa)
        {
            _context.Update(tarefa);
            _context.SaveChanges();
        }

        public void Remove(Student tarefa)
        {
            _context.Remove(tarefa);
            _context.SaveChanges();
        }

        public async Task<Student> GetByCPF(string cpf)
        {
            return await _context.Students.SingleOrDefaultAsync(u => u.CPF == cpf);
        }
    }
}
