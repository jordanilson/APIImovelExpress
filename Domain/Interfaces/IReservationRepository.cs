using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetReservationRepositoryAll();
        Task<Reservation> CreateReservationRepository(Reservation reservation);
        Task<Reservation> GetReservationRepositoryById(string id);
        Task<Reservation> DeleteReservationRepository(string id);
    }
}
