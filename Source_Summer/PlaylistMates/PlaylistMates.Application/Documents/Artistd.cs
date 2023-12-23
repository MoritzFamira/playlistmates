using System.ComponentModel.DataAnnotations;

namespace PlaylistMates.Application.Documents
{
    public record Artistd([property: MaxLength(255)] string Name);
}