
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaaces
{
    public interface IOwnersAuthServices
    {
        Task<OwnersDTO> GetOwnersAsync();
        Task<RegisterOwnersDTO> GetOwnersAsyncById();
        Task<RegisterOwnersDTO> RegisterOwnersAsync(RegisterOwnersDTO registerOwnersDTO);
        Task<LoginOwnersDTO> LoginOwnersAsync(LoginOwnersDTO loginOwnersDTO);
    }
}
