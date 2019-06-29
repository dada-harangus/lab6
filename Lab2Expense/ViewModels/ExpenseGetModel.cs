using Lab2Expense.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.ViewModels
{
    //public enum ExpenseType
    //{ food, utilities, transportation, outing, groceries, clothes, electronics, other }
    public class ExpenseGetModel
    {
        public string Description { get; set; }
        public double Sum { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
       public string ExpenseType { get; set; }
        public int NumberOfComments { get; set; }

        public static ExpenseGetModel FromExpense(Expense expense)
        {
            string expenseType = "other";
            if (expense.ExpenseType == Models.ExpenseType.food)
            {
                expenseType = "food";
            }
            else if (expense.ExpenseType == Models.ExpenseType.utilities)
            {
                expenseType = "utilities";
            }
            else if (expense.ExpenseType == Models.ExpenseType.transportation)
            {
                expenseType = "transportation";
            }
            else if (expense.ExpenseType == Models.ExpenseType.outing)
            {
                expenseType = "outing";
            }
            else if (expense.ExpenseType == Models.ExpenseType.groceries)
            {
                expenseType = "groceries";
            }
            else if (expense.ExpenseType == Models.ExpenseType.clothes)
            {
                expenseType = "clothes";
            }
            else if (expense.ExpenseType == Models.ExpenseType.electronics)
            {
                expenseType = "electronics";
            }




            return new ExpenseGetModel
            {
                Description = expense.Description,
                Sum = expense.Sum,
                Location = expense.Location,
                Date = expense.Date,
                Currency = expense.Currency,
                ExpenseType = expenseType,
                NumberOfComments = expense.Comments.Count
            };
        }
    }
}
