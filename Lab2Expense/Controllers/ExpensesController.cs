﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Lab2Expense.Models;
using Lab2Expense.Services;
using Lab2Expense.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2Expense.Controllers
{
 //   [Authorize(Roles = "Admin,Regular")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private IExpenseService expenseService;
        private IUsersService usersService;
        public ExpensesController(IExpenseService expenseService, IUsersService usersService)
        {
            this.expenseService = expenseService;
            this.usersService = usersService;
        }
        // GET: api/Expenses
        /// <summary>
        /// Get all the expenses
        /// </summary>
        /// <param name="from"> Optional,filter by minimum DatePicked</param>
        /// <param name="to">Optional,filter by maximim DatePicked</param>
        /// <param name="type">Optional filter by expense type</param>
        /// <returns>A list of Expense objects</returns>
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<ExpenseGetModel> Get([FromQuery]string from, [FromQuery]string to, [FromQuery]string type, [FromQuery]int page = 1)
        {

            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            if (from != "undefined" && to != "undefined"&& from !=null && to!=null)
            {
                dateFrom = DateTime.ParseExact(from, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                dateTo = DateTime.ParseExact(to, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            ExpenseType expenseType = new ExpenseType();

            if (type == "none")
            {
                expenseType = Models.ExpenseType.none;
            }
                if (type == "other")
                {
                    expenseType = Models.ExpenseType.other;
                }
                else if (type == "food")
                {
                    expenseType = Models.ExpenseType.food;
                }
                else if (type == "utilities")
                {
                    expenseType = Models.ExpenseType.utilities;
                }
                else if (type == "transportation")
                {
                    expenseType = Models.ExpenseType.transportation;
                }
                else if (type == "outing")
                {
                    expenseType = Models.ExpenseType.outing;
                }
                else if (type == "groceries")
                {
                    expenseType = Models.ExpenseType.groceries;
                }
                else if (type == "clothes")
                {
                    expenseType = Models.ExpenseType.clothes;
                }
                else if (type == "electronics")
                {
                    expenseType = Models.ExpenseType.electronics;
                }
            
            page = Math.Max(page, 1);
            return expenseService.GetAll(page, dateFrom, dateTo,expenseType);

        }

        // GET: api/Expenses/5
        /// <summary>
        /// Get the Expense that has the id requested
        /// </summary>
        /// <param name="id">The id of the Expense</param>
        /// <returns>The expense with the given Id</returns>
        [HttpGet("{id}", Name = "GetExpense")]
        public IActionResult Get(int id)
        {
            var found = expenseService.GetById(id);

            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
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
        public void Post([FromBody] ExpensePostModel expense)
        {
            User addedBy = usersService.GetCurrentUser(HttpContext);
            expenseService.Create(expense, addedBy);
        }

        // PUT: api/Expenses/5
        /// <summary>
        /// Update the Expense with the given id
        /// </summary>
        ///  /// <remarks>
        /// Sample request:
        /// PUT/expenses
        /// {
        ///   "description": "cheeseburger2",
        ///   "sum": 10,
        /// "location": "fastfood",
        ///"date": "2019-05-09T12:30:00",
        /// "currency": "EUR",
        /// "expenseType": 5
        /// }
        /// </remarks>
        /// <param name="id">The id of the expense we want to update</param>
        /// <param name="expense">The Expense that contains the new data</param>
        /// <returns>An Expense object</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Expense expense)
        {
            var result = expenseService.Upsert(id, expense);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Delete the Expense with the given id
        /// </summary>
        /// <param name="id">The id of the expense we want to delete</param>
        /// <returns>an Expense object</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var existing = expenseService.Delete(id);
            if (existing == null)
            {
                return NotFound();
            }

            return Ok(existing);
        }
    }
}
