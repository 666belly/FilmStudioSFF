using System.Collections.Generic;
using FilmStudioSFF.Models;
using FilmStudioSFF.Services;
using FilmStudioSFF.Interfaces;
using FilmStudioSFF.Controllers;
using System.Security.Cryptography;
using System.Text;

namespace FilmStudioSFF.Services
{
    public class UserService
    {
        private readonly List<User> _users = new List<User>();

        public User? RegisterUser(IUserRegister userRegister)
        {
            if (_users.Any(u => u.Username == userRegister.Username))
            {
                return null; //user already exists
            }

            var hashedPassword = HashPassword(userRegister.Password);
            var user = new User
            {
                UserId = _users.Count + 1,
                Username = userRegister.Username,
                Password = hashedPassword,
                Role = userRegister.Role
            };

            _users.Add(user);
            return user;
        }

        //Authonticate user
        public User? AuthenticateUser(IUserAuthenticate loginRequest)
        {
            var user = _users.FirstOrDefault(u => u.Username == loginRequest.Username);
            if (user == null || !VerifyPassword(loginRequest.Password, user.Password))
            {
                return null;
            }
            
            return user; 
        }

        //Get user by id
        public User GetUserById(int id)
        {

            return _users.FirstOrDefault(u => u.UserId == id)!;

        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        //Hash password
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        //Verify password
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }


}