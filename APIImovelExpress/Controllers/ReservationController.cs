using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIImovelExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowRequest")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Clients")]

    public class ReservationController : ControllerBase
    {
        private readonly IReservatioService _reservatioService;

        public ReservationController(IReservatioService reservatioService)
        {
            _reservatioService = reservatioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservationDTO>>> GetReservation()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            return await _reservatioService.GetReservationAsyncAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDTO>> GetReservationById(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            if (id == null)
            {
                return NotFound($"Reserva com {id} não encotrado!");
            }

            var propertiesBById = await _reservatioService.GetReservationAsyncById(id);
            return Ok(propertiesBById);
        }

        [HttpPost]
        public async Task<ActionResult> PostReservation([FromForm] ReservationDTO reservationDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var reservation = new ReservationDTO
            {
                ReservationProperties = reservationDTO.ReservationProperties,
                ClientsId = userId,          
            };

            await _reservatioService.CreateReservationAsync(reservation);
            return Ok(reservation);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ReservationDTO>> DeleteReservation(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var deletedProperty = await _reservatioService.DeleteReservationAsync(id);
            if (deletedProperty == null)
            {
                return NotFound("Propriedade não encontrada.");
            }

            return Ok(deletedProperty);
        }
    }
}
