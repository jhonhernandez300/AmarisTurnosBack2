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
        [HttpGet("GetTurno/{id}")]
        public async Task<IActionResult> GetTurno(int id)
        {
            if (id <= 0)
                return BadRequest("El id proporcionado no es válido.");
            
            (Turnos turno, string message) = await _turnoService.GetTurnoByIdAsync(id); 
            
            if (message == "No existe un turno con ese Id." || message == "No se encontró el turno.")
                return NotFound(message); // Cambio: devolver el mensaje del SP

            
            if (message != "Consulta exitosa.")
                return StatusCode(500, message); 
            
            var turnoCompletoDto = _mapper.Map<TurnoCompletoDTO>(turno);

            return Ok(new { turno = turnoCompletoDto, message }); 
        }

        [AllowAnonymous]
        [HttpPut("UpdateTurno/{id}")]
        public async Task<IActionResult> UpdateTurno(int id, [FromBody] TurnosDTO turnoDto)
        {
            if (id <= 0)
                return BadRequest("El id proporcionado no es válido.");

            // Validación: si el DTO es nulo, retornar BadRequest
            if (turnoDto == null)
                return BadRequest("Los datos proporcionados para la actualización del turno no son válidos.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var mapeado = _mapper.Map<Turnos>(turnoDto);
            mapeado.IdTurno = id;

            // llamar al servicio para actualizar el turno
            var (success, message) = await _turnoService.UpdateTurnoAsync(mapeado);

            if (!success)
                return StatusCode(500, message); 

            return Ok(new { message }); 
        }

        [AllowAnonymous]
        [HttpPost("CreateTurno")]
        public async Task<IActionResult> CreateTurno([FromBody] TurnosDTO turnoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mapeado = _mapper.Map<Turnos>(turnoDto);
            int newIdTurno = await _turnoService.CreateTurnoAsync(mapeado);

            // Retornar el resultado con el Id generado
            return Ok(new {mapeado});
        }        

        //[AllowAnonymous]
        //[HttpPost("CrearTurnoAsync")]
        //public async Task<IActionResult> CrearTurnoAsync([FromBody] TurnosDTO turnoDto)
        //{           
        //    if (turnoDto == null)
        //    {
        //        return BadRequest(new { message= "El DTO no puede ser nulo."});
        //    }

        //    if (string.IsNullOrWhiteSpace(turnoDto.Estado))
        //    {
        //        return BadRequest(new { message = "El estado es requerido." });
        //    }

        //    if (turnoDto.FechaTurno == default(DateTime))
        //    {
        //        return BadRequest(new { message = "La Fecha del turno es requerida." });
        //    }

        //    try
        //    {                
        //        var turnos = _mapper.Map<Turnos>(turnoDto);                               
        //        //turno.IdTurno = 1;

        //        var nuevoUsuario =  _unitOfWork.Turnos.AddAsync(turnos);                
        //        var result = await _unitOfWork.CompleteAsync();

        //        if (result > 0)
        //        {
        //            return Ok(new {message = "Turno creado exitosamente." });
        //        }
        //        else
        //        {
        //            return StatusCode(500, new { message = "No se pudo crear el turno. Inténtelo nuevamente." });
        //        }
        //    }
        //    catch (DbUpdateException dbEx)            {

        //        return StatusCode(500, new { message = "Error al guardar el turno en la base de datos." });
        //    }
        //    catch (Exception ex)
        //    {                
        //        return StatusCode(500, new { message = "Error al crear el turno." });
        //    }
        //}

    }
}
