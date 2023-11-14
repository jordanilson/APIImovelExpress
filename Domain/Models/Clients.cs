using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Clients : ApplicationUser
    {
        public string Name { get; set; } = string.Empty;
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
