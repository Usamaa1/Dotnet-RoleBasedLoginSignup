using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleBasedLoginSignup.Data;
using RoleBasedLoginSignup.DTO;

namespace RoleBasedLoginSignup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly MyProjContext _context;
        private readonly TokenService _tokenService;

        public AuthController(MyProjContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTO.RegisterDTO registerDTO)
        {
            var existingUser = await _context.Users
                .AnyAsync(u => u.Username == registerDTO.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }
            var hashedPassword = PasswordHasher.Hash(registerDTO.Password);
            var user = new Models.User
            {
                Username = registerDTO.Username,
                PasswordHash = hashedPassword,
                Role = registerDTO.Role
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);
            if (user == null)
                return Unauthorized("User not found");

            if (user.PasswordHash != PasswordHasher.Hash(loginDTO.Password))
                return Unauthorized("Invalid password");

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });


        }





    }
}
