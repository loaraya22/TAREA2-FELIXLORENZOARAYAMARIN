using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAREA2_FELIXLORENZOARAYAMARIN
{
    /// <summary>
    /// Representa una categoría de productos.
    /// </summary>
    public class Categoria
    {
        // Campos privados (encapsulamiento)
        private int idCategoria;
        private string nombre;
        private string descripcion;

        // Propiedades con get y set
        public int IdCategoria
        {
            get { return idCategoria; }
            set { idCategoria = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        // Constructor vacío
        public Categoria()
        {
            nombre = string.Empty;
            descripcion = string.Empty;
        }

        // Constructor con parámetros
        public Categoria(int idCategoria, string nombre, string descripcion)
        {
            this.idCategoria = idCategoria;
            this.nombre = nombre;
            this.descripcion = descripcion;
        }

        // Permite que el ComboBox muestre el nombre de la categoría
        public override string ToString()
        {
            return nombre;
        }
    }
}
