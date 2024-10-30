using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmProductosListado : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        string strProcedure = "";

        public FrmProductosListado()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmProductosListado_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void FrmProductosListado_Load(object sender, EventArgs e)
        {
            LlenarCboCategoria();
            LlenarCboProveedor();
            Utils.ConfDgv(Dgv);
        }

        private void LlenarCboCategoria()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand("Sp_Categorias_Seleccionar_V2", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                cboCategoria.DataSource = tbl;
                cboCategoria.DisplayMember = "Categoria";
                cboCategoria.ValueMember = "Id";
                Utils.ActualizarBarraDeEstado(this);
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

        private void LlenarCboProveedor()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand("Sp_Proveedores_Seleccionar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                cboProveedor.DataSource = tbl;
                cboProveedor.DisplayMember = "Proveedor";
                cboProveedor.ValueMember = "Id";
                Utils.ActualizarBarraDeEstado(this);
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdInicial.Text = txtIdFinal.Text = txtProducto.Text = "";
            cboCategoria.SelectedIndex = cboProveedor.SelectedIndex = 0;
            Dgv.DataSource = null;
            Utils.ActualizarBarraDeEstado(this);
        }

        private void btnListarTodos_Click(object sender, EventArgs e)
        {
            strProcedure = "Sp_Productos_All";
            LlenarDgv(sender);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            strProcedure = "Sp_Productos_Buscar_V2";
            LlenarDgv(sender);
        }

        private void txtIdInicial_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtIdFinal_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtIdInicial_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdIni(txtIdInicial, txtIdFinal);
        }

        private void txtIdFinal_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdFin(txtIdInicial, txtIdFinal);
        }

        private void LlenarDgv(object sender)
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand(strProcedure, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (((Button)sender).Tag.ToString() == "Buscar")
                {
                    cmd.Parameters.AddWithValue("IdIni", txtIdInicial.Text);
                    cmd.Parameters.AddWithValue("IdFin", txtIdFinal.Text);
                    cmd.Parameters.AddWithValue("Producto", txtProducto.Text);
                    cmd.Parameters.AddWithValue("Categoria", cboCategoria.SelectedValue);
                    cmd.Parameters.AddWithValue("Proveedor", cboProveedor.SelectedValue);
                }
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                Dgv.DataSource = tbl;
                ConfDgv();
                Utils.ActualizarBarraDeEstado(this, $"Se encontraron {Dgv.RowCount} registros");
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

        private void ConfDgv()
        {
            Dgv.Columns["IdCategoria"].Visible = false;
            Dgv.Columns["IdProveedor"].Visible = false;

            Dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Cantidad por unidad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Precio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Unidades en inventario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Unidades en pedido"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Punto de pedido"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Descontinuado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Categoría"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Unidades en inventario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Unidades en pedido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Punto de pedido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            Dgv.Columns["Precio"].DefaultCellStyle.Format = "c";
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            btnLimpiar.PerformClick();
        }

        private void tabcOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLimpiar.PerformClick();
        }
    }
}
