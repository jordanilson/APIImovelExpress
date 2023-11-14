using Application.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReservatioService
    {
        Task<List<ReservationDTO>> GetReservationAsyncAll();
        Task<ReservationDTO> CreateReservationAsync(ReservationDTO reservationDTO);
        Task<ReservationDTO> GetReservationAsyncById(string id);
        Task<ReservationDTO> DeleteReservationAsync(string id);
    }
}
