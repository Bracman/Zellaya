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
            Console.WriteLine($"Попытка входа: Email = '{login}', Password = '{password}'");
            var user = _context.Users
                .FirstOrDefault(u => u.login == login && u.password == password);

            if (user != null)
            {
                Console.WriteLine("Пользователь найден!");
                return RedirectToAction("Index", "Home");
            }

            Console.WriteLine("Пользователь не найден.");
            ViewBag.ErrorMessage = "Неверный email или пароль";
            return View();
        }
    }
}
