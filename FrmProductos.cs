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
    /// Capa de Interfaz: formulario de productos.
    /// Solo toma datos de los controles, llama a la capa de Lógica y muestra los resultados.
    /// </summary>
    public partial class FrmProductos : Form
    {
        private readonly ProductoLogica productoLogica = new ProductoLogica();
        private readonly CategoriaLogica categoriaLogica = new CategoriaLogica();

        public FrmProductos()
        {
            InitializeComponent();

            // Eventos del formulario y de los controles
            Load += FrmProductos_Load;
            btnGuardar.Click += btnGuardar_Click;
            btnModificar.Click += btnModificar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnBuscar.Click += btnBuscar_Click;
            dgvProductos.CellClick += dgvProductos_CellClick;
        }

        // ----- Eventos -----

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            CargarCategorias();
            CargarProductos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!CamposValidos())
            {
                return;
            }

            string mensaje;
            bool exito = productoLogica.GuardarProducto(ObtenerProductoDelFormulario(), out mensaje);
            MostrarResultado(exito, mensaje);

            if (exito)
            {
                LimpiarFormulario();
                CargarProductos();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!CamposValidos())
            {
                return;
            }

            string mensaje;
            bool exito = productoLogica.ModificarProducto(ObtenerProductoDelFormulario(), out mensaje);
            MostrarResultado(exito, mensaje);

            if (exito)
            {
                LimpiarFormulario();
                CargarProductos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int idProducto = ObtenerIdSeleccionado();

            // Solo se pide confirmación si hay un producto seleccionado
            if (idProducto > 0)
            {
                DialogResult respuesta = MessageBox.Show(
                    "¿Desea eliminar el producto \"" + txtNombre.Text + "\"?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes)
                {
                    return;
                }
            }

            string mensaje;
            bool exito = productoLogica.EliminarProducto(idProducto, out mensaje);
            MostrarResultado(exito, mensaje);

            if (exito)
            {
                LimpiarFormulario();
                CargarProductos();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            CargarProductos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string mensaje;
            List<Producto> productos = productoLogica.BuscarProductos(txtBuscar.Text, out mensaje);
            MostrarProductos(productos);

            if (mensaje != string.Empty)
            {
                MessageBox.Show(mensaje, "Búsqueda de productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Al hacer clic en una fila, sus datos pasan a los campos
        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Producto producto = dgvProductos.Rows[e.RowIndex].DataBoundItem as Producto;
            if (producto == null)
            {
                return;
            }

            txtIdProducto.Text = producto.IdProducto.ToString();
            txtNombre.Text = producto.Nombre;
            txtPrecio.Text = producto.Precio.ToString("0.00");
            txtStock.Text = producto.Stock.ToString();
            cmbCategoria.SelectedValue = producto.IdCategoria;
            errProvider.Clear();
        }

        // ----- Carga de datos -----

        private void CargarCategorias()
        {
            string mensaje;
            List<Categoria> categorias = categoriaLogica.ObtenerCategorias(out mensaje);

            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "IdCategoria";
            cmbCategoria.DataSource = categorias;
            cmbCategoria.SelectedIndex = -1;

            if (mensaje != string.Empty)
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProductos()
        {
            string mensaje;
            List<Producto> productos = productoLogica.ObtenerProductos(out mensaje);
            MostrarProductos(productos);

            if (mensaje != string.Empty)
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarProductos(List<Producto> productos)
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = productos;
            ConfigurarColumnas();
            dgvProductos.ClearSelection();
        }

        private void ConfigurarColumnas()
        {
            ConfigurarColumna("IdProducto", "Id", null);
            ConfigurarColumna("Nombre", "Nombre", null);
            ConfigurarColumna("Precio", "Precio", "N2");
            ConfigurarColumna("Stock", "Stock", null);
            ConfigurarColumna("FechaRegistro", "Fecha de registro", "dd/MM/yyyy HH:mm");
            ConfigurarColumna("NombreCategoria", "Categoría", null);
            ConfigurarColumna("ValorInventario", "Valor del inventario", "N2");

            // El id de la categoría no se muestra al usuario
            if (dgvProductos.Columns["IdCategoria"] != null)
            {
                dgvProductos.Columns["IdCategoria"].Visible = false;
            }
        }

        private void ConfigurarColumna(string nombreColumna, string encabezado, string formato)
        {
            DataGridViewColumn columna = dgvProductos.Columns[nombreColumna];
            if (columna == null)
            {
                return;
            }

            columna.HeaderText = encabezado;
            if (formato != null)
            {
                columna.DefaultCellStyle.Format = formato;
            }
        }

        // ----- Validación y lectura del formulario -----

        // Muestra los errores de validación junto a cada campo (ErrorProvider)
        private bool CamposValidos()
        {
            string errorNombre = productoLogica.ValidarNombre(txtNombre.Text);
            string errorPrecio = productoLogica.ValidarPrecio(txtPrecio.Text);
            string errorStock = productoLogica.ValidarStock(txtStock.Text);
            string errorCategoria = productoLogica.ValidarCategoria(ObtenerIdCategoriaSeleccionada());

            errProvider.SetError(txtNombre, errorNombre);
            errProvider.SetError(txtPrecio, errorPrecio);
            errProvider.SetError(txtStock, errorStock);
            errProvider.SetError(cmbCategoria, errorCategoria);

            return errorNombre == string.Empty
                && errorPrecio == string.Empty
                && errorStock == string.Empty
                && errorCategoria == string.Empty;
        }

        private Producto ObtenerProductoDelFormulario()
        {
            return productoLogica.CrearProducto(
                ObtenerIdSeleccionado(),
                txtNombre.Text,
                txtPrecio.Text,
                txtStock.Text,
                ObtenerIdCategoriaSeleccionada());
        }

        // Devuelve el id del producto seleccionado, o 0 si no hay ninguno
        private int ObtenerIdSeleccionado()
        {
            int idProducto;
            if (int.TryParse(txtIdProducto.Text, out idProducto))
            {
                return idProducto;
            }
            return 0;
        }

        // Devuelve el id de la categoría elegida en el ComboBox, o 0 si no hay ninguna
        private int ObtenerIdCategoriaSeleccionada()
        {
            int idCategoria = 0;
            if (cmbCategoria.SelectedValue != null)
            {
                int.TryParse(cmbCategoria.SelectedValue.ToString(), out idCategoria);
            }
            return idCategoria;
        }

        // ----- Mensajes y limpieza -----

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
            txtIdProducto.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtBuscar.Clear();
            cmbCategoria.SelectedIndex = -1;
            errProvider.Clear();
            txtNombre.Focus();
        }
    }
}
