using Forum.Auth.Model;
using Forum.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace Forum.Data.Entities
{
    public class Topic : IUserOwnedResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreationTimeUtc { get; set; }

        [Required]
        public string UserId { get; set; }
        public Users User { get; set; }
    }
}