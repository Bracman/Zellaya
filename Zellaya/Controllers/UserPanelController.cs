using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zellaya.Data;
using Zellaya.Models;

namespace Zellaya.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserPanelController(ApplicationDbContext db)
        {
            _db = db;
           
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task <ActionResult> UserOrders()
        {
            var orders = await _db.Board_Orders.ToListAsync();        
          
            return View(orders);
        }

        public IActionResult UserOrderDetails(int id)
        {
            var order = _db.Board_Orders.FirstOrDefault(o => o.id_order == id);

            if (order == null)
                return NotFound();

            var items = _db.Set<OrderItemDetails>()
                .FromSqlRaw("CALL show_order_details({0})", id)
                .ToList();

            var model = new OrderDetails
            {
                order_number = order.order_number,
                CreatedDate = order.date_order_creation,
                ReadyDate = order.date_ready_order,
                Items = items
            };

            return View("UserOrderDetails", model);
        }

        [HttpPost]
        public IActionResult ChangeStatus(string id, string status)
        {
            var order = _db.Board_Orders.FirstOrDefault(o => o.order_number == id);
            if (order == null)
                return RedirectToAction("UserOrders");

            order.status_order = status;
            _db.SaveChanges();

            return RedirectToAction("UserOrders");
        }
    }
}
