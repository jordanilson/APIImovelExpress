using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPropertiesRepository
    {
        Task<List<Properties>> GetPropertiesRepository();
        Task<Properties> GetPropertiesRepositoryById(string id);
        Task<Properties> CreatePropertiesRepository(Properties properties);
        Task<Properties> UpdatePropertiesRepository(Properties properties);
        Task<Properties> DeletePropertiesRepository(string id);
    }
}
