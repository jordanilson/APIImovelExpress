using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReservationDTO
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string ReservationProperties { get; set; } = string.Empty;
        public string ClientsId { get; set; }
        [JsonIgnore]
        public List<Reservation>? Reservations { get; set; }
    }
}
