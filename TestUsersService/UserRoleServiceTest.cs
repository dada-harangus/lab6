using Lab2Expense.Models;
using Lab2Expense.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestUsersService
{
    class UserRoleServiceTest
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "dsadhjcghduihdfhdifd8ih"
            });
        }

        [Test]
        public void ValidRegisterShouldCreateANewRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewRole))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                UserRoleService userRoleService = new UserRoleService(context);
                var added = new Lab2Expense.ViewModels.UserRolePostModel
                {
                    Name = "Regular",
                    Description = "jskds"

                };
                var result = userRoleService.Create(added);
                Assert.IsNotNull(result);


            }
        }


        [Test]
        public void ValidDelete()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidDelete))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                UserRoleService userRoleService = new UserRoleService(context);
                var added = new Lab2Expense.ViewModels.UserRolePostModel
                {
                    Name = "Regular",
                    Description = "jskds"

                };
                var result = userRoleService.Create(added);
                var resultDelete = userRoleService.Delete(result.Id);
                var resultNull = userRoleService.Delete(38743);
                Assert.IsNotNull(resultDelete);
                Assert.IsNull(resultNull);


            }
        }

        [Test]
        public void ValidGetAll()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidGetAll))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                UserRoleService userRoleService = new UserRoleService(context);
                var added = new Lab2Expense.ViewModels.UserRolePostModel
                {
                    Name = "Regular",
                    Description = "jskds"

                };
                var result = userRoleService.GetAll();

                Assert.IsNotNull(result);


            }
        }

        [Test]
        public void ValidUpsert1()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidUpsert1))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                UserRoleService userRoleService = new UserRoleService(context);
                var added = new Lab2Expense.ViewModels.UserRolePostModel
                {
                    Name = "Regular",
                    Description = "jskds"

                };

                var addedForUpdate = new Lab2Expense.ViewModels.UserRolePostModel
                {
                    Name = "Admin",
                    Description = "smands"
                };
                var addedForUpdate2 = new Lab2Expense.Models.UserRole
                {
                    Name = "UserManager",
                    Description = "smands"
                };

                var result = userRoleService.Create(added);
                context.Entry(result).State = EntityState.Detached;
                added.Name = "Admin";

                var resultUpsert = userRoleService.Upsert(result.Id, added);
                     var resultNull = userRoleService.Upsert(8, added);
                     Assert.AreEqual("Admin", resultNull.Name);

                Assert.AreEqual("Admin", resultUpsert.Name);


            }



        }
    }
}