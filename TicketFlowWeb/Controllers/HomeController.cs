using Microsoft.AspNetCore.Mvc;

namespace TicketFlowWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
