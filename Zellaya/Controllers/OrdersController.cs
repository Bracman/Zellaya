using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Zellaya.Data;
using Zellaya.Models;

namespace Zellaya.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrdersController(ApplicationDbContext db) => _db = db;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var todayShort = DateTime.Today.ToString("dd-MM");
            var countToday = await _db.Board_Orders.CountAsync(b => b.date_order_creation.Date == DateTime.Today);

            var nextNum = $"{todayShort}-{countToday + 1}";

            var vm = new OrderCreateViewModel
            {
                Boards = await _db.Boards
                .OrderBy(b => b.name_board)
                .Select(b => new SelectListItem { Value = b.id_board.ToString(), Text = b.name_board })
                .ToListAsync(),

                Components = await _db.Components
                .OrderBy(c => c.name)  
                .Select(c => new SelectListItem
                {
                Value = c.id_component.ToString(),  
                Text = c.name            
                })
                .ToListAsync(),

                ComponentLines = new List<ComponentLine> { new ComponentLine() },
                OrderNumber=nextNum, 
                CreatedDate = DateTime.Today,
                ReadyDate = null,                
            };
            return View(vm); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            model.Boards = await _db.Boards
             .OrderBy(b => b.name_board)
             .Select(b => new SelectListItem
             {
                 Value = b.id_board.ToString(),
                 Text = b.name_board
             })
             .ToListAsync();

            model.Components = await _db.Components
                .OrderBy(c => c.name)
                .Select(c => new SelectListItem
                {
                    Value = c.id_component.ToString(),
                    Text = c.name
                })
                .ToListAsync();

            if (!ModelState.IsValid)
                return View(model);

            var order = new Order
            {
                OrderNumber = model.OrderNumber,
                CreatedDate = model.CreatedDate,
                ReadyDate = model.ReadyDate,
                BoardQuantity = model.BoardQuantity
            };


            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Сохраняем компоненты
            if (model.ComponentLines != null)
            {
                foreach (var component in model.ComponentLines)
                {
                    if (component.ComponentId.HasValue && component.Quantity > 0)
                    {
                        var orderComponent = new OrderComponent
                        {
                            OrderId = order.Id,
                            ComponentId = component.ComponentId.Value,
                            Quantity = component.Quantity
                        };
                        _db.OrderComponents.Add(orderComponent);
                    }
                }

                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBoard (string nameBoard,string codeBoard)
        {
            if(ModelState.IsValid)
            {
                var board = new Board
                {
                    name_board = nameBoard,
                    code_board = codeBoard
                };

                _db.Boards.Add(board);
                await _db.SaveChangesAsync();
                return Json(new { succes = true });

            }
            return Json(new { succes = false, message = "Пожалуйста, проверьте введенные данные." }); 
            
        }

       
        private readonly string _connectionString = "Server=localhost;Database=db_zellaya;User ID=admin;Password=Metall50;";


        [HttpGet]
        public IActionResult ListOrders()
        {
            var orders = new List<Orders>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM db_zellaya.board_orders;", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Orders
                        {
                            id_order = reader.GetInt32(0),
                            num_order = reader.GetString(2),
                            CreatedDate = reader.GetDateTime(3),
                            ReadyDate = reader.GetDateTime(4),
                            count_board = reader.GetInt32(5)
                        });
                    }
                }
            }

            return View(orders);
        }
    }
}
