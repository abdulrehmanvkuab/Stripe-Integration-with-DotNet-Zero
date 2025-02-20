using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Arch.Payments.Dto;

namespace Arch.Payments
{
    public interface IPaymentGatewayAppService : IApplicationService
    {
        Task<StripePaymentResponseDto> ProcessStripePayment(CreateStripePaymentDto input);
    }
}
