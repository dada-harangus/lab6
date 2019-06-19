using Lab2Expense.Models;
using Lab2Expense.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.Services
{
    public interface ICommentService
    {

        IEnumerable<CommentGetModel> GetAll(string filter);
        Comment GetById(int id);
        Comment Create(CommentPostModel expense, int id);
        //  Comment Upsert(int id, Comment expense);
        Comment Delete(int id);
    }
    public class CommentService : ICommentService
    {
        private ExpensesDbContext context;
        public CommentService(ExpensesDbContext context)
        {
            this.context = context;
        }

        public Comment Create(CommentPostModel comment, int id)
        {
            Comment toAdd = CommentPostModel.ToComment(comment);
            Expense expense = context.Expenses.FirstOrDefault(exp => exp.Id == id);
            expense.Comments.Add(toAdd);
            context.SaveChanges();
            return toAdd;


        }

        public Comment Delete(int id)
        {
            var existing = context.Comments.FirstOrDefault(comment => comment.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Comments.Remove(existing);
            context.SaveChanges();
            return existing;
        }

        public IEnumerable<CommentGetModel> GetAll(string text)
        {
            IQueryable<CommentGetModel> result = context.Comments.Select(x => new CommentGetModel

            {



                Id = x.Id,
                Text = x.Text,
                Important = x.Important,
                ExpenseId = (from movies in context.Expenses
                             where movies.Comments.Contains(x)
                             select movies.Id).FirstOrDefault()
            });
            //var result = context.Comments.Select(x 

            if (text != null)
            {
                result = result.Where(comment => comment.Text.Contains(text));
            }

            return result;
        }

        public Comment GetById(int id)
        {
            return context.Comments
                // .Include(e => e.Comments)
                .FirstOrDefault(e => e.Id == id);
        }

        //public comment upsert(int id, comment expense)
        //{
        //    var existing = context.comments.asnotracking().firstordefault(f => f.id == id);
        //    if (existing == null)
        //    {
        //        context.comments.add(expense);
        //        context.savechanges();
        //        return expense;
        //    }
        //    expense.id = id;
        //    context.comments.update(expense);
        //    context.savechanges();
        //    return expense;
        //}
    }
}
