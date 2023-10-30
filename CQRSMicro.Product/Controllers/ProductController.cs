using CQRSMicro.Product.CQRS.Commands.Request;
using CQRSMicro.Product.CQRS.Queries.Request;
using CQRSMicro.Product.Fuzzy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Patika.Framework.Shared.Controllers;

namespace CQRSMicro.Product.Controllers
{

    [Route("api/product/[controller]")]
    public class ProductController : GenericApiController
    {
        IMediator Mediator { get; }
        public ProductController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Mediator = GetService<IMediator>();
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] GetAllProductQueryRequest requestModel)
        {
            var allProducts = await Mediator.Send(requestModel);
            return Ok(allProducts);
        }


        [HttpGet("id")]
        public async Task<IActionResult> GetAsync([FromQuery] GetByIdProductQueryRequest requestModel)
        {
            var product = await Mediator.Send(requestModel);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductCommandRequest requestModel)
        {
            var response = await Mediator.Send(requestModel);
            return Ok(response);
        }

        [HttpPost("mobile")]
        [Produces("application/json")]
        public async Task<IActionResult> SendOTPAsync([FromBody] SendOTPCommandRequest requestModel)
        {
            var response = await Mediator.Send(requestModel);
            return Ok(response);
        }

        [HttpGet("FuzzySharpSearch")]
        public async Task<IActionResult> FuzzySharpSearchAsync([FromQuery] string key)
        {
            var fuzzy = new FuzzySharpSearch();
            var response = fuzzy.SearchAll(key);
            return Ok(response);
        }

        [HttpGet("FuzzySharpSearch2")]
        public async Task<IActionResult> FuzzySharpSearch2Async([FromQuery] string key)
        {
            var fuzzy = new FuzzySharpSearch2();
            var response = fuzzy.SearchAll(key);
            return Ok(response);
        }


        //[HttpGet("FuzzyStringSearch")]
        //public async Task<IActionResult> FuzzyStringSearchAsync([FromQuery] string key)
        //{
        //    var fuzzy = new FuzzyStringSearch();
        //    var response = fuzzy.SearchAll(key);
        //    return Ok(response);
        //}


    }
}
