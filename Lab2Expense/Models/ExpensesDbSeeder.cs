using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.Models
{
    public class ExpensesDbSeeder
    {

        public static void Initialize(ExpensesDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any product
            if (context.Expenses.Any())
            {
                return;   // DB has been seeded
            }

            context.Expenses.AddRange(
                new Expense
                {
                    Description = "buying drinks for this week",
                    Sum = 10,
                    Location = "DrinksShop",
                    Date = DateTime.Parse("2019-05-06T12:30"),
                    Currency = "EUR",
                    //ExpenseType = ExpenseType.clothes

                }

            );
            context.SaveChanges();
        }
    }
}

   