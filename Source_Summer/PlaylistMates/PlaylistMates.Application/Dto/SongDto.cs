using System.ComponentModel.DataAnnotations;
using PlaylistMates.Application.Model;
using PlaylistMates.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace PlaylistMates.Application.Dto
{
    public record SongDto
    (
        [Required]
        Guid Guid,

        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "ISRC code must be 12 characters long.")]
        string IsrcCode,

        [Required]
        [StringLength(100, ErrorMessage = "Title must be at most 100 characters long.")]
        string Titel,

        [Required]
        DateTime ReleaseDate,

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0 milliseconds.")]
        int DurationInMillis,

        [Required]
        [MinLength(1, ErrorMessage = "At least one artist is required.")]
        List<Artist> Artists
        );
    //) : IValidatableObject
    //{
    //    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //    {

    //        var db = validationContext.GetRequiredService<Context>();
    //        foreach (var item in SongCollectionGuids)
    //        {
    //            if (!db.SongCollections.Any(s => s.Guid == item))
    //            {
    //                yield return new ValidationResult("SongCollection does not exist", new[] { nameof(SongCollectionGuids) });
    //            }
    //        }
    //    }
    //}
}
