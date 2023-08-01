using CQRSMicro.Product.CQRS.Commands.Request;
using CQRSMicro.Product.CQRS.Queries.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Patika.Framework.Shared.Controllers;

namespace CQRSMicro.Product.Controllers
{

    [Route("[controller]")]
    public class ProductController : GenericApiController
    {
        IMediator Mediator { get; }
        public ProductController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Mediator = GetService<IMediator>();
        }

        //[HttpGet]
        //public async Task<IActionResult> ListAsync([FromQuery] GetAllProductQueryRequest requestModel)
        //{
        //    var allProducts = await Mediator.Send(requestModel);
        //    return Ok(allProducts);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductCommandRequest requestModel)
        {
            var response = await Mediator.Send(requestModel);
            return Ok(response);
        }
    }
}
