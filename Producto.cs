using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAREA2_FELIXLORENZOARAYAMARIN
{
    /// <summary>
    /// Representa un producto de la tienda.
    /// </summary>
    public class Producto
    {
        // Campos privados (encapsulamiento)
        private int idProducto;
        private string nombre;
        private decimal precio;
        private int stock;
        private DateTime fechaRegistro;
        private int idCategoria;
        private string nombreCategoria;

        // Propiedades con get y set
        public int IdProducto
        {
            get { return idProducto; }
            set { idProducto = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public decimal Precio
        {
            get { return precio; }
            set { precio = value; }
        }

        public int Stock
        {
            get { return stock; }
            set { stock = value; }
        }

        public DateTime FechaRegistro
        {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }

        public int IdCategoria
        {
            get { return idCategoria; }
            set { idCategoria = value; }
        }

        // Solo para mostrar en el DataGridView (viene del JOIN con Categorias)
        public string NombreCategoria
        {
            get { return nombreCategoria; }
            set { nombreCategoria = value; }
        }

        // Propiedad calculada: NO se guarda en la base de datos (RF-06)
        public decimal ValorInventario
        {
            get { return precio * stock; }
        }

        // Constructor vacío
        public Producto()
        {
            nombre = string.Empty;
            nombreCategoria = string.Empty;
        }

        // Constructor con parámetros
        public Producto(int idProducto, string nombre, decimal precio, int stock,
                        DateTime fechaRegistro, int idCategoria)
        {
            this.idProducto = idProducto;
            this.nombre = nombre;
            this.precio = precio;
            this.stock = stock;
            this.fechaRegistro = fechaRegistro;
            this.idCategoria = idCategoria;
            this.nombreCategoria = string.Empty;
        }
    }
}
