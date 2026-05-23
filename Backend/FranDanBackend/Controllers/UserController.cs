using FranDanBackend.DTO;
using FranDanBackend.Models;
using FranDanBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
namespace FranDanBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        public UserController(UserService _userService)
        {
            userService = _userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [EndpointSummary("Registers user")]
        [EndpointDescription("During registration sends registration code.")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public ActionResult<string> RegisterUser([FromBody] AuthRegisterDTO request)
        {
            if (request == null)
            {
                return BadRequest("Dane rejestracji nie mogą być puste.");
            }
            try
            {
                userService.add(request);
                return Ok("Register success and code send.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Register error: {ex.Message}");
            }
        }

        [HttpPost("verify")]
        [AllowAnonymous]
        [EndpointSummary("Verify user's email code.")]
        [EndpointDescription("User has to paste verification code here.")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public ActionResult<string> VerifyUser([FromBody] AuthVerifyDTO request)
        {
            if (request == null)
            {
                return BadRequest("Dane rejestracji nie mogą być puste.");
            }
            try
            {
                userService.verify(request);
                return Ok("Verification success! You can log in!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Verification error: {ex.Message}");
            }
        }
    }
}