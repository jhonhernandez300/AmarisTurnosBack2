using Microsoft.AspNetCore.Mvc;
using Core;
using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Cors;
using EF.Utils;
using Newtonsoft.Json;
using EF.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EF;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace KontrolarCloud.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigins")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public IConfiguration _configuration;
        private ApplicationDbContext _context;

        public UsuarioController(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTOs loginRequestDTOs)
        {
            try
            {                
                if (loginRequestDTOs == null)
                {                    
                    return BadRequest(new { message = "El usuario no puede ser nulo." });
                }
                             
                if (string.IsNullOrWhiteSpace(loginRequestDTOs.Correo) ||
                    string.IsNullOrWhiteSpace(loginRequestDTOs.Password))
                {                
                    return BadRequest(new { message = "El correo o el password no pueden ser nulos o estar vacíos." });
                }

                var usuario = await _unitOfWork.Usuarios.FindAsync(u => u.Correo == loginRequestDTOs.Correo);
                                
                if (usuario == null || usuario.Password != loginRequestDTOs.Password)
                {
                    return Unauthorized(new { message = "Correo o password incorrectos." });
                }

                var token = GenerateJwtToken(loginRequestDTOs.Correo, loginRequestDTOs.Password);                
                                
                return Ok(new { token });
            }
            catch (Exception ex)
            {                
                return StatusCode(500, new { message = $"Ocurrió un error interno en el servidor: {ex.Message}" });
            }
        }    

        private string GenerateJwtToken(string correo, string password)
        {
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("correo", correo),
                new Claim("password", password)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string EncryptToken(string token)
        {
            var tokenJson = JsonConvert.SerializeObject(token);
            return CryptoHelper.Encrypt(tokenJson);
        }
    }
}