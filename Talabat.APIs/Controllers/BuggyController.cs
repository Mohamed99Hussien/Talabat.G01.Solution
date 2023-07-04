using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet ("notfound")] // GET: NotFound

        public ActionResult GetNotFoundRequest() 
        {
            var product = _context.products.Find(100);
            if (product == null) return NotFound(new ApiResponse(404));

            return Ok(product); 
        }

        [HttpGet("servererror")] // GET : buggy/severerror => Excepation

        public ActionResult GetSeverError()
        {
            var product = _context.products.Find(100);
           var ProductToReturn = product.ToString(); // will Throw Excepation [NullReferenceExcepation]

            return Ok(ProductToReturn);
        }

        [HttpGet("badrequest")] //GET : buggy/badrequest
        public ActionResult GetBadRequest() 
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")] //GET : buggy/badrequest/five => send string
        public ActionResult GetBadRequest(int id) // validation Error
        {
            return Ok();
        }
    }
}
