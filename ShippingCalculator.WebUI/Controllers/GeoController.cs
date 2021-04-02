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
            List<Continent> continents = _manager.GetContinentsJsonList();
            ViewBag.ListOfContinent = continents;
            return View();
        }
        public PartialViewResult GetCountriesDD(int id)
        {
            List<Country> countries = _manager.GetCountriesByContinentIdJson(id);
            return PartialView("GetCountriesDD",countries);
        }
        public PartialViewResult GetCitiesDD(string code)
        {
            List<City> cities = _manager.GetCitiesByCountryCodeJson(code);
            return PartialView("GetCitiesDD", cities);
        }
        public PartialViewResult GetCountiesDD(int id)
        {
            List<County> counties = _manager.GetCountiesByCityIdJson(id);
            return PartialView("GetCountiesDD", counties);
        }
    }
}
