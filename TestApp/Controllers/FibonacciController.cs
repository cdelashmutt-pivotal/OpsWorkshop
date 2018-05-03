using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace TestApp.Controllers
{
    public class FibonacciController : Controller
    {
        public static BigInteger Fibonacci(int n)
        {
            BigInteger a = 0;
            BigInteger b = 1;
            // In N steps compute Fibonacci sequence iteratively.
            for (int i = 0; i < n; i++)
            {
                BigInteger temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calculate(int quantity)
        {
            BigInteger calculated = 0;
            for(int i = 0; i <= quantity; i++)
            {
                calculated = Fibonacci(i);
            }
            ViewData["value"] = calculated;
            return View("Index");
        }
    }
}
