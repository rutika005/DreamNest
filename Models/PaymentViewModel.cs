using Microsoft.AspNetCore.Mvc;

namespace Aesthetica.Models
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }  // Primary key for the Payments table
        public int UserId { get; set; }
        public string PropertyId { get; set; }  // Assuming PropertyId is a string
        public int PropertyID { get; internal set; }
        public decimal Amount { get; set; }
        public string OrderId { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PropertyTitle { get; set; }
        public string PropertyLocation { get; set; }
        public decimal RentAmount { get; set; }
        public string RazorpayPaymentId { get; set; }
    }
}
