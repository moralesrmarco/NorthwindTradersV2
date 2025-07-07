using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class MDIPrincipal : Form
    {

        private int childFormNumber = 0;
        public static MDIPrincipal Instance { get; private set; }
        public string UsuarioLogueado { get; set; }
        public int IdUsuarioLogueado { get; set; }

        public ToolStripStatusLabel ToolStripEstado
        {
            get { return tsslEstado; }
            set { tsslEstado = value; }
        }

        public static void ActualizarBarraDeEstado(string mensaje = "Listo.", bool error = false)
        {
            if (Instance != null && !Instance.IsDisposed)
            {
                if (mensaje != "Listo.")
                {
                    if (error)
                        Instance.ToolStripEstado.BackColor = System.Drawing.Color.Red;
                    else
                        Instance.ToolStripEstado.BackColor = SystemColors.ActiveCaption;
                }
                else
                {
                    if (error)
                    {
                        Instance.ToolStripEstado.ForeColor = System.Drawing.Color.White;
                        Instance.ToolStripEstado.Font = new Font(Instance.ToolStripEstado.Font, FontStyle.Bold);
                    }
                    else
                    {
                        Instance.ToolStripEstado.ForeColor = SystemColors.ControlText;
                        Instance.ToolStripEstado.Font = new Font(Instance.ToolStripEstado.Font, FontStyle.Regular);
                    }
                }
                Instance.ToolStripEstado.Text = mensaje;
                Instance.Refresh();
            }
        }

        public MDIPrincipal()
        {
            InitializeComponent();
            Instance = this;
            this.Text = Utils.nwtr;
            this.WindowState = FormWindowState.Maximized;
        }

        private void MDIPrincipal_Load(object sender, EventArgs e)
        {
            string textoParaToolStripTextBox1 = "Usuario: »" + UsuarioLogueado + "«";
            // Medir el tamaño del texto
            Size sizeTextoParaToolStripTextBox1 = TextRenderer.MeasureText(textoParaToolStripTextBox1, toolStripTextBox1.Font);
            // Asignar el ancho con un pequeño margen adicional
            toolStripTextBox1.Width = sizeTextoParaToolStripTextBox1.Width + 20; // se suman 20 píxeles para un margen adicional
            this.toolStripTextBox1.Text = textoParaToolStripTextBox1;
            IniciarSesion();
        }

        public void IniciarSesion()
        {
            // Obtener los permisos del usuario logueado
            var permisos = ObtenerPermisosUsuario(IdUsuarioLogueado);
            // Ajustar el menú por permisos
            AjustarMenuPorPermisos(permisos);
            if (permisos.Count == 0)
            {
                MessageBox.Show("El usuario no tiene permisos asignados.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //return;
            }
            ActualizarBarraDeEstado("Sesión iniciada correctamente");
        }

        private void AjustarMenuPorPermisos(HashSet<int> permisos)
        {
            tsmiEmpleados.Enabled = false;
            tsmiClientes.Enabled = false;
            tsmiProveedores.Enabled = false;
            tsmiCategorias.Enabled = false;
            tsmiProductos.Enabled = false;
            tsmiPedidos.Enabled = false;
            tsmiAdministracion.Enabled = false;
            foreach (int permisoId in permisos)
            {
                if (permisoId == 1)
                    tsmiEmpleados.Enabled = true; // Permiso para Empleados
                else if (permisoId == 2)
                    tsmiClientes.Enabled = true; // Permiso para Clientes
                else if (permisoId == 3)
                    tsmiProveedores.Enabled = true; // Permiso para Proveedores
                else if (permisoId == 4)
                    tsmiCategorias.Enabled = true; // Permiso para Categorías
                else if (permisoId == 5)
                    tsmiProductos.Enabled = true; // Permiso para Productos
                else if (permisoId == 6)
                    tsmiPedidos.Enabled = true; // Permiso para Pedidos
                else if (permisoId == 7)
                    tsmiAdministracion.Enabled = true; // Permiso para Administración
            }
        }

        private HashSet<int> ObtenerPermisosUsuario(int idUsuarioLogueado)
        {
            var permisos = new HashSet<int>();
            using (var cn = new System.Data.SqlClient.SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
            {
                using (var cmd = new System.Data.SqlClient.SqlCommand("SELECT PermisoId FROM Permisos WHERE UsuarioId = @UsuarioId", cn))
                {
                    cmd.Parameters.AddWithValue("@UsuarioId", idUsuarioLogueado);
                    if (cn.State != System.Data.ConnectionState.Open)
                        cn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            permisos.Add(rdr.GetInt32(0));
                        }
                    }
                }
            }
            return permisos;
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

        private void tsmiMantenimientoDeProveedores_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProveedoresCrud frmProveedoresCrud = new FrmProveedoresCrud
            {
                MdiParent = this
            };
            frmProveedoresCrud.Show();
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
        }

        private void reporteDeEmpleadosConFotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        private void directorioDeClientesYProveedoresPorCiudadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorioxCiudad frmRptClientesyProveedoresDirectorioxCiudad = new FrmRptClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void directorioDeClientesYProveedoresPorPaísToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorioxPais frmRptClientesyProveedoresDirectorioxPais = new FrmRptClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorioxPais.Show();
        }

        private void directorioDeClientesYProveedoresPorCiudadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxCiudad frmClientesyProveedoresDirectorioxCiudad = new FrmClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void directorioDeClientesYProveedoresPorPaísToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxPais frmClientesyProveedoresDirectorioxPais = new FrmClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxPais.Show();
        }

        private void consultaDeProductosPorProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProveedoresProductos frmProveedoresProductos = new FrmProveedoresProductos
            {
                MdiParent = this
            };
            frmProveedoresProductos.Show();
        }

        private void directorioDeClientesYProveedoresPorCiudadToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxCiudad frmClientesyProveedoresDirectorioxCiudad = new FrmClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void directorioDeClientesYProveedoresPorPaísToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxPais frmClientesyProveedoresDirectorioxPais = new FrmClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxPais.Show();
        }

        private void directorioDeProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProveedores frmRptProveedores = new FrmRptProveedores
            {
                MdiParent = this
            };
            frmRptProveedores.Show();
        }

        private void directorioDeClientesYProveedoresToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorio frmRptClientesyProveedoresDirectorio = new FrmRptClientesyProveedoresDirectorio
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorio.Show();
        }

        private void directorioDeClientesYProveedoresPorCiudadToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorioxCiudad frmRptClientesyProveedoresDirectorioxCiudad = new FrmRptClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void directorioDeClientesYProveedoresPorPaísToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorioxPais frmRptClientesyProveedoresDirectorioxPais = new FrmRptClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorioxPais.Show();
        }

        private void directorioDeClientesYProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorio frmRptClientesyProveedoresDirectorio = new FrmRptClientesyProveedoresDirectorio
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorio.Show();
        }

        private void directorioDeClientesYProveedoresToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorio frmClientesyProveedoresDirectorio = new FrmClientesyProveedoresDirectorio
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorio.Show();
        }

        private void directorioDeClientesYProveedoresToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorio frmClientesyProveedoresDirectorio = new FrmClientesyProveedoresDirectorio
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorio.Show();
        }

        private void reporteDeEmpleadosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptEmpleados frmRptEmpleados = new FrmRptEmpleados
            {
                MdiParent = this
            };
            frmRptEmpleados.Show();
        }

        private void reporteDeEmpleadosConFotoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptEmpleadosConFoto frmRptEmpleadosConFoto = new FrmRptEmpleadosConFoto
            {
                MdiParent = this
            };
            frmRptEmpleadosConFoto.Show();
        }

        private void reporteDeEmpleadosConFoto2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptEmpleado2 frmRptEmpleado2 = new FrmRptEmpleado2
            {
                MdiParent = this
            };
            frmRptEmpleado2.Show();
        }

        private void consultaDeProductosPorCategoríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmCategoriasProductos frmCategoriasProductos = new FrmCategoriasProductos
            {
                MdiParent = this
            };
            frmCategoriasProductos.Show();
        }

        private void listadoDeProductosPorCategoríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosPorCategoriasListado frmProductosPorCategoriasListado = new FrmProductosPorCategoriasListado
            {
                MdiParent = this
            };
            frmProductosPorCategoriasListado.Show();
        }

        private void reporteDeCategoríasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptCategorias frmRptCategorias = new FrmRptCategorias
            {
                MdiParent = this
            };
            frmRptCategorias.Show();
        }

        private void reporteDeProductosPorCategoríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProductosPorCategorias frmRptProductosPorCategorias = new FrmRptProductosPorCategorias
            {
                MdiParent = this
            };
            frmRptProductosPorCategorias.Show();
        }

        private void reporteDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProductos frmRptProductos = new FrmRptProductos
            {
                MdiParent = this
            };
            frmRptProductos.Show();
        }

        private void reporteDeProductosPorCategoríaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProductosPorCategorias frmRptProductosPorCategorias = new FrmRptProductosPorCategorias
            {
                MdiParent = this
            };
            frmRptProductosPorCategorias.Show();
        }

        private void reporteDeProductosPorProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProductosPorProveedor frmRptProductosPorProveedor = new FrmRptProductosPorProveedor
            {
                MdiParent = this
            };
            frmRptProductosPorProveedor.Show();
        }

        private void reporteDeProductosPorProveedorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProductosPorProveedor frmRptProductosPorProveedor = new FrmRptProductosPorProveedor
            {
                MdiParent = this
            };
            frmRptProductosPorProveedor.Show();
        }

        private void reporteAlfabéticoDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProductosAlfabetico frmRptProductosAlfabetico = new FrmRptProductosAlfabetico
            {
                MdiParent = this
            };
            frmRptProductosAlfabetico.Show();
        }

        private void reporteDeProductosPorProveedorConDetalleDelProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProdPorProvConDetProv frmRptProdPorProvConDetProv = new FrmRptProdPorProvConDetProv
            {
                MdiParent = this
            };
            frmRptProdPorProvConDetProv.Show();
        }

        private void reporteDeProductosPorProveedorConDetalleDelProveedorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProdPorProvConDetProv frmRptProdPorProvConDetProv = new FrmRptProdPorProvConDetProv
            {
                MdiParent = this
            };
            frmRptProdPorProvConDetProv.Show();
        }

        private void reporteDePedidosPorRangoDeFechaDePedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptPedPorRangoFechaPed frmRptPedPorRangoFechaPed = new FrmRptPedPorRangoFechaPed
            {
                MdiParent = this
            };
            frmRptPedPorRangoFechaPed.Show();
        }

        private void reporteDePedidosPorDiferentesCriteriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptPedPorDifCriterios frmRptPedPorDifCriterios = new FrmRptPedPorDifCriterios
            {
                MdiParent = this
            };
            frmRptPedPorDifCriterios.Show();
        }

        private void mantenimientoDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmUsuariosCrud frmUsuariosCrud = new FrmUsuariosCrud
            {
                MdiParent = this
            };
            frmUsuariosCrud.Show();
        }

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmCambiarContrasena frmCambiarContrasena = new FrmCambiarContrasena
            {
                MdiParent = this,
                UsuarioLogueado = this.UsuarioLogueado
            };
            frmCambiarContrasena.Show();
        }

        private void mantenimientoDePermisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmPermisosCrud frmPermisosCrud = new FrmPermisosCrud
            {
                MdiParent = this
            };
            frmPermisosCrud.Show();
        }
    }
}
