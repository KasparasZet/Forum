using Forum.Auth.Model;
using Forum.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data.Entities
{
    public class Post : IUserOwnedResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatioTimeUTC { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }

        [Required]
        public string UserId { get; set; }
        public Users User { get; set; }
    }
}