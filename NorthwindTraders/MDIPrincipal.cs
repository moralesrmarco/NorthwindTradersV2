using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class MDIPrincipal : Form
    {
        private int childFormNumber = 0;

        public ToolStripStatusLabel ToolStripEstado
        {
            get { return tsslEstado; }
            set { tsslEstado = value; }
        }

        public MDIPrincipal()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Ventana " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
        }

        private void tsmiMantenimientoDeEmpleados_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmEmpleadosCrud frmEmpleadosCrud = new FrmEmpleadosCrud
            {
                MdiParent = this
            };
            frmEmpleadosCrud.Show();
        }

        private void tsmiMantenimientoDeClientes_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesCrud frmClientesCrud = new FrmClientesCrud
            {
                MdiParent = this
            };
            frmClientesCrud.Show();
        }

        private void tsmiDirectorioDeClientesYProveedoresPorCiudad_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxCiudad frmClientesyProveedoresDirectorioxCiudad = new FrmClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void tsmiDirectorioDeClientesYProveedoresPorPais_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxPais frmClientesyProveedoresDirectorioxPais = new FrmClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxPais.Show();
        }

        private void tsmiMantenimientoDeProveedores_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProveedoresCrud frmProveedoresCrud = new FrmProveedoresCrud
            {
                MdiParent = this
            };
            frmProveedoresCrud.Show();
        }

        private void tsmiProveedoresProductos_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProveedoresProductos frmProveedoresProductos = new FrmProveedoresProductos
            {
                MdiParent = this
            };
            frmProveedoresProductos.Show();
        }

        private void tsmi2DirectorioDeClientesYProveedoresPorCiudad_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxCiudad frmClientesyProveedoresDirectorioxCiudad = new FrmClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void tsmi2DirectorioDeClientesYProveedoresPorPais_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxPais frmClientesyProveedoresDirectorioxPais = new FrmClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxPais.Show();
        }

        private void tsmiMantenimientoDeCategorías_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmCategoriasCrud frmCategoriasCrud = new FrmCategoriasCrud
            {
                MdiParent = this
            };
            frmCategoriasCrud.Show();
        }

        private void tsmiCategoriasProductos_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmCategoriasProductos frmCategoriasProductos = new FrmCategoriasProductos
            {
                MdiParent = this
            };
            frmCategoriasProductos.Show();
        }

        private void tsmi2ListadoDeProductosPorCategorías_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosPorCategoriasListado frmProductosPorCategoriasListado = new FrmProductosPorCategoriasListado
            {
                MdiParent = this
            };
            frmProductosPorCategoriasListado.Show();
        }

        private void tsmiMantenimientoDeProductos_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosCrud frmProductosCrud = new FrmProductosCrud
            {
                MdiParent = this
            };
            frmProductosCrud.Show();
        }

        private void tsmiConsultaDeProductosPorCategoría_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmCategoriasProductos frmCategoriasProductos = new FrmCategoriasProductos
            {
                MdiParent = this
            };
            frmCategoriasProductos.Show();
        }

        private void tsmiConsultaDeProductosPorProveedor_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProveedoresProductos frmProveedoresProductos = new FrmProveedoresProductos
            {
                MdiParent = this
            };
            frmProveedoresProductos.Show();
        }

        private void tsmiListadoDeProductosPorCategorias_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosPorCategoriasListado frmProductosPorCategoriasListado = new FrmProductosPorCategoriasListado
            {
                MdiParent = this
            };
            frmProductosPorCategoriasListado.Show();
        }

        private void tsmiConsultaAlfabeticaDeProductos_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosConsultaAlfabetica frmProductosConsultaAlfabetica = new FrmProductosConsultaAlfabetica
            {
                MdiParent = this
            };
            frmProductosConsultaAlfabetica.Show();
        }

        private void tsmiProductosPorEncimaPrecioProm_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosPorEncimaPrecioPromedio frmProductosPorEncimaPrecioPromedio = new FrmProductosPorEncimaPrecioPromedio
            {
                MdiParent = this
            };
            frmProductosPorEncimaPrecioPromedio.Show();
        }

        private void tsmiListadoDeProductos_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosListado frmProductosListado = new FrmProductosListado
            {
                MdiParent = this
            };
            frmProductosListado.Show();
        }

        private void tsmiMantenimientoDePedidos_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmPedidosCrud frmPedidosCrud = new FrmPedidosCrud
            {
                MdiParent = this
            };
            frmPedidosCrud.Show();
        }

        private void tsmiMantenimientoDeDetalleDePedidos_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmPedidosDetalleCrud frmPedidosDetalleCrud = new FrmPedidosDetalleCrud
            {
                MdiParent = this
            };
            frmPedidosDetalleCrud.Show();
        }

        private void tsmiMantenimientoDePedidosV2_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmPedidosCrudV2 frmPedidosCrudV2 = new FrmPedidosCrudV2
            {
                MdiParent = this
            };
            frmPedidosCrudV2.Show();
        }

        private void reporteDeEmpleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptEmpleados frmRptEmpleados = new FrmRptEmpleados
            {
                MdiParent = this
            };
            frmRptEmpleados.Show();
        }

        private void reporteDeEmpleadosConFotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptEmpleadosConFoto frmRptEmpleadosConFoto = new FrmRptEmpleadosConFoto
            {
                MdiParent = this
            };
            frmRptEmpleadosConFoto.Show();
        }

        private void directorioDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientes frmRptClientes = new FrmRptClientes
            {
                MdiParent = this
            };
            frmRptClientes.Show();
        }
    }
}
