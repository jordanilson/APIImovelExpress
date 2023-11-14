
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OwnersDTO : ApplicationUser
    {
        public string Name { get; set; } = string.Empty;
        public List<PropertiesDTO> Properties { get; set; } = new List<PropertiesDTO>();
    }
}
