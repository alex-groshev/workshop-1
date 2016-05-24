using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// Represents base customer.
    /// </summary>
    public abstract class BaseCustomer : BaseUser
    {
        protected UserType UserType;
        protected UserStatus UserStatus;
        protected List<Payment> Payments;

        protected BaseCustomer(int id, string name, UserType userType,
            UserStatus userStatus, List<Payment> payments)
        {
            Id = id;
            Name = name;
            UserType = userType;
            UserStatus = userStatus;
            Payments = payments;
        }

        /// <summary>
        /// Activate customer
        /// </summary>
        public virtual bool Activate()
        {
            UserStatus = UserStatus.Active;
            return true;
        }

        /// <summary>
        /// Deactivate customer.
        /// </summary>
        public virtual bool Deactivate()
        {
            UserStatus = UserStatus.Disabled;
            return true;
        }

        /// <summary>
        /// Level up customer
        /// </summary>
        /// <returns>Leveled up user</returns>
        public abstract BaseCustomer LevelUp();

        public User ToUser()
        {
            return new User
            {
                Id = Id,
                Name = Name,
                UserType = UserType,
                UserStatus = UserStatus,
                Payments = Payments
            };
        }
    }
}