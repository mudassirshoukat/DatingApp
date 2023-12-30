namespace API.Interfaces.RepoInterfaces
{
    public interface IUnitOfWork
    {

        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikesRepository LikesRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        public Task<bool> Complete();
        public bool HasChanges();

    }
}
