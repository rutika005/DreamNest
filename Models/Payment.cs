namespace Aesthetica.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }  // Primary key
        public int UserId { get; set; }
        public string PropertyId { get; set; }
        public decimal Amount { get; set; }
        public string OrderId { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string RazorpayPaymentId { get; set; }
    }

}
