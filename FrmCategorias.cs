using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAREA2_FELIXLORENZOARAYAMARIN.Logica;

namespace TAREA2_FELIXLORENZOARAYAMARIN
{
    /// <summary>
    /// Capa de Interfaz: formulario de categorías.
    /// Solo toma datos de los controles, llama a la capa de Lógica y muestra los resultados.
    /// </summary>
    public partial class FrmCategorias : Form
    {
        private readonly CategoriaLogica categoriaLogica = new CategoriaLogica();

        public FrmCategorias()
        {
            InitializeComponent();

            // Eventos del formulario y de los controles
            Load += FrmCategorias_Load;
            btnGuardar.Click += btnGuardar_Click;
            btnModificar.Click += btnModificar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            dgvCategorias.CellClick += dgvCategorias_CellClick;
        }

        // ----- Eventos -----

        private void FrmCategorias_Load(object sender, EventArgs e)
        {
            CargarCategorias();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!CamposValidos())
            {
                return;
            }

            string mensaje;
            bool exito = categoriaLogica.GuardarCategoria(ObtenerCategoriaDelFormulario(), out mensaje);
            MostrarResultado(exito, mensaje);

            if (exito)
            {
                LimpiarFormulario();
                CargarCategorias();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!CamposValidos())
            {
                return;
            }

            string mensaje;
            bool exito = categoriaLogica.ModificarCategoria(ObtenerCategoriaDelFormulario(), out mensaje);
            MostrarResultado(exito, mensaje);

            if (exito)
            {
                LimpiarFormulario();
                CargarCategorias();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int idCategoria = ObtenerIdSeleccionado();

            // Solo se pide confirmación si hay una categoría seleccionada
            if (idCategoria > 0)
            {
                DialogResult respuesta = MessageBox.Show(
                    "¿Desea eliminar la categoría \"" + txtNombre.Text + "\"?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes)
                {
                    return;
                }
            }

            string mensaje;
            bool exito = categoriaLogica.EliminarCategoria(idCategoria, out mensaje);
            MostrarResultado(exito, mensaje);

            if (exito)
            {
                LimpiarFormulario();
                CargarCategorias();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // Al hacer clic en una fila, sus datos pasan a los campos de texto
        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Categoria categoria = dgvCategorias.Rows[e.RowIndex].DataBoundItem as Categoria;
            if (categoria == null)
            {
                return;
            }

            txtIdCategoria.Text = categoria.IdCategoria.ToString();
            txtNombre.Text = categoria.Nombre;
            txtDescripcion.Text = categoria.Descripcion;
            errProvider.Clear();
        }

        // ----- Métodos de apoyo -----

        private void CargarCategorias()
        {
            string mensaje;
            List<Categoria> categorias = categoriaLogica.ObtenerCategorias(out mensaje);

            dgvCategorias.DataSource = null;
            dgvCategorias.DataSource = categorias;
            ConfigurarColumnas();
            dgvCategorias.ClearSelection();

            if (mensaje != string.Empty)
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvCategorias.Columns["IdCategoria"] != null)
            {
                dgvCategorias.Columns["IdCategoria"].HeaderText = "Id";
            }
            if (dgvCategorias.Columns["Nombre"] != null)
            {
                dgvCategorias.Columns["Nombre"].HeaderText = "Nombre";
            }
            if (dgvCategorias.Columns["Descripcion"] != null)
            {
                dgvCategorias.Columns["Descripcion"].HeaderText = "Descripción";
            }
        }

        // Muestra los errores de validación junto a cada campo (ErrorProvider)
        private bool CamposValidos()
        {
            string errorNombre = categoriaLogica.ValidarNombre(txtNombre.Text);
            string errorDescripcion = categoriaLogica.ValidarDescripcion(txtDescripcion.Text);

            errProvider.SetError(txtNombre, errorNombre);
            errProvider.SetError(txtDescripcion, errorDescripcion);

            return errorNombre == string.Empty && errorDescripcion == string.Empty;
        }

        private Categoria ObtenerCategoriaDelFormulario()
        {
            Categoria categoria = new Categoria();
            categoria.IdCategoria = ObtenerIdSeleccionado();
            categoria.Nombre = txtNombre.Text;
            categoria.Descripcion = txtDescripcion.Text;
            return categoria;
        }

        // Devuelve el id de la categoría seleccionada, o 0 si no hay ninguna
        private int ObtenerIdSeleccionado()
        {
            int idCategoria;
            if (int.TryParse(txtIdCategoria.Text, out idCategoria))
            {
                return idCategoria;
            }
            return 0;
        }

        private void MostrarResultado(bool exito, string mensaje)
        {
            if (exito)
            {
                MessageBox.Show(mensaje, "Operación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LimpiarFormulario()
        {
            txtIdCategoria.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            errProvider.Clear();
            dgvCategorias.ClearSelection();
            txtNombre.Focus();
        }
    }
}
