using System.Collections.Generic;

namespace Domain
{
    public sealed class PlatinumCustomer : BaseCustomer
    {
        public PlatinumCustomer(int id, string name, UserStatus userStatus, List<Payment> payments)
            : base(id, name, UserType.PlatinumCustomer, userStatus, payments)
        {
        }

        public override BaseCustomer LevelUp()
        {
            // do nothing for the time being
            return this;
        }

        public override bool Deactivate()
        {
            // do nothing for the time being
            return false;
        }
    }
}

