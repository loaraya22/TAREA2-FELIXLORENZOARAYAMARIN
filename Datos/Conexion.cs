using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAREA2_FELIXLORENZOARAYAMARIN.Datos
{
    /// <summary>
    /// Capa de Datos: centraliza la conexión a SQL Server.
    /// </summary>
    public class Conexion
    {
        // Servidor: SQL Server LocalDB. Autenticación de Windows (Integrated Security).
        private static readonly string cadenaConexion =
            @"Server=(localdb)\MSSQLLocalDB;Database=GestionProductosDB;Integrated Security=True;";

        // Devuelve una conexión nueva (sin abrir). Quien la use debe abrirla y cerrarla.
        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        // Comprueba que se pueda conectar a la base de datos.
        public static bool ProbarConexion(out string mensaje)
        {
            try
            {
                using (SqlConnection conexion = ObtenerConexion())
                {
                    conexion.Open();
                    mensaje = "Conexión exitosa con la base de datos " + conexion.Database + ".";
                    return true;
                }
            }
            catch (SqlException ex)
            {
                mensaje = "No se pudo conectar con SQL Server: " + ex.Message;
                return false;
            }
        }
    }
}
