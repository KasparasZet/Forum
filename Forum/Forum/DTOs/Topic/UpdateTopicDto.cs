using System.ComponentModel.DataAnnotations;

namespace Forum.DTOs.Topic
{
    public record UpdateTopicDto([Required] string Name);
}
