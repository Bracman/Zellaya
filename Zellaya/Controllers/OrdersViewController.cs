using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data.SqlClient;
using Zellaya.Models;

namespace Zellaya.Controllers
{
    public class OrdersViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly string _connectionString = "Server=localhost;Database=db_zellaya;User ID=admin;Password=Metall50;";


        [HttpGet]
        public IActionResult ListOrders()
        {
            var orders = new List<Orders>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM db_zellaya.board_orders;", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Orders
                        {
                            id_order = reader.GetInt32(0),
                            num_order = reader.GetString(2),
                            CreatedDate = reader.GetDateTime(3),
                            ReadyDate = reader.GetDateTime(4),
                            count_board = reader.GetInt32(5)
                        });
                    }
                }
            }

            return View(orders);
        }

    }
}
