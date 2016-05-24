﻿using System;
using System.Collections.Generic;
using Domain;
using Infrastructure;

namespace Dao
{
    public class CachingUserRepository : IUserRepository
    {
        private readonly ICache _cache;
        private readonly IUserRepository _userRepository;

        public CachingUserRepository(IUserRepository userRepository)
        {
            _cache = new Cache();
            _userRepository = userRepository;
        }

        public void Create(User user)
        {
            _userRepository.Create(user);
            _cache.Update(user);
        }

        public void Modify(User user)
        {
            _userRepository.Modify(user);
            _cache.Update(user);
        }

        public void Delete(User user)
        {
            _userRepository.Delete(user);
            _cache.Delete(user.Id);
        }

        public List<User> FindAll()
        {
            // no caching here
            return _userRepository.FindAll();
        }

        public User FindById(int id)
        {
            var user = _cache.TryGet(id) as User;
            if (user == null)
            {
                user = _userRepository.FindById(id);
                if (user != null)
                {
                    // user found, update cache!
                    _cache.Update(user);
                }
            }
            return user;
        }
    }
}

