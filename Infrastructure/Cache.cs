namespace Infrastructure
{
    /// <summary>
    /// Hi! I'm Cache. It takes ages to construct and destroy me!
    /// Think twice before you put me in your tests.
    /// </summary>
    public class Cache : ICache
    {
        public void Clean()
        {
            // do nothing for the time being
        }

        public void Update(object o)
        {
            // do nothing for the time being
        }

        public object TryGet(int id)
        {
            // do nothing for the time being
            return null;
        }

        public void Delete(int id)
        {
            // do nothing for the time being
        }
    }
}
