using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Zellaya.Data;
using Zellaya.Models;


namespace Zellaya.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly FileStorageOptions _storage;

        public OrdersController(ApplicationDbContext db, IOptions<FileStorageOptions> storage)
        {
            _db = db;
            _storage = storage.Value;
        }       
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
        public async Task<IActionResult> AddNewBoard (string nameBoard,string codeBoard, IFormFile file)
        {
            if(string.IsNullOrWhiteSpace(nameBoard))
                 return Json(new { success = false, message = "Введите название платы" });

            if (string.IsNullOrWhiteSpace(codeBoard))
                return Json(new { success = false, message = "Введите код платы" });

            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "Выберите PDF файл" });



            var boardsPath = Path.Combine(_storage.BasePath, "boards");
            Directory.CreateDirectory(boardsPath);

           
            var ext = Path.GetExtension(file.FileName); 
            var safeFileName = $"{codeBoard}_{DateTime.Now:yyyyMMdd_HHmmss}{ext}";
            var fullPath = Path.Combine(boardsPath, safeFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var doc = new Documents
            {
                file_name = Path.GetFileNameWithoutExtension(file.FileName),
                file_ext = ext,
                file_path = fullPath
            };

            _db.Documents.Add(doc);
            await _db.SaveChangesAsync();

            var board = new Board
            {
                name_board = nameBoard,
                code_board = codeBoard
            };

            _db.Boards.Add(board);
            await _db.SaveChangesAsync();


            return Json(new { success = true, message = "Плата успешно добавлена!" });

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
                            num_order = reader.GetString(1),
                            CreatedDate = reader.GetDateTime(2),
                            ReadyDate = reader.GetDateTime(3),
                            count_board = reader.GetInt32(4)
                        });
                    }
                }
            }

            return View(orders);
        }



        public IActionResult Details(int id)
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

            return View("OrderDetails", model);
        }


        [HttpPost]
        public async Task<IActionResult> UploadPdf(int orderId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран");

            var orderPath = Path.Combine(
                _storage.BasePath,
                "orders",
                orderId.ToString()
            );

            Directory.CreateDirectory(orderPath);

            var fileNameOnly = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExt = Path.GetExtension(file.FileName); 
            var fileName = Path.GetFileName(file.FileName);

            var fullPath = Path.Combine(orderPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            _db.Documents.Add(new Documents
            {
               
                file_name = fileNameOnly,
                file_ext = fileExt,
                file_path = fullPath
            });


            await _db.SaveChangesAsync();

            return RedirectToAction("Details", new { id = orderId });
        }

        public IActionResult ChangeStatus(string id, string status)
        {
            var order = _db.Board_Orders.FirstOrDefault(o => o.order_number == id);
            if (order == null)
                return RedirectToAction("Details");

            order.status_order = status;
            _db.SaveChanges();

            return RedirectToAction("Details");
        }
    }
}
