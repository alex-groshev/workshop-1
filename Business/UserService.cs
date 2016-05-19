﻿using System;
using System.Linq;
using Dao;
using Domain;
using Infrastructure;

namespace Services
{
    /// <summary>
    /// Hi, I'm UserService. I'm very-very important. So many responsibilities!
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICache _cache;

        public UserService()
            : this(new UserRepository(), new PaymentRepository(), new Cache())
        {
        }

        public UserService(
            IUserRepository userRepository,
            IPaymentRepository paymentRepository,
            ICache cache)
        {
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _cache = cache;
        }

        /// <summary>
        /// Activates the user.
        /// </summary>
        /// <returns><c>true</c>, if user was activated, <c>false</c> otherwise.</returns>
        /// <param name="id">User Id.</param>
        public bool Activate(int id)
        {
            bool result;
            var user = _userRepository.FindById(id);

            if (user == null)
            {
                result = false; // not found
            }
            else if (user.UserType == UserType.Admin || user.UserType == UserType.Support)
            {
                result = false; // cannot (de-)activate admin/support
            }
            else if (user.UserType == UserType.Customer ||
                     user.UserType == UserType.SilverCustomer ||
                     user.UserType == UserType.GoldCustomer)
            {
                user.UserStatus = UserStatus.Active;
                _userRepository.Modify(user);
                result = true;
            }
            else
            {
                throw new ArgumentException("Undefined user type");
            }

            if (result)
            {
                _cache.Update(user);
            }

            return result;
        }

        /// <summary>
        /// Deactivates the user.
        /// </summary>
        /// <returns><c>true</c>, if user was deactivated, <c>false</c> otherwise.</returns>
        /// <param name="id">User Id.</param>
        public bool Deactivate(int id)
        {
            bool result;
            var user = _userRepository.FindById(id);

            if (user == null)
            {
                result = false; // not found
            }
            else if (user.UserType == UserType.Admin || user.UserType == UserType.Support)
            {
                result = false; // cannot (de-)activate admin/support
            }
            else if (user.UserType == UserType.Customer)
            {
                var payments = _paymentRepository.GetUserPayments(user.Id);

                if (payments == null)
                {
                    result = true; // no payments - deactivate
                }
                else if (payments.Any(x => x.PaymentStatus == PaymentStatus.Pending))
                {
                    result = false; // cannot deactivate customer with pending payments
                }
                else
                {
                    user.UserStatus = UserStatus.Disabled;
                    _userRepository.Modify(user);
                    result = true;
                }
            }
            else if (user.UserType == UserType.SilverCustomer)
            {
                var payments = _paymentRepository.GetUserPayments(user.Id);

                if (payments == null)
                {
                    result = true; // no payments - deactivate
                }
                else if (payments.Any(x => x.PaymentStatus == PaymentStatus.Pending) ||
                    payments.Any(x => x.Date == DateTime.Today))
                {
                    // cannot deactivate silver customer with pending payments OR
                    // who's made a payment today.
                    result = false;
                }
                else
                {
                    user.UserStatus = UserStatus.Disabled;
                    _userRepository.Modify(user);
                    result = true;
                }
            }
            else if (user.UserType == UserType.GoldCustomer)
            {
                // cannot be done online, only relationship manager can deactivate gold customer
                result = false;
            }
            else
            {
                throw new ArgumentException("Undefined user type");
            }

            if (result)
            {
                _cache.Update(user);
            }

            return result;
        }

        /// <summary>
        /// Promotes (levels up) the customer if possible.
        /// </summary>
        /// <returns><c>true</c>, if user was promoted, <c>false</c> otherwise.</returns>
        /// <param name="id">User Id.</param>
        public bool LevelUp(int id)
        {
            var user = _userRepository.FindById(id);

            if (user == null)
            {
                return false; // not found
            }

            var payments = _paymentRepository.GetUserPayments(user.Id);

            if (user.LevelUp(payments))
            {
                _userRepository.Modify(user);
                _cache.Update(user);
                return true;
            }

            return false;
        }
    }
}
