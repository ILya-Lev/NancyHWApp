using LoyaltyProgram.Model;
using System;
using System.Collections.Generic;

namespace LoyaltyProgram.Services
{
    public class InMemoryLoyaltyProgramStore : ILoyaltyProgramStore
    {
        private static readonly Dictionary<int, LoyaltyProgramUser> _storage = new Dictionary<int, LoyaltyProgramUser>();

        public LoyaltyProgramUser GetUser(int userId)
        {
            if (_storage.ContainsKey(userId))
                return _storage[userId];

            return CreateDummyUser();
        }

        public int AddUser(LoyaltyProgramUser user)
        {
            var id = GenerateNextUserId();
            user.Id = id;
            _storage.Add(id, user);
            return id;
        }

        public void UpdateUser(int userId, LoyaltyProgramUser user)
        {
            if (!_storage.ContainsKey(userId))
                throw new Exception($"There is no {nameof(LoyaltyProgramUser)} with id {userId}.");

            var existingUser = _storage[userId];
            UpdateProperty(user, existingUser, nameof(LoyaltyProgramUser.Name));
            UpdateProperty(user, existingUser, nameof(LoyaltyProgramUser.LoyaltyPoints));
            UpdateProperty(user, existingUser, nameof(LoyaltyProgramUser.Settings));
        }

        private void UpdateProperty(LoyaltyProgramUser source, LoyaltyProgramUser target, string propertyName)
        {
            var propertyInfo = typeof(LoyaltyProgramUser).GetProperty(propertyName);
            var sourceValue = propertyInfo.GetValue(source);
            var targetValue = propertyInfo.GetValue(target);
            if (!sourceValue.Equals(targetValue))
                propertyInfo.SetValue(target, sourceValue);
        }

        private static LoyaltyProgramUser CreateDummyUser()
        {
            var user = new LoyaltyProgramUser { Id = GenerateNextUserId() };
            _storage.Add(user.Id, user);
            return user;
        }

        private static int GenerateNextUserId() => _storage.Count + 1;
    }
}