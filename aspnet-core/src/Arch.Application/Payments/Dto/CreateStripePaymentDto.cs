using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.Payments.Dto
{
    public class CreateStripePaymentDto
    {
        [Required]
        public string Token { get; set; }  // Stripe Token from Frontend

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "usd";

        [Required]
        public string Description { get; set; }
    }

    public class StripePaymentResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
    }
}
