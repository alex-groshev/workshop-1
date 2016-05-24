using System;
using System.Linq;
using Dao;
using Domain;

namespace Services
{
    public class CustomerLevelUpService : ICustomerLevelUpService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;

        public CustomerLevelUpService()
            : this(new UserRepository(), new PaymentRepository())
        {
        }

        public CustomerLevelUpService(
            IUserRepository userRepository,
            IPaymentRepository paymentRepository)
        {
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Promotes (levels up) the customer if possible.
        /// </summary>
        /// <returns><c>true</c>, if user was promoted, <c>false</c> otherwise.</returns>
        /// <param name="id">User Id.</param>
        public bool LevelUp(int id)
        {
            bool result;
            var user = _userRepository.FindById(id);

            if (user == null)
            {
                result = false; // not found
            }
            else if (user.UserType == UserType.Admin || user.UserType == UserType.Support)
            {
                result = false; // technical users cannot be promoted
            }
            else if (user.UserType == UserType.Customer) // to Silver
            {
                if (user.UserStatus == UserStatus.Active)
                {
                    var payments = _paymentRepository.GetUserPayments(user.Id);

                    if (payments == null ||
                        payments.Count < 5 ||
                        payments.Count(x => x.PaymentStatus == PaymentStatus.Approved) < 3 ||
                        payments.Any(x => x.PaymentStatus == PaymentStatus.Rejected) ||
                        payments.Any(x => x.PaymentStatus == PaymentStatus.Pending))
                    {
                        // no pending or rejected payments allowed.
                        // customer must have at least 5 payments,
                        // at least 3 of them must be approved.
                        result = false;
                    }
                    else
                    {
                        user.UserType = UserType.SilverCustomer;
                        _userRepository.Modify(user);
                        result = true;
                    }
                }
                else
                {
                    result = false; // cannot promote inactive user
                }
            }
            else if (user.UserType == UserType.SilverCustomer) // to Gold
            {
                if (user.UserStatus == UserStatus.Active)
                {
                    var payments = _paymentRepository.GetUserPayments(user.Id);

                    if (payments == null ||
                        payments.Count < 7 ||
                        payments.Count(x => x.PaymentStatus == PaymentStatus.Approved) < 5 ||
                        payments.Any(x => x.PaymentStatus == PaymentStatus.Rejected) ||
                        payments.Any(x => x.PaymentStatus == PaymentStatus.Pending))
                    {
                        // no pending or rejected payments allowed.
                        // customer must have at least 7 payments,
                        // at least 5 of them must be approved.
                        result = false;
                    }
                    else
                    {
                        user.UserType = UserType.GoldCustomer;
                        _userRepository.Modify(user);
                        result = true;
                    }
                }
                else
                {
                    result = false; // cannot promote inactive user
                }
            }
            else if (user.UserType == UserType.GoldCustomer)
            {
                result = false; // i'm at the top of the food chain! (forever?)
            }
            else
            {
                throw new ArgumentException("Undefined user type");
            }

            return result;
        }
    }
}
