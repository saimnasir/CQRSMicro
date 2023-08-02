using CQRSMicro.CustomerApp.CQRS.Commands.Request;
using CQRSMicro.CustomerApp.CQRS.Queries.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Patika.Framework.Shared.Controllers;

namespace CQRSMicro.Customer.Controllers
{

    [Route("[controller]")]
    public class CustomerController : GenericApiController
    {
        IMediator Mediator { get; }
        public CustomerController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Mediator = GetService<IMediator>();
        }

        //[HttpGet]
        //public async Task<IActionResult> ListAsync([FromQuery] GetAllCustomerQueryRequest requestModel)
        //{
        //    var allCustomers = await Mediator.Send(requestModel);
        //    return Ok(allCustomers);
        //}


        [HttpGet("id")]
        public async Task<IActionResult> GetAsync([FromQuery] GetByIdCustomerQueryRequest requestModel)
        {
            var Customer = await Mediator.Send(requestModel);
            return Ok(Customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerCommandRequest requestModel)
        {
            var response = await Mediator.Send(requestModel);
            return Ok(response);
        }
    }
}
