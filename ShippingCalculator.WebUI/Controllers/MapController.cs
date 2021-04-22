using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ShippingCalculator.WebUI.Controllers
{
    public class MapController : Controller
    {
        private readonly ILogger<MapController> _logger;

        public MapController(ILogger<MapController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
