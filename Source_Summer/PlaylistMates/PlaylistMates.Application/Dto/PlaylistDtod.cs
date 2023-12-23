namespace PlaylistMates.Application.Dto;

public class PlaylistDtod
{
    public string Title { get; set; }
    public Guid guid { get; set; }
    public List<SongDtod> Songs { get; set; } // Add the Songs property
}