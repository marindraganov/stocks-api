using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Text;
using StocksAPI.Controllers.Models;

namespace StocksAPI.Data
{
    public class UserService
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private readonly DbPersister _dbPersister;
        private readonly Dictionary<int, User> _inMemoryUsers;

        public UserService(DbPersister dbPersister)
        {
            _dbPersister = dbPersister;
            _inMemoryUsers = GetUsersDictionary(_dbPersister.GetUsers());
        }

        private Dictionary<int, User> GetUsersDictionary(IEnumerable<User> users)
        {
            var result = new Dictionary<int, User>();

            foreach (var user in users)
            {
                result.Add(user.ID, user);
            }

            return result;
        }

        internal User Authenticate(string password, string email)
        {
            var user = _inMemoryUsers.
                Where(user => user.Value.Email == email).
                Select(u => u.Value).FirstOrDefault();

            if (user != null && 
                user.Password == GetHashedPassword(password))
            {
                return user;
            }

            return null;
        }

        public string GetHashedPassword(string pass)
        {
            return HashPasword(pass);
        }

        string HashPasword(string password)
        {
            var salt = Encoding.UTF8.GetBytes("myUnsecureSalt");
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        internal bool HasUserWithEmail(string email)
        {
            return _inMemoryUsers.Any(u => u.Value.Email == email);
        }

        internal bool RegisterUser(UserRegisterData userData)
        {
            if (!HasUserWithEmail(userData.Email))
            {
                var user = _dbPersister.AddUser(new User
                {
                    Email = userData.Email,
                    FisrtName = userData.FirstName,
                    LastName = userData.LastName,
                    Password = GetHashedPassword(userData.Password)
                });

                _inMemoryUsers.Add(user.ID, user);

                return true;
            }

            return false;
        }

        internal bool EditUser(int userID, UserUpdateData userData)
        {
            if (_inMemoryUsers.ContainsKey(userID))
            {
                var user = _dbPersister.EditUser(new User
                {
                    ID = userID,
                    FisrtName = userData.FirstName,
                    LastName = userData.LastName,
                    AvatarID= userData.AvatarID
                });

                _inMemoryUsers[user.ID] = user;

                return true;
            }

            return false;
        }
    }
}
