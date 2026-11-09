using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAREA2_FELIXLORENZOARAYAMARIN.Datos;

namespace TAREA2_FELIXLORENZOARAYAMARIN.Logica
{
    /// <summary>
    /// Capa de Lógica: validaciones y reglas del sistema para los productos.
    /// No usa controles ni MessageBox: devuelve mensajes para que la interfaz los muestre.
    /// </summary>
    public class ProductoLogica
    {
        private const decimal PrecioMaximo = 99999999.99m;

        private readonly ProductoDatos productoDatos = new ProductoDatos();

        // ----- Validaciones por campo (devuelven "" si el dato es válido) -----

        public string ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre es obligatorio.";
            }
            if (nombre.Trim().Length > 100)
            {
                return "El nombre no puede tener más de 100 caracteres.";
            }
            return string.Empty;
        }

        public string ValidarPrecio(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return "El precio es obligatorio.";
            }

            decimal precio;
            if (!decimal.TryParse(texto.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out precio))
            {
                return "El precio debe ser un número válido.";
            }

            return ValidarValorPrecio(precio);
        }

        public string ValidarStock(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return "El stock es obligatorio.";
            }

            int stock;
            if (!int.TryParse(texto.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out stock))
            {
                return "El stock debe ser un número entero.";
            }

            return ValidarValorStock(stock);
        }

        public string ValidarCategoria(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                return "Debe seleccionar una categoría.";
            }
            return string.Empty;
        }

        // ----- Creación del objeto Producto a partir de los textos del formulario -----

        // Se usa después de validar los campos: convierte los textos a sus tipos numéricos
        public Producto CrearProducto(int idProducto, string nombre, string precioTexto,
                                      string stockTexto, int idCategoria)
        {
            decimal precio;
            decimal.TryParse(precioTexto.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out precio);

            int stock;
            int.TryParse(stockTexto.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out stock);

            Producto producto = new Producto();
            producto.IdProducto = idProducto;
            producto.Nombre = nombre;
            producto.Precio = Math.Round(precio, 2);
            producto.Stock = stock;
            producto.IdCategoria = idCategoria;
            return producto;
        }

        // ----- Operaciones -----

        public List<Producto> ObtenerProductos(out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                return productoDatos.ObtenerTodos();
            }
            catch (SqlException)
            {
                mensaje = "No se pudieron cargar los productos. Verifique la conexión con la base de datos.";
                return new List<Producto>();
            }
        }

        public List<Producto> BuscarProductos(string texto, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(texto))
            {
                return ObtenerProductos(out mensaje);
            }

            try
            {
                List<Producto> productos = productoDatos.BuscarPorNombre(texto.Trim());
                if (productos.Count == 0)
                {
                    mensaje = "No se encontraron productos que coincidan con la búsqueda.";
                }
                return productos;
            }
            catch (SqlException)
            {
                mensaje = "No se pudo realizar la búsqueda. Verifique la conexión con la base de datos.";
                return new List<Producto>();
            }
        }

        public bool GuardarProducto(Producto producto, out string mensaje)
        {
            mensaje = ValidarProducto(producto);
            if (mensaje != string.Empty)
            {
                return false;
            }

            try
            {
                // La fecha de registro se asigna automáticamente al crear el producto
                producto.FechaRegistro = DateTime.Now;

                productoDatos.Insertar(producto);
                mensaje = "Producto guardado correctamente.";
                return true;
            }
            catch (SqlException)
            {
                mensaje = "No se pudo guardar el producto. Verifique la conexión con la base de datos.";
                return false;
            }
        }

        public bool ModificarProducto(Producto producto, out string mensaje)
        {
            if (producto.IdProducto <= 0)
            {
                mensaje = "Seleccione un producto de la lista para modificarlo.";
                return false;
            }

            mensaje = ValidarProducto(producto);
            if (mensaje != string.Empty)
            {
                return false;
            }

            try
            {
                int filas = productoDatos.Modificar(producto);
                if (filas == 0)
                {
                    mensaje = "No se encontró el producto que se intentó modificar.";
                    return false;
                }

                mensaje = "Producto modificado correctamente.";
                return true;
            }
            catch (SqlException)
            {
                mensaje = "No se pudo modificar el producto. Verifique la conexión con la base de datos.";
                return false;
            }
        }

        public bool EliminarProducto(int idProducto, out string mensaje)
        {
            if (idProducto <= 0)
            {
                mensaje = "Seleccione un producto de la lista para eliminarlo.";
                return false;
            }

            try
            {
                int filas = productoDatos.Eliminar(idProducto);
                if (filas == 0)
                {
                    mensaje = "No se encontró el producto que se intentó eliminar.";
                    return false;
                }

                mensaje = "Producto eliminado correctamente.";
                return true;
            }
            catch (SqlException)
            {
                mensaje = "No se pudo eliminar el producto. Verifique la conexión con la base de datos.";
                return false;
            }
        }

        // ----- Apoyo interno -----

        private string ValidarValorPrecio(decimal precio)
        {
            if (precio <= 0)
            {
                return "El precio debe ser mayor que cero.";
            }
            if (precio > PrecioMaximo)
            {
                return "El precio es demasiado grande.";
            }
            return string.Empty;
        }

        private string ValidarValorStock(int stock)
        {
            if (stock < 0)
            {
                return "El stock no puede ser negativo.";
            }
            return string.Empty;
        }

        // Valida el objeto completo antes de enviarlo a la base de datos
        private string ValidarProducto(Producto producto)
        {
            string error = ValidarNombre(producto.Nombre);
            if (error != string.Empty)
            {
                return error;
            }

            error = ValidarValorPrecio(producto.Precio);
            if (error != string.Empty)
            {
                return error;
            }

            error = ValidarValorStock(producto.Stock);
            if (error != string.Empty)
            {
                return error;
            }

            error = ValidarCategoria(producto.IdCategoria);
            if (error != string.Empty)
            {
                return error;
            }

            producto.Nombre = producto.Nombre.Trim();
            return string.Empty;
        }
    }
}
