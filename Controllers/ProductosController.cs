using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_michdeaver.Models;
using tl2_tp8_2025_michdeaver.Repositories;
using tl2_tp8_2025_michdeaver.ViewModels;
using tl2_tp8_2025_michdeaver.Interfaces;
using SQLitePCL;

namespace tl2_tp8_2025_michdeaver.Controllers
{
    public class ProductosController : Controller
    {
        private IProductoRepository _productoRepo;
        private IAuthenticationServices _authService;
        private ILogger<ProductosController> _logger;

        public ProductosController(IProductoRepository productoRepo, IAuthenticationServices auth, ILogger<ProductosController> logger)
        {
            _productoRepo = productoRepo;
            _authService = auth;
            _logger = logger;
        }


        public IActionResult Index()
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            var productos = _productoRepo.GetProductos();
            return View(productos);
        }


        public IActionResult Create()
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            return View(new ProductoViewModel());
        }


        [HttpPost]
        public IActionResult Create(ProductoViewModel model)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {

                if (!ModelState.IsValid)
                    return View(model);

                var producto = new Producto
                {
                    Descripcion = model.Descripcion,
                    Precio = model.Precio
                };

                _productoRepo.CreateProducto(producto);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear Producto");
                return View("Error");
            }

        }


        public IActionResult Edit(int IdProducto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {
                var producto = _productoRepo.GetProducto(IdProducto);
                if (producto is null) //error normal, no es una excepcion
                    return NotFound();

                var model = new ProductoViewModel
                {
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio
                };


                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el producto");
                return View("Error");
            }
        }


        [HttpPost]
        public IActionResult Edit(ProductoViewModel model, int IdProducto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var producto = new Producto
                {
                    Descripcion = model.Descripcion,
                    Precio = model.Precio
                };

                _productoRepo.UpdateProducto(IdProducto, producto);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el producto");
                return View("Error");
            }

        }


        public IActionResult Delete(int IdProducto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {
                var model = _productoRepo.GetProducto(IdProducto);
                if (model is null) //error normal
                    return NotFound();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Borrar el producto");
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int IdProducto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            try
            {
                _productoRepo.DeleteProducto(IdProducto);
                 return RedirectToAction("Index");
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error al borrar el producto");
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
                return RedirectToAction("Index", "Login");

            if (!_authService.HasAccessLevel("Administrador"))
                return RedirectToAction(nameof(AccesoDenegado));

            return null;
        }
    }
}