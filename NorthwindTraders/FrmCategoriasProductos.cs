using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmCategoriasProductos : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        BindingSource bsCategorias = new BindingSource();
        BindingSource bsProductos = new BindingSource();

        public FrmCategoriasProductos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmCategoriasProductos_Load(object sender, EventArgs e)
        {
            dgvCategorias.DataSource = bsCategorias;
            dgvProductos.DataSource = bsProductos;
            GetData();
            Utils.ConfDgv(dgvCategorias);
            Utils.ConfDgv(dgvProductos);
            ConfDgvCategorias(dgvCategorias);
            ConfDgvProductos(dgvProductos);
            Utils.ActualizarBarraDeEstado(this, $"Se encontraron {dgvCategorias.RowCount} registros en categorías y {dgvProductos.RowCount} registros de productos en la categoría {dgvCategorias.CurrentRow.Cells["Categoría"].Value}");
        }

        private void GetData()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                DataSet ds = new DataSet();
                ds.Locale = System.Globalization.CultureInfo.InvariantCulture;
                SqlDataAdapter dapCategorias = new SqlDataAdapter("Sp_Categorias_Listar", cn);
                dapCategorias.Fill(ds, "Categorias");
                SqlDataAdapter dapProductos = new SqlDataAdapter("Sp_Productos_all", cn);
                dapProductos.Fill(ds, "Productos");
                // en la siguiente instrucción se deben de proporcionar los nombres de los campos (alias) que devuelve el store procedure
                DataRelation dataRelation = new DataRelation("CategoriasProductos", ds.Tables["Categorias"].Columns["Id"], ds.Tables["Productos"].Columns["IdCategoria"]);
                ds.Relations.Add(dataRelation);
                bsCategorias.DataSource = ds;
                bsCategorias.DataMember = "Categorias";
                bsProductos.DataSource = bsCategorias;
                bsProductos.DataMember = "CategoriasProductos";
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(this, ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(this, ex);
            }
        }

        private void ConfDgvCategorias(DataGridView dgv)
        {
            dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Categoría"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Foto"].Width = 50;
            dgv.Columns["Foto"].DefaultCellStyle.Padding = new Padding(4, 4, 4, 4);
            ((DataGridViewImageColumn)dgv.Columns["Foto"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void ConfDgvProductos(DataGridView dgv)
        {
            dgv.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Precio"].DefaultCellStyle.Format = "c";
            dgv.Columns["Unidades en inventario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Unidades en pedido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Punto de pedido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Producto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Cantidad por unidad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Precio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Unidades en inventario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Unidades en pedido"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Punto de pedido"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dgv.Columns["Descontinuado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Categoría"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Descripción de categoría"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Proveedor"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv.Columns["IdCategoria"].Visible = false;
            dgv.Columns["IdProveedor"].Visible = false;
        }

        private void FrmCategoriasProductos_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void dgvCategorias_SelectionChanged(object sender, EventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this, $"Se encontraron {dgvCategorias.RowCount} registros en categorías y {dgvProductos.RowCount} registros de productos, en la categoría {dgvCategorias.CurrentRow.Cells["Categoría"].Value}");
        }

        private void dgvCategorias_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Utils.ActualizarBarraDeEstado(this, $"Se encontraron {dgvCategorias.RowCount} registros en categorías y {dgvProductos.RowCount} registros de productos, en la categoría seleccionada.");
        }

    }
}
