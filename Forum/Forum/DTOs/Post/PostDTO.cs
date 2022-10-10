using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.DTOs.Post
{
    public record PostDTO(int id, string name, string description);
}
