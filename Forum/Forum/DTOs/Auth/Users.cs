using Microsoft.AspNetCore.Identity;

namespace Forum.DTOs.Auth
{
    public class Users : IdentityUser
    {
        [PersonalData]
        public string? AdditionalInfo { get; set; }
    }
}
