using Microsoft.AspNetCore.Mvc;

namespace TicketFlowWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UsuarioNome = HttpContext.Session.GetString("UsuarioNome");
            ViewBag.UsuarioRole = HttpContext.Session.GetString("UsuarioRole");

            return View();
        }
    }
}