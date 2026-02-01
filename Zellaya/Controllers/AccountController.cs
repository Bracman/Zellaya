using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zellaya.Data;
using Zellaya.Models;

namespace Zellaya.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var user = _context.Users
         .Where(u => u.login == login
                     && u.password_hash == password
                     && u.is_active_user == 1)
         .Join(_context.Roles,
               u => u.id_role,
               r => r.id_role,
               (u, r) => new
               {
                   u.id_user,
                   u.login,
                   RoleName = r.role_name
               })
         .FirstOrDefault();

            if (user != null)
            {
               
                HttpContext.Session.SetString("UserLogin", user.login);
                HttpContext.Session.SetString("UserRole", user.RoleName);

               
                if (user.RoleName.ToLower() == "admin")
                    return RedirectToAction("Index", "Orders");   

                if (user.RoleName.ToLower() == "user")
                    return RedirectToAction("Index", "UserPanel");     
            }

            ViewBag.ErrorMessage = "Неверный логин или пароль";
            return View();
        }
    }
}
