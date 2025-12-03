using Microsoft.AspNetCore.Mvc;

namespace Zellaya.Controllers
{
    public class KnowledgeBaseController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }
        

        public IActionResult BoardsCheck() => View(); // Проверка плат
        public IActionResult Components() => View();
    }
}
