using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.Models;
using StockRequester.Utility;
using StockRequesterWeb.Areas.CompanyUser.Controllers;
using StockRequesterWeb.Areas.SiteAdmin.Controllers;
using System.Diagnostics;

namespace StockRequesterWeb.Areas.Home.Controllers
{
    [Area(nameof(Home))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(SD.Role_SiteAdmin))
            {
                return RedirectToAction(
                    nameof(AllCompaniesController.Index),
                    ControllerUtility.ControllerName(typeof(AllCompaniesController)),
                    new { area = nameof(SiteAdmin) }
                );
            }
            else if (User.IsInRole(SD.Role_CompanyAdmin) ||
                     User.IsInRole(SD.Role_CompanyUser))
            {
                return RedirectToAction(
                    nameof(CompanyController.Index), 
                    ControllerUtility.ControllerName(typeof(CompanyController)),
                    new { area = nameof(CompanyUser) }
                );
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Donate()
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
