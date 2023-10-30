using CQRSMicro.Product.CQRS.Commands.Response;
using MediatR;
using Patika.Framework.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSMicro.Product.CQRS.Commands.Request
{
    public class SendOTPCommandRequest : DTO, IRequest<DTO>
    {
        public string MobileNumber { get; set; } = string.Empty;
    }
}
