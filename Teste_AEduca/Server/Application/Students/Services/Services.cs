using Application.Students.ViewModel;
using Application.Students.Interfaces;
using AutoMapper;
using Domain.Students.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Validation;
using System.Net;
using Application.Http;
using System.Linq;
using System.Text.Json;
using Domain.Students.Entidades;

namespace Application.Students.Services
{
    public class Services: IStudentServices
    {
        private IStudent _studentRepository;
        private IMapper _mapper;
        public Services(IStudent studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public HttpResponseMessage Add(StudentView student)
        {
            HttpResponse httpResponse = new HttpResponse();
            var validationFields = new ValidationFields(student).IsValidWithErrors();
            List<string> errorMessages = new List<string>(validationFields.ErrorMessages.Select(result => result.ErrorMessage));

            try
            {
                if (!validationFields.IsValid)
                {
                    string messageError = string.Join(", ", errorMessages);
                    return httpResponse.Response(HttpStatusCode.BadRequest,
                        new StringContent(JsonSerializer.Serialize(errorMessages)),
                        messageError);
                }

                Student studentRegistred = GetByCPF(student.CPF).Result;
                if (studentRegistred != null)
                {
                    return httpResponse.Response(HttpStatusCode.BadRequest, null, $"There is already a registered user with the CPF: {student.CPF}.");
                }

                var studentMap = _mapper.Map<Student>(student);
                _studentRepository.Add(studentMap);

                return httpResponse.Response(HttpStatusCode.OK, null, "OK");
            }
            catch (Exception ex)
            {
                return httpResponse.Response(HttpStatusCode.InternalServerError, new StringContent(JsonSerializer.Serialize(ex.Message)), "Internal server error");
            }
        }

        public async Task<IEnumerable<StudentView>> GetList()
        {
            var students = await _studentRepository.GetList();
            return _mapper.Map<IEnumerable<StudentView>>(students);
        }

        public async Task<StudentView> GetById(int ra)
        {
            Student tarefa = await _studentRepository.GetById(ra);
            return _mapper.Map<StudentView>(tarefa);
        }

        public HttpResponseMessage Remove(int ra)
        {
            HttpResponse httpResponse = new HttpResponse();

            var studentToRemove = _studentRepository.GetById(ra).Result;

            if (studentToRemove != null)
            {
                _studentRepository.Remove(studentToRemove);
                return httpResponse.Response(HttpStatusCode.OK, null, "OK");
            }

            return httpResponse.Response(HttpStatusCode.NotFound, null, "Student not found!");
        }

        public HttpResponseMessage Update(StudentView student)
        {
            HttpResponse httpResponse = new HttpResponse();
            var validationFields = new ValidationFields(student).IsValidWithErrors();
            List<string> errorMessages = new List<string>(validationFields.ErrorMessages.Select(result => result.ErrorMessage));

            if (!validationFields.IsValid)
            {
                string messageError = string.Join(", ", errorMessages);

                return httpResponse.Response(HttpStatusCode.BadRequest,
                    new StringContent(JsonSerializer.Serialize(errorMessages)),
                    messageError);
            }

            var studentToUpdate = _studentRepository.GetById(student.RA).Result;

            if (studentToUpdate != null)
            {
                studentToUpdate.Nome = student.Nome;
                studentToUpdate.Email = student.Email;

                _studentRepository.Update(studentToUpdate);
                return httpResponse.Response(HttpStatusCode.OK, null, "OK");
            }

            return httpResponse.Response(HttpStatusCode.NotFound, null, "Student not found!");
        }

        private async Task<Student> GetByCPF(string CPF)
        {
            return await _studentRepository.GetByCPF(CPF);
        }
    }
}
