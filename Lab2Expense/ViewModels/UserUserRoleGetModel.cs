using Lab2Expense.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.ViewModels
{
    public class UserUserRoleGetModel
    {
        public User User { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }



    }
}
