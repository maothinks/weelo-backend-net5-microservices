using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Weelo.Microservices.AuthAndUsers.API.Authorization;
using Weelo.Microservices.AuthAndUsers.API.Entities;
using Weelo.Microservices.AuthAndUsers.API.Helpers;
using Weelo.Microservices.AuthAndUsers.API.Models.Users;
using Weelo.Microservices.AuthAndUsers.API.Services;

namespace Weelo.Microservices.AuthAndUsers.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// User Controller Constructor
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="mapper"></param>
        /// <param name="appSettings"></param>
        /// <param name="logger"></param>
        public UsersController(
            IUserService userService,
            IMapper mapper,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Authenticate Users
        /// </summary>
        /// <param name="model">Login Credential sent from clients</param>
        /// <returns>User Logged Data or Empty if credentials are not valid</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            try
            {
                var response = _userService.Authenticate(model);

                if (response == null) {
                    return Ok(new JsonResponse { Success = false, Message = "" });
                }
                
                return Ok(new JsonResponse { Success = true, Message = response });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Register New User
        /// </summary>
        /// <param name="model">New User Data</param>
        /// <returns>Register Message</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            try
            {
                string result = _userService.Register(model);

                if(result != "")
                    return Ok(new JsonResponse{ Success=false, Message = result });

                return Ok(new JsonResponse { Success = true, Message = "Registration successful" });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns>List with Users</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var users = _userService.GetAll();
                return Ok(new JsonResponse() { Success = true, Message = users });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get Specific user by UserId
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>User Data</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<JsonResponse> GetById(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                return Ok(new JsonResponse() { Success = true, Message = user });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="id">UserID</param>
        /// <param name="model">User Data</param>
        /// <returns>Return Update Confirmation message</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateRequest model)
        {
            try
            {
                _userService.Update(id, model);
                return Ok(new JsonResponse { Success = true, Message = "User updated successfully" });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete Specific User
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>COnfirmation Delete Message</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.Delete(id);
                return Ok(new JsonResponse { Success = true, Message = "User deleted successfully" }); ;
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
