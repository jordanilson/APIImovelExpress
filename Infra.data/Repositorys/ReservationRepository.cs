using Domain.Interfaces;
using Domain.Models;
using Infra.data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infra.data.Repositorys
{
    public class ReservationRepository : IReservationRepository
    {
        public readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetReservationRepositoryAll()
        {
            try
            {
               return await _context.Reservations.ToListAsync();
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Erro{ex.Message}");
                throw;
            }
        }

        public async Task<Reservation> GetReservationRepositoryById(string id)
        {
            try
            {
               return await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex) 
            {

                Console.WriteLine($"Erro{ex.Message}");
                throw;
            }
        }

        public async Task<Reservation> CreateReservationRepository(Reservation reservation)
        {
            try
            {
                reservation.Id = Guid.NewGuid().ToString();
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return reservation;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Erro{ex.Message}");
                throw;
            }
        }

        public async Task<Reservation> DeleteReservationRepository(string id)
        {
            try
            {
                var reservationById = await GetReservationRepositoryById(id);
                if (reservationById != null)
                {
                    _context.Remove(reservationById);
                    await _context.SaveChangesAsync();
                }
                return reservationById;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Erro{ex.Message}");
                throw;
            }
        }
       
    }
}
