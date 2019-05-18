using Lab2Expense.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.Services
{
    public interface IExpenseService {
        IEnumerable<Expense> GetAll(DateTime? from = null, DateTime? to = null,ExpenseType? type=null);
        Expense GetById(int id);
        Expense Create(Expense expense);
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


        public Expense Create(Expense expense)
        {
            context.Expenses.Add(expense);
            context.SaveChanges();
            return expense;


        }

        public Expense Delete(int id)
        {
            var existing = context.Expenses.FirstOrDefault(expense => expense.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Expenses.Remove(existing);
            context.SaveChanges();
            return existing;
        }

        public IEnumerable<Expense> GetAll(DateTime? from = null, DateTime? to = null, ExpenseType? type = null)
        {

            IQueryable<Expense> result = context.Expenses.Include(f => f.Comments);
            if (from == null && to == null && type == null)
            {
                return result;
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
            return result;
        }

        public Expense GetById(int id)
        {
            return context.Expenses
                .Include(e=>e.Comments)
                .FirstOrDefault(e => e.Id == id);
        }

        public Expense Upsert(int id, Expense expense)
        {
            var existing = context.Expenses.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                context.Expenses.Add(expense);
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
