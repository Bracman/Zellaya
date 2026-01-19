using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using Zellaya.Data;
using Zellaya.Models;

namespace Zellaya.Controllers
{
    public class DirectoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly FileStorageOptions _storage;

        public DirectoriesController(ApplicationDbContext db,IOptions<FileStorageOptions> storage)
        {
            _db = db;
            _storage = storage.Value;
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
        public async Task<IActionResult> ListArchiveBoards()
        {
            var boards = await _db.Boards
         .Where(b => b.is_active == 0)
         .ToListAsync();
            return View(boards);
        }

        public IActionResult BoardCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBoard(string nameBoard, string codeBoard, IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(nameBoard))
                return Json(new { success = false, message = "Введите название платы" });

            if (string.IsNullOrWhiteSpace(codeBoard))
                return Json(new { success = false, message = "Введите код платы" });

            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "Выберите файл PDF/DOCX" });

            
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
                code_board = codeBoard,
                id_document = doc.id_document,     
                is_active = 1            
            };

            _db.Boards.Add(board);
            await _db.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Плата успешно добавлена!",
                boardId = board.id_board
            });
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

        [HttpPost]
        public async Task<IActionResult> ArchiveBoard(int id)
        {
            var board = await _db.Boards.FirstOrDefaultAsync(b => b.id_board == id);
            if (board == null) return NotFound();

            board.is_active = 0;
            await _db.SaveChangesAsync();
            
            return RedirectToAction("ListBoarders");
        }
    }
}
