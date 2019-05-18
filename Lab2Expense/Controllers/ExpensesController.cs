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
        /// <summary>
        /// Get all the expenses
        /// </summary>
        /// <param name="from"> Optional,filter by minimum DatePicked</param>
        /// <param name="to">Optional,filter by maximim DatePicked</param>
        /// <param name="type">Optional filter by expense type</param>
        /// <returns>A list of Expense objects</returns>
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
        /// <summary>
        /// Get the Expense that has the id requested
        /// </summary>
        /// <param name="id">The id of the Expense</param>
        /// <returns>The flower with the given Id</returns>
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

        /// <summary>
        /// Add an Expense
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST/expenses
        /// {
        ///   "description": "cheeseburger2",
        ///   "sum": 10,
        /// "location": "fastfood",
        ///"date": "2019-05-09T12:30:00",
        /// "currency": "EUR",
        /// "expenseType": 5
        /// }
        /// </remarks>
        /// <param name="expense">The expense that we want to add</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody] Expense expense )
        {
            context.Expenses.Add(expense);
            context.SaveChanges();
        }

        // PUT: api/Expenses/5
        /// <summary>
        /// Update the Expense with the given id
        /// </summary>
        /// <param name="id">The id of the expense we want to update</param>
        /// <param name="expense">The Expense that contains the new data</param>
        /// <returns>An Expense object</returns>
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
        /// <summary>
        /// Delete the Expense with the given id
        /// </summary>
        /// <param name="id">The id of the expense we want to delete</param>
        /// <returns>an Expense object</returns>
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
