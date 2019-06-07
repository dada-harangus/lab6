using Lab2Expense.Models;
using Lab2Expense.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestUsersService
{
    class CommentServiceTest
    {
        [Test]
        public void GetAllShouldReturnCorrectNumberOfPages()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var commentService = new CommentService(context);
                var expenseService = new ExpenseService(context);
                var addedFlower = expenseService.Create(new Lab2Expense.ViewModels.ExpensePostModel
                {
                    Description = "jshdkhsakjd",
                    Sum = 1.23,
                    Location = "jsfkdsf",
                    Date = new DateTime(),
                    Currency = "euro",
                    ExpenseType = "food",

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "asd",
                            Owner = null
                        }
                    },

                }, null);

                var allComments = commentService.GetAll(string.Empty);
                Assert.NotNull(allComments);
            }
        }
        [Test]
        public void GetByIdTest()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var commentService = new CommentService(context);
                var expenseService = new ExpenseService(context);
                var addedExpense = expenseService.Create(new Lab2Expense.ViewModels.ExpensePostModel
                {
                    Description = "jshdkhsakjd",
                    Sum = 1.23,
                    Location = "jsfkdsf",
                    Date = new DateTime(),
                    Currency = "euro",
                    ExpenseType = "food",

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "asd",
                            Owner = null
                        }
                    },

                }, null);

                //   var comment = commentService.GetById(addedExpense.Comments.Find());
                //  Assert.NotNull(allComments);
            }
        }

    }
}
