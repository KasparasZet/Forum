using System.ComponentModel.DataAnnotations;

namespace Forum.DTOs.Comment
{   
    public record CreateCommentDTO([Required] string description, DateTime CreationTimeUtc);
}
