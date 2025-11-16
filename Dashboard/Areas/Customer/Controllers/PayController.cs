using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Dashboard.Areas.Customer.Controllers
{
    public class PayController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Cart> _cartRepository;
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Pay()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null) return NotFound();

            //var cart = await _cartRepository.GetAsync(e => e.ApplicationUserId == user.Id, includes: [e => e.Product]);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/identity/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/identity/checkout/cancel",
            };

            //foreach (var item in cart)
            //{
            //    options.LineItems.Add(new SessionLineItemOptions
            //    {
            //        PriceData = new SessionLineItemPriceDataOptions
            //        {
            //            Currency = "egp",
            //            ProductData = new SessionLineItemPriceDataProductDataOptions
            //            {
            //                Name = item.Product.Name,
            //                Description = item.Product.Description,
            //            },
            //            UnitAmount = (long)item.Price * 100,
            //        },
            //        Quantity = item.Count,
            //    });
            //}

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}
