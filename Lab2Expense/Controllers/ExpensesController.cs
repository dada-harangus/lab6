using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2Expense.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2Expense.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private ExpensesDbContext context;
        public ExpensesController(ExpensesDbContext context)
        {
            this.context = context;
        }
        // GET: api/Expenses
        [HttpGet]
        public IEnumerable<Expense> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to,[FromQuery]ExpenseType? type) 
        {
            IQueryable<Expense> result = context.Expenses.Include(f=>f.Comments);
            if (from == null && to == null&&type==null)
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

        // GET: api/Expenses/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var existing = context.Expenses
                .Include(f => f.Comments)
                .FirstOrDefault(expense => expense.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            return Ok(existing);
        }

        // POST: api/Expenses
        [HttpPost]
        public void Post([FromBody] Expense expense )
        {
            context.Expenses.Add(expense);
            context.SaveChanges();
        }

        // PUT: api/Expenses/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Expense expense)
        {
            var existing = context.Expenses.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                context.Expenses.Add(expense);
                context.SaveChanges();
                return Ok(expense);
            }
            expense.Id = id;
            context.Expenses.Update(expense);
            context.SaveChanges();
            return Ok(expense);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = context.Expenses.FirstOrDefault(expense => expense.Id == id);
            if (existing == null)
            {
                return NotFound();
            }
            context.Expenses.Remove(existing);
            context.SaveChanges();
            return Ok();
        }
    }
}
