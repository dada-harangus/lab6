using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2Expense.Models;
using Lab2Expense.Services;
using Lab2Expense.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab2Expense.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Regular")]
    public class UserRoleController : ControllerBase
    {
        private IUserRoleService userRoleService;
        private IUsersService usersService;
        public UserRoleController(IUserRoleService userRoleService, IUsersService usersService)
        {
            this.userRoleService = userRoleService;
            this.usersService = usersService;
        }
        // GET: api/UserRole
        /// <summary>
        /// Get all the userRoles
        /// </summary>

        /// <returns>A list of Expense objects</returns>
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<UserRoleGetModel> Get()
        {

            return userRoleService.GetAll();

        }

        //GET: api/Expenses/5
        /// <summary>
        /// Get the Expense that has the id requested
        /// </summary>
        /// <param name = "id" > The id of the Expense</param>
        /// <returns>The expense with the given Id</returns>

        //    [HttpGet("{id}", Name = "GetExpense")]
        //public IActionResult Get(int id)
        //{
        //    var found = expenseService.GetById(id);

        //    if (found == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(found);
        //}

        // POST: api/UserRole

        /// <summary>
        /// Add an userRole
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST/userRole
        /// { "Name":admin
        ///   "description": "cheeseburger2",
        ///   
        /// }
        /// </remarks>
        /// <param name="expense">The userRole that we want to add</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody] UserRolePostModel userRolePostModel)
        {
            User addedBy = usersService.GetCurrentUser(HttpContext);
            userRoleService.Create(userRolePostModel,addedBy);
        }

        // PUT: api/UserRole/5
        /// <summary>
        /// Update the userRole with the given id
        /// </summary>
        ///  /// <remarks>
        /// Sample request:
        /// PUT/expenses
        /// {
        /// "Name": "admin"  
        /// "description": "cheeseburger2",
        ///   
        /// }
        /// </remarks>
        /// <param name="id">The id of the userRole we want to update</param>
        /// <param name="expense">The UserRole that contains the new data</param>
        /// <returns>An userRole object</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] UserRole userRole)
        {
            var result = userRoleService.Upsert(id, userRole);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Delete the userRole with the given id
        /// </summary>
        /// <param name="id">The id of the userRole we want to delete</param>
        /// <returns>an userRole object</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var existing = userRoleService.Delete(id);
            if (existing == null)
            {
                return NotFound();
            }

            return Ok(existing);
        }
    }
}