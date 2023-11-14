using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Reservation
    {
        public string Id { get; set; }
        public string  ReservationProperties { get; set; } = string.Empty;
        public string ClientsId { get; set; }
        public Clients? Clients { get; set;}
    }
}
