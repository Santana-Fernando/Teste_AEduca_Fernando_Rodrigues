using Application.Students.ViewModel;
using Domain.Students.Entidades;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Application.Students.Interfaces
{
    public interface IStudentServices
    {
        HttpResponseMessage Add(StudentView student);
        Task<IEnumerable<StudentView>> GetList();
        Task<StudentView> GetById(int ra);
        HttpResponseMessage Update(StudentView student);
        HttpResponseMessage Remove(int ra);
    }
}
