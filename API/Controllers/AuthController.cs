using API.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/login")]
    [EnableCors("AllowAll")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        // Tiêm JwtService vào AuthController thông qua DI
        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }
        
        //// login sys
        //[HttpPost("login")]
        //public IActionResult Login([FromBody] LoginModel login)
        //{
        //    // Giả sử xác thực người dùng thành công từ database qua ADO.NET
        //    if (login.Username == "testuser" && login.Password == "password")
        //    {
        //        var token = _jwtService.GenerateToken(login.Username);
        //        return Ok(new { token });
        //    }

        //    return Unauthorized();
        //}
    }
}
