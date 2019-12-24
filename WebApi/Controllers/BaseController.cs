using BookService.Logger.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BookService.WebApi.Controllers
{
    internal abstract class BaseController : ControllerBase
    {
        protected readonly ILog log;

        protected BaseController(ILog log)
        {
            this.log = log;
        }
    }
}
