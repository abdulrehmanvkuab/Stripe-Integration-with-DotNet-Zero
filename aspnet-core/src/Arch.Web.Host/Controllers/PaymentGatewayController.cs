using Abp.AspNetCore.Mvc.Controllers;
using Arch.Payments.Dto;
using Arch.Payments;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Arch.Web.Controllers
{
  
        [Route("api/[controller]")]
        [ApiController]
        public class PaymentGatewayController : AbpController
        {
            private readonly IPaymentGatewayAppService _paymentGatewayAppService;

            public PaymentGatewayController(IPaymentGatewayAppService paymentGatewayAppService)
            {
                _paymentGatewayAppService = paymentGatewayAppService;
            }

            [HttpPost("stripe")]
            public async Task<IActionResult> ProcessStripePayment([FromBody] CreateStripePaymentDto input)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _paymentGatewayAppService.ProcessStripePayment(input);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
        }
    
}
