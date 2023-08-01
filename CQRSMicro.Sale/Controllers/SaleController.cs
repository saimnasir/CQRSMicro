using CQRSMicro.Sale.CQRS.Commands.Request;
using CQRSMicro.Sale.CQRS.Queries.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Patika.Framework.Shared.Controllers;

namespace CQRSMicro.Sale.Controllers
{

    [Route("[controller]")]
    public class SaleController : GenericApiController
    {
        IMediator Mediator { get; }
        public SaleController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Mediator = GetService<IMediator>();
        }

        //[HttpGet]
        //public async Task<IActionResult> ListAsync([FromQuery] GetAllSaleQueryRequest requestModel)
        //{
        //    var allSales = await Mediator.Send(requestModel);
        //    return Ok(allSales);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSaleCommandRequest requestModel)
        {
            var response = await Mediator.Send(requestModel);
            return Ok(response);
        }
    }
}
