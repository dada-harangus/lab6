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
        PaginatedList<ExpenseGetModel> GetAll(int page, DateTime? from = null, DateTime? to = null, ExpenseType? type = null);
        Expense GetById(int id);
        Expense Create(ExpensePostModel expense, User addedBy);
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


        public Expense Create(ExpensePostModel expense, User addedBy)
        {
            Expense toAdd = ExpensePostModel.ToExpense(expense);

            toAdd.Owner = addedBy;
            context.Expenses.Add(toAdd);
            context.SaveChanges();
            return toAdd;


        }

        public Expense Delete(int id)
        {
            var existing = context.Expenses
                .Include(f => f.Comments)
                .FirstOrDefault(expense => expense.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Expenses.Remove(existing);
            context.SaveChanges();
            return existing;
        }





        public PaginatedList<ExpenseGetModel> GetAll(int page, DateTime? from = null, DateTime? to = null, ExpenseType? type = null)
        {

            IQueryable<Expense> result = context.Expenses.OrderBy(f => f.Id).Include(f => f.Comments);
            PaginatedList<ExpenseGetModel> paginatedResult = new PaginatedList<ExpenseGetModel>();
            paginatedResult.CurrentPage = page;

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
            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<ExpenseGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<ExpenseGetModel>.EntriesPerPage)
                .Take(PaginatedList<ExpenseGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(f => ExpenseGetModel.FromExpense(f)).ToList();

            return paginatedResult;
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
