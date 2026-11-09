using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAREA2_FELIXLORENZOARAYAMARIN.Datos
{
    /// <summary>
    /// Capa de Datos: operaciones SQL sobre la tabla Categorias.
    /// No muestra mensajes ni valida: solo habla con la base de datos.
    /// </summary>
    public class CategoriaDatos
    {
        // Consulta: devuelve todas las categorías
        public List<Categoria> ObtenerTodas()
        {
            List<Categoria> categorias = new List<Categoria>();
            string sql = "SELECT IdCategoria, Nombre, Descripcion FROM Categorias ORDER BY Nombre";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Categoria categoria = new Categoria();
                        categoria.IdCategoria = lector.GetInt32(0);
                        categoria.Nombre = lector.GetString(1);
                        categoria.Descripcion = lector.IsDBNull(2) ? string.Empty : lector.GetString(2);
                        categorias.Add(categoria);
                    }
                }
            }

            return categorias;
        }

        // Inserción: devuelve la cantidad de filas afectadas
        public int Insertar(Categoria categoria)
        {
            string sql = "INSERT INTO Categorias (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                comando.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }

        // Modificación: devuelve la cantidad de filas afectadas
        public int Modificar(Categoria categoria)
        {
            string sql = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion " +
                         "WHERE IdCategoria = @IdCategoria";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                comando.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                comando.Parameters.AddWithValue("@IdCategoria", categoria.IdCategoria);
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }

        // Eliminación: devuelve la cantidad de filas afectadas
        public int Eliminar(int idCategoria)
        {
            string sql = "DELETE FROM Categorias WHERE IdCategoria = @IdCategoria";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCategoria", idCategoria);
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }

        // Indica si ya existe otra categoría con el mismo nombre.
        // Al insertar se pasa idCategoria = 0; al modificar, el id de la categoría actual.
        public bool ExisteNombre(string nombre, int idCategoria)
        {
            string sql = "SELECT COUNT(*) FROM Categorias " +
                         "WHERE Nombre = @Nombre AND IdCategoria <> @IdCategoria";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@Nombre", nombre);
                comando.Parameters.AddWithValue("@IdCategoria", idCategoria);
                conexion.Open();
                return (int)comando.ExecuteScalar() > 0;
            }
        }

        // Indica si la categoría tiene productos asociados (por la llave foránea)
        public bool TieneProductos(int idCategoria)
        {
            string sql = "SELECT COUNT(*) FROM Productos WHERE IdCategoria = @IdCategoria";

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCategoria", idCategoria);
                conexion.Open();
                return (int)comando.ExecuteScalar() > 0;
            }
        }
    }
}
