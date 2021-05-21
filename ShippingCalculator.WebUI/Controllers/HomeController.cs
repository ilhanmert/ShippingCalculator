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
        public PartialViewResult PackageCalculate(int distance, List<Package> packages)
        {
            List<ShippingCompany> shippingCompanies = _shippingCompanyManager.GetShippingCompaniesJsonList();
            List<decimal> prices = new List<decimal>();
            int desi;
            int toplamdesi = 0;
            foreach (var item in packages)
            {
                desi = _cargoPriceManager.DesiCalculator(item);
                toplamdesi = toplamdesi + desi;
            }
            foreach (var item in shippingCompanies)
            {
                if (distance <= 1)
                {
                    prices.Add(item.Price = _cargoPriceManager.LocalPriceCalculator(toplamdesi, item.LocalFactor));
                }
                else if (distance <= 200 && distance > 1)
                {
                    prices.Add(item.Price = _cargoPriceManager.ClosePriceCalculator(toplamdesi, item.CloseFactor));
                }
                else if (distance <= 600 && distance > 200)
                {
                    prices.Add(item.Price = _cargoPriceManager.ShortPriceCalculator(toplamdesi, item.ShortFactor));
                }
                else if (distance <= 1000 && distance > 600)
                {
                    prices.Add(item.Price = _cargoPriceManager.MidlinePriceCalculator(toplamdesi, item.MiddleFactor));
                }
                else
                {
                    prices.Add(item.Price = _cargoPriceManager.LongPriceCalculator(toplamdesi, item.LongFactor));
                }
            }
            return PartialView("CargoCompaniesList", shippingCompanies);
        }
        [HttpPost]
        public PartialViewResult DocumentCalculate(int distance)
        {
            List<ShippingCompany> shippingCompanies = _shippingCompanyManager.GetShippingCompaniesJsonList();
            List<decimal> prices = new List<decimal>();
            int desi = 1;
            foreach (var item in shippingCompanies)
            {
                if (distance <= 1)
                {
                    prices.Add(item.Price = _cargoPriceManager.LocalPriceCalculator(desi, item.LocalFactor));
                }
                else if (distance <= 200 && distance > 1)
                {
                    prices.Add(item.Price = _cargoPriceManager.ClosePriceCalculator(desi, item.CloseFactor));
                }
                else if (distance <= 600 && distance > 200)
                {
                    prices.Add(item.Price = _cargoPriceManager.ShortPriceCalculator(desi, item.ShortFactor));
                }
                else if (distance <= 1000 && distance > 600)
                {
                    prices.Add(item.Price = _cargoPriceManager.MidlinePriceCalculator(desi, item.MiddleFactor));
                }
                else
                {
                    prices.Add(item.Price = _cargoPriceManager.LongPriceCalculator(desi, item.LongFactor));
                }
            }
            return PartialView("CargoCompaniesList", shippingCompanies);
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
            Package p = new Package();
            packages.Add(p);
            return PartialView("GenerateNewPackage", packages);
        }
    }
}
