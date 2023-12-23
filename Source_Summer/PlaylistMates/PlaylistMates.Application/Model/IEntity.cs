namespace PlaylistMates.Application.Model
{
    public interface IEntity<T>
    {
        public T Id { get; }
    }
}
