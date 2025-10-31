public class PresupuestoDetalle
{
    private Productos producto;
    private int cantidad;

    public PresupuestoDetalle() { }
    public int Cantidad { get => cantidad; set => cantidad = value; }
    public Productos Producto { get => producto; set => producto = value; }
}