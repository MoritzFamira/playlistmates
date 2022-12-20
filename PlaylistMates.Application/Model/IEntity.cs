namespace PlaylistMates.Application.Model
{

    public interface IEntity<T>
    {
        T Id { get; }
    }
}
