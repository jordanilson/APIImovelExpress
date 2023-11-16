using Azure.Storage.Blobs;
using Domain.Interfaces;
using Domain.Models;
using Infra.data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infra.data.Repositorys
{
    public class PropertiesRepository : IPropertiesRepository
    {
        private readonly AppDbContext _context;
       

        public PropertiesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Properties>> GetPropertiesRepository()
        {
            return await _context.Properties.ToListAsync();
        }

        public async Task<Properties> GetPropertiesRepositoryById(string id)
        {
            try
            {
                return await _context.Properties.FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                throw;
            }
        }

        public async Task<Properties> CreatePropertiesRepository(Properties properties)
        {
            try
            {
                properties.Id = Guid.NewGuid().ToString();
                _context.Properties.Add(properties);
                await _context.SaveChangesAsync();
                return properties;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                throw;
            }
        }

        public async Task<Properties> UpdatePropertiesRepository(Properties properties)
        {
            try
            {
                var updateProperties = await _context.Properties.FirstOrDefaultAsync(p => p.Id == properties.Id);
                if (updateProperties != null)
                {
                    updateProperties.Availability = properties.Availability;
                    updateProperties.Amenities = properties.Amenities;
                    updateProperties.Price = properties.Price;
                    updateProperties.City = properties.City;
                    updateProperties.State = properties.State;
                    updateProperties.Name = properties.Name;
                    updateProperties.Description = properties.Description;

                    if (!string.IsNullOrEmpty(properties.ImgUrl))
                    {
                        updateProperties.ImgUrl = properties.ImgUrl;
                    }

                    await _context.SaveChangesAsync();
                }
                return updateProperties;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                throw;
            }
        }

        public async Task<Properties> DeletePropertiesRepository(string id)
        {
            try
            {
                var deleteProperties = await GetPropertiesRepositoryById(id);
                if (deleteProperties != null)
                {
                    _context.Properties.Remove(deleteProperties);
                    await _context.SaveChangesAsync();
                }
                return deleteProperties;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                throw;
            }
        }
    }
}
