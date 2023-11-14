using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReservationServices : IReservatioService
    {
        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;

        public ReservationServices(IReservationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<ReservationDTO>> GetReservationAsyncAll()
        {
            var repository = await _repository.GetReservationRepositoryAll();
            return _mapper.Map<List<ReservationDTO>>(repository);
        }

        public async Task<ReservationDTO> GetReservationAsyncById(string id)
        {
            var repository = await _repository.GetReservationRepositoryById(id);
            return _mapper.Map<ReservationDTO>(repository);
        }

        public async Task<ReservationDTO> CreateReservationAsync(ReservationDTO reservationDTO)
        {
            var reservation =  _mapper.Map<Reservation>(reservationDTO);
            var repository = await _repository.CreateReservationRepository(reservation);
            return _mapper.Map<ReservationDTO>(repository);    
        }

        public async Task<ReservationDTO> DeleteReservationAsync(string id)
        {
            var repository = await _repository.DeleteReservationRepository(id);
            return _mapper.Map<ReservationDTO>(repository);
        }

    }
}
