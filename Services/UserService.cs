using System.Collections.Generic;
using FilmStudioSFF.Models;
using FilmStudioSFF.Interfaces;
using System.Linq;
using BCrypt.Net; // Importera BCrypt.Net för lösenordshashning
using System;
using Microsoft.AspNetCore.Mvc;

namespace FilmStudioSFF.Services
{
    public class UserService 
    {
        private readonly List<UserRegister> _users = new List<UserRegister>();

        // Register a new user
        public UserRegister? RegisterUser(IUserRegister userRegister)
        {
            // Logga antalet användare innan registreringen
            Console.WriteLine($"Total users before registration: {_users.Count}");

            if (_users.Any(u => u.Username == userRegister.Username))
            {
                return null; // Användaren finns redan
            }

            var hashedPassword = HashPassword(userRegister.Password);
            var user = new UserRegister
            {
                UserId = _users.Count + 1,
                Username = userRegister.Username,
                Password = hashedPassword,
                Role = userRegister.Role
            };

            _users.Add(user);

            // Logga alla användare efter registreringen
            Console.WriteLine("Current users in list after registration:");
            foreach (var u in _users)
            {
                Console.WriteLine($"User: {u.Username}, Password Hash: {u.Password}");
            }

            // Logga antalet användare efter registreringen
            Console.WriteLine($"Total users after registration: {_users.Count}");

            return user;
        }


        // Authenticate user
        public UserRegister? AuthenticateUser(IUserAuthenticate loginRequest)
        {
            // Logga antalet användare innan autentisering
            Console.WriteLine($"Total users before authentication: {_users.Count}");

            // Logga alla användare i systemet innan inloggning
            Console.WriteLine("All users in the system (before login attempt):");
            foreach (var user in _users)
            {
                Console.WriteLine($"User: {user.Username}, Password Hash: {user.Password}");
            }

            Console.WriteLine($"Attempting to log in with Username: {loginRequest.Username}");

            var existingUser = _users.FirstOrDefault(u => string.Equals(u.Username, loginRequest.Username, StringComparison.OrdinalIgnoreCase));
            
            if (existingUser == null)
            {
                Console.WriteLine($"No user found with username: {loginRequest.Username}");
                return null; // Användaren hittades inte
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, existingUser.Password))
            {
                Console.WriteLine($"Password mismatch for user: {loginRequest.Username}");
                return null; // Lösenordet stämmer inte
            }

            // Logga antalet användare efter autentisering
            Console.WriteLine($"Total users after authentication: {_users.Count}");

            Console.WriteLine($"User {loginRequest.Username} authenticated successfully.");
            return existingUser;
        }

        // Get user by ID
        public UserRegister GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.UserId == id)!;
        }

        // Get all users
        public List<UserRegister> GetAllUsers()
        {
            return _users;
        }

        // Hash password using BCrypt
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); // Hash the password using BCrypt
        }
    }
}
