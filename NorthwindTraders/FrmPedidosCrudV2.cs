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
    public partial class FrmPedidosCrudV2 : Form
    {
        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        private TabPage lastSelectedTab;
        bool EventoCargardo = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar
        int IdDetalle = 1;

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
            if (tabcOperacion.SelectedTab != tabpConsultar)
            {
                if (cboCliente.SelectedIndex > 0 || cboEmpleado.SelectedIndex > 0 || cboTransportista.SelectedIndex > 0 || cboCategoria.SelectedIndex > 0 || cboProducto.SelectedIndex > 0 || dgvDetalle.RowCount > 0)
                {
                    DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (respuesta == DialogResult.No)
                        e.Cancel = true;
                }
            }
        }

        private void FrmPedidosCrudV2_Load(object sender, EventArgs e)
        {
            dtpHoraRequerido.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpHoraEnvio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DeshabilitarControles();
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
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = cboCategoria.Enabled = cboProducto.Enabled = false;
            dtpPedido.Enabled = dtpHoraPedido.Enabled = dtpRequerido.Enabled = dtpHoraRequerido.Enabled = dtpEnvio.Enabled = dtpHoraEnvio.Enabled = false;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = true;
            btnAgregar.Enabled = btnGenerar.Enabled = false;
            DeshabilitarControlesProducto();
        }

        private void HabilitarControles()
        {
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = cboCategoria.Enabled = cboProducto.Enabled = true;
            dtpPedido.Enabled = dtpRequerido.Enabled = dtpEnvio.Enabled = true;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = false;
            btnAgregar.Enabled = btnGenerar.Enabled = true;
        }

        private void DeshabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = false;
        }

        private void HabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = true;
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
            // me falta ver la logica para validar el total y demas controles.
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
                DeshabilitarControles();
            dgvPedidos.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tabpRegistrar)
                DeshabilitarControles();
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
                            DeshabilitarControlesProducto();
                            MessageBox.Show("No hay este producto en existencia", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cboProducto.SelectedIndex = 0;
                            InicializarValoresProducto();
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
            DeshabilitarControlesProducto();
            txtPrecio.Text = txtPrecio.Text.Replace("$", "");
            dgvDetalle.Rows.Add(new object[] { IdDetalle, cboProducto.Text, txtPrecio.Text, txtCantidad.Text, txtDescuento.Text, ((decimal.Parse(txtPrecio.Text) * decimal.Parse(txtCantidad.Text)) * (1 - decimal.Parse(txtDescuento.Text))).ToString(), "Modificar", "Eliminar", cboProducto.SelectedValue });
            CalcularTotal();
            ++IdDetalle;
            cboCategoria.SelectedIndex = cboProducto.SelectedIndex = 0;
            InicializarValoresProducto();
            cboCategoria.Focus();
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
                BorrarDatosBusqueda();
                HabilitarControles();
                btnGenerar.Text = "Generar pedido";
                btnGenerar.Visible = true;
                btnGenerar.Enabled = true;
                btnAgregar.Visible = true;
                btnAgregar.Enabled = true;
                dgvDetalle.Columns["Eliminar"].Visible = true;
                grbProducto.Enabled = true;
            }
            else
            {

            }
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tabpRegistrar)
            {
                BorrarDatosPedido();
                DataGridViewRow dgvr = dgvPedidos.CurrentRow;
                txtId.Text = dgvr.Cells["Id"].Value.ToString();
                LlenarDatosPedido();
                LlenarDatosDetallePedido();
                DeshabilitarControles();
                if (tabcOperacion.SelectedTab == tabpModificar)
                {
                    HabilitarControles();
                    btnGenerar.Enabled = true;
                }
                else if (tabcOperacion.SelectedTab == tabpEliminar)
                    btnGenerar.Enabled = true;
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
                    if (DateTime.TryParse(rdr["OrderDate"].ToString(), out DateTime fecha))
                        dtpPedido.Value = dtpHoraPedido.Value = fecha;
                    else
                    {
                        dtpPedido.Value = dtpPedido.MinDate;
                        dtpHoraPedido.Value = dtpHoraPedido.MinDate;
                        dtpPedido.Checked = false;
                    }
                    if (DateTime.TryParse(rdr["RequiredDate"].ToString(), out fecha))
                        dtpRequerido.Value = dtpHoraRequerido.Value = fecha;
                    else
                    {
                        dtpRequerido.Value = dtpRequerido.MinDate;
                        dtpHoraRequerido.Value = dtpHoraRequerido.MinDate;
                        dtpRequerido.Checked = false;
                    }
                    if (DateTime.TryParse(rdr["ShippedDate"].ToString(), out fecha))
                        dtpEnvio.Value = dtpHoraEnvio.Value = fecha;
                    else
                    {
                        dtpEnvio.Value = dtpEnvio.MinDate;
                        dtpHoraEnvio.Value = dtpHoraEnvio.MinDate;
                        dtpEnvio.Checked = false;
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

    }
}
