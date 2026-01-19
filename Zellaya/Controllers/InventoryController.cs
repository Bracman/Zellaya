using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zellaya.Data;
using Zellaya.Models;

namespace Zellaya.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public InventoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Balances()
        {
            
            var balances = await _db.Components
                .Where(c => c.is_active_component == 1)
                .Select(c => new InventoryBalanceRow
                {
                    id_component = c.id_component,
                    name = c.name,
                    type_component = c.type_component,
                    part_number = c.part_number,

                    stock_qty = _db.StockMovements
                        .Where(m => m.id_component == c.id_component)
                        .Sum(m => m.movement_type == "IN" ? m.qty : -m.qty)
                })
                .OrderBy(x => x.type_component)
                .ThenBy(x => x.name)
                .ToListAsync();

            return View(balances);
        }

        [HttpGet]
        public async Task<IActionResult> Move()
        {
            var components = await _db.Components
                .Where(c => c.is_active_component == 1)
                .OrderBy(c => c.name)
                .ToListAsync();

            ViewBag.Components = components;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Move(int id_component, string movement_type, int qty, string comment)
        {
            if (qty <= 0)
                return Json(new { success = false, message = "Количество должно быть больше 0" });

            if (movement_type != "IN" && movement_type != "OUT")
                return Json(new { success = false, message = "Тип движения должен быть IN или OUT" });

            var comp = await _db.Components.FirstOrDefaultAsync(x => x.id_component == id_component);
            if (comp == null)
                return Json(new { success = false, message = "Компонент не найден" });
            
            var currentStock = await _db.StockMovements
                .Where(m => m.id_component == id_component)
                .SumAsync(m => m.movement_type == "IN" ? m.qty : -m.qty);

            if (movement_type == "OUT" && currentStock < qty)
                return Json(new { success = false, message = $"Недостаточно на складе. Остаток: {currentStock}" });

            var move = new StockMovement
            {
                id_component = id_component,
                movement_type = movement_type,
                qty = qty,
                comment = comment?.Trim(),
                created_at = DateTime.Now
            };

            _db.StockMovements.Add(move);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Движение добавлено" });
        }

        [HttpGet]
        public async Task<IActionResult> Movements()
        {
            var moves = await _db.StockMovements
                .Include(m => m.Component)
                .OrderByDescending(m => m.created_at)
                .Take(300)
                .ToListAsync();

            return View(moves);
        }
    }
}
