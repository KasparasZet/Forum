using System.ComponentModel.DataAnnotations;

namespace Forum.DTOs.Auth
{
    public record RegisterDTO([Required] string UserName, 
        [EmailAddress][Required] string Email, 
        [Required] string Password);
}
