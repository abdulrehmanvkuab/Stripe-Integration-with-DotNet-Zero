using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Arch.Payments.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Arch.Payments
{
    public class PaymentGatewayAppService : ApplicationService, IPaymentGatewayAppService
    {
        private readonly string _stripeSecretKey;
        private readonly ILogger<PaymentGatewayAppService> _logger;

        public PaymentGatewayAppService(IConfiguration configuration, ILogger<PaymentGatewayAppService> logger)
        {
            _stripeSecretKey = configuration["Payment:Stripe:SecretKey"]
                ?? throw new ArgumentNullException(nameof(configuration), "Stripe SecretKey is missing in appsettings.json");

            StripeConfiguration.ApiKey = _stripeSecretKey;
            _logger = logger;
        }

        public async Task<StripePaymentResponseDto> ProcessStripePayment(CreateStripePaymentDto input)
        {
            try
            {
                _logger.LogInformation("Processing Stripe payment for amount: {Amount} {Currency}", input.Amount, input.Currency);

                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = (long)(input.Amount * 100), // Convert to cents
                    Currency = input.Currency,
                    Description = input.Description,
                    Source = input.Token,
                };

                var chargeService = new ChargeService();
                Charge charge = await chargeService.CreateAsync(chargeOptions);

                if (charge.Status == "succeeded")
                {
                    _logger.LogInformation("Payment succeeded with Transaction ID: {TransactionId}", charge.Id);
                    return new StripePaymentResponseDto
                    {
                        Success = true,
                        Message = "Payment successful!",
                        TransactionId = charge.Id
                    };
                }

                _logger.LogWarning("Payment failed for amount: {Amount} {Currency}", input.Amount, input.Currency);
                return new StripePaymentResponseDto
                {
                    Success = false,
                    Message = "Payment failed!",
                    TransactionId = null
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe Error: {ErrorMessage}", ex.Message);
                return new StripePaymentResponseDto
                {
                    Success = false,
                    Message = $"Stripe Error: {ex.Message}",
                    TransactionId = null
                };
            }
        }
    }
}
