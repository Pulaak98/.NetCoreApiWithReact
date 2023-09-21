using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.BLL;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentManager _studentManager;

        public StudentsController(StudentManager studentManager)
        {
            _studentManager = studentManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] StudentRegister request)
        {
            var response = await _studentManager.RegisterAsync(request);

            if (response.Success)
            {
                return Ok("Registration successful");
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] StudentLogin request)
        {
            var response = await _studentManager.LoginAsync(request);

            if (response.Success)
            {
                return Ok(new { Token = response.Token });
            }
            else
            {
                return Unauthorized(response);
            }
        }
    }
}
