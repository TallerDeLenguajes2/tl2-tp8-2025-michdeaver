using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using tl2_tp8_2025_michdeaver.Models;

namespace tl2_tp8_2025_michdeaver.Interfaces
{
    public interface IProductoRepository
    {
        List<Producto> GetProductos();
        Producto GetProducto(int id);
        void CreateProducto(Producto newProducto);
        void UpdateProducto(int id, Producto newProducto);
        void DeleteProducto(int id);
    }
}