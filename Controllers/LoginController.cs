using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_michdeaver.Models;
using tl2_tp8_2025_michdeaver.Repositories;
using tl2_tp8_2025_michdeaver.ViewModels;
using tl2_tp8_2025_michdeaver.Interfaces;

namespace tl2_tp8_2025_michdeaver.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IAuthenticationServices auth, ILogger<LoginController> logger)
        {
            _authenticationServices = auth;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (_authenticationServices.Login(model.Username, model.Password))
            {
                _logger.LogInformation("Login exitoso para el usuario {Username}", model.Username);

                return RedirectToAction("Index", "Home");
            }
        
            _logger.LogWarning("Intento fallido para el usuario {Username}", model.Username);
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos");
            return View("Index", model);
        }

        public IActionResult Logout()
        {
            _authenticationServices.Logout();
            return RedirectToAction("Index");
        }
    }
}