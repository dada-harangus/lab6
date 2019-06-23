using Lab2Expense.Models;
using Lab2Expense.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.Services
{
    public interface IUserRoleService
    {
        IEnumerable<UserRoleGetModel> GetAll();
        UserRole Create(UserRolePostModel userRolePostModel);
        UserRole Upsert(int id, UserRolePostModel userRole);
        UserRole Delete(int id);

    }
    public class UserRoleService : IUserRoleService
    {
        private ExpensesDbContext context;
        public UserRoleService(ExpensesDbContext context)
        {
            this.context = context;
        }


        public UserRole Create(UserRolePostModel userPostModel)
        {
            UserRole toAdd = UserRolePostModel.ToUserRole(userPostModel);

            
            context.UserRole.Add(toAdd);
            context.SaveChanges();
            return toAdd;


        }

        public UserRole Delete(int id)
        {
            var existing = context.UserRole

                .FirstOrDefault(userRole => userRole.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.UserRole.Remove(existing);
            context.SaveChanges();
            return existing;
        }





        public IEnumerable<UserRoleGetModel> GetAll()
        {
            var result = context.UserRole.Select(userRole => new UserRoleGetModel
            {
                Id = userRole.Id,
                Name = userRole.Name,
                Description = userRole.Description
            }).ToList();
            return result;
        }




        public UserRole Upsert(int id, UserRolePostModel userRole)
        {
            UserRole toAdd = UserRolePostModel.ToUserRole(userRole);
            var existing = context.UserRole.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                
                context.
                    UserRole.Add(toAdd);
                context.SaveChanges();
                return toAdd;
            }
            toAdd.Id = id;
            context.UserRole.Update(toAdd);
            context.SaveChanges();
            return toAdd;
        }
    }
}
