using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public sealed class Customer : BaseCustomer
    {
        public Customer(int id, string name, UserStatus userStatus, List<Payment> payments)
            : base(id, name, UserType.Customer, userStatus, payments)
        {
        }

        public override BaseCustomer LevelUp()
        {
            if (UserStatus == UserStatus.Active)
            {
                if (Payments == null ||
                    Payments.Count < 5 ||
                    Payments.Count(x => x.PaymentStatus == PaymentStatus.Approved) < 3 ||
                    Payments.Any(x => x.PaymentStatus == PaymentStatus.Rejected) ||
                    Payments.Any(x => x.PaymentStatus == PaymentStatus.Pending))
                {
                    // no pending or rejected payments allowed.
                    // customer must have at least 5 payments,
                    // at least 3 of them must be approved.
                    return this;
                }

                return new SilverCustomer(Id, Name, UserStatus, Payments);
            }
            return this;
        }

        public override bool Deactivate()
        {
            if (Payments == null || !Payments.Any() ||
                Payments.All(x => x.PaymentStatus != PaymentStatus.Pending))
            {
                // deactivate if no payments or if no pending payments found
                UserStatus = UserStatus.Disabled;
                return true;
            }
            return false;
        }
    }
}