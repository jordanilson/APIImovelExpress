using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RegisterOwnersDTO
    {
        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo name é obrigatório.")]
        public string?  Name { get; set; }
        [Required(ErrorMessage = "O campo de e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um endereço de e-mail válido.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo password é obrigatório.")]
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        
    }
}
