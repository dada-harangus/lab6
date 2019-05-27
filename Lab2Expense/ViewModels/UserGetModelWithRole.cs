using Lab2Expense.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.ViewModels
{
    public class UserGetModelWithRole
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole UserRole { get; set; }

        public static UserGetModelWithRole FromUser(User user)
        {
            return new UserGetModelWithRole
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                UserRole = user.UserRole
            };
        }
    }
}
