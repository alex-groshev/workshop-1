using Dao;

namespace Services
{
    public class CustomerActivationService : ICustomerActivationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;

        public CustomerActivationService()
            : this(new UserRepository(), new PaymentRepository())
        {
        }

        public CustomerActivationService(
            IUserRepository userRepository,
            IPaymentRepository paymentRepository)
        {
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Activates the user.
        /// </summary>
        /// <returns><c>true</c>, if user was activated, <c>false</c> otherwise.</returns>
        /// <param name="id">User Id.</param>
        public bool Activate(int id)
        {
            var factory = new CustomerFactory(_userRepository, _paymentRepository);
            var customer = factory.Create(id);

            if (customer == null)
            {
                return false;
            }

            customer.Activate();

            _userRepository.Modify(customer.ToUser());

            return true;
        }

        /// <summary>
        /// Deactivates the user.
        /// </summary>
        /// <returns><c>true</c>, if user was deactivated, <c>false</c> otherwise.</returns>
        /// <param name="id">User Id.</param>
        public bool Deactivate(int id)
        {
            var factory = new CustomerFactory(_userRepository, _paymentRepository);
            var customer = factory.Create(id);

            if (customer == null)
            {
                return false;
            }

            customer.Deactivate();

            _userRepository.Modify(customer.ToUser());

            return true;
        }
    }
}

