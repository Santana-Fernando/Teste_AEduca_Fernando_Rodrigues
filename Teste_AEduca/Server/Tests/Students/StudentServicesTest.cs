using Application.Http;
using Application.Students.Interfaces;
using Application.Students.Services;
using Application.Students.ViewModel;
using Application.Validation;
using AutoMapper;
using Infra.Data.Context;
using Infra.Data.Repository.Students;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Tests.Helper;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Students
{
    public class StudentServicesTest
    {
        private readonly Services _studentService;
        private readonly StudentRepository _studentRepository;
        private readonly IMapper _mapperMock;

        public StudentServicesTest()
        {
            _studentRepository = TarefaRepositoryStub();
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var config = appSettingsMock.configIMapper();
            _mapperMock = config.CreateMapper();

            _studentService = new Services(_studentRepository, _mapperMock);
        }

        private StudentRepository TarefaRepositoryStub()
        {
            AppSettingsMock appSettingsMock = new AppSettingsMock();
            var options = appSettingsMock.OptionsDatabaseStub();
            var dbContext = new ApplicationDbContext(options);
            return new StudentRepository(dbContext);
        }

        #region ValidateFields
        [Fact(DisplayName = "Should validate Titulo Nome is empty")]
        public void TarefaServices_ShouldValidateFieldTituloIsEmpty()
        {
            StudentView studentView = new StudentView
            {
                Nome = string.Empty,
                Email = "system2@gmail.com",
                CPF = "39788175007"
            };

            var validationFields = new ValidationFields(studentView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field Nome is required");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field Nome is Long")]
        public void TarefaServices_ShouldValidateFieldNomeIsLong()
        {
            StudentView studentView = new StudentView
            {
                Nome = "fasdfasdfasdfasdfasfhfjhsdfdjflkjilfjlaksdjflksdjlfkjasdlkfjlksdf ljfhklashf lkjkfhasdhfkjasdhkj af çikah fjk ahsjklfh asldjkfhlkj",
                Email = "system2@gmail.com",
                CPF = "39788175007"
            };

            var validationFields = new ValidationFields(studentView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field Nome must be a string or array type with a maximum length of '100'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field Nome is short")]
        public void TarefaServices_ShouldValidateFieldTituloIsShort()
        {
            StudentView studentView = new StudentView
            {
                Nome = "sd",
                Email = "system2@gmail.com",
                CPF = "39788175007"
            };

            var validationFields = new ValidationFields(studentView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field Nome must be a string or array type with a minimum length of '10'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field Email is empty")]
        public void TarefaServices_ShouldValidateFieldSLAIsEmpty()
        {
            StudentView studentView = new StudentView
            {
                Nome = "system2000",
                Email = string.Empty,
                CPF = "39788175007"
            };

            var validationFields = new ValidationFields(studentView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages.FirstOrDefault(result => result.ErrorMessage == "The field E-mail is required");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field Email is Long")]
        public void TarefaServices_ShouldValidateFieldSLAIsLong()
        {
            StudentView studentView = new StudentView
            {
                Nome = "system2000",
                Email = "fasdfasdfasdfasdfasfhfjhsdfdjflkjilfjlaksdjflksdjlfkjasdlkfjlksdf ljfhklashf lkjkfhasdhfkjasdhkj af çikah fjk ahsjklfh asldjkfhlkj",
                CPF = "39788175007"
            };

            var validationFields = new ValidationFields(studentView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field Email must be a string or array type with a maximum length of '100'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }

        [Fact(DisplayName = "Should validate field Email is Short")]
        public void TarefaServices_ShouldValidateFieldEmailIsShort()
        {
            StudentView studentView = new StudentView
            {
                Nome = "system2000",
                Email = "mail",
                CPF = "39788175007"
            };

            var validationFields = new ValidationFields(studentView).IsValidWithErrors();
            var requiredEmailError = validationFields.ErrorMessages
                .FirstOrDefault(result => result.ErrorMessage == "The field Email must be a string or array type with a minimum length of '10'.");

            Assert.False(validationFields.IsValid);
            Assert.NotNull(requiredEmailError);
        }        

        [Fact(DisplayName = "Should validate fields and return ok")]
        public void TarefaServices_ShouldValidateFielsdAndReturOk()
        {
            StudentView studentView = new StudentView
            {
                Nome = "system2000",
                Email = "system2@gmail.com",
                CPF = "39788175007"
            };

            var validationFields = new ValidationFields(studentView).IsValidWithErrors();

            Assert.True(validationFields.IsValid);
        }
        #endregion

        [Fact(DisplayName = "Should call the function add")]
        public void TarefaServices_ShouldCallTheFunctionAdd()
        {
            StudentView studentView = new StudentView();

            var result = _studentService.Add(studentView);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should return a internal server erro if process falied")]
        public void TarefaServices_ShouldReturnInternalServerErrorIfProcessFalied()
        {
            StudentView studentView = new StudentView
            {
                Nome = "system03",
                Email = "system03@gmail.com",
                CPF = "39788175007"
            };

            HttpResponse httpResponse = new HttpResponse();
            var studentService = new Mock<IStudentServices>();

            studentService.Setup(x => x.Add(studentView)).Returns(httpResponse.Response(HttpStatusCode.InternalServerError, null, string.Empty));

            var result = studentService.Object.Add(studentView);

            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Fact(DisplayName = "Should return ok if is all ok")]
        public void TarefaServices_ShouldReturnOkIfIdAllOk()
        {
            StudentView studentView = new StudentView
            {
                Nome = "system2",
                Email = "system2@gmail.com",
                CPF = "39788175007"
            };

            HttpResponse httpResponse = new HttpResponse();
            var studentService = new Mock<IStudentServices>();

            studentService.Setup(x => x.Add(studentView)).Returns(httpResponse.Response(HttpStatusCode.OK, null, "OK"));

            HttpResponseMessage result = studentService.Object.Add(studentView);
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [Fact(DisplayName = "Should call GetById and return a student")]
        public async void TarefaServices_ShouldCallGetByIdAndReturnAUser()
        {
            var result = await _studentService.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.RA);
            Assert.Equal("system", result.Nome);
        }

        [Fact(DisplayName = "Should call GetList and return a students list")]
        public async void TarefaServices_ShouldCallGetListdAndReturnAUserList()
        {
            HttpResponse httpResponse = new HttpResponse();
            var studentService = new Mock<IStudentServices>();

            IEnumerable<StudentView> lstTarefa = new List<StudentView>
            {
                new StudentView()
                {
                    Nome = "system2",
                    Email = "system2@gmail.com",
                    CPF = "39788175007"
                }
            };

            studentService.Setup(x => x.GetList()).ReturnsAsync(lstTarefa);

            var result = await studentService.Object.GetList();
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = "Should call Update and update student")]
        public void TarefaServices_ShouldCallUpdate()
        {
            HttpResponse httpResponse = new HttpResponse();
            var studentService = new Mock<IStudentServices>();

            var student = new StudentView();
            studentService.Setup(x => x.Update(student)).Returns(httpResponse.Response(HttpStatusCode.OK, null, "OK"));

            var statusReturn = studentService.Object.Update(student);

            Assert.Equal(System.Net.HttpStatusCode.OK, statusReturn.StatusCode);
        }

        [Fact(DisplayName = "Should return badRequest id Update not found student")]
        public async void TarefaServices_ShouldUpdateReturnNotFound()
        {
            var student = await _studentService.GetById(1);

            student.Nome = "santana100";
            student.RA = 1000;

            var statusReturn = _studentService.Update(student);

            Assert.NotNull(statusReturn);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, statusReturn.StatusCode);
            Assert.Equal("Student not found!", statusReturn.ReasonPhrase);
        }

        [Fact(DisplayName = "Should return badRequest id Update not found student")]
        public async void TarefaServices_ShouldUpdateReturnBadRequest()
        {
            var student = await _studentService.GetById(1);

            student.Nome = "";

            var statusReturn = _studentService.Update(student);

            Assert.NotNull(statusReturn);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, statusReturn.StatusCode);
        }

        [Fact(DisplayName = "Should Call and Remove student")]
        public void TarefaServices_ShouldCallAndRemoveUser()
        {
            HttpResponse httpResponse = new HttpResponse();
            var studentService = new Mock<IStudentServices>();

            studentService.Setup(x => x.Remove(0)).Returns(httpResponse.Response(HttpStatusCode.OK, null, "OK"));

            var result = studentService.Object.Remove(0);
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [Fact(DisplayName = "Should return badRequest id Delete not found student")]
        public void TarefaServices_ShouldRemoveReturnBadRequest()
        {
            var statusReturn = _studentService.Remove(1000);

            Assert.NotNull(statusReturn);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, statusReturn.StatusCode);
            Assert.Equal("Student not found!", statusReturn.ReasonPhrase);
        }
    }
}
