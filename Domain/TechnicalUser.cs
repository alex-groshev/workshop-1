namespace Domain
{
    /// <summary>
    /// Represents technical user (Admin, Support).
    /// </summary>
    public class TechnicalUser : BaseUser
    {
        public TechnicalUser(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}