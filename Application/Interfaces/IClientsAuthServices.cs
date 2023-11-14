using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClientsAuthServices
    {
        Task<ClientsRegisterDTO> GetClientsAsync();
        Task<ClientsLoginDTO> ClientsLoginAsync(ClientsLoginDTO clientsLoginDTO);
        Task<ClientsRegisterDTO> ClientsRegisterAsync(ClientsRegisterDTO clientsRegisterDTO);
    }
}
