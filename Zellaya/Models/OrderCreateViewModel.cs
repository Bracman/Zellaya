using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class OrderCreateViewModel
    {
        [Display(Name = "Номер заказа")]
        [Required]
        public string OrderNumber { get; set; }

        [Display(Name = "Плата")]
        public int? BoardId { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Дата готовности")]
        [DataType(DataType.Date)]
        public DateTime? ReadyDate { get; set; }

        [Display(Name = "Количество плат")]
        public int BoardQuantity { get; set; } = 1;
        [Display(Name = "Количество компонентов")]
        public int ComponentQuantity { get; set; } = 1;

        public IEnumerable<SelectListItem> Boards { get; set; } = Array.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Components { get; set; } = Array.Empty<SelectListItem>();

        // Списки для выпадающих списков      
        public List<int?> SelectedBoards { get; set; } = new List<int?>(); // Список выбранных плат
        public List<ComponentLine> ComponentLines { get; set; } = new List<ComponentLine>(); // Список компонентов
    }

    public class ComponentLine
    {
        [Display(Name = "Радиокомпонент")]
        public int? ComponentId { get; set; }

        [Display(Name = "Количество")]
        public int Quantity { get; set; } = 1;
    }

}

