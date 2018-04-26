using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TestApp.Controllers
{
    public class SqlServerController : Controller
    {
        public IActionResult Index([FromServices] TestContext context)
        {
            var connection = context.Database.GetDbConnection();
            ViewData["sqlEnabled"] = (connection.Database != null && connection.Database != "");
            if ((bool)ViewData["sqlEnabled"])
            {
                Console.WriteLine($"Retrieving data from {connection.DataSource}/{connection.Database}");
                return View(context.TestData.ToList());
            } else
            {
                return View();
            }
        }
    }
}
