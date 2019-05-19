using Lab2Expense.Models;
using Lab2Expense.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.Services
{
    public interface IExpenseService
    {
        IEnumerable<ExpenseGetModel> GetAll(DateTime? from = null, DateTime? to = null, ExpenseType? type = null);
        Expense GetById(int id);
        Expense Create(ExpensePostModel expense);
        Expense Upsert(int id, Expense expense);
        Expense Delete(int id);

    }
    public class ExpenseService : IExpenseService
    {
        private ExpensesDbContext context;
        public ExpenseService(ExpensesDbContext context)
        {
            this.context = context;
        }


        public Expense Create(ExpensePostModel expense)
        {
            Expense toAdd = ExpensePostModel.ToExpense(expense);
            context.Expenses.Add(toAdd);
            context.SaveChanges();
            return toAdd;


        }

        public Expense Delete(int id)
        {
            var existing = context.Expenses
                .Include(f=>f.Comments)
                .FirstOrDefault(expense => expense.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Expenses.Remove(existing);
            context.SaveChanges();
            return existing;
        }

        public IEnumerable<ExpenseGetModel> GetAll(DateTime? from = null, DateTime? to = null, ExpenseType? type = null)
        {

            IQueryable<Expense> result = context.Expenses.Include(f => f.Comments);
            if (from == null && to == null && type == null)
            {
                return result.Select(f => ExpenseGetModel.FromExpense(f));
            }
            if (from != null)
            {
                result = result.Where(f => f.Date >= from);
            }
            if (to != null)
            {
                result = result.Where(f => f.Date <= to);
            }
            if (type != null)
            {
                result = result.Where(f => f.ExpenseType == type);
            }
            return result.Select(f => ExpenseGetModel.FromExpense(f)); ;
        }

        public Expense GetById(int id)
        {
            return context.Expenses
                .Include(e => e.Comments)
                .FirstOrDefault(e => e.Id == id);
        }

        public Expense Upsert(int id, Expense expense)
        {
            var existing = context.Expenses.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                context.
                    Expenses.Add(expense);
                context.SaveChanges();
                return expense;
            }
            expense.Id = id;
            context.Expenses.Update(expense);
            context.SaveChanges();
            return expense;
        }
    }
}
