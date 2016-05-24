namespace Infrastructure
{
    /// <summary>
    /// Hi, I'm Cache interface. I heard that classes which implement me are quite important.
    /// </summary>
    public interface ICache
    {
        void Clean();

        void Update(object o);

        object TryGet(int id);

        void Delete(int id);
    }
}
