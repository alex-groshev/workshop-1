using System.Collections.Generic;

namespace Domain
{
    public sealed class GoldCustomer : BaseCustomer
    {
        public GoldCustomer(int id, string name, UserStatus userStatus, List<Payment> payments)
            : base(id, name, UserType.GoldCustomer, userStatus, payments)
        {
        }

        public override BaseCustomer LevelUp()
        {
            // at the moment we cannot promote gold customer
            return this;
        }

        public override bool Deactivate()
        {
            // cannot be done online, only relationship manager can deactivate gold customer
            return false;
        }
    }
}