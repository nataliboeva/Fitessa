using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Fitessa.Data.Entities;
using Fitessa.Models;
using Fitessa.Services.Interfaces;
using Stripe;
using System.Security.Claims;

namespace Fitessa.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly IEmailService _emailService;

        public PaymentController(
            UserManager<ApplicationUser> userManager,
            ISubscriptionPlanService subscriptionPlanService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _subscriptionPlanService = subscriptionPlanService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var plans = _subscriptionPlanService.GetAll();
            
            ViewBag.User = user;
            return View(plans);
        }

        public async Task<IActionResult> Checkout(int planId)
        {
            var plan = _subscriptionPlanService.GetById(planId);
            if (plan == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            
            ViewBag.Plan = plan;
            ViewBag.User = user;
            ViewBag.StripePublishableKey = "pk_test_your_stripe_publishable_key";
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int planId, string stripeToken)
        {
            try
            {
                var plan = _subscriptionPlanService.GetById(planId);
                if (plan == null) return NotFound();

                var user = await _userManager.GetUserAsync(User);
                
                StripeConfiguration.ApiKey = "sk_test_your_stripe_secret_key";

                var options = new ChargeCreateOptions
                {
                    Amount = (long)(plan.Price * 100), // Convert to cents
                    Currency = "usd",
                    Source = stripeToken,
                    Description = $"Fitessa Premium Plan: {plan.Name}",
                    Metadata = new Dictionary<string, string>
                    {
                        { "user_id", user.Id },
                        { "plan_id", planId.ToString() },
                        { "user_email", user.Email }
                    }
                };

                var service = new ChargeService();
                var charge = service.Create(options);

                if (charge.Status == "succeeded")
                {
                    var userSubscription = new UserSubscription
                    {
                        UserId = user.Id,
                        SubscriptionPlanId = planId,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddDays(plan.DurationDays),
                        IsActive = true,
                        Status = "Active",
                        RenewalType = "Manual",
                        PaymentId = charge.Id,
                        User = user,
                        SubscriptionPlan = plan
                    };
                    
                    _subscriptionPlanService.CreateUserSubscription(userSubscription);
                    
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Payment Confirmation - Fitessa",
                        $"Thank you for subscribing to {plan.Name}! Your payment of ${plan.Price} has been processed successfully."
                    );

                    TempData["Success"] = "Payment processed successfully! Welcome to Fitessa Premium!";
                    return RedirectToAction("Success");
                }
                else
                {
                    TempData["Error"] = "Payment failed. Please try again.";
                    return RedirectToAction("Checkout", new { planId });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred during payment processing. Please try again.";
                return RedirectToAction("Checkout", new { planId });
            }
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePaymentIntent(int planId)
        {
            try
            {
                var plan = _subscriptionPlanService.GetById(planId);
                if (plan == null) return NotFound();

                StripeConfiguration.ApiKey = "sk_test_your_stripe_secret_key";

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(plan.Price * 100),
                    Currency = "usd",
                    Metadata = new Dictionary<string, string>
                    {
                        { "plan_id", planId.ToString() }
                    }
                };

                var service = new PaymentIntentService();
                var intent = service.Create(options);

                return Json(new { clientSecret = intent.ClientSecret });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 