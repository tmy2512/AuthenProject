using API.Service;
using Data;
using Data.UserRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/user")]
    [EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public object TempData { get; private set; }

        public UserController(UserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        // register account
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userRepository.RegisterUser(user);
            return Ok(user);
        }

        // Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel login)
        {
            var result = await _userRepository.ValidateUser(login.Email, login.Password);
            if (result.Result )
            {
                var token = _jwtService.GenerateToken(login.Email);

                //return RedirectToAction("LoginUser");
                return Ok(new { token });
            }

            return Unauthorized(new {message = result.Message});
        }
    }


    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }



}
