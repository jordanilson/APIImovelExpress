
using Application.DTOs;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<Properties, PropertiesDTO>().ReverseMap();
            CreateMap<Owners,OwnersDTO>().ReverseMap();
            CreateMap<LoginOwnersDTO,RegisterOwnersDTO >().ReverseMap();
            CreateMap<Clients,ClientsDTO >().ReverseMap();
            CreateMap<Reservation,ReservationDTO >().ReverseMap();
            CreateMap<ClientsRegister,ClientsRegisterDTO>().ReverseMap();
            CreateMap<ClientsLogin,ClientsLoginDTO>().ReverseMap();
        }
    }
}
