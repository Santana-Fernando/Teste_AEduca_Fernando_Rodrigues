using Xunit.Abstractions;
using Xunit;
using Tests.Helper;
using Infra.Data.Context;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using Application.Students.Services;
using Infra.Data.Repository.Students;
using Application.Students.ViewModel;

namespace Tests.Student
{
    public class StudentControllerTest
    {
        private readonly Services _studentService;
        private readonly StudentRepository _studentRepository;
        private readonly Presentation.Controllers.Students _tarefaController;
        public StudentControllerTest()
        {
            _studentRepository = StudentRepositoryStub();
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var config = appSettingsMock.configIMapper();
            var mapperMock = config.CreateMapper();

            _studentService = new Services(_studentRepository, mapperMock);
            _tarefaController = new Presentation.Controllers.Students(_studentService);
        }

        private StudentRepository StudentRepositoryStub()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            var dbContext = new ApplicationDbContext(options);
            return new StudentRepository(dbContext);
        }

        [Fact(DisplayName = "Should call GetList")]
        public async void StudentController_ShouldCallGetList()
        {
            var result = await _tarefaController.GetList();

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should call the register")]
        public void StudentController_shouldCallRegister()
        {
            StudentView studentView = new StudentView();
            var result = _tarefaController.Register(studentView);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should return 400 register return error")]
        public void StudentController_shouldRegisterReturn400()
        {
            StudentView studentView = new StudentView();
            var result = _tarefaController.Register(studentView);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should return 500 if Register not OK")]
        public void StudentController_shouldRegisterReturn500AddIfNotOk()
        {
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            var tarefaService = new Services(_studentRepository, mapperMock.Object);

            StudentView studentView = new StudentView()
            {
                Nome = "system2100",
                Email = "system2@gmail.com",
                CPF = "26507814083"
            };

            StudentView tarefas = new StudentView()
            {
            };

            mapperMock.Setup(x => x.Map<StudentView>(studentView)).Returns(tarefas);

            var controller = new Presentation.Controllers.Students(tarefaService);

            var result = controller.Register(studentView);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should call Function GetById")]
        public void StudentController_shouldCallFunctionGetById()
        {
            var result = _tarefaController.GetById(1);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should call Function GetById")]
        public async Task StudentController_shouldReturn200if()
        {
            var result = await _tarefaController.GetById(1);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should call Function Update")]
        public void StudentController_shouldCallFunctionUpdate()
        {
            StudentView studentViewModel = new StudentView();
            var result = _tarefaController.Update(studentViewModel);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should return 404 if update dont found student")]
        public void StudentController_shouldReturn404NotFoundIfUpdateDontFindStudents()
        {
            StudentView studentView = new StudentView()
            {
                Nome = "system2100",
                Email = "system2@gmail.com",
                CPF = "39788175007"
            };

            var result = _tarefaController.Update(studentView);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should return 400 if update have a missing field")]
        public void StudentController_shouldReturn400BadRequestIfUpdateMissingField()
        {
            StudentView studentView = new StudentView();
            var result = _tarefaController.Update(studentView);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            }
        }

        [Fact(DisplayName = "Should call function Remove")]
        public void StudentController_shouldCallFunctionRemove()
        {
            var result = _tarefaController.Remove(0);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should return 404 if remove dont found student")]
        public void StudentController_shouldReturn404NotFoundIfRemoveDontFindStudents()
        {
            StudentView studentView = new StudentView();

            var result = _tarefaController.Remove(0);

            if (result is ObjectResult objectResult)
            {
                Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            }
        }

    }
}
