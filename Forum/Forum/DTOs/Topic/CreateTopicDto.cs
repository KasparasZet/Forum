using System.ComponentModel.DataAnnotations;

namespace Forum.DTOs.Topic
{
    public record CreateTopicDto([Required] string Name, [Required] string Description);
}
