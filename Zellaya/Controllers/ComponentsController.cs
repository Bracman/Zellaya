using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;
using Zellaya.Data;
using Zellaya.Models;
using Microsoft.EntityFrameworkCore;

namespace Zellaya.Controllers
{
    public class ComponentsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ComponentsController(ApplicationDbContext db)
        {
            _db = db;            
        }


        public async Task<IActionResult> ListComponents(string type = "all")
        {
            
            var types = await _db.Components
                .Where(c => c.is_active_component == 1 && c.type_component != null && c.type_component != "")
                .Select(c => c.type_component)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            var query = _db.Components.Where(c => c.is_active_component == 1);

            if (!string.IsNullOrEmpty(type) && type != "all")
                query = query.Where(c => c.type_component == type);

            var components = await query
                .OrderBy(c => c.type_component)
                .ThenBy(c => c.name)
                .ToListAsync();

            ViewBag.Types = types;
            ViewBag.SelectedType = type;

            return View(components);
        }


        [HttpGet]
        public IActionResult ComponentCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ComponentCreate(string name_component, string type_component, string serial_number, string params_text)
        {
            name_component = name_component?.Trim();
            type_component = type_component?.Trim()?.ToLower();
            serial_number = serial_number?.Trim();
            params_text = params_text?.Trim();

            if (string.IsNullOrWhiteSpace(name_component))
                ModelState.AddModelError("", "Введите название компонента");

            if (string.IsNullOrWhiteSpace(type_component))
                ModelState.AddModelError("", "Введите тип компонента");          
                      
            if (!ModelState.IsValid)
                return View();

           

            var component = new Component
            {
                name = name_component,
                type_component = type_component,
                part_number = serial_number,
                param_text = params_text,
                is_active_component=1
            };
            _db.Components.Add(component);
            await _db.SaveChangesAsync();

            return View();
        }
    
    }
}
