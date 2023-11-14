
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaaces
{
    public interface IPropertiesServices
    {
        Task<List<PropertiesDTO>> GetPropertiesAsyncAll();
        Task<List<PropertiesDTO>> GetPropertiesAsync( string userId);
        Task<PropertiesDTO> GetPropertiesAsyncById(string Id);
        Task<PropertiesDTO> CreatePropertiesAsyncById(PropertiesDTO propertiesDTO);
        Task<PropertiesDTO> UpdatePropertiesAsync(PropertiesDTO propertiesDTO);
        Task<PropertiesDTO> DeletePropertiesAsyncById(string id);
    }
}
