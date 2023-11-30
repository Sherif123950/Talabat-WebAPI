using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data.Contexts;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreDbContext _dbContext;

        public BuggyController(StoreDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        [HttpGet("NotFound")]
        public ActionResult GetNotFound()
        {
            var Products = _dbContext.Products.Find(100);
            if (Products is null) return NotFound(new ApiResponse(404));
            return Ok(Products);
        }
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var Products = _dbContext.Products.Find(100);
           var ProductToReturn=Products.ToString();
            return Ok(ProductToReturn);
        }
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }

    }
}
