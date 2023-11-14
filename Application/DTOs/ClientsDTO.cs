using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ClientsDTO : ApplicationUser
    {
        public string Name { get; set; } = string.Empty;
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
