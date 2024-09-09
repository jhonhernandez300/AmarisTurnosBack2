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
using Core.Modelos;
using System.Runtime.ConstrainedExecution;
using Core.Modelos.Interfaces;

namespace KontrolarCloud.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigins")]
    [ApiController]
    public class TurnoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public IConfiguration _configuration;
        private ApplicationDbContext _context;
        private readonly ITurnoService _turnoService;

        public TurnoController(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context, ITurnoService turnoService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
            _turnoService = turnoService;
        }

        [AllowAnonymous]
        [HttpGet("ContarTurnos/{idUsuario}")]
        public async Task<IActionResult> ContarTurnos(string idUsuario)
        {
            try
            {
                if (idUsuario == null)
                {
                    return BadRequest(new { message = "El id del usuario no puede ser nulo." });
                }
                
                int numeroDeTurnos = await _turnoService.ContarTurnosPorUsuarioAsync(idUsuario);
                
                return Ok(new { NumeroDeTurnos = numeroDeTurnos });
            }
            catch (Exception ex)
            {
                // Retorna un error si ocurre algún problema
                return BadRequest(new { Message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpGet("ObtenerTurnosActivados")]
        public async Task<IActionResult> ObtenerTurnosActivados()
        {
            try
            {
                var turnos = await _turnoService.GetTurnosActivadosAsync();
                if (turnos == null || !turnos.Any())
                {
                    // Devolver un estado OK con un mensaje indicando que no hay registros
                    return Ok(new { message = "No hay turnos activados.", data = (object)null });
                }

                return Ok(turnos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error del servidor: {ex.Message}" });
            }
        }

        [AllowAnonymous]
        [HttpGet("ObtenerTurnoPorId/{id}")]
        public async Task<IActionResult> ObtenerTurnoPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "El id proporcionado no es válido." });

                (Turnos turno, string message) = await _turnoService.GetTurnoByIdAsync(id);

                if (message == "No existe un turno con ese Id." || message == "No se encontró el turno.")
                    return NotFound(new { message = "No existe un turno con ese Id" }); 


                if (message != "Consulta exitosa.")
                    return StatusCode(500, new{message = $"Error al realizar la consulta, {message}"});

                var turnoCompletoDto = _mapper.Map<TurnoCompletoDTO>(turno);

                return Ok(new { turno = turnoCompletoDto, message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error del servidor: {ex.Message}" });
            }             
        }

        [AllowAnonymous]
        [HttpPut("ActualizarTurno")]
        public async Task<IActionResult> ActualizarTurno([FromBody] TurnoCompletoDTO turnoCompletoDto)
        {
            try
            {
                if (turnoCompletoDto == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud no puede ser nulo." });
                }
                
                if (turnoCompletoDto.IdTurno == 0 ||
                    turnoCompletoDto.IdUsuario == 0 ||
                    turnoCompletoDto.IdSucursal == 0 ||
                    string.IsNullOrEmpty(turnoCompletoDto.Estado))
                {
                    return BadRequest(new { message = "Todos los campos son obligatorios y no pueden estar vacíos." });
                }

                var mapeado = _mapper.Map<Turnos>(turnoCompletoDto);
                (Turnos turno, string messageBusqueda) = await _turnoService.GetTurnoByIdAsync(mapeado.IdTurno);

                if (messageBusqueda == "No existe un turno con ese Id." || messageBusqueda == "No se encontró el turno.")
                    return NotFound(new { message = "No existe un turno con ese Id" });

                var (success, message) = await _turnoService.UpdateTurnoAsync(mapeado);

                if (!success)
                    return StatusCode(500, message);

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error del servidor: {ex.Message}" });
            }             
        }

        [AllowAnonymous]
        [HttpPost("CrearTurno")]
        public async Task<IActionResult> CrearTurno([FromBody] TurnosDTO turnoDto)
        {
            try
            {
                if (turnoDto == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud no puede ser nulo." });
                }

                var mapeado = _mapper.Map<Turnos>(turnoDto);
                int newIdTurno = await _turnoService.CreateTurnoAsync(mapeado);

                // Retornar el resultado con el Id generado
                return Ok(new { mapeado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error del servidor: {ex.Message}" });
            }           
        }        
    }
}
