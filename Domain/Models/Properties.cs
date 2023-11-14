using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Properties
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Amenities { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public bool Availability { get; set; }
        public string OwnersId { get; set; }
        public Owners? Owners { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }


    }
}
