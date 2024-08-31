using Application.Students.Interfaces;
using Application.Students.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Students : Controller
    {
        private readonly IStudentServices _studentServices;
        public Students(IStudentServices tarefaServices)
        {
            _studentServices = tarefaServices;
        }

        [Route("GetList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentView>))]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var result = await _studentServices.GetList();
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Register(StudentView studentView)
        {
            var result = _studentServices.Add(studentView);
            var response = new { Message = result.ReasonPhrase };

            switch (result.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    return StatusCode(StatusCodes.Status400BadRequest, result.ReasonPhrase);
                case System.Net.HttpStatusCode.InternalServerError:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.ReasonPhrase);

                default:
                    return Ok(response);
            }            
        }

        [HttpGet]
        [Route("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentView))]
        public async Task<IActionResult> GetById(int ra)
        {
            var result = await _studentServices.GetById(ra);
            return StatusCode(StatusCodes.Status200OK, result);            
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type=typeof(string))]
        public IActionResult Update(StudentView studentView)
        {
            try
            {
                var result = _studentServices.Update(studentView);
                switch (result.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        return StatusCode(StatusCodes.Status400BadRequest, result.ReasonPhrase);
                    case System.Net.HttpStatusCode.NotFound:
                        return StatusCode(StatusCodes.Status404NotFound, result.ReasonPhrase);

                    default:
                        return Ok(StatusCodes.Status200OK);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Remove(int ra)
        {
            try
            {
                var result = _studentServices.Remove(ra);
                switch (result.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return StatusCode(StatusCodes.Status404NotFound, result.ReasonPhrase);

                    default:
                        return Ok(StatusCodes.Status200OK);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
