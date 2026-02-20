using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;
using MVC.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace MVC.Controllers
{
    public class PresupuestosController : Controller
    {
        private IPresupuestoRepository _presupuestoRepo;
        private IProductoRepository _productoRepo;
        private IAuthenticationServices _authService;
        private ILogger<PresupuestosController> _logger;

        public PresupuestosController(IPresupuestoRepository presupuestoRepo, IProductoRepository productoRepo, IAuthenticationServices auth, ILogger<PresupuestosController> logger)
        {
            _presupuestoRepo = presupuestoRepo;
            _productoRepo = productoRepo;
            _authService = auth;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Index", "Login");

            if (_authService.HasAccessLevel("Administrador") || _authService.HasAccessLevel("Cliente"))
            {
                var presupuestos = _presupuestoRepo.GetPresupuestos();
                return View(presupuestos);
            }


            return RedirectToAction("Index", "Login");
        }

        public IActionResult Create()
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            return View(new PresupuestoViewModel());
        }

        [HttpPost]
        public IActionResult Create(PresupuestoViewModel model)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                if (model.FechaCreacion > DateTime.Today)
                    return View(model);

                var presupuesto = new Presupuesto
                {
                    NombreDestinatario = model.NombreDestinatario,
                    FechaCreacion = model.FechaCreacion
                };

                _presupuestoRepo.CreatePresupuesto(presupuesto);

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el producto");
                return View("Error");
            }

        }

        public IActionResult GetPresupuesto(int IdPresupuesto)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Index", "Login");

            if (!_authService.HasAccessLevel("Administrador") && !_authService.HasAccessLevel("Cliente"))
                return RedirectToAction(nameof(AccesoDenegado));

            try
            {
                var model = _presupuestoRepo.GetPresupuesto(IdPresupuesto);
                if (model is null)
                    return NotFound();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el presupuesto");
                return View("Error");
            }
        }

        public IActionResult Edit(int IdPresupuesto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            var presupuesto = _presupuestoRepo.GetPresupuesto(IdPresupuesto);
            if (presupuesto is null)
                return NotFound();

            var model = new PresupuestoViewModel
            {
                NombreDestinatario = presupuesto.NombreDestinatario,
                FechaCreacion = presupuesto.FechaCreacion
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Presupuesto model)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;


            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                _presupuestoRepo.EditPresupuesto(model.IdPresupuesto, model);

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Editar el objeto");
                return View("Error");
            }
        }

        public IActionResult Delete(int IdPresupuesto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {
                var presupuesto = _presupuestoRepo.GetPresupuesto(IdPresupuesto);
                if (presupuesto is null)
                    return NotFound();

                var model = new PresupuestoViewModel
                {
                    NombreDestinatario = presupuesto.NombreDestinatario,
                    FechaCreacion = presupuesto.FechaCreacion
                };

                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar el presupuesto");
                return View("Error");
            }

        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int IdPresupuesto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {
                _presupuestoRepo.DeletePresupuesto(IdPresupuesto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar el presupuesto");
                return View("Error");
            }
        }

        public IActionResult AgregarProducto(int IdPresupuesto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {
                var productos = _productoRepo.GetProductos();

                var model = new AgregarProductoViewModel
                {
                    IdPresupuesto = IdPresupuesto,
                    ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un producto");
                return View("Error");
            }

        }

        [HttpPost]
        public IActionResult AgregarProducto(AgregarProductoViewModel model)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {
                if (!ModelState.IsValid)
                {
                    var productos = _productoRepo.GetProductos();
                    model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
                    return View(model);
                }
                _presupuestoRepo.AddDetallePresupuesto(model.IdPresupuesto, model.IdProducto, model.Cantidad);

                return RedirectToAction(nameof(GetPresupuesto), new { IdPresupuesto = model.IdPresupuesto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el producto");
                return View("Error");
            }

        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }
        private IActionResult CheckAdminPermissions()
        {
            if (!_authService.IsAuthenticated())
            {
                _logger.LogWarning("Acceso sin autenticar");
                return RedirectToAction("Index", "Login");
            }

            if (!_authService.HasAccessLevel("Administrador"))
            {
                _logger.LogWarning("Acceso denegado. Usuario sin rol Administrador");
                return RedirectToAction(nameof(AccesoDenegado));
            }

            return null;
        }
    }
}