using System.ComponentModel.DataAnnotations;

namespace PlaylistMates.Application.Model
{
    public record Artist([property: MaxLength(255)] string Name);
}