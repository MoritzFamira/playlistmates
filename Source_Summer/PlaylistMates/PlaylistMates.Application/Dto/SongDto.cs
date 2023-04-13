using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistMates.Application.Model;
using PlaylistMates.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace PlaylistMates.Application.Dto
{
    public record SongDto
    (
        Guid Guid,
        string IsrcCode,
        string Titel,
        DateTime ReleaseDate,
        int DurationInMillis,
        List<Artist> Artists,
        List<Guid> SongCollectionGuids
    ) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var db = validationContext.GetRequiredService<Context>();
            foreach (var item in SongCollectionGuids)
            {
                if (!db.SongCollections.Any(s => s.Guid == item))
                {
                    yield return new ValidationResult("SongCollection does not exist", new[] { nameof(SongCollectionGuids) });
                }
            }
        }
    }
}
