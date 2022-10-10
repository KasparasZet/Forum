using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.DTOs.Post
{
    public record UpdatePostDTO(string name, string description);
}
