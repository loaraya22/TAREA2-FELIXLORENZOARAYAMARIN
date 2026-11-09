using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAREA2_FELIXLORENZOARAYAMARIN.Datos;

namespace TAREA2_FELIXLORENZOARAYAMARIN
{
    /// <summary>
    /// Capa de Interfaz: menú principal del sistema.
    /// </summary>
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();

            btnCategorias.Click += btnCategorias_Click;
            btnProductos.Click += btnProductos_Click;
            btnProbarConexion.Click += btnProbarConexion_Click;
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            using (FrmCategorias frmCategorias = new FrmCategorias())
            {
                frmCategorias.ShowDialog();
            }
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            using (FrmProductos frmProductos = new FrmProductos())
            {
                frmProductos.ShowDialog();
            }
        }

        // Evidencia de la conexión con SQL Server
        private void btnProbarConexion_Click(object sender, EventArgs e)
        {
            string mensaje;
            bool conectado = Conexion.ProbarConexion(out mensaje);

            MessageBox.Show(
                mensaje,
                "Conexión con SQL Server",
                MessageBoxButtons.OK,
                conectado ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }
    }
}
