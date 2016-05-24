using NUnit.Framework;
using Domain;

namespace Tests
{
    [TestFixture]
    public class SilverCustomerTests
    {
        [Test]
        public void GivenDeactivatedSilverWhenActivatingThenActivated()
        {
            // Arrange
            var customer = new SilverCustomer(1, "test", UserStatus.Disabled, null);

            // Act / Assert
            Assert.That(customer.Activate());
        }

        [Test]
        public void GivenSilverWithNoPaymentsWhenDeactivatingThenDeactivated()
        {
            // Arrange
            var customer = new SilverCustomer(1, "test", UserStatus.Active, null);

            // Act / Assert
            Assert.That(customer.Deactivate());
        }
    }
}