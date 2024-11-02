using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmPedidosCrud : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        bool EventoCargardo = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar
        int IdDetalle = 1;

        public FrmPedidosCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void GrbPaint2(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint2(this, sender, e);
        }

        private void FrmPedidosCrud_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void FrmPedidosCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tabpConsultar)
            {
                if (cboCliente.SelectedIndex > 0 || cboEmpleado.SelectedIndex > 0 || cboTransportista.SelectedIndex > 0 || cboCategoria.SelectedIndex > 0 || cboProducto.SelectedIndex > 0)
                {
                    DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (respuesta == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void FrmPedidosCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarCboCliente();
            LlenarCboEmpleado();
            LlenarCboTransportista();
            LlenarCboCategoria();
            Utils.ConfDgv(dgvPedidos);
            Utils.ConfDgv(dgvDetalle);
            LlenarDgvPedidos(null);
            ConfDgvPedidos();
            ConfDgvDetalle();
            dgvDetalle.Columns["Eliminar"].Visible = false;
            txtPrecio.Text = txtFlete.Text = "$0.00";
            txtDescuento.Text = "0.00";
        }

        private void DeshabilitarControles()
        {
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = cboCategoria.Enabled = cboProducto.Enabled = false;
            dtpPedido.Enabled = dtpHoraPedido.Enabled = dtpRequerido.Enabled = dtpHoraRequerido.Enabled = dtpEnvio.Enabled = dtpHoraEnvio.Enabled = false;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = true;
            txtCantidad.ReadOnly = txtDescuento.ReadOnly = true;
            btnAgregar.Enabled = btnGenerar.Enabled = false;
        }

        private void HabilitarControles()
        {
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = cboCategoria.Enabled = cboProducto.Enabled = true;
            dtpPedido.Enabled = dtpHoraPedido.Enabled = dtpRequerido.Enabled = dtpHoraRequerido.Enabled = dtpEnvio.Enabled = dtpHoraEnvio.Enabled = true;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = false;
            txtCantidad.ReadOnly = txtDescuento.ReadOnly = false;
            btnAgregar.Enabled = btnGenerar.Enabled = true;
        }

        private void LlenarCboCliente()
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Clientes_Seleccionar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                cboCliente.DataSource = tbl;
                cboCliente.DisplayMember = "Cliente";
                cboCliente.ValueMember = "Id";
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

        private void LlenarCboEmpleado()
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Empleados_Seleccionar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                cboEmpleado.DataSource = tbl;
                cboEmpleado.DisplayMember = "Empleado";
                cboEmpleado.ValueMember = "Id";
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

        private void LlenarCboTransportista()
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Transportistas_Seleccionar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                cboTransportista.DataSource = tbl;
                cboTransportista.DisplayMember = "Transportista";
                cboTransportista.ValueMember = "Id";
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

        private void LlenarCboCategoria()
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Categorias_Seleccionar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                cboCategoria.DataSource = tbl;
                cboCategoria.DisplayMember = "Categoria";
                cboCategoria.ValueMember = "Id";
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

        private void LlenarDgvPedidos(object sender)
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            try
            {
                SqlCommand cmd;
                if (sender == null)
                    cmd = new SqlCommand("Sp_Pedidos_Listar20", cn);
                else
                {
                    cmd = new SqlCommand("Sp_Pedidos_Buscar", cn);
                    cmd.Parameters.AddWithValue("IdInicial", txtBIdInicial.Text);
                    cmd.Parameters.AddWithValue("IdFinal", txtBIdFinal.Text);
                    cmd.Parameters.AddWithValue("Cliente", txtBCliente.Text);
                    if (dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked)
                    {
                        cmd.Parameters.AddWithValue("FPedido", true); // este parametro es requerido para que funcione el store procedure con la misma logica que he venido usando en las demas busquedas
                        dtpBFPedidoIni.Value = Convert.ToDateTime(dtpBFPedidoIni.Value.ToShortDateString() + " 00:00:00.000");
                        cmd.Parameters.AddWithValue("FPedidoIni", dtpBFPedidoIni.Value);
                        dtpBFPedidoFin.Value = Convert.ToDateTime(dtpBFPedidoFin.Value.ToShortDateString() + " 23:59:59.998"); // se usa .998 porque lo redondea a .997 por la presición de los campos tipo datetime de sql server, el cual es el maximo valor de milesimas de segundo que puede guardarse en la db. Si se usa .999 lo redondea al segundo 0.000 del siquiente dia e incluye los datos del siguiente día que es un comportamiento que no se quiere por que solo se deben mostrar los datos de la fecha indicada. Ya se comprobo el comportamiento en la base de datos.
                        cmd.Parameters.AddWithValue("FPedidoFin", dtpBFPedidoFin.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("FPedido", false);
                        cmd.Parameters.AddWithValue("FPedidoIni", DBNull.Value);
                        cmd.Parameters.AddWithValue("FPedidoFin", DBNull.Value);
                    }
                    if (chkBFPedidoNull.Checked)
                        cmd.Parameters.AddWithValue("FPedidoNull", true);
                    else
                        cmd.Parameters.AddWithValue("FPedidoNull", false);
                    if (dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked)
                    {
                        cmd.Parameters.AddWithValue("FRequerido", true);
                        dtpBFRequeridoIni.Value = Convert.ToDateTime(dtpBFRequeridoIni.Value.ToShortDateString() + " 00:00:00.000");
                        cmd.Parameters.AddWithValue("FRequeridoIni", dtpBFRequeridoIni.Value);
                        dtpBFRequeridoFin.Value = Convert.ToDateTime(dtpBFRequeridoFin.Value.ToShortDateString() + " 23:59:59.998");
                        cmd.Parameters.AddWithValue("FRequeridoFin", dtpBFRequeridoFin.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("FRequerido", false);
                        cmd.Parameters.AddWithValue("FRequeridoIni", DBNull.Value);
                        cmd.Parameters.AddWithValue("FRequeridoFin", DBNull.Value);
                    }
                    if (chkBFRequeridoNull.Checked)
                        cmd.Parameters.AddWithValue("FRequeridoNull", true);
                    else
                        cmd.Parameters.AddWithValue("FRequeridoNull", false);
                    if (dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked)
                    {
                        cmd.Parameters.AddWithValue("FEnvio", true);
                        dtpBFEnvioIni.Value = Convert.ToDateTime(dtpBFEnvioIni.Value.ToShortDateString() + " 00:00:00.000");
                        cmd.Parameters.AddWithValue("FEnvioIni", dtpBFEnvioIni.Value);
                        dtpBFEnvioFin.Value = Convert.ToDateTime(dtpBFEnvioFin.Value.ToShortDateString() + " 23:59:59.998");
                        cmd.Parameters.AddWithValue("FEnvioFin", dtpBFEnvioFin.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("FEnvio", false);
                        cmd.Parameters.AddWithValue("FEnvioIni", DBNull.Value);
                        cmd.Parameters.AddWithValue("FEnvioFin", DBNull.Value);
                    }
                    if (chkBFEnvioNull.Checked)
                        cmd.Parameters.AddWithValue("FEnvioNull", true);
                    else
                        cmd.Parameters.AddWithValue("FEnvioNull", false);
                    cmd.Parameters.AddWithValue("Empleado", txtBEmpleado.Text);
                    cmd.Parameters.AddWithValue("CompañiaT", txtBCompañiaT.Text);
                    cmd.Parameters.AddWithValue("Dirigidoa", txtBDirigidoa.Text);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                dgvPedidos.DataSource = tbl;
                if (sender == null)
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran los últimos {dgvPedidos.RowCount} pedidos registrados");
                else
                    Utils.ActualizarBarraDeEstado(this, $"Se encontraron {dgvPedidos.RowCount} registros");
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

        private void ConfDgvPedidos()
        {
            dgvPedidos.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvPedidos.Columns["Fecha de pedido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPedidos.Columns["Fecha requerido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPedidos.Columns["Fecha de envío"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPedidos.Columns["Compañía transportista"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPedidos.Columns["Vendedor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPedidos.Columns["Fecha de pedido"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
            dgvPedidos.Columns["Fecha requerido"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
            dgvPedidos.Columns["Fecha de envío"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
        }

        private void ConfDgvDetalle()
        {
            dgvDetalle.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalle.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetalle.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalle.Columns["Descuento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalle.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            BorrarDatosBusqueda();
            if (tabcOperacion.SelectedTab != tabpRegistrar)
                DeshabilitarControles();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tabpRegistrar)
                DeshabilitarControles();
            LlenarDgvPedidos(sender);
        }

        private void BorrarDatosPedido()
        {
            txtId.Text = "";
            cboCliente.SelectedIndex = cboEmpleado.SelectedIndex = cboTransportista.SelectedIndex = cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            dtpPedido.Value = dtpRequerido.Value = dtpEnvio.Value = DateTime.Now;
            dtpHoraPedido.Value = dtpHoraRequerido.Value = dtpHoraEnvio.Value = DateTime.Now;
            dtpRequerido.Checked = dtpEnvio.Checked = dtpHoraRequerido.Checked = dtpHoraEnvio.Checked = false;
            txtDirigidoa.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCP.Text = txtPais.Text = "";
            txtFlete.Text = txtPrecio.Text = "$0.00";
            txtCantidad.Text = "0";
            txtDescuento.Text = "0.00";
            txtTotal.Text = "$0.00";
            dgvDetalle.Rows.Clear();
        }

        private void BorrarMensajesError()
        {
            errorProvider1.SetError(cboProducto, "");
            errorProvider1.SetError(txtCantidad, "");
            errorProvider1.SetError(txtDescuento, "");
            errorProvider1.SetError(cboCliente, "");
            errorProvider1.SetError(cboEmpleado, "");
            errorProvider1.SetError(dtpPedido, "");
            errorProvider1.SetError(dtpHoraPedido, "");
            errorProvider1.SetError(cboTransportista, "");
            errorProvider1.SetError(btnAgregar, "");
        }

        private void BorrarDatosBusqueda()
        {
            txtBIdInicial.Text = txtBIdFinal.Text = txtBCliente.Text = txtBEmpleado.Text = txtBCompañiaT.Text = txtBDirigidoa.Text = "";
            dtpBFPedidoIni.Checked = dtpBFPedidoFin.Checked = dtpBFRequeridoIni.Checked = dtpBFRequeridoFin.Checked = dtpBFEnvioIni.Checked = dtpBFEnvioFin.Checked = false;
            chkBFPedidoNull.Checked = chkBFRequeridoNull.Checked = chkBFEnvioNull.Checked = false;
        }

        private void txtBIdInicial_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtBIdFinal_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtBIdInicial_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdIni(txtBIdInicial, txtBIdFinal);
        }

        private void txtBIdFinal_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdFin(txtBIdInicial, txtBIdFinal);
        }

        private void dtpBFPedidoIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked)
            {
                dtpBFPedidoFin.Checked = true;
                chkBFPedidoNull.Checked = false;
            }
            else
                dtpBFPedidoFin.Checked = false;
            dtpBFPedidoFin.Value = dtpBFPedidoIni.Value;
        }

        private void dtpBFPedidoFin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFPedidoFin.Checked)
            {
                dtpBFPedidoIni.Checked = true;
                chkBFPedidoNull.Checked = false;
            }
            else
                dtpBFPedidoIni.Checked = false;
        }

        private void dtpBFRequeridoIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFRequeridoIni.Checked)
            {
                dtpBFRequeridoFin.Checked = true;
                chkBFRequeridoNull.Checked = false;
            }
            else
                dtpBFRequeridoFin.Checked = false;
            dtpBFRequeridoFin.Value = dtpBFRequeridoIni.Value;
        }

        private void dtpBFRequeridoFin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFRequeridoFin.Checked)
            {
                dtpBFRequeridoIni.Checked = true;
                chkBFRequeridoNull.Checked = false;
            }
            else
                dtpBFRequeridoIni.Checked = false;
        }

        private void dtpBFEnvioIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFEnvioIni.Checked)
            {
                dtpBFEnvioFin.Checked = true;
                chkBFEnvioNull.Checked = false;
            }
            else
                dtpBFEnvioFin.Checked = false;
            dtpBFEnvioFin.Value = dtpBFEnvioIni.Value;
        }

        private void dtpBFEnvioFin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFEnvioFin.Checked)
            {
                dtpBFEnvioIni.Checked = true;
                chkBFEnvioNull.Checked = false;
            }
            else
                dtpBFEnvioIni.Checked = false;
        }

        private void chkBFPedidoNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBFPedidoNull.Checked)
            {
                dtpBFPedidoIni.Checked = false;
                dtpBFPedidoFin.Checked = false;
            }
        }

        private void chkBFRequeridoNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBFRequeridoNull.Checked)
            {
                dtpBFRequeridoIni.Checked = false;
                dtpBFRequeridoFin.Checked = false;
            }
        }

        private void chkBFEnvioNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBFEnvioNull.Checked)
            {
                dtpBFEnvioIni.Checked = false;
                dtpBFEnvioFin.Checked = false;
            }
        }
    }
}
