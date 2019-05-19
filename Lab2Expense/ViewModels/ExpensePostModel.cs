using Lab2Expense.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.ViewModels
{
    public class ExpensePostModel
    {
        public string Description { get; set; }
        public double Sum { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }

        public string ExpenseType { get; set; }
        public List<Comment> Comments { get; set; }

        public static Expense ToExpense(ExpensePostModel expense)
        {
            ExpenseType expenseType = Models.ExpenseType.other;
            if (expense.ExpenseType == "food")
            {
                expenseType = Models.ExpenseType.food;
            }
            else if (expense.ExpenseType == "utilities")
            {
                expenseType = Models.ExpenseType.utilities;
            }
            else if (expense.ExpenseType == "transportation")
            {
                expenseType = Models.ExpenseType.transportation;
            }
            else if (expense.ExpenseType == "outing")
            {
                expenseType = Models.ExpenseType.outing;
            }
            else if (expense.ExpenseType == "groceries")
            {
                expenseType = Models.ExpenseType.groceries;
            }
            else if (expense.ExpenseType == "clothes")
            {
                expenseType = Models.ExpenseType.clothes;
            }
            else if (expense.ExpenseType == "electronics")
            {
                expenseType = Models.ExpenseType.electronics;
            }
            return new Expense
            {
                Description = expense.Description,
                Sum = expense.Sum,
                Location = expense.Location,
                Date = expense.Date,
                Comments = expense.Comments,
                ExpenseType = expenseType
            };
        }
    }
}

