using Lab2Expense.Models;
using Lab2Expense.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Expense.Services
{public interface ICommentService {

        IEnumerable<CommentGetModel> GetAll(string filter);
        Comment GetById(int id);
       // Comment Create(CommentPostModel expense);
        Comment Upsert(int id, Comment expense);
        Comment Delete(int id);
    }
    public class CommentService : ICommentService
    {
        private ExpensesDbContext context;
        public CommentService(ExpensesDbContext context)
        {
            this.context = context;
        }

        //public Comment Create(CommentPostModel comment)
        //{
        //    Comment toAdd = CommentPostModel.ToComment(comment);
        //    context.Comments.Add(toAdd);
        //    context.SaveChanges();
        //    return toAdd;


        //}

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

        public IEnumerable<CommentGetModel> GetAll(string filter)
        {
            //IQueryable<Comment> result = context.Comments;
            ////.Include(f => f.Comments);
            //if (filter != null)
            //{
            //    result = result.Where(f => f.Text.Contains(filter));
            //}
            //return result.Select(f => CommentGetModel.FromComment(f)); ;
            List<CommentGetModel> list=new List<CommentGetModel>();
            List<CommentGetModel> list2=new List<CommentGetModel>();
            IQueryable<Expense> result = context.Expenses.Include(f => f.Comments);
            //result = result
            //    .Where(e => e.Comments
            //    .Any(c => c.Text.Contains(filter)));
            if (filter == null) {
                foreach (Expense e in result)
                {
                    var temp = e.Comments;
                    foreach (Comment c in temp)
                    {                     
                        {  var m = new CommentGetModel
                            {
                                ExpenseId = e.Id,
                                Id = c.Id,
                                Text = c.Text,
                                Important = c.Important
                            };
                            list2.Add(m);
                        }
                    }

                }

                return list2;
            }
            foreach (Expense e in result)
            {   
               var temp = e.Comments;
                foreach (Comment c in temp)
                {
                    if (c.Text.Contains(filter)&&filter!=null)
                    {
                        var m = new CommentGetModel
                        {
                            ExpenseId = e.Id,
                            Id = c.Id,
                            Text = c.Text,
                            Important = c.Important
                        };
                        list.Add(m);
                    }
                }
                
            }
       
            return list;
            
        }

        public Comment GetById(int id)
        {
            return context.Comments
                // .Include(e => e.Comments)
                .FirstOrDefault(e => e.Id == id);
        }

        public Comment Upsert(int id, Comment expense)
        {
            var existing = context.Comments.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                context.Comments.Add(expense);
                context.SaveChanges();
                return expense;
            }
            expense.Id = id;
            context.Comments.Update(expense);
            context.SaveChanges();
            return expense;
        }
    }
}
