using Lab2Expense.Models;
using Lab2Expense.Services;
using Lab2Expense.Validators;
using Lab2Expense.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
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

        /// <summary>
        /// TODO: AAA - Arrange, Act, Assert
        /// </summary>
        [Test]
        public void ValidRegisterShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewUser))// "ValidRegisterShouldCreateANewUser")
              .Options;
            var validator = new RegisterValidator();
            using (var context = new ExpensesDbContext(options))
            {
                UsersService usersService = new UsersService(context,validator, config);
                var added = new Lab2Expense.ViewModels.RegisterPostModel
                {
                    Email = "a@a.b",
                    FirstName = "fdsfsdfs",
                    LastName = "fdsfs",
                    Password = "1234567",
                    Username = "test_username"
                };
                var result = usersService.Register(added);

                Assert.IsNotNull(result);
             //   Assert.AreEqual(added.Username, result.Username);
            }
        }

        [Test]

        public void ShouldAuthentificate()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(ShouldAuthentificate))// "ValidRegisterShouldCreateANewUser")
                .Options;
            using (var context = new ExpensesDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context,validator, config);
                var added = new Lab2Expense.ViewModels.RegisterPostModel
                {
                    Email = "a@a.b",
                    FirstName = "fdsfsdfs",
                    LastName = "fdsfs",
                    Password = "1234567",
                    Username = "test_username"
                };
                var result = usersService.Register(added);
                var auth = new Lab2Expense.ViewModels.LoginPostModel
                {
                    Username = added.Username,
                    Password = added.Password
                };

                var resultAuth = usersService.Authenticate(auth.Username, auth.Password);

                Assert.IsNotNull(resultAuth.Token);


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
                var validator = new RegisterValidator();
                var usersService = new UsersService(context,validator, config);
                var added = new Lab2Expense.ViewModels.RegisterPostModel
                {
                    Email = "a@a.b",
                    FirstName = "fdsfsdfs",
                    LastName = "fdsfs",
                    Password = "1234567",
                    Username = "test_username"
                };
                var result = usersService.GetAll();
                Assert.IsNotNull(result);

            }
        }

        [Test]
        public void ValidDeleteIsRemovedShoulBeTrue()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidDeleteIsRemovedShoulBeTrue))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context,validator, config);
                var added = new Lab2Expense.ViewModels.RegisterPostModel
                {
                    Email = "a@a.b",
                    FirstName = "fdsfsdfs",
                    LastName = "fdsfs",
                    Password = "1234567",
                    Username = "test_username"
                };
                var resultAdded = usersService.Register(added);
            //    var resultDelete = usersService.Delete(resultAdded.Id);

          //      Assert.AreEqual(true, resultDelete.isRemoved);
            }
        }
    }
}