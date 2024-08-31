using Xunit.Abstractions;
using Xunit;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Moq;
using Tests.Helper;
using Domain.Students.Entidades;
using System.Linq;
using System.Collections.Generic;
using System;
using Infra.Data.Repository.Students;

namespace Tests.Students
{
    public class StudentRepositoryTest
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly StudentRepository _studentRepository;
        public StudentRepositoryTest(ITestOutputHelper output)
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            _dbContext = new ApplicationDbContext(options);
            _studentRepository = new StudentRepository(_dbContext);

        }


        [Fact(DisplayName = "Should call the function Add")]
        public async Task StudentRepository_ShouldCallFunctionAdd()
        {
            new AppSettingsMock().RemoverAllStudents();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var student = new Domain.Students.Entidades.Student
            {
                Nome = "system2",
                Email = "system2@gmail.com",
                CPF = "39788175007"
            };

            try
            {
                _studentRepository.Add(student);

                var studentInserida = await _dbContext.Students.SingleOrDefaultAsync(u => u.RA == student.RA);
                Assert.NotNull(studentInserida);
                Assert.True(studentInserida.RA > 0);

                new AppSettingsMock().RemoverAllStudents();
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact(DisplayName = "Should call the function GetByRA")]
        public async Task StudentRepository_ShouldCallFunctionGetByRA()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var student = await _studentRepository.GetById(1);

                Assert.NotNull(student);
                Assert.True(student.RA == 1);
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact(DisplayName = "Should call the function GetList")]
        public async Task StudentRepository_ShouldCallFunctionGetList()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            
            try
            {
                var student = await _studentRepository.GetList();

                Assert.NotNull(student);
                Assert.True(student.ToList().Count > 0);
}
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact(DisplayName = "Should call the function Update")]
        public async Task StudentRepository_ShouldCallFunctionUpdate()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var studentInserida = await _dbContext.Students.SingleOrDefaultAsync(u => u.Nome == "system");

                Assert.NotNull(studentInserida);
                Assert.True(studentInserida.Nome == "system");

                studentInserida.Nome = "system2";

                _studentRepository.Update(studentInserida);

                var studentModificado = await _dbContext.Students.SingleOrDefaultAsync(u => u.Nome == "system2");

                Assert.NotNull(studentModificado);
                Assert.True(studentModificado.Nome == "system2");

                new AppSettingsMock().RemoverAllStudents();
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }

        [Fact(DisplayName = "Should call the function Remove")]
        public async Task StudentRepository_ShouldCallFunctionRemove()
        {
            new AppSettingsMock().RemoverAllStudents();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var student = new Domain.Students.Entidades.Student
            {
                Nome = "system02",
                Email = "system02@gmail.com",
                CPF = "39788175007"
            };

            try
            {
                _studentRepository.Add(student);

                var studentInserida = await _dbContext.Students.SingleOrDefaultAsync(u => u.Nome == student.Nome);

                Assert.NotNull(studentInserida);
                Assert.True(studentInserida.Nome == student.Nome);

                _studentRepository.Remove(studentInserida);

                var studentRemovida = await _dbContext.Students.SingleOrDefaultAsync(u => u.Nome == student.Nome);
                Assert.Null(studentRemovida);
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
