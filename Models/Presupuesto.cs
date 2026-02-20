using System.Linq;
namespace tl2_tp8_2025_michdeaver.Models
{
    public class Presupuesto
    {
        private int idPresupuesto;
        private string nombreDestinatario;
        private DateTime fechaCreacion;
        private List<PresupuestoDetalle> detalles;

        public Presupuesto(){}

        public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion, List<PresupuestoDetalle> detalles)
        {
            this.idPresupuesto = idPresupuesto;
            this.nombreDestinatario = nombreDestinatario;
            this.fechaCreacion = fechaCreacion;
            this.detalles = detalles;
        }

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public List<PresupuestoDetalle> Detalles { get => detalles; set => detalles = value; }


        //metodos

        public decimal MontoPresupuesto()
        {
            /*
            decimal total = 0;
            foreach (var p in Detalles)
            {
                total += p.Cantidad * p.Producto.Precio;
            }
            */
            var total = detalles.Sum(d => d.Producto.Precio * d.Cantidad);
            return total;
        }

        public decimal MontoPresupuestoConIva()
        {
            var total = detalles.Sum(d => d.Producto.Precio * d.Cantidad * 1.21);
            return (decimal)total;
        }

        public int CantidadProductos()
        {
            return detalles?.Count ?? 0;
            /*
            int total = 0;
            foreach (var d in Detalles)
            {
                total += d.Cantidad;
            }

            return total;
            */
        }
    }
}
