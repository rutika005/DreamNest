using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Razorpay.Api;
using Aesthetica.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aesthetica.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public PaymentController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Payment(int propertyId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                ViewBag.IsLoggedIn = false;
                return View(); // Not logged in view
            }

            var paymentDetails = GetPaymentDetails(userId.Value, propertyId);

            if (paymentDetails != null)
            {
                ViewBag.PaymentDetails = paymentDetails;
            }
            else
            {
                ViewBag.PaymentDetails = null; 
            }

            ViewBag.IsLoggedIn = true;
            return View();
        }

        private PaymentViewModel GetPaymentDetails(int userId, int propertyId)
        {
            var model = (from payment in _context.payments
                         join property in _context.properties
                         on payment.PropertyId equals property.PropertyId.ToString()  
                         where payment.UserId == userId && payment.PropertyId == propertyId.ToString()  
                         select new PaymentViewModel
                         {
                             UserId = userId,
                             PropertyId = property.PropertyId.ToString(),
                             PropertyTitle = property.Title,
                             PropertyLocation = property.Address,
                             RentAmount = property.Price,
                         }).FirstOrDefault();

            return model;
        }


        [HttpPost]
        public IActionResult ProceedPayment(int userId, string propertyId, decimal amount)
        {
             int finalAmount = 500;

            if (finalAmount < 100) 
            {
                amount = 100;
                ViewBag.ErrorMessage = "Amount should be at least ₹1.";
                return View("Error");
            }

            var key = _configuration["Razorpay:Key"];
            var secret = _configuration["Razorpay:Secret"];

            var client = new RazorpayClient(key, secret);

            var options = new Dictionary<string, object>


    {
        { "amount", (int)(100) }, 
        { "currency", "INR" },
        { "receipt", Guid.NewGuid().ToString() },
        { "payment_capture", 1 }
    };

            try
            {
                var order = client.Order.Create(options);
                string orderId = order["id"].ToString();

                SavePayment(orderId, userId, propertyId, amount);

                ViewBag.orderId = orderId;
                ViewBag.Amount = amount;
                ViewBag.Key = key;
                ViewBag.UserId = userId;

                return View("PaymentConfirmation");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }


        public void SavePayment(string orderId, int userId, string propertyId, decimal amount)
        {
            try
            {
                var payment = new Models.Payment
                {
                    OrderId = orderId,
                    UserId = userId,
                    PropertyId = propertyId,
                    Amount = amount,
                    PaymentDate = DateTime.Now
                };

                _context.payments.Add(payment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while saving payment: " + ex.Message, ex);
            }
        }




        public IActionResult VerifyPayment(string razorpayPaymentId, string orderId, string razorpaySignature)
        {
            try
            {
                UpdatePaymentStatus(razorpayPaymentId, orderId);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Payment update failed: " + ex.Message;
                return View("Error");
            }
        }

        private void UpdatePaymentStatus(string paymentId, string orderId)
        {
            var payment = _context.payments.FirstOrDefault(p => p.OrderId == orderId);

            if (payment != null)
            {
                payment.PaymentStatus = "Success";
                payment.RazorpayPaymentId = paymentId;
                payment.PaymentDate = DateTime.Now;

                _context.SaveChanges();
            }
        }
    }
}
