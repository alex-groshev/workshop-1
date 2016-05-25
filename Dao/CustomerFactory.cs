using Domain;

namespace Dao
{
    public class CustomerFactory
    {
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;

        public CustomerFactory(IUserRepository userRepository,
            IPaymentRepository paymentRepository)
        {
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
        }

        public BaseCustomer Create(int id)
        {
            BaseCustomer result;

            var user = _userRepository.FindById(id);
            if (user == null)
            {
                // return null for the time being.
                // it would be nice to have NullCustomer so it could be processed safely.
                return null;
            }

            switch (user.UserType)
            {
                case UserType.Customer:
                    result = new Customer(user.Id, user.Name, user.UserStatus,
                        _paymentRepository.GetUserPayments(user.Id));
                    break;
                case UserType.SilverCustomer:
                    result = new SilverCustomer(user.Id, user.Name, user.UserStatus,
                        _paymentRepository.GetUserPayments(user.Id));
                    break;
                case UserType.GoldCustomer:
                    result = new GoldCustomer(user.Id, user.Name, user.UserStatus,
                        _paymentRepository.GetUserPayments(user.Id));
                    break;
                case UserType.PlatinumCustomer:
                    result = new PlatinumCustomer(user.Id, user.Name, user.UserStatus,
                        _paymentRepository.GetUserPayments(user.Id));
                    break;
                default:
                    //case UserType.Admin:
                    //case UserType.Support:
                    // we don't need technical users, return null for the time being.
                    // it would be nice to have NullCustomer so it could be processed safely.
                    return null;
            }

            return result;
        }
    }
}