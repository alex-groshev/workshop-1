using Dao;

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
            var factory = new CustomerFactory(_userRepository, _paymentRepository);
            var customer = factory.Create(id);

            if (customer == null)
            {
                return false;
            }

            customer.LevelUp();

            _userRepository.Modify(customer.ToUser());

            return true;
        }
    }
}
