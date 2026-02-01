using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zellaya.Data;
using Zellaya.Models;

namespace Zellaya.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SettingsController(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateUser()
        {
            return View();
        }
        public async Task<IActionResult> ListUsers()
        {
            var users = await _db.Users
            .Join(_db.Roles,
              u => u.id_role,
              r => r.id_role,
              (u, r) => new ListUserItem
              {
                  id_user = u.id_user,
                  login = u.login,
                  role_name = r.role_name,
                  is_active_user = u.is_active_user == 1
              })
              .ToListAsync();

            return View(users);
        }

        [HttpPost]
        public IActionResult ToggleUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null) return NotFound();

            user.is_active_user = user.is_active_user == 1 ? 0 : 1;
            _db.SaveChanges();

            return RedirectToAction("ListUsers");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string login, string password, int role_id)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return View();

            var user = new User
            {
                login = login,
                password_hash = password,
                id_role = role_id,
                is_active_user = 1
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
