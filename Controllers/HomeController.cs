using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StaticFileServer.Models;

namespace StaticFileServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions() { });

        public HomeController(ILogger<HomeController> logger)
        {
            //1.最简单使用方式
            memoryCache.Set("mykey", "myvalue", TimeSpan.FromSeconds(3));

            //2.绝对过期时间，3秒后过期
            memoryCache.Set("key1", "value1", TimeSpan.FromSeconds(3));

            _logger = logger;
        }

        public IActionResult Index()
        {
            //1.最简单使用方式
            var ss = memoryCache.Get<string>("mykey");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
