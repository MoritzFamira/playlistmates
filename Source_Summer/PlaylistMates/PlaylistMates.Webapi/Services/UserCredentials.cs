namespace PlaylistMates.Webapi.Services
{
    /// <summary>
    /// DTO Objekt für den HTTP Request Body.
    /// </summary>
    public class UserCredentials
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}