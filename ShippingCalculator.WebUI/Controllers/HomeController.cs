using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingCalculator.BusinessLogicLayer.Concrete;
using ShippingCalculator.Entities.Concrete;
using System;

namespace ShippingCalculator.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CargoPriceManager _cargoPriceManager;
        public HomeController(ILogger<HomeController> logger, CargoPriceManager cargoPriceManager)
        {
            _cargoPriceManager = cargoPriceManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Main()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Desi(Package package)
        {
            int desi = _cargoPriceManager.DesiCalculator(package);
            return View(desi);
        }
    }
}
