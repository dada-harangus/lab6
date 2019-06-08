using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2Expense.Models;
using Lab2Expense.ViewModels;

namespace Lab2Expense.Validators
{

    public interface IUserRoleValidator
    {
        ErrorsCollection Validate(UserRole userRole, ExpensesDbContext context);
    }
    public class RoleValidator : IUserRoleValidator
    {
        public ErrorsCollection Validate(UserRole userRole, ExpensesDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(UserRole) };

            var existing = context.UserRole.FirstOrDefault(userRole1 => userRole1.Name == userRole.Name);
            if (existing == null)
            {
                errorsCollection.ErrorMessages.Add("The role does not exist !");
            }
            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;

        }

        
    }
}
