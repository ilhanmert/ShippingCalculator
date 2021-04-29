using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingCalculator.BusinessLogicLayer.Concrete;
using ShippingCalculator.Entities.Concrete;
using System.Collections.Generic;

namespace ShippingCalculator.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CargoPriceManager _cargoPriceManager;
        private readonly ShippingCompanyManager _shippingCompanyManager;
        public HomeController(ILogger<HomeController> logger, CargoPriceManager cargoPriceManager, ShippingCompanyManager shippingCompanyManager)
        {
            _cargoPriceManager = cargoPriceManager;
            _logger = logger;
            _shippingCompanyManager = shippingCompanyManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public decimal PackageCalculate(int distance, Package package)
        {
            int desi = _cargoPriceManager.DesiCalculator(package);
            decimal price;
            if (distance <= 1)
            {
                price = _cargoPriceManager.LocalPriceCalculator(desi);
            }
            else if (distance <= 200 && distance > 1)
            {
                price = _cargoPriceManager.ClosePriceCalculator(desi);
            }
            else if (distance <= 600 && distance > 200)
            {
                price = _cargoPriceManager.ShortPriceCalculator(desi);
            }
            else if (distance <= 1000 && distance > 600)
            {
                price = _cargoPriceManager.MidlinePriceCalculator(desi);
            }
            else
            {
                price = _cargoPriceManager.LongPriceCalculator(desi);
            }
            return price;
        }
        [HttpPost]
        public decimal DocumentCalculate(int distance)
        {
            int desi = 1;
            decimal price;
            if (distance <= 1)
            {
                price = _cargoPriceManager.LocalPriceCalculator(desi);
            }
            else if (distance > 1 && distance <= 200)
            {
                price = _cargoPriceManager.ClosePriceCalculator(desi);
            }
            else if (distance > 200 && distance <= 600)
            {
                price = _cargoPriceManager.ShortPriceCalculator(desi);
            }
            else if (distance > 600 && distance <= 1000)
            {
                price = _cargoPriceManager.MidlinePriceCalculator(desi);
            }
            else
            {
                price = _cargoPriceManager.LongPriceCalculator(desi);
            }
            return price;
        }
        public PartialViewResult CargoCompaniesList()
        {
            List<ShippingCompany> cargoCompanies = _shippingCompanyManager.GetShippingCompaniesJsonList();
            if (cargoCompanies != null && cargoCompanies.Count > 0)
            {
                return PartialView("CargoCompaniesList", cargoCompanies);
            }
            return PartialView("CargoCompaniesList", null);
        }
        [HttpPost]
        public PartialViewResult GenerateNewPackage(List<Package> packages)
        {
            return PartialView("GenerateNewPackage", packages);
        }
    }
}
