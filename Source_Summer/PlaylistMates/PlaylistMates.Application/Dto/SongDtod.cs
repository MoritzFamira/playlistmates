namespace PlaylistMates.Application.Dto;

public class SongDtod
{
    public string Titel { get; set; }
    public Guid guid { get; set; }
    public List<string> Artists { get; set; }
}