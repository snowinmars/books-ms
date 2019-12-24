using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmptyService.Logger.Abstractions;
using EmptyService.WebApi.Mapper;
using EmptyService.WebApi.Models;
using Logic.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EmptyService.WebApi
{
    // ReSharper disable once AllowPublicClass
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        public BooksController(ILog log,
            IBookLogic logic)
        {
            this.log = log;
            this.logic = logic;
        }

        private readonly ILog log;

        private readonly IBookLogic logic;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken token = default)
        {
            var item = await logic.GetAsync(id, token);

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(BookModel model, CancellationToken token = default)
        {
            var book = model.ToBook();

            var freshItem = await logic.AddAsync(book, token);

            return Ok(freshItem);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, BookModel freshModel, CancellationToken token = default)
        {
            if (freshModel.Id != default && id != freshModel.Id)
            {
                throw new
                    InvalidOperationException($"The id in route doesn't match the id of the body entity: {id} vs {freshModel.Id}");
            }

            var freshBook = freshModel.ToBook();

            var updatedItem = await logic.UpdateAsync(freshBook, token);

            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var item = await logic.DeleteAsync(id, token);

            return Ok(item);
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListAsync(CancellationToken token = default)
        {
            var books = await logic.ListAsync(token);

            return Ok(books.Select(x => x.ToBookModel()));
        }
    }
}
