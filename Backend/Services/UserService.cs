using System.Collections.Generic;
using FilmStudioSFF.Models;
using FilmStudioSFF.Interfaces;
using System.Linq;
using BCrypt.Net;
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
        public UserRegister AuthenticateUser(UserAuthenticate loginRequest)
        {
            var user = _users.FirstOrDefault(u => u.Username == loginRequest.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return null;
            }
            return user;
        }

        // Get user by ID
        public UserRegister GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.UserId == id);
            
        }
        // Get all users
        public List<UserRegister> GetAllUsers()
        {
            return _users;
        }

        // Hash password using BCrypt
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); 
        }
    }
}
