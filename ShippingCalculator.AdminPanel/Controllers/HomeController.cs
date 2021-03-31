using Microsoft.AspNetCore.Mvc;
using ShippingCalculator.BusinessLogicLayer.Concrete;
using ShippingCalculator.Entities.Concrete;

namespace ShippingCalculator.AdminPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdminManager _manager;
        public HomeController(AdminManager adminManager)
        {
            _manager = adminManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Admin data)
        {
            if (_manager.CheckAdmins(data))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(data);
            }
        }
    }
}
