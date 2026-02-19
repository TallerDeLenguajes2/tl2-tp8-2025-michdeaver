using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using tl2_tp8_2025_michdeaver.Models;

namespace tl2_tp8_2025_michdeaver.Interfaces
{
    public interface IPresupuestoRepository
    {
        List<Presupuesto> GetPresupuestos();
        Presupuesto GetPresupuesto(int id);
        void CreatePresupuesto(Presupuesto newPresupuesto);
        void AddDetallePresupuesto(int idPresupuesto, int idProducto, int cantidad);
        void DeletePresupuesto(int id);
        void EditPresupuesto(int id, Presupuesto newPresupuesto);
    }
}