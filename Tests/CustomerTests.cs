using System;
using System.Collections.Generic;
using NUnit.Framework;
using Domain;

namespace Services.Tests
{
    // Why not to follow http://martinfowler.com/bliki/GivenWhenThen.html?
    // http://www.nunit.org/index.php?p=quickStart&r=2.6.3
    // https://github.com/Moq/moq4/wiki/Quickstart

    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void GivenDeactivatedCustomerWhenActivatingThenActivated()
        {
            // Arrange
            var customer = new Customer(1, "test", UserStatus.Disabled, null);

            // Act / Assert
            Assert.That(customer.Activate());
        }

        [Test]
        public void GivenCustomerWithNoPaymentsWhenDeactivatingThenDeactivated()
        {
            // Arrange
            var customer = new Customer(1, "test", UserStatus.Active, null);

            // Act / Assert
            Assert.That(customer.Deactivate());
        }

        [Test]
        public void GivenCustomerWithoutPendingPaymentWhenDeactivatingThenDeactivated()
        {
            // Arrange
            var payments = new List<Payment>
                {
                    new Payment
                    {
                        Id = 1,
                        UserId = 1,
                        Date = DateTime.Today,
                        PaymentStatus = PaymentStatus.Approved
                    }
                };
            var customer = new Customer(1, "test", UserStatus.Active, payments);

            // Act / Assert
            Assert.That(customer.Deactivate());
        }

        [Test]
        public void GivenCustomerWithPendingPaymentWhenDeactivatingThenNotDeactivated()
        {
            // Arrange
            var payments = new List<Payment>
                {
                    new Payment
                    {
                        Id = 1,
                        UserId = 1,
                        Date = DateTime.Today,
                        PaymentStatus = PaymentStatus.Pending
                    }
                };
            var customer = new Customer(1, "test", UserStatus.Active, payments);

            // Act / Assert
            Assert.That(customer.Deactivate() == false);
        }

        [Test]
        public void GivenDisabledCustomerWhenLevelUpThenNotLeveled()
        {
            // Arrange
            var customer = new Customer(1, "test", UserStatus.Disabled, null);

            // Act / Assert
            Assert.That(customer.LevelUp() as SilverCustomer == null);
        }

        [Test]
        public void GivenCustomerWithoutPaymentWhenLevelUpThenNotLeveled()
        {
            // Arrange
            var customer = new Customer(1, "test", UserStatus.Active, null);

            // Act / Assert
            Assert.That(customer.LevelUp() as SilverCustomer == null);
        }

        [Test]
        public void GivenCustomerWith1PaymentWhenLevelUpThenNotLeveled()
        {
            // Arrange
            var payments = new List<Payment>
                {
                    new Payment
                    {
                        Id = 1,
                        UserId = 1,
                        Date = DateTime.Today,
                        PaymentStatus = PaymentStatus.Approved
                    }
                };
            var customer = new Customer(1, "test", UserStatus.Active, payments);

            // Act / Assert
            Assert.That(customer.LevelUp() as SilverCustomer == null);
        }

        [Test]
        public void GivenCustomerWithPendingPaymentWhenLevelUpThenNotLeveled()
        {
            // Arrange
            var payments = new List<Payment>
                {
                    new Payment { Id = 1, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 2, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 3, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 4, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 5, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Pending }
                };
            var customer = new Customer(1, "test", UserStatus.Active, payments);

            // Act / Assert
            Assert.That(customer.LevelUp() as SilverCustomer == null);
        }

        [Test]
        public void GivenCustomerWithRejectedPaymentWhenLevelUpThenNotLeveled()
        {
            // Arrange
            var payments = new List<Payment>
                {
                    new Payment { Id = 1, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 2, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 3, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 4, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 5, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Rejected }
                };
            var customer = new Customer(1, "test", UserStatus.Active, payments);

            // Act / Assert
            Assert.That(customer.LevelUp() as SilverCustomer == null);
        }

        [Test]
        public void GivenCustomerWith5ApprovedPaymentsWhenLevelUpThenLeveled()
        {
            // Arrange
            var payments = new List<Payment>
                {
                    new Payment { Id = 1, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 2, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 3, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 4, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved },
                    new Payment { Id = 5, UserId = 1, Date = DateTime.Today, PaymentStatus = PaymentStatus.Approved }
                };
            var customer = new Customer(1, "test", UserStatus.Active, payments);

            // Act / Assert
            Assert.That(customer.LevelUp() as SilverCustomer != null);
        }
    }
}
