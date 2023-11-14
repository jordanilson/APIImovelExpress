using Application.Interfaaces;
using AutoMapper;
using Azure.Storage.Blobs;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PropertiesServices : IPropertiesServices
    {
        private readonly IPropertiesRepository _repository;
        private readonly IMapper _mapper;
        public PropertiesServices(IPropertiesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PropertiesDTO>> GetPropertiesAsyncAll()
        {
            var repos = await _repository.GetPropertiesRepository();
            return _mapper.Map<List<PropertiesDTO>>(repos);
        }
        public async Task<List<PropertiesDTO>> GetPropertiesAsync(string userId)
        {
            var repos = await _repository.GetPropertiesRepository();
            return _mapper.Map<List<PropertiesDTO>>(repos);
        }

        public async Task<PropertiesDTO> GetPropertiesAsyncById(string id)
        {
            var properties = await _repository.GetPropertiesRepositoryById(id);
            return _mapper.Map<PropertiesDTO>(properties);
        }

        public async Task<PropertiesDTO> CreatePropertiesAsyncById(PropertiesDTO propertiesDTO)
        {
            var porperties = _mapper.Map<Properties>(propertiesDTO);
            var repos = await _repository.CreatePropertiesRepository(porperties);
            return _mapper.Map<PropertiesDTO>(repos);
        }

        public async Task<PropertiesDTO> DeletePropertiesAsyncById(string id)
        {
            var properties = await _repository.GetPropertiesRepositoryById(id);
            if (properties == null)
            {
                return null;
            }

            await _repository.DeletePropertiesRepository(properties.Id);
            return _mapper.Map<PropertiesDTO>(properties);
        }

        public async Task<PropertiesDTO> UpdatePropertiesAsync(PropertiesDTO propertiesDTO)
        {
            var properties = await _repository.GetPropertiesRepositoryById(propertiesDTO.Id);
            if (properties == null)
            {
                return null;
            }

            properties.Name = propertiesDTO.Name;
            properties.Price = propertiesDTO.Price;
            properties.Description = propertiesDTO.Description;
            properties.State = propertiesDTO.State;
            properties.City = propertiesDTO.City;
            properties.Amenities = propertiesDTO.Amenities;
            properties.Availability = propertiesDTO.Availability;
            properties.OwnersId = propertiesDTO.OwnersId;

            if (!string.IsNullOrEmpty(propertiesDTO.ImgUrl))
            {
                properties.ImgUrl = propertiesDTO.ImgUrl;
            }

            await _repository.UpdatePropertiesRepository(properties);

            return _mapper.Map<PropertiesDTO>(properties);
        }

    }
}
