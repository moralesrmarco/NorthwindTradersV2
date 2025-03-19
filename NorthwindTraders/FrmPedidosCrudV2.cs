using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmPedidosCrudV2 : Form
    {
        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        private TabPage lastSelectedTab;
        bool EventoCargardo = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar
        int IdDetalle = 1;
        bool PedidoGenerado = false;

        public FrmPedidosCrudV2()
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

        private void FrmPedidosCrudV2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void FrmPedidosCrudV2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab == tabpRegistrar)
            {
                if (cboCliente.SelectedIndex > 0 || cboEmpleado.SelectedIndex > 0 || cboTransportista.SelectedIndex > 0 || cboCategoria.SelectedIndex > 0 || cboProducto.SelectedIndex > 0 || dgvDetalle.RowCount > 0)
                {
                    DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (respuesta == DialogResult.No)
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }
            }
        }

        private void FrmPedidosCrudV2_Load(object sender, EventArgs e)
        {
            dtpHoraRequerido.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpHoraEnvio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DeshabilitarControles();
            DeshabilitarControlesProducto();
            Utils.LlenarCbo(this, cboCliente, "Sp_Clientes_Seleccionar", "Cliente", "Id", cn);
            Utils.LlenarCbo(this, cboEmpleado, "Sp_Empleados_Seleccionar", "Empleado", "Id", cn);
            Utils.LlenarCbo(this, cboTransportista, "Sp_Transportistas_Seleccionar", "Transportista", "Id", cn);
            Utils.LlenarCbo(this, cboCategoria, "Sp_Categorias_Seleccionar", "Categoria", "Id", cn);
            Utils.ConfDgv(dgvPedidos);
            Utils.ConfDgv(dgvDetalle);
            LlenarDgvPedidos(null);
            ConfDgvPedidos();
            ConfDgvDetalle();
            OcultarCols();
            InicializarValores();
        }

        private void DeshabilitarControles()
        {
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = false;
            dtpPedido.Enabled = dtpHoraPedido.Enabled = dtpRequerido.Enabled = dtpHoraRequerido.Enabled = dtpEnvio.Enabled = dtpHoraEnvio.Enabled = false;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = true;
            btnGenerar.Enabled = false;
        }

        private void HabilitarControles()
        {
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = true;
            dtpPedido.Enabled = dtpRequerido.Enabled = dtpEnvio.Enabled = true;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = false;
            btnGenerar.Enabled = true;
        }

        private void DeshabilitarControlesProducto()
        {
            cboCategoria.Enabled = cboProducto.Enabled = false;
            txtCantidad.Enabled = txtDescuento.Enabled = false;
            btnAgregar.Enabled = false;
        }

        private void HabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = true;
            btnAgregar.Enabled = true;
        }

        private bool ValidarControles()
        {
            errorProvider1.Clear();
            bool valida = true;
            if (cboCliente.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboCliente, "Ingrese el cliente");
            }
            if (cboEmpleado.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboEmpleado, "Ingrese el empleado");
            }
            if (dtpPedido.Checked == false)
            {
                valida = false;
                errorProvider1.SetError(dtpPedido, "Ingrese la fecha de pedido");
            }
            if (cboTransportista.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboTransportista, "Ingrese la compañía transportista");
            }
            if (cboProducto.SelectedIndex > 0) 
            {
                valida = false;
                errorProvider1.SetError(cboProducto, "Ha seleccionado un producto y no lo ha agregado al pedido");
            }
            string total = txtTotal.Text.Replace("$", "");
            if (txtTotal.Text == "" || decimal.Parse(total) == 0)
            {
                valida = false;
                errorProvider1.SetError(btnAgregar, "Ingrese el detalle del pedido");
            }
            return valida;
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
                    cmd.Parameters.AddWithValue("FPedidoNull", chkBFPedidoNull.Checked);
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
                    cmd.Parameters.AddWithValue("FRequeridoNull", chkBFRequeridoNull.Checked);
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
                    cmd.Parameters.AddWithValue("FEnvioNull", chkBFEnvioNull.Checked);
                    cmd.Parameters.AddWithValue("Empleado", txtBEmpleado.Text);
                    cmd.Parameters.AddWithValue("CompañiaT", txtBCompañiaT.Text);
                    cmd.Parameters.AddWithValue("Dirigidoa", txtBDirigidoa.Text);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                dgvPedidos.DataSource = dt;
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

        private void OcultarCols()
        {
            dgvDetalle.Columns["Modificar"].Visible = dgvDetalle.Columns["Eliminar"].Visible = false;
        }

        private void MostrarCols()
        {
            dgvDetalle.Columns["Modificar"].Visible = dgvDetalle.Columns["Eliminar"].Visible = true;
        }

        private void InicializarValores()
        {
            txtFlete.Text = txtTotal.Text = txtPrecio.Text = "$0.00";
            txtDescuento.Text = "0.00";
            txtUInventario.Text = txtCantidad.Text = "0";
        }

        private void InicializarValoresProducto()
        {
            txtPrecio.Text = "$0.00";
            txtUInventario.Text = txtCantidad.Text = "0";
            txtDescuento.Text = "0.00";
        }

        private void InicializarValoresTransportar() => txtDirigidoa.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCP.Text = txtPais.Text = "";

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            BorrarDatosBusqueda();
            if (tabcOperacion.SelectedTab != tabpRegistrar)
            {
                DeshabilitarControles();
                DeshabilitarControlesProducto();
            }
            btnNota.Enabled = false;
            dgvPedidos.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tabpRegistrar)
            {
                DeshabilitarControles();
                DeshabilitarControlesProducto();
            }
            btnNota.Enabled=false;
            LlenarDgvPedidos(sender);
            dgvPedidos.Focus();
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            btnLimpiar.PerformClick();
            LlenarDgvPedidos(null);
            dgvPedidos.Focus();
        }

        private void BorrarDatosPedido()
        {
            txtId.Text = "";
            cboCliente.SelectedIndex = cboEmpleado.SelectedIndex = cboTransportista.SelectedIndex = cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            dtpPedido.Value = dtpRequerido.Value = dtpEnvio.Value = DateTime.Now;
            dtpHoraPedido.Value = DateTime.Now;
            dtpHoraRequerido.Value = dtpHoraEnvio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpRequerido.Checked = dtpEnvio.Checked = false;
            txtDirigidoa.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCP.Text = txtPais.Text = "";
            InicializarValores();
            btnNota.Visible = false;
            dgvDetalle.Rows.Clear();
        }

        private void BorrarMensajesError()
        {
            errorProvider1.Clear();
        }

        private void BorrarDatosBusqueda()
        {
            txtBIdInicial.Text = txtBIdFinal.Text = txtBCliente.Text = txtBEmpleado.Text = txtBCompañiaT.Text = txtBDirigidoa.Text = "";
            dtpBFPedidoIni.Value = dtpBFPedidoFin.Value = dtpBFRequeridoIni.Value = dtpBFRequeridoFin.Value = dtpBFEnvioIni.Value = dtpBFEnvioFin.Value = DateTime.Today;
            dtpBFPedidoIni.Checked = dtpBFPedidoFin.Checked = dtpBFRequeridoIni.Checked = dtpBFRequeridoFin.Checked = dtpBFEnvioIni.Checked = dtpBFEnvioFin.Checked = false;
            chkBFPedidoNull.Checked = chkBFRequeridoNull.Checked = chkBFEnvioNull.Checked = false;
        }

        private void txtBIdInicial_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdFinal_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdInicial_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtBIdInicial, txtBIdFinal);

        private void txtBIdFinal_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtBIdInicial, txtBIdFinal);

        private void dtpBFPedidoIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked)
            {
                dtpBFPedidoFin.Checked = true;
                chkBFPedidoNull.Checked = false;
            }
            else
                dtpBFPedidoFin.Checked = false;
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

        private void dtpBFPedidoIni_Leave(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked)
                if (dtpBFPedidoFin.Value < dtpBFPedidoIni.Value)
                    dtpBFPedidoFin.Value = dtpBFPedidoIni.Value;
        }

        private void dtpBFPedidoFin_Leave(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked)
                if (dtpBFPedidoFin.Value < dtpBFPedidoIni.Value)
                    dtpBFPedidoIni.Value = dtpBFPedidoFin.Value;
        }

        private void dtpBFRequeridoIni_Leave(object sender, EventArgs e)
        {
            if (dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked)
                if (dtpBFRequeridoFin.Value < dtpBFRequeridoIni.Value)
                    dtpBFRequeridoFin.Value = dtpBFRequeridoIni.Value;
        }

        private void dtpBFRequeridoFin_Leave(object sender, EventArgs e)
        {
            if (dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked)
                if (dtpBFRequeridoFin.Value < dtpBFRequeridoIni.Value)
                    dtpBFRequeridoIni.Value = dtpBFRequeridoFin.Value;
        }

        private void dtpBFEnvioIni_Leave(object sender, EventArgs e)
        {
            if (dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked)
                if (dtpBFEnvioFin.Value < dtpBFEnvioIni.Value)
                    dtpBFEnvioFin.Value = dtpBFEnvioIni.Value;
        }

        private void dtpBFEnvioFin_Leave(object sender, EventArgs e)
        {
            if (dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked)
                if (dtpBFEnvioFin.Value < dtpBFEnvioIni.Value)
                    dtpBFEnvioIni.Value = dtpBFEnvioFin.Value;
        }

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            InicializarValoresProducto();
            if (cboCategoria.SelectedIndex > 0)
            {
                Utils.LlenarCbo(this, cboProducto, "Sp_Productos_Seleccionar", "Producto", "Id", cn, "Categoria", cboCategoria.SelectedValue);
                cboProducto.Enabled = true;
            }
            else
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                DataTable tbl = new DataTable();
                tbl.Columns.Add("Id", typeof(int));
                tbl.Columns.Add("Producto", typeof(string));
                DataRow dr = tbl.NewRow();
                dr["Id"] = 0;
                dr["Producto"] = "«--- Seleccione ---»";
                tbl.Rows.Add(dr);
                cboProducto.DataSource = tbl;
                cboProducto.DisplayMember = "Producto";
                cboProducto.ValueMember = "Id";
            }
            Utils.ActualizarBarraDeEstado(this, $"Se muestran {dgvPedidos.RowCount} registros en pedidos");
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex > 0)
            {
                try
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                    SqlCommand cmd = new SqlCommand($"Select Top 1 ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry From Orders Where CustomerId = '{cboCliente.SelectedValue}' Order by OrderId Desc", cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (rdr.Read())
                    {
                        txtDirigidoa.Text = (rdr["ShipName"] == DBNull.Value) ? "" : rdr.GetString(rdr.GetOrdinal("ShipName"));
                        txtDomicilio.Text = (rdr["ShipAddress"] == DBNull.Value) ? "" : rdr.GetString(rdr.GetOrdinal("ShipAddress"));
                        txtCiudad.Text = (rdr["ShipCity"] == DBNull.Value) ? "" : rdr.GetString(rdr.GetOrdinal("ShipCity"));
                        txtRegion.Text = (rdr["ShipRegion"] == DBNull.Value) ? "" : rdr.GetString(rdr.GetOrdinal("ShipRegion"));
                        txtCP.Text = (rdr["ShipPostalCode"] == DBNull.Value) ? "" : rdr.GetString(rdr.GetOrdinal("ShipPostalCode"));
                        txtPais.Text = (rdr["ShipCountry"] == DBNull.Value) ? "" : rdr.GetString(rdr.GetOrdinal("ShipCountry"));
                    }
                    else
                        InicializarValoresTransportar();
                    rdr.Close();
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran {dgvPedidos.RowCount} registros en pedidos");
                }
                catch (SqlException ex)
                {
                    Utils.MsgCatchOueclbdd(this, ex);
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(this, ex);
                }
                finally
                {
                    cn.Close();
                }
            }
            else
                InicializarValoresTransportar();
        }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProducto.SelectedIndex > 0)
            {
                try
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                    InicializarValoresProducto();
                    SqlCommand cmd = new SqlCommand($"Select UnitPrice, UnitsInStock From Products Where ProductId = {cboProducto.SelectedValue}", cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (rdr.Read())
                    {
                        txtPrecio.Text = rdr["UnitPrice"] == DBNull.Value ? "$0.00" : rdr.GetDecimal(rdr.GetOrdinal("UnitPrice")).ToString("c");
                        txtUInventario.Text = rdr["UnitsInStock"] == DBNull.Value ? "0" : rdr.GetInt16(rdr.GetOrdinal("UnitsInStock")).ToString();
                        if (int.Parse(txtUInventario.Text) == 0)
                        {
                            txtCantidad.Leave -= new EventHandler(txtCantidad_Leave);
                            txtCantidad.Validating -= new CancelEventHandler(txtCantidad_Validating);
                            DeshabilitarControlesProducto();
                            txtCantidad.Leave += new EventHandler(txtCantidad_Leave);
                            txtCantidad.Validating += new CancelEventHandler(txtCantidad_Validating);
                            MessageBox.Show("No hay este producto en existencia", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            InicializarValoresProducto();
                            BorrarMensajesError();
                            cboCategoria.Enabled = true;
                            cboProducto.SelectedIndex = 0;
                        }
                        else
                            HabilitarControlesProducto();
                    }
                    else
                    {
                        DeshabilitarControlesProducto();
                        InicializarValoresProducto();
                    }
                    rdr.Close();
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran {dgvPedidos.RowCount} registros en pedidos");
                }
                catch (SqlException ex)
                {
                    Utils.MsgCatchOueclbdd(this, ex);
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(this, ex);
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                DeshabilitarControlesProducto();
                cboCategoria.Enabled = true;
                InicializarValoresProducto();
            }
        }

        private void CalcularTotal()
        {
            decimal total = 0, importe = 0;
            foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
            {
                importe = decimal.Parse(dgvr.Cells["Importe"].Value.ToString());
                total += importe;
            }
            txtTotal.Text = string.Format("{0:c}", total);
        }

        private void txtDescuento_Enter(object sender, EventArgs e)
        {
            txtDescuento.Text = "";
        }

        private void txtDescuento_Leave(object sender, EventArgs e)
        {
            if (txtDescuento.Text == "")
                txtDescuento.Text = "0.00";
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            if (txtCantidad.Text == "" || int.Parse(txtCantidad.Text) == 0) txtCantidad.Text = "1";
        }

        private void txtFlete_Enter(object sender, EventArgs e)
        {
            if (txtFlete.Text.Contains("$")) txtFlete.Text = txtFlete.Text.Replace("$", "");
            if (decimal.Parse(txtFlete.Text) == 0) txtFlete.Text = "";
        }

        private void txtFlete_Leave(object sender, EventArgs e)
        {
            if (txtFlete.Text == "") txtFlete.Text = "0.00";
            decimal flete = decimal.Parse(txtFlete.Text);
            txtFlete.Text = flete.ToString("c");
        }

        private void dtpPedido_ValueChanged(object sender, EventArgs e)
        {
            if (dtpPedido.Checked)
            {
                dtpHoraPedido.Value = DateTime.Now; // este es para que me ponga el componente del time
                dtpHoraPedido.Enabled = true;
            }
            else
            {
                dtpHoraPedido.Value = DateTime.Today; // este es para que no me ponga el componente del time
                dtpHoraPedido.Enabled = false;
            }
        }

        private void dtpRequerido_ValueChanged(object sender, EventArgs e)
        {
            if (dtpRequerido.Checked)
            {
                dtpHoraRequerido.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " 12:00:00.000");
                dtpHoraRequerido.Enabled = true;
            }
            else
            {
                dtpHoraRequerido.Value = DateTime.Today;
                dtpHoraRequerido.Enabled = false;
            }
        }

        private void dtpEnvio_ValueChanged(object sender, EventArgs e)
        {
            if (dtpEnvio.Checked)
            {
                dtpHoraEnvio.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " 12:00:00.000");
                dtpHoraEnvio.Enabled = true;
            }
            else
            {
                dtpHoraEnvio.Value = DateTime.Today;
                dtpHoraEnvio.Enabled = false;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            if (cboCategoria.SelectedIndex <= 0)
            {
                errorProvider1.SetError(cboCategoria, "Seleccione la categoría");
                return;
            }
            if (cboProducto.SelectedIndex <= 0)
            {
                errorProvider1.SetError(cboProducto, "Seleccione el producto");
                return;
            }
            if (txtCantidad.Text == "" || int.Parse(txtCantidad.Text) == 0)
            {
                errorProvider1.SetError(txtCantidad, "Ingrese la cantidad");
                return;
            }
            if (decimal.Parse(txtDescuento.Text) > 1 || decimal.Parse(txtDescuento.Text) < 0)
            {
                errorProvider1.SetError(txtDescuento, "El descuento no puede ser mayor que 1 o menor que 0");
                return;
            }
            if (int.Parse(txtCantidad.Text) > int.Parse(txtUInventario.Text))
            {
                errorProvider1.SetError(txtCantidad, "La cantidad de productos en el pedido excede el inventario disponible");
                return;
            }
            int numProd = int.Parse(cboProducto.SelectedValue.ToString());
            bool productoDuplicado = false;
            foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
            {
                if (int.Parse(dgvr.Cells["ProductoId"].Value.ToString()) == numProd)
                {
                    productoDuplicado = true;
                    break;
                }
            }
            if (productoDuplicado)
            {
                errorProvider1.SetError(cboProducto, "No se puede tener un producto duplicado en el detalle del pedido");
                return;
            }
            string total = txtTotal.Text.Replace("$", "");
            if (txtTotal.Text == "" || (decimal.Parse(total) + (decimal.Parse(txtPrecio.Text.Replace("$", "")) * int.Parse(txtCantidad.Text) * (1 - decimal.Parse(txtDescuento.Text))) == 0))
            {
                errorProvider1.SetError(btnAgregar, "Ingrese el detalle del pedido");
                return;
            }
            DeshabilitarControlesProducto();
            if (tabcOperacion.SelectedTab == tabpRegistrar & !PedidoGenerado)
            {
                txtPrecio.Text = txtPrecio.Text.Replace("$", "");
                dgvDetalle.Rows.Add(new object[] { IdDetalle, cboProducto.Text, txtPrecio.Text, txtCantidad.Text, txtDescuento.Text, ((decimal.Parse(txtPrecio.Text) * decimal.Parse(txtCantidad.Text)) * (1 - decimal.Parse(txtDescuento.Text))).ToString(), "Modificar", "Eliminar", cboProducto.SelectedValue, txtUInventario.Text });
                CalcularTotal();
                ++IdDetalle;
                cboCategoria.SelectedIndex = cboProducto.SelectedIndex = 0;
                InicializarValoresProducto();
                cboCategoria.Focus();
            }
            else if (tabcOperacion.SelectedTab == tabpModificar | (tabcOperacion.SelectedTab == tabpRegistrar & PedidoGenerado))
            {
                byte numRegs = 0;
                try
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.insertandoRegistro);
                    PedidoDetalle pedidoDetalle = new PedidoDetalle();
                    pedidoDetalle.ProductId = (int)cboProducto.SelectedValue;
                    pedidoDetalle.UnitPrice = decimal.Parse(txtPrecio.Text.Replace("$", ""));
                    pedidoDetalle.Quantity = short.Parse(txtCantidad.Text);
                    pedidoDetalle.Discount = decimal.Parse(txtDescuento.Text);
                    pedidoDetalle.ProductName = cboProducto.Text;
                    PedidoDetalleDB pedidoDetalleDB = new PedidoDetalleDB();
                    pedidoDetalleDB.PedidoId = int.Parse(txtId.Text);
                    numRegs = pedidoDetalleDB.Add(pedidoDetalle);
                    if (numRegs > 0)
                        MessageBox.Show($"El producto: {pedidoDetalle.ProductName} del Pedido: {pedidoDetalleDB.PedidoId}, se registró satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show($"El producto: {pedidoDetalle.ProductName} del Pedido: {pedidoDetalleDB.PedidoId}, NO se registró en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran {dgvPedidos.RowCount} registros de pedidos");
                }
                catch (SqlException ex) when (ex.Number == 2627)
                {
                    MessageBox.Show($"Error, ya existe un registro del producto {cboProducto.Text} en la base de datos, modifique la cantidad del producto de ese registro", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BorrarDatosAgregarProducto();
                    HabilitarControlesProducto();
                }
                catch (SqlException ex)
                {
                    Utils.MsgCatchOueclbdd(this, ex);
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(this, ex);
                }
                if (numRegs > 0)
                {
                    BorrarDatosDetallePedido();
                    LlenarDatosDetallePedido();
                    cboCategoria.Enabled = true;
                    btnAgregar.Enabled = true;
                    btnNota.Enabled = true;
                    btnNota.Visible = true;
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran {dgvPedidos.RowCount} registros de pedidos");
                    dgvDetalle.Focus();
                }
            }
        }

        private void dgvDetalle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("c");
            if (e.ColumnIndex == 3 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("n0");
            if (e.ColumnIndex == 4 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("n2");
            if (e.ColumnIndex == 5 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("c");
        }

        private void txtFlete_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosConPunto(sender, e);
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosConPunto(sender, e);
        }

        private void txtCantidad_Validating(object sender, CancelEventArgs e)
        {
            if (txtCantidad.Text != "")
            {
                if (int.Parse(txtCantidad.Text.Replace(",", "")) > 32767)
                {
                    errorProvider1.SetError(txtCantidad, "La cantidad no puede ser mayor a 32,767");
                    e.Cancel = true;
                    return;
                }
                else
                    errorProvider1.SetError(txtCantidad, "");
                if (int.Parse(txtCantidad.Text) > int.Parse(txtUInventario.Text))
                {
                    errorProvider1.SetError(txtCantidad, "La cantidad de productos en el pedido excede el inventario disponible");
                    e.Cancel = true;
                }
            }
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            lastSelectedTab = e.TabPage; // actualizar la pestaña actual
            IdDetalle = 1;
            BorrarDatosPedido();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tabpRegistrar)
            {
                if (EventoCargardo)
                {
                    dgvPedidos.CellClick -= new DataGridViewCellEventHandler(dgvPedidos_CellClick);
                    EventoCargardo = false;
                }
                PedidoGenerado = false;
                BorrarDatosBusqueda();
                HabilitarControles();
                cboCategoria.Enabled = true;
                btnGenerar.Text = "Generar pedido";
                btnGenerar.Visible = true;
                btnGenerar.Enabled = true;
                btnAgregar.Visible = true;
                btnAgregar.Enabled = true;
                dgvDetalle.Columns["Eliminar"].Visible = true;
                dgvDetalle.Columns["Modificar"].Visible = true;
                grbProducto.Enabled = true;
                btnNota.Visible = true;
                btnNota.Enabled = false;
                btnNuevo.Visible = true;
                btnNuevo.Enabled = false;
            }
            else
            {
                if (!EventoCargardo)
                {
                    dgvPedidos.CellClick += new DataGridViewCellEventHandler(dgvPedidos_CellClick);
                    EventoCargardo = true;
                }
                DeshabilitarControles();
                DeshabilitarControlesProducto();
                OcultarCols();
                if (tabcOperacion.SelectedTab == tabpConsultar)
                {
                    btnGenerar.Visible = false;
                    btnAgregar.Visible = false;
                    btnNota.Visible = true;
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                    btnNuevo.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tabpModificar)
                {
                    PedidoGenerado = false;
                    btnGenerar.Text = "Modificar pedido";
                    btnGenerar.Visible = true;
                    btnAgregar.Visible = true;
                    btnNota.Visible = true;
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                    btnNuevo.Enabled = false;
                    MostrarCols();
                }
                else if (tabcOperacion.SelectedTab == tabpEliminar)
                {
                    btnGenerar.Text = "Eliminar pedido";
                    btnGenerar.Visible = true;
                    btnAgregar.Visible = false;
                    btnNota.Visible = false;
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                    btnNuevo.Enabled = false;
                }
            }
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnNota.Enabled=false;
            if (tabcOperacion.SelectedTab != tabpRegistrar)
            {
                BorrarDatosPedido();
                DataGridViewRow dgvr = dgvPedidos.CurrentRow;
                txtId.Text = dgvr.Cells["Id"].Value.ToString();
                LlenarDatosPedido();
                LlenarDatosDetallePedido();
                DeshabilitarControles();
                DeshabilitarControlesProducto();
                if (tabcOperacion.SelectedTab == tabpConsultar)
                {
                    btnNota.Visible = true;
                    btnNota.Enabled = true;
                    btnNuevo.Visible = false;
                }
                else if (tabcOperacion.SelectedTab == tabpModificar)
                {
                    HabilitarControles();
                    btnGenerar.Enabled = true;
                    cboCategoria.Enabled = true;
                    btnNota.Visible = true;
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                }
                else if (tabcOperacion.SelectedTab == tabpEliminar)
                {
                    btnGenerar.Enabled = true;
                    btnNota.Visible = false;
                    btnNuevo .Visible = false;
                }
            }
        }

        private void LlenarDatosPedido()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand("Sp_Pedidos_Listar1", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("PedidoId", txtId.Text);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (rdr.Read())
                {
                    cboCliente.SelectedIndexChanged -= new EventHandler(cboCliente_SelectedIndexChanged);
                    cboCliente.SelectedValue = rdr["CustomerId"] == DBNull.Value ? 0 : rdr["CustomerId"];
                    cboCliente.SelectedIndexChanged += new EventHandler(cboCliente_SelectedIndexChanged);
                    cboEmpleado.SelectedValue = rdr["EmployeeId"] == DBNull.Value ? 0 : rdr["EmployeeId"];
                    cboTransportista.SelectedValue = rdr["ShipVia"] == DBNull.Value ? 0 : rdr["ShipVia"];
                    txtDirigidoa.Text = rdr["ShipName"] == DBNull.Value ? "" : rdr["ShipName"].ToString();
                    txtDomicilio.Text = rdr["ShipAddress"] == DBNull.Value ? "" : rdr["ShipAddress"].ToString();
                    txtCiudad.Text = rdr["ShipCity"] == DBNull.Value ? "" : rdr["ShipCity"].ToString();
                    txtRegion.Text = rdr["ShipRegion"] == DBNull.Value ? "" : rdr["ShipRegion"].ToString();
                    txtCP.Text = rdr["ShipPostalCode"] == DBNull.Value ? "" : rdr["ShipPostalCode"].ToString();
                    txtPais.Text = rdr["ShipCountry"] == DBNull.Value ? "" : rdr["ShipCountry"].ToString();
                    txtFlete.Text = rdr["Freight"] == DBNull.Value ? "" : rdr["Freight"].ToString();
                    if (decimal.TryParse(txtFlete.Text, out decimal flete))
                        txtFlete.Text = flete.ToString("c");
                    DateTime fecha;
                    if (DateTime.TryParse(rdr["OrderDate"].ToString(), out fecha))
                    {
                        dtpPedido.Value = fecha;
                        dtpHoraPedido.Value = fecha;
                    }
                    else
                    {
                        dtpPedido.Value = dtpPedido.MinDate;
                        dtpPedido.Checked = false;
                        dtpHoraPedido.Value = dtpHoraPedido.MinDate;
                    }
                    if (DateTime.TryParse(rdr["RequiredDate"].ToString(), out fecha))
                    {
                        dtpRequerido.Value = fecha;
                        dtpHoraRequerido.Value = fecha;
                    }
                    else
                    {
                        dtpRequerido.Value = dtpRequerido.MinDate;
                        dtpRequerido.Checked = false;
                        dtpHoraRequerido.Value = dtpHoraRequerido.MinDate;
                    }
                    if (DateTime.TryParse(rdr["ShippedDate"].ToString(), out fecha))
                    {
                        dtpEnvio.Value = fecha;
                        dtpHoraEnvio.Value = fecha;
                    }
                    else
                    {
                        dtpEnvio.Value = dtpEnvio.MinDate;
                        dtpEnvio.Checked = false;
                        dtpHoraEnvio.Value = dtpHoraEnvio.MinDate;
                    }
                }
                else
                {
                    MessageBox.Show($"El pedido: {txtId.Text} fue eliminado por otro usuario de la red", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                rdr.Close();
                Utils.ActualizarBarraDeEstado(this, $"Se muestran {dgvPedidos.RowCount} registros en pedidos");
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(this, ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(this, ex);
            }
            finally
            {
                cn.Close();
            }
        }

        private void LlenarDatosDetallePedido()
        {
            try
            {
                IdDetalle = 1;
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand("Sp_DetallePedidos_Productos_Listar1", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("PedidoId", txtId.Text);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                PedidoDetalle pedidoDetalle;
                if (rdr.Read())
                {
                    do
                    {
                        pedidoDetalle = new PedidoDetalle();
                        pedidoDetalle.ProductId = (int)rdr["Id Producto"];
                        pedidoDetalle.ProductName = rdr["Producto"].ToString();
                        pedidoDetalle.UnitPrice = (decimal)rdr["Precio"];
                        pedidoDetalle.Quantity = (short)rdr["Cantidad"];
                        pedidoDetalle.Discount = decimal.Parse(rdr["Descuento"].ToString());
                        dgvDetalle.Rows.Add(new object[] { IdDetalle, pedidoDetalle.ProductName, pedidoDetalle.UnitPrice, pedidoDetalle.Quantity, pedidoDetalle.Discount, (pedidoDetalle.UnitPrice * pedidoDetalle.Quantity) * (1 - pedidoDetalle.Discount), "Modificar", "Eliminar", pedidoDetalle.ProductId });
                        ++IdDetalle;
                    } while (rdr.Read());
                }
                else
                    MessageBox.Show("No se encontraron detalles para el pedido especificado", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                rdr.Close();
                CalcularTotal();
                Utils.ActualizarBarraDeEstado(this, $"Se muestran {dgvPedidos.RowCount} registros en pedidos");
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(this, ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(this, ex);
            }
            finally
            {
                cn.Close();
            }
        }

        // se anidan las clases para evitar interfieran con otro código similar del sistema, solo son accesibles desde su tipo contenedor
        private class PedidoDetalle
        {
            public int ProductId { get; set; }
            public decimal UnitPrice { get; set; }
            public short Quantity { get; set; }
            public decimal Discount { get; set; }
            public string ProductName { get; set; }
        }

        private class Pedido
        {
            public int OrderId { get; set; }
            public string CustomerId { get; set; }
            public int EmployeeId { get; set; }
            public DateTime? OrderDate { get; set; }
            public DateTime? RequiredDate { get; set; }
            public DateTime? ShippedDate { get; set; }
            public int ShipVia { get; set; }
            public decimal Freight { get; set; }
            public string ShipName { get; set; }
            public string ShipAddress { get; set; }
            public string ShipCity { get; set; }
            public string ShipRegion { get; set; }
            public string ShipPostalCode { get; set; }
            public string ShipCountry { get; set; }
        }

        private class PedidosDB
        {
            public int PedidoId { get; set; }

            public byte Add(Pedido pedido, List<PedidoDetalle> lst, TextBox textBox, string cliente)
            {
                // las excepciones generadas en este segmento de código son capturadas en un nivel superior, por eso no uso bloque try
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Id");
                    dt.Columns.Add("ProductId");
                    dt.Columns.Add("UnitPrice");
                    dt.Columns.Add("Quantity");
                    dt.Columns.Add("Discount");
                    int i = 1;
                    foreach (var item in lst)
                    {
                        dt.Rows.Add(i, item.ProductId, item.UnitPrice, item.Quantity, item.Discount);
                        ++i;
                    }
                    using (SqlCommand cmd = new SqlCommand("Sp_Pedidos_Insertar_v2", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("OrderId", 0);
                        cmd.Parameters["OrderId"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("CustomerId", pedido.CustomerId);
                        cmd.Parameters.AddWithValue("EmployeeId", pedido.EmployeeId);
                        if (pedido.OrderDate == null) cmd.Parameters.AddWithValue("OrderDate", DBNull.Value);
                        else cmd.Parameters.AddWithValue("OrderDate", pedido.OrderDate);
                        if (pedido.RequiredDate == null) cmd.Parameters.AddWithValue("RequiredDate", DBNull.Value);
                        else cmd.Parameters.AddWithValue("RequiredDate", pedido.RequiredDate);
                        if (pedido.ShippedDate == null) cmd.Parameters.AddWithValue("ShippedDate", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShippedDate", pedido.ShippedDate);
                        cmd.Parameters.AddWithValue("ShipVia", pedido.ShipVia);
                        cmd.Parameters.AddWithValue("Freight", pedido.Freight);
                        if (pedido.ShipName.Trim() == "") cmd.Parameters.AddWithValue("ShipName", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipName", pedido.ShipName);
                        if (pedido.ShipAddress.Trim() == "") cmd.Parameters.AddWithValue("ShipAddress", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipAddress", pedido.ShipAddress);
                        if (pedido.ShipCity.Trim() == "") cmd.Parameters.AddWithValue("ShipCity", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipCity", pedido.ShipCity);
                        if (pedido.ShipRegion.Trim() == "") cmd.Parameters.AddWithValue("ShipRegion", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipRegion", pedido.ShipRegion);
                        if (pedido.ShipPostalCode.Trim() == "") cmd.Parameters.AddWithValue("ShipPostalCode", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipPostalCode", pedido.ShipPostalCode);
                        if (pedido.ShipCountry.Trim() == "") cmd.Parameters.AddWithValue("ShipCountry", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipCountry", pedido.ShipCountry);
                        var sqlParameter = new SqlParameter("lstOrderDetails", SqlDbType.Structured);
                        sqlParameter.TypeName = "dbo.OrderDetails";
                        sqlParameter.Value = dt;
                        cmd.Parameters.Add(sqlParameter);
                        cn.Open();
                        numRegs = (byte)cmd.ExecuteNonQuery();
                        PedidoId = (int)cmd.Parameters["OrderId"].Value;
                        textBox.Text = PedidoId.ToString();
                        cn.Close();
                        if (numRegs > 0) MessageBox.Show($"El pedido con Id: {PedidoId} del Cliente: {cliente}, se registró satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                return numRegs;
            }

            public byte Update(Pedido pedido, string cliente)
            {
                // las excepciones generadas en este segmento de código son capturadas en un nivel superior, por eso no uso bloque try
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_Pedidos_Actualizar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("OrderId", pedido.OrderId);
                        cmd.Parameters.AddWithValue("CustomerId", pedido.CustomerId);
                        cmd.Parameters.AddWithValue("EmployeeId", pedido.EmployeeId);
                        if (pedido.OrderDate == null) cmd.Parameters.AddWithValue("OrderDate", DBNull.Value);
                        else cmd.Parameters.AddWithValue("OrderDate", pedido.OrderDate);
                        if (pedido.RequiredDate == null) cmd.Parameters.AddWithValue("RequiredDate", DBNull.Value);
                        else cmd.Parameters.AddWithValue("RequiredDate", pedido.RequiredDate);
                        if (pedido.ShippedDate == null) cmd.Parameters.AddWithValue("ShippedDate", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShippedDate", pedido.ShippedDate);
                        cmd.Parameters.AddWithValue("ShipVia", pedido.ShipVia);
                        cmd.Parameters.AddWithValue("Freight", pedido.Freight);
                        if (pedido.ShipName.Trim() == "") cmd.Parameters.AddWithValue("ShipName", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipName", pedido.ShipName);
                        if (pedido.ShipAddress.Trim() == "") cmd.Parameters.AddWithValue("ShipAddress", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipAddress", pedido.ShipAddress);
                        if (pedido.ShipCity.Trim() == "") cmd.Parameters.AddWithValue("ShipCity", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipCity", pedido.ShipCity);
                        if (pedido.ShipRegion.Trim() == "") cmd.Parameters.AddWithValue("ShipRegion", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipRegion", pedido.ShipRegion);
                        if (pedido.ShipPostalCode.Trim() == "") cmd.Parameters.AddWithValue("ShipPostalCode", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipPostalCode", pedido.ShipPostalCode);
                        if (pedido.ShipCountry.Trim() == "") cmd.Parameters.AddWithValue("ShipCountry", DBNull.Value);
                        else cmd.Parameters.AddWithValue("ShipCountry", pedido.ShipCountry);
                        cn.Open();
                        numRegs = (byte)cmd.ExecuteNonQuery();
                        cn.Close();
                        if (numRegs > 0) MessageBox.Show($"El pedido con Id: {pedido.OrderId} del Cliente: {cliente}, se actualizó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("No se pudo realizar la modificación, es posible que el registro se haya eliminado previamente por otro usuario de la red", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return numRegs;
            }

            public byte Delete(Pedido pedido, string cliente)
            {
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_Pedidos_Eliminar_V2", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("OrderId", pedido.OrderId);
                        cn.Open();
                        numRegs = (byte)cmd.ExecuteNonQuery();
                        cn.Close();
                        if (numRegs > 0)
                            MessageBox.Show($"El pedido con Id: {pedido.OrderId} del Cliente: {cliente}, se eliminó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("No se pudo realizar la eliminación, es posible que el registro haya sido eliminado previamente por otro usuario de la red", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return numRegs;
            }
        }

        private class PedidoDetalleDB
        {
            public int PedidoId { get; set; }

            public byte Add(PedidoDetalle pedidoDetalle)
            {
                // las excepciones generadas en este segmento de código son capturadas en un nivel superior, por eso no uso bloque try
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_PedidosDetalle_Insertar_V2", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("OrderId", PedidoId);
                        cmd.Parameters.AddWithValue("ProductId", pedidoDetalle.ProductId);
                        cmd.Parameters.AddWithValue("UnitPrice", pedidoDetalle.UnitPrice);
                        cmd.Parameters.AddWithValue("Quantity", pedidoDetalle.Quantity);
                        cmd.Parameters.AddWithValue("Discount", pedidoDetalle.Discount);
                        cn.Open();
                        numRegs = (byte)cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
                return numRegs;
            }

            public byte Delete(PedidoDetalle pedidoDetalle)
            {
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_PedidosDetalle_Eliminar_V2", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("OrderId", PedidoId);
                        cmd.Parameters.AddWithValue("ProductId", pedidoDetalle.ProductId);
                        cn.Open();
                        numRegs = (byte)cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
                return numRegs;
            }
        }

        private void tabcOperacion_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!PedidoGenerado & (lastSelectedTab == tabpRegistrar && e.TabPage != tabpRegistrar && dgvDetalle.RowCount > 0))
            {
                DialogResult respuesta = MessageBox.Show("Se han agregado productos al detalle del pedido, si cambia de pestaña se perderan los datos no guardados.\n¿Desea cambiar de pestaña?", Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            byte numRegs = 0;
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tabpRegistrar)
            {
                try
                {
                    if (ValidarControles())
                    {
                        Utils.ActualizarBarraDeEstado(this, Utils.insertandoRegistro);
                        DeshabilitarControles();
                        DeshabilitarControlesProducto();
                        List<PedidoDetalle> lstDetalle = new List<PedidoDetalle>();
                        // llenado de elementos hijos
                        foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
                        {
                            PedidoDetalle detalle = new PedidoDetalle();
                            detalle.ProductId = int.Parse(dgvr.Cells["ProductoId"].Value.ToString());
                            detalle.UnitPrice = decimal.Parse(dgvr.Cells["Precio"].Value.ToString());
                            detalle.Quantity = short.Parse(dgvr.Cells["Cantidad"].Value.ToString());
                            detalle.Discount = decimal.Parse(dgvr.Cells["Descuento"].Value.ToString());
                            lstDetalle.Add(detalle);
                        }
                        Pedido pedido = new Pedido();
                        pedido.CustomerId = cboCliente.SelectedValue.ToString();
                        pedido.EmployeeId = (int)cboEmpleado.SelectedValue;
                        if (!dtpPedido.Checked) pedido.OrderDate = null;
                        else pedido.OrderDate = Convert.ToDateTime(dtpPedido.Value.ToShortDateString() + " " + dtpHoraPedido.Value.ToLongTimeString());
                        if (!dtpRequerido.Checked) pedido.RequiredDate = null;
                        else pedido.RequiredDate = Convert.ToDateTime(dtpRequerido.Value.ToShortDateString() + " " + dtpHoraRequerido.Value.ToLongTimeString());
                        if (!dtpEnvio.Checked) pedido.ShippedDate = null;
                        else pedido.ShippedDate = Convert.ToDateTime(dtpEnvio.Value.ToShortDateString() + " " + dtpHoraEnvio.Value.ToLongTimeString());
                        pedido.ShipVia = (int)cboTransportista.SelectedValue;
                        pedido.ShipName = txtDirigidoa.Text;
                        pedido.ShipAddress = txtDomicilio.Text;
                        pedido.ShipCity = txtCiudad.Text;
                        pedido.ShipRegion = txtRegion.Text;
                        pedido.ShipPostalCode = txtCP.Text;
                        pedido.ShipCountry = txtPais.Text;
                        if (txtFlete.Text.Contains("$")) txtFlete.Text = txtFlete.Text.Replace("$", "");
                        pedido.Freight = decimal.Parse(txtFlete.Text);
                        PedidosDB pedidosDB = new PedidosDB();
                        numRegs = pedidosDB.Add(pedido, lstDetalle, txtId, cboCliente.Text);
                    }
                }
                catch (SqlException ex) when (ex.Number == 547)
                {
                    MessageBox.Show("Algún producto en el pedido fue previamente eliminado por otro usuario de la red.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (SqlException ex) when (ex.Number == 2627)
                {
                    MessageBox.Show($"Error, existe un producto duplicado en el pedido, elimine el producto duplicado y modifique la cantidad del producto", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (SqlException ex)
                {
                    Utils.MsgCatchOueclbdd(this, ex);
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(this, ex);
                }
                if (numRegs > 0)
                {
                    PedidoGenerado = true;
                    IdDetalle = 1;
                    btnNota.Enabled = true;
                    btnNota.Visible = true;
                    btnNuevo.Enabled = true;
                    btnNuevo.Visible = true;
                    cboCategoria.SelectedIndex = 0;
                    cboCategoria.Enabled = true;
                    BorrarDatosBusqueda();
                    LlenarDgvPedidos(null);
                }
            }
            else if (tabcOperacion.SelectedTab == tabpModificar)
            {
                try
                {
                    if (ValidarControles())
                    {
                        Utils.ActualizarBarraDeEstado(this, Utils.modificandoRegistro);
                        DeshabilitarControles();
                        DeshabilitarControlesProducto();
                        Pedido pedido = new Pedido();
                        pedido.OrderId = int.Parse(txtId.Text);
                        pedido.CustomerId = cboCliente.SelectedValue.ToString();
                        pedido.EmployeeId = (int)cboEmpleado.SelectedValue;
                        if (!dtpPedido.Checked) pedido.OrderDate = null;
                        else pedido.OrderDate = Convert.ToDateTime(dtpPedido.Value.ToShortDateString() + " " + dtpHoraPedido.Value.ToLongTimeString());
                        if (!dtpRequerido.Checked) pedido.RequiredDate = null;
                        else pedido.RequiredDate = Convert.ToDateTime(dtpRequerido.Value.ToShortDateString() + " " + dtpHoraRequerido.Value.ToLongTimeString());
                        if (!dtpEnvio.Checked) pedido.ShippedDate = null;
                        else pedido.ShippedDate = Convert.ToDateTime(dtpEnvio.Value.ToShortDateString() + " " + dtpHoraEnvio.Value.ToLongTimeString());
                        pedido.ShipVia = (int)cboTransportista.SelectedValue;
                        pedido.ShipName = txtDirigidoa.Text;
                        pedido.ShipAddress = txtDomicilio.Text;
                        pedido.ShipCity = txtCiudad.Text;
                        pedido.ShipRegion = txtRegion.Text;
                        pedido.ShipPostalCode = txtCP.Text;
                        pedido.ShipCountry = txtPais.Text;
                        if (txtFlete.Text.Contains("$")) txtFlete.Text = txtFlete.Text.Replace("$", "");
                        pedido.Freight = decimal.Parse(txtFlete.Text);
                        PedidosDB pedidosDB = new PedidosDB();
                        numRegs = pedidosDB.Update(pedido, cboCliente.Text);
                        if (numRegs > 0)
                        {
                            PedidoGenerado = true;
                            btnNota.Enabled = true;
                            btnNota.Visible = true;
                            btnNuevo.Visible = false;
                            cboCategoria.Enabled = true;
                            btnAgregar.Enabled = true;
                            LlenarDgvPedidos(null);
                        }
                    }
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
            else if (tabcOperacion.SelectedTab == tabpEliminar)
            {
                if (txtId.Text == "")
                {
                    MessageBox.Show("Seleccione el pedido a eliminar", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult respuesta = MessageBox.Show($"¿Esta seguro de eliminar el pedido con Id: {txtId.Text} del Cliente: {cboCliente.Text}?", Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Yes)
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.eliminandoRegistro);
                    btnGenerar.Enabled = false;
                    try
                    {
                        Pedido pedido = new Pedido();
                        pedido.OrderId = int.Parse(txtId.Text);
                        PedidosDB pedidosDB = new PedidosDB();
                        numRegs = pedidosDB.Delete(pedido, cboCliente.Text);
                    }
                    catch (SqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(this, ex);
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(this, ex);
                    }
                    if (numRegs > 0)
                    {
                        BorrarDatosBusqueda();
                        txtBIdInicial.Text = txtBIdFinal.Text = txtId.Text;
                        btnBuscar.PerformClick();
                        btnLimpiar.PerformClick();
                    }
                }
                else
                {
                    BorrarDatosPedido();
                    btnGenerar.Enabled = false;
                }
            }
        }

        private void dgvDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || (e.ColumnIndex != dgvDetalle.Columns["Eliminar"].Index & e.ColumnIndex != dgvDetalle.Columns["Modificar"].Index))
                return;
            if (!PedidoGenerado & e.ColumnIndex == dgvDetalle.Columns["Eliminar"].Index & tabcOperacion.SelectedTab == tabpRegistrar)
            {
                dgvDetalle.Rows.RemoveAt(e.RowIndex);
                CalcularTotal();
            }
            if (!PedidoGenerado & e.ColumnIndex == dgvDetalle.Columns["Modificar"].Index & tabcOperacion.SelectedTab == tabpRegistrar)
            {
                DataGridViewRow dgvr = dgvDetalle.CurrentRow;
                using (FrmPedidosDetalleModificar2 frmPedidosDetalleModificar2 = new FrmPedidosDetalleModificar2())
                {
                    frmPedidosDetalleModificar2.Owner = this;
                    frmPedidosDetalleModificar2.ProductoId = int.Parse(dgvr.Cells["ProductoId"].Value.ToString());
                    frmPedidosDetalleModificar2.Producto = dgvr.Cells["Producto"].Value.ToString();
                    frmPedidosDetalleModificar2.Precio = float.Parse(dgvr.Cells["Precio"].Value.ToString());
                    frmPedidosDetalleModificar2.Cantidad = short.Parse(dgvr.Cells["Cantidad"].Value.ToString());
                    frmPedidosDetalleModificar2.Descuento = float.Parse(dgvr.Cells["Descuento"].Value.ToString());
                    frmPedidosDetalleModificar2.Importe = float.Parse(dgvr.Cells["Importe"].Value.ToString());
                    frmPedidosDetalleModificar2.UInventario = short.Parse(dgvr.Cells["UInventario"].Value.ToString());
                    DialogResult dialogResult = frmPedidosDetalleModificar2.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        // Actualiza los valores en la fila actual del DataGridView
                        dgvr.Cells["Cantidad"].Value = frmPedidosDetalleModificar2.Cantidad;
                        dgvr.Cells["Descuento"].Value = frmPedidosDetalleModificar2.Descuento;
                        dgvr.Cells["Importe"].Value = frmPedidosDetalleModificar2.Importe;

                        CalcularTotal();
                    }
                }
            }
            if ((e.ColumnIndex == dgvDetalle.Columns["Eliminar"].Index & tabcOperacion.SelectedTab == tabpModificar) | (PedidoGenerado & e.ColumnIndex == dgvDetalle.Columns["Eliminar"].Index & tabcOperacion.SelectedTab == tabpRegistrar))
            {
                DataGridViewRow dgvr = dgvDetalle.CurrentRow;
                string productName = dgvr.Cells["Producto"].Value.ToString();
                int productId = (int)dgvr.Cells["ProductoId"].Value;
                int orderId = int.Parse(txtId.Text);
                EliminarProducto(productName, productId, orderId);
            }
            if ((e.ColumnIndex == dgvDetalle.Columns["Modificar"].Index & tabcOperacion.SelectedTab == tabpModificar) | (PedidoGenerado & e.ColumnIndex == dgvDetalle.Columns["Modificar"].Index & tabcOperacion.SelectedTab == tabpRegistrar))
            {
                DataGridViewRow dgvr = dgvDetalle.CurrentRow;
                using (FrmPedidosDetalleModificar frmPedidosDetalleModificar = new FrmPedidosDetalleModificar())
                {
                    frmPedidosDetalleModificar.Owner = this;
                    frmPedidosDetalleModificar.PedidoId = int.Parse(txtId.Text);
                    frmPedidosDetalleModificar.ProductoId = int.Parse(dgvr.Cells["ProductoId"].Value.ToString());
                    frmPedidosDetalleModificar.Producto = dgvr.Cells["Producto"].Value.ToString();
                    frmPedidosDetalleModificar.Precio = decimal.Parse(dgvr.Cells["Precio"].Value.ToString());
                    frmPedidosDetalleModificar.Cantidad = short.Parse(dgvr.Cells["Cantidad"].Value.ToString());
                    frmPedidosDetalleModificar.Descuento = decimal.Parse(dgvr.Cells["Descuento"].Value.ToString());
                    frmPedidosDetalleModificar.Importe = decimal.Parse(dgvr.Cells["Importe"].Value.ToString());
                    DialogResult dialogResult = frmPedidosDetalleModificar.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        btnNota.Enabled = true;
                        btnNota.Visible = true;
                        BorrarDatosDetallePedido();
                        LlenarDatosDetallePedido();
                    }
                }
            }
        }

        private void EliminarProducto(string productName, int productId, int orderId)
        {
            byte numRegs = 0;
            BorrarMensajesError();
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            InicializarValoresProducto();
            try
            {
                DialogResult respuesta = MessageBox.Show($"¿Esta seguro de eliminar el producto: {productName} del pedido: {orderId}?", Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Yes)
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.eliminandoRegistro);
                    DeshabilitarControlesProducto();
                    PedidoDetalle pedidoDetalle = new PedidoDetalle();
                    pedidoDetalle.ProductId = productId;
                    PedidoDetalleDB pedidoDetalleDB = new PedidoDetalleDB();
                    pedidoDetalleDB.PedidoId = orderId;
                    numRegs = pedidoDetalleDB.Delete(pedidoDetalle);
                    if (numRegs > 0)
                        MessageBox.Show($"El producto: {productName} del Pedido: {pedidoDetalleDB.PedidoId}, se eliminó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show($"El producto: {productName} del Pedido: {pedidoDetalleDB.PedidoId}, NO se eliminó en la base de datos, es posible que el registro se haya eliminado por otro usuario de la red", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex) when (ex.Number == 220)
            {
                MessageBox.Show("Al tratar de devolver las unidades vendidas al inventario, la cantidad de unidades excede las 32,767 unidades, rango máximo para un campo de tipo smallint", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (numRegs > 0)
            {
                BorrarDatosDetallePedido();
                LlenarDatosDetallePedido();
                cboCategoria.Enabled = true;
                btnNota.Enabled = true;
                btnNota.Visible = true;
                Utils.ActualizarBarraDeEstado(this, $"Se muestran {dgvPedidos.RowCount} registros de pedidos");
            }
        }

        private void BorrarDatosDetallePedido()
        {
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
            txtTotal.Text = "$0.00";
            dgvDetalle.Rows.Clear();
        }

        private void BorrarDatosAgregarProducto()
        {
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
        }

        private void btnNota_Click(object sender, EventArgs e)
        {
            FrmRptNotaRemision frmRptNotaRemision = new FrmRptNotaRemision();
            frmRptNotaRemision.Id = int.Parse(txtId.Text);
            frmRptNotaRemision.ShowDialog();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            HabilitarControles();
            btnNota.Enabled = false;
            btnNota.Visible = true;
            btnNuevo.Enabled = false;
            btnNuevo.Visible = true;
            PedidoGenerado = false;
        }
    }
}
