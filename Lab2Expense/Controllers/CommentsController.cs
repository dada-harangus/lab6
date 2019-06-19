using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2Expense.Models;
using Lab2Expense.Services;
using Lab2Expense.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab2Expense.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService commentService;
        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        // GET: api/Comments
        /// <summary>
        /// Get all the comments
        /// </summary>
        /// <param name="filter">Optional filter to get the comment whose text contains the filter</param>
        /// <returns>A list of comment objects</returns>
        [HttpGet]
        public IEnumerable<CommentGetModel> Get([FromQuery]string filter)
        {
            return commentService.GetAll(filter);
        }

        // GET: api/Comments/5
        /// <summary>
        /// Get the comment with the specified id
        /// </summary>
        /// <param name="id">The id of the comment</param>
        /// <returns>The comment with the given id</returns>
        [HttpGet("{id}", Name = "GetComment")]
        public IActionResult Get(int id)
        {
            var found = commentService.GetById(id);

            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        // POST: api/Comments
        /// <summary>
        /// Add a comment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST/comments
        /// {"text": "great shop",
        ///   "important": true, 
        /// "expenseId": 5
        /// }
        /// </remarks>
        /// <param name="comment">the comment that we want to add</param>
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public void Post([FromBody] CommentPostModel comment)
        //{
        //    commentService.Create(comment);
        //}

        // PUT: api/Comments/5
        /// <summary>
        /// update the comment with the specified id
        /// </summary>
        /// <param name="id">the id of the comment we want to update</param>
        /// <param name="comment">a comment that contains the new data</param>
        /// <returns>a comment object</returns>
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] Comment comment)
        //{
        //    var result = commentService.Upsert(id, comment);
        //    return Ok(result);
        //}

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Delete the comment with the specified id
        /// </summary>
        /// <param name="id">The id of the comment we want to delete</param>
        /// <returns>a comment object</returns>
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var existing = commentService.Delete(id);
        //    if (existing == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(existing);
        //}
    }
}
