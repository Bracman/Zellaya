using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Zellaya.Data;
using Zellaya.Models;

namespace Zellaya.Controllers
{
    public class DirectoriesController : Controller
    {
        private readonly ApplicationDbContext _db;


        public DirectoriesController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListBoarders()
        {
            var boards = await _db.Boards
         .Where(b => b.is_active == 1)
         .ToListAsync();
            return View(boards);
        }

        public IActionResult OpenDoc(int id)
        {
            try
            {
                var doc = _db.Set<Documents>()
               .FromSqlRaw("CALL get_doc({0})", id)
               .AsEnumerable()
               .FirstOrDefault();
                if (doc == null)
                    return NotFound("Документ для платы не найден");

                if (!System.IO.File.Exists(doc.file_path))
                    return NotFound($"Файл не найден: {doc.file_path}");

                return PhysicalFile(doc.file_path, "application/pdf", doc.file_name + doc.file_ext); ;
            }
            catch (Exception ex)
            {
                return Content("Ошибка: " + ex.Message);

            }
        }
    }
}
