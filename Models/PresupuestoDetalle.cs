namespace tl2_tp8_2025_michdeaver.Models
{
    public class PresupuestoDetalle
    {
        private Producto producto;
        private int cantidad;

        public PresupuestoDetalle() { }

        public PresupuestoDetalle(Producto producto, int cantidad)
        {
            this.producto = producto;
            this.cantidad = cantidad;
        }

        public Producto Producto { get => producto; set => producto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
    }
}
