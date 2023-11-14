using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PropertiesDTO
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(100, ErrorMessage = "Campo deve conter até 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "decimal(12, 2)")]
        public decimal Price { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Campo deve conter até 150 caracteres")]
        public string Description { get; set; } = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage = "Campo deve conter até 100 caracteres")]
        public string State { get; set; } = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage = "Campo deve conter até 100 caracteres")]
        public string City { get; set; } = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage = "Campo deve conter até 100 caracteres")]
        public string Amenities { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        [Required]
        public bool Availability { get; set; }
        [Required]
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string OwnersId { get; set; }
        [JsonIgnore]
        public Owners? Owners { get; set; }

    }
}
