using Application.Students.ViewModel;
using AutoMapper;
using Domain.Students.Entidades;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Linq;

namespace Tests.Helper
{
    public class AppSettingsMock
    {
        public Mock<IConfiguration> configurationMockStub()
        {
            const string jwtKey = "ChaveSuperSecreta123";
            const string jwtIssuer = "FERNANDO";
            const string jwtAudience = "AplicacaoWebAPI";
            const int jwtExpirationInMinutes = 30;

            var configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(x => x["Jwt:Key"]).Returns(jwtKey);
            configurationMock.Setup(x => x["Jwt:Issuer"]).Returns(jwtIssuer);
            configurationMock.Setup(x => x["Jwt:Audience"]).Returns(jwtAudience);
            configurationMock.Setup(x => x["Jwt:ExpirationInMinutes"]).Returns(jwtExpirationInMinutes.ToString());

            return configurationMock;
        }

        public DbContextOptions<ApplicationDbContext> OptionsDatabaseStub()
        {
            const string defaultConnectionString = "Host=localhost;Database=Registration;Username=postgres;Password=Fern@nd01331";

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(defaultConnectionString)
                .Options;

            return options;
        }

        public MapperConfiguration configIMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Domain.Students.Entidades.Student, StudentView > ()
                .ForMember(dest => dest.RA, opt => opt.MapFrom(src => src.RA))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF));

                cfg.CreateMap<StudentView, Domain.Students.Entidades.Student>()
                .ForMember(dest => dest.RA, opt => opt.MapFrom(src => src.RA))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF));
            });

            return config;
        }

        public void RemoverAllStudents()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var tarefasParaRemover = dbContext.Students.Where(t => t.RA != 1).ToList();

                var Student = new Domain.Students.Entidades.Student
                {
                    RA = 1,
                    Nome = "system",
                    Email = "system@gmail.com",
                    CPF = "39788175007"
                };

                if (tarefasParaRemover.Count > 0)
                {
                    dbContext.Students.RemoveRange(tarefasParaRemover);
                    dbContext.Students.Update(Student);
                    dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Students', RESEED, 1)");
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
