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
        [EnumDataType(typeof(ExpenseType))]
        public ExpenseType ExpenseType { get; set; }
        public int NumberOfComments { get; set; }

        public static ExpenseGetModel FromExpense(Expense expense)
        {
            return new ExpenseGetModel
            {
                Description = expense.Description,
                Sum = expense.Sum,
                Location = expense.Location,
                Date = expense.Date,
                Currency = expense.Currency,
                ExpenseType = expense.ExpenseType,
                NumberOfComments = expense.Comments.Count
            };
        }
    }
}
