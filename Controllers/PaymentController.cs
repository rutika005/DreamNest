//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Http;
//using Razorpay.Api;
//using Aesthetica.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Aesthetica.Controllers
//{
//    public class PaymentController : Controller
//    {
//        private readonly IConfiguration _configuration;
//        private readonly AppDbContext _context;

//        public PaymentController(IConfiguration configuration, AppDbContext context)
//        {
//            _configuration = configuration;
//            _context = context;
//        }

//        public IActionResult Payment(int propertyId)
//        {
//            var userId = HttpContext.Session.GetInt32("UserId");

//            if (!userId.HasValue)
//            {
//                ViewBag.IsLoggedIn = false;
//                return View(); // Not logged in view
//            }

//            var paymentDetails = GetPaymentDetails(userId.Value, propertyId);

//            ViewBag.IsLoggedIn = true;
//            ViewBag.PaymentDetails = paymentDetails;

//            return View();
//        }

//        private PaymentViewModel GetPaymentDetails(int userId, int propertyId)
//        {
//            var model = _context.Properties
//                .Where(p => p.PropertyId == propertyId)
//                .Select(p => new PaymentViewModel
//                {
//                    UserId = userId,
//                    PropertyId = p.PropertyId.ToString(),
//                    PropertyTitle = p.Title,
//                    PropertyLocation = p.Address,
//                    RentAmount = p.Price
//                })
//                .FirstOrDefault();

//            return model;
//        }

//        [HttpPost]
//        public IActionResult ProceedPayment(int userId, string propertyId, decimal amount)
//        {
//            var key = _configuration["Razorpay:Key"];
//            var secret = _configuration["Razorpay:KeySecret"];

//            var client = new RazorpayClient(key, secret);

//            var options = new Dictionary<string, object>
//            {
//                { "amount", amount * 100 },
//                { "currency", "INR" },
//                { "receipt", Guid.NewGuid().ToString() },
//                { "payment_capture", 1 }
//            };

//            try
//            {
//                var order = client.Order.Create(options);
//                string orderId = order["id"].ToString();

//                SavePayment(orderId, userId, propertyId, amount);

//                ViewBag.orderId = orderId;
//                ViewBag.Amount = amount;
//                ViewBag.Key = key;
//                ViewBag.UserId = userId;

//                return View("PaymentConfirmation");
//            }
//            catch (Exception ex)
//            {
//                ViewBag.ErrorMessage = ex.Message;
//                return View("Error");
//            }
//        }

//        private void SavePayment(string orderId, int userId, string propertyId, decimal amount)
//        {
//            var payment = new PaymentViewModel
//            {
//                UserId = userId,
//                PropertyId = propertyId,
//                Amount = amount,
//                OrderId = orderId,
//                PaymentStatus = "Pending",
//                PaymentDate = DateTime.Now
//            };

//            _context.Payments.Add(payment);  // Add Payment entity to the context
//            _context.SaveChanges();  // Save changes to the database
//        }


//        public IActionResult VerifyPayment(string razorpayPaymentId, string orderId, string razorpaySignature)
//        {
//            try
//            {
//                UpdatePaymentStatus(razorpayPaymentId, orderId);
//                return RedirectToAction("Index", "Home");
//            }
//            catch (Exception ex)
//            {
//                ViewBag.ErrorMessage = "Payment update failed: " + ex.Message;
//                return View("Error");
//            }
//        }

//        private void UpdatePaymentStatus(string paymentId, string orderId)
//        {
//            var payment = _context.Payments.FirstOrDefault(p => p.OrderId == orderId);

//            if (payment != null)
//            {
//                payment.PaymentStatus = "Success";
//                payment.RazorpayPaymentId = paymentId;
//                payment.PaymentDate = DateTime.Now;

//                _context.SaveChanges();
//            }
//        }
//    }
//}
