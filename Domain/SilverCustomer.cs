using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public sealed class SilverCustomer : BaseCustomer
    {
        public SilverCustomer(int id, string name, UserStatus userStatus, List<Payment> payments)
            : base(id, name, UserType.SilverCustomer, userStatus, payments)
        {
        }

        public override BaseCustomer LevelUp()
        {
            if (UserStatus == UserStatus.Active)
            {
                if (Payments == null ||
                    Payments.Count < 7 ||
                    Payments.Count(x => x.PaymentStatus == PaymentStatus.Approved) < 5 ||
                    Payments.Any(x => x.PaymentStatus == PaymentStatus.Rejected) ||
                    Payments.Any(x => x.PaymentStatus == PaymentStatus.Pending))
                {
                    // no pending or rejected payments allowed.
                    // customer must have at least 7 payments,
                    // at least 5 of them must be approved.
                    return this;
                }

                return new GoldCustomer(Id, Name, UserStatus, Payments);
            }
            return this;
        }

        public override bool Deactivate()
        {
            if (Payments == null || !Payments.Any() ||
                Payments.All(x => x.PaymentStatus != PaymentStatus.Pending) ||
                Payments.All(x => x.Date != DateTime.Today))
            {
                // deactivate if no payments or if no pending payments found or
                // no payments made today
                UserStatus = UserStatus.Disabled;
                return true;
            }
            return false;
        }
    }
}