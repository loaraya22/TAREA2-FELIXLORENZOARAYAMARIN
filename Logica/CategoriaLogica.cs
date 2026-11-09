using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAREA2_FELIXLORENZOARAYAMARIN.Datos;

namespace TAREA2_FELIXLORENZOARAYAMARIN.Logica
{
    /// <summary>
    /// Capa de Lógica: validaciones y reglas del sistema para las categorías.
    /// Recibe datos de la interfaz, los valida y llama a la capa de Datos.
    /// No usa controles ni MessageBox: devuelve mensajes para que la interfaz los muestre.
    /// </summary>
    public class CategoriaLogica
    {
        private readonly CategoriaDatos categoriaDatos = new CategoriaDatos();

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

        public string ValidarDescripcion(string descripcion)
        {
            if (descripcion != null && descripcion.Trim().Length > 250)
            {
                return "La descripción no puede tener más de 250 caracteres.";
            }
            return string.Empty;
        }

        // ----- Operaciones -----

        public List<Categoria> ObtenerCategorias(out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                return categoriaDatos.ObtenerTodas();
            }
            catch (SqlException)
            {
                mensaje = "No se pudieron cargar las categorías. Verifique la conexión con la base de datos.";
                return new List<Categoria>();
            }
        }

        public bool GuardarCategoria(Categoria categoria, out string mensaje)
        {
            mensaje = ValidarCategoria(categoria);
            if (mensaje != string.Empty)
            {
                return false;
            }

            try
            {
                if (categoriaDatos.ExisteNombre(categoria.Nombre, 0))
                {
                    mensaje = "Ya existe una categoría con ese nombre.";
                    return false;
                }

                categoriaDatos.Insertar(categoria);
                mensaje = "Categoría guardada correctamente.";
                return true;
            }
            catch (SqlException)
            {
                mensaje = "No se pudo guardar la categoría. Verifique la conexión con la base de datos.";
                return false;
            }
        }

        public bool ModificarCategoria(Categoria categoria, out string mensaje)
        {
            if (categoria.IdCategoria <= 0)
            {
                mensaje = "Seleccione una categoría de la lista para modificarla.";
                return false;
            }

            mensaje = ValidarCategoria(categoria);
            if (mensaje != string.Empty)
            {
                return false;
            }

            try
            {
                if (categoriaDatos.ExisteNombre(categoria.Nombre, categoria.IdCategoria))
                {
                    mensaje = "Ya existe otra categoría con ese nombre.";
                    return false;
                }

                int filas = categoriaDatos.Modificar(categoria);
                if (filas == 0)
                {
                    mensaje = "No se encontró la categoría que se intentó modificar.";
                    return false;
                }

                mensaje = "Categoría modificada correctamente.";
                return true;
            }
            catch (SqlException)
            {
                mensaje = "No se pudo modificar la categoría. Verifique la conexión con la base de datos.";
                return false;
            }
        }

        public bool EliminarCategoria(int idCategoria, out string mensaje)
        {
            if (idCategoria <= 0)
            {
                mensaje = "Seleccione una categoría de la lista para eliminarla.";
                return false;
            }

            try
            {
                if (categoriaDatos.TieneProductos(idCategoria))
                {
                    mensaje = "No se puede eliminar la categoría porque tiene productos asociados.";
                    return false;
                }

                int filas = categoriaDatos.Eliminar(idCategoria);
                if (filas == 0)
                {
                    mensaje = "No se encontró la categoría que se intentó eliminar.";
                    return false;
                }

                mensaje = "Categoría eliminada correctamente.";
                return true;
            }
            catch (SqlException)
            {
                mensaje = "No se pudo eliminar la categoría. Verifique la conexión con la base de datos.";
                return false;
            }
        }

        // ----- Apoyo interno -----

        // Valida todos los campos y deja los textos sin espacios sobrantes
        private string ValidarCategoria(Categoria categoria)
        {
            string errorNombre = ValidarNombre(categoria.Nombre);
            if (errorNombre != string.Empty)
            {
                return errorNombre;
            }

            string errorDescripcion = ValidarDescripcion(categoria.Descripcion);
            if (errorDescripcion != string.Empty)
            {
                return errorDescripcion;
            }

            categoria.Nombre = categoria.Nombre.Trim();
            categoria.Descripcion = (categoria.Descripcion ?? string.Empty).Trim();
            return string.Empty;
        }
    }
}

