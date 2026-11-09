using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAREA2_FELIXLORENZOARAYAMARIN.Datos
{
    /// <summary>
    /// Capa de Datos: operaciones SQL sobre la tabla Productos.
    /// No muestra mensajes ni valida: solo habla con la base de datos.
    /// </summary>
    public class ProductoDatos
    {
        // Consulta base: une Productos con Categorias para obtener el nombre de la categoría
        private const string ConsultaBase =
            "SELECT p.IdProducto, p.Nombre, p.Precio, p.Stock, p.FechaRegistro, " +
            "p.IdCategoria, c.Nombre AS NombreCategoria " +
            "FROM Productos p INNER JOIN Categorias c ON c.IdCategoria = p.IdCategoria ";

        // Consulta: devuelve todos los productos
        public List<Producto> ObtenerTodos()
        {
            string sql = ConsultaBase + "ORDER BY p.Nombre";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                return LeerProductos(comando);
            }
        }

        // Búsqueda: productos cuyo nombre contiene el texto indicado
        public List<Producto> BuscarPorNombre(string texto)
        {
            string sql = ConsultaBase + "WHERE p.Nombre LIKE @Texto ORDER BY p.Nombre";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@Texto", "%" + texto + "%");
                return LeerProductos(comando);
            }
        }

        // Inserción: devuelve la cantidad de filas afectadas
        public int Insertar(Producto producto)
        {
            string sql = "INSERT INTO Productos (Nombre, Precio, Stock, FechaRegistro, IdCategoria) " +
                         "VALUES (@Nombre, @Precio, @Stock, @FechaRegistro, @IdCategoria)";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                AgregarParametrosProducto(comando, producto);
                comando.Parameters.AddWithValue("@FechaRegistro", producto.FechaRegistro);
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }

        // Modificación: no cambia la fecha de registro. Devuelve las filas afectadas
        public int Modificar(Producto producto)
        {
            string sql = "UPDATE Productos SET Nombre = @Nombre, Precio = @Precio, Stock = @Stock, " +
                         "IdCategoria = @IdCategoria WHERE IdProducto = @IdProducto";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                AgregarParametrosProducto(comando, producto);
                comando.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }

        // Eliminación: devuelve la cantidad de filas afectadas
        public int Eliminar(int idProducto)
        {
            string sql = "DELETE FROM Productos WHERE IdProducto = @IdProducto";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@IdProducto", idProducto);
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }

        // ----- Métodos privados de apoyo (evitan código duplicado) -----

        private List<Producto> LeerProductos(SqlCommand comando)
        {
            List<Producto> productos = new List<Producto>();

            comando.Connection.Open();
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    productos.Add(ConvertirAProducto(lector));
                }
            }

            return productos;
        }

        private Producto ConvertirAProducto(SqlDataReader lector)
        {
            Producto producto = new Producto();
            producto.IdProducto = lector.GetInt32(0);
            producto.Nombre = lector.GetString(1);
            producto.Precio = lector.GetDecimal(2);
            producto.Stock = lector.GetInt32(3);
            producto.FechaRegistro = lector.GetDateTime(4);
            producto.IdCategoria = lector.GetInt32(5);
            producto.NombreCategoria = lector.GetString(6);
            return producto;
        }

        // Parámetros comunes de Insertar y Modificar
        private void AgregarParametrosProducto(SqlCommand comando, Producto producto)
        {
            comando.Parameters.AddWithValue("@Nombre", producto.Nombre);

            // El precio se declara como DECIMAL(10,2), igual que en la tabla
            SqlParameter parametroPrecio = comando.Parameters.Add("@Precio", SqlDbType.Decimal);
            parametroPrecio.Precision = 10;
            parametroPrecio.Scale = 2;
            parametroPrecio.Value = producto.Precio;

            comando.Parameters.AddWithValue("@Stock", producto.Stock);
            comando.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
        }
    }
}
