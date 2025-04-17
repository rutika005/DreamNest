using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System;
using System.Collections.Generic;

[Route("Payment")]
public class PaymentController : Controller
{
    // Use environment variables for API keys to keep them secure
    private readonly string _razorpayKey = "rzp_test_4hNsPD4OI0Nsk"; // Public Key
    private readonly string _razorpaySecret = "4UGkHGQw376kTuYWhIBaOEvC"; // Secret Key (store this securely)

    [HttpPost("CreateOrder")]
    public IActionResult CreateOrder([FromBody] RazorpayOrderRequestModel model)
    {
        try
        {
            RazorpayClient client = new RazorpayClient(_razorpayKey, _razorpaySecret);

            // Define the order parameters
            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", model.Amount * 100 }, // Amount in paise (multiply by 100)
                { "currency", "INR" },
                { "receipt", $"rcptid_{Guid.NewGuid()}" },
                { "payment_capture", 1 } // Immediate payment capture
            };

            // Create the Razorpay order
            Razorpay.Api.Order order = client.Order.Create(options);

            // Return the orderId and public key to the frontend
            return Json(new
            {
                orderId = order["id"].ToString(),
                key = _razorpayKey // Public key (to be used in the frontend for Razorpay checkout)
            });
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error creating Razorpay order: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred while creating the payment order." });
        }
    }

    [HttpPost("VerifyPayment")]
    public IActionResult VerifyPayment([FromBody] RazorpayPaymentResponse response)
    {
        try
        {
            RazorpayClient client = new RazorpayClient(_razorpayKey, _razorpaySecret);

            // Verify the payment with the payment ID received from frontend
            Payment payment = client.Payment.Fetch(response.PaymentId);
            if (payment["status"].ToString() == "captured")
            {
                // Payment successful
                return Json(new { success = true, message = "Payment verified" });
            }
            else
            {
                // Payment failed
                return Json(new { success = false, message = "Payment failed" });
            }
        }
        catch (Exception ex)
        {
            // Handle verification errors
            Console.WriteLine($"Error verifying payment: {ex.Message}");
            return StatusCode(500, new { message = "Payment verification failed" });
        }
    }

    public IActionResult Success()
    {
        return View();
    }

    public IActionResult Index()
    {
        return View();
    }
}

public class RazorpayPaymentResponse
{
    public string PaymentId { get; set; } // Corrected to public property for deserialization
}

public class RazorpayOrderRequestModel
{
    public int Amount { get; set; }
}
