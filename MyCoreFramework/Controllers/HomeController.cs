using MCF.Data.Orm;
using MCF.Model;
using Microsoft.AspNetCore.Mvc;
using MyCoreFramework.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MyCoreFramework.Controllers
{
    public class HomeController : Controller
    {
        IOrmDbEntity db=DbVisit.GetDbEntity();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            int id = 17616;
            var model = db.Entity<BBS_User>(" and Id=@id ", new SqlParameter("@id", id));
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