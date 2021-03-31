using Microsoft.AspNetCore.Mvc;
using ShippingCalculator.BusinessLogicLayer.Concrete;
using ShippingCalculator.Entities.Concrete;
using System.Collections.Generic;

namespace ShippingCalculator.WebUI.Controllers
{
    public class GeoController : Controller
    {
        private readonly GeoManager _manager;
        public GeoController(GeoManager geoManager)
        {
            _manager = geoManager;
        }
        public IActionResult Index()
        {
            List<Continent> continents = _manager.GetContinents();
            ViewBag.ListOfContinent = continents;
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
