using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmPedidosDetalleCrud : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        int IdDetalle = 1;

        public FrmPedidosDetalleCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmPedidosDetalleCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarCboCategoria();
            LlenarDgvPedidos(null);
            Utils.ConfDgv(DgvPedidos);
            Utils.ConfDgv(DgvDetalle);
            ConfDgvPedidos();
            ConfDgvDetalle();
            txtPrecio.Text = "$0.00";
            txtDescuento.Text = "0.00";
            txtTotal.Text = "$0.00";
            txtUInventario.Text = txtCantidad.Text = "0";
        }

        private void FrmPedidosDetalleCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cboCategoria.SelectedIndex > 0 || cboProducto.SelectedIndex > 0)
            {
                DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void FrmPedidosDetalleCrud_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void DeshabilitarControles()
        {
            cboCategoria.Enabled = cboProducto.Enabled = false;
            //txtCantidad.ReadOnly = txtDescuento.ReadOnly = true;
            btnAgregar.Enabled = false;
        }

        private void HabilitarControles()
        {
            cboCategoria.Enabled = cboProducto.Enabled = true;
            //txtCantidad.ReadOnly = txtDescuento.ReadOnly = false;
            btnAgregar.Enabled = true;
        }

        private void DeshabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = false;
        }

        private void HabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = true;
        }

        private void LlenarCboCategoria()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
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
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
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
                        dtpBFPedidoIni.Value = Convert.ToDateTime(dtpBFPedidoIni.Value.ToShortDateString() + " " + "00:00:00.000");
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
                DgvPedidos.DataSource = tbl;
                if (sender == null)
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran los últimos {DgvPedidos.RowCount} pedidos registrados");
                else
                    Utils.ActualizarBarraDeEstado(this, $"Se encontraron {DgvPedidos.RowCount} registros");
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
            DgvPedidos.Columns["Fecha de pedido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvPedidos.Columns["Fecha requerido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvPedidos.Columns["Fecha de envío"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvPedidos.Columns["Compañía transportista"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvPedidos.Columns["Vendedor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DgvPedidos.Columns["Fecha de pedido"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
            DgvPedidos.Columns["Fecha requerido"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
            DgvPedidos.Columns["Fecha de envío"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
        }

        private void ConfDgvDetalle()
        {
            DgvDetalle.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvDetalle.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvDetalle.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvDetalle.Columns["Descuento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvDetalle.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvDetalle.Columns["Modificar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            DgvDetalle.Columns["Eliminar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            DgvDetalle.Columns["Precio"].DefaultCellStyle.Format = "c";
            DgvDetalle.Columns["Descuento"].DefaultCellStyle.Format = "n2";
            DgvDetalle.Columns["Importe"].DefaultCellStyle.Format = "c";
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            BorrarDatosBusqueda();
            DeshabilitarControles();
            DgvPedidos.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            DeshabilitarControles();
            LlenarDgvPedidos(sender);
            DgvPedidos.Focus();
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            btnLimpiar.PerformClick();
            LlenarDgvPedidos(null);
            DgvPedidos.Focus();
        }

        private void BorrarDatosPedido()
        {
            errorProvider1.Clear();
            txtId.Text = txtCliente.Text = "";
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
            DgvDetalle.Rows.Clear();
        }

        private void BorrarMensajesError()
        {
            //errorProvider1.SetError(cboCategoria, "");
            //errorProvider1.SetError(cboProducto, "");
            //errorProvider1.SetError(txtCantidad, "");
            //errorProvider1.SetError(txtDescuento, "");
            //errorProvider1.SetError(btnAgregar, "");
            errorProvider1.Clear();
        }

        private bool ValidarControles()
        {
            bool valida = true;
            if (cboCategoria.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboCategoria, "Seleccione la categoría");
            }
            if (cboProducto.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboProducto, "Seleccione el producto");
            }
            if (txtCantidad.Text == "" || int.Parse(txtCantidad.Text) == 0)
            {
                valida = false;
                errorProvider1.SetError(txtCantidad, "Ingrese la cantidad");
            }
            if (int.Parse(txtCantidad.Text) > int.Parse(txtUInventario.Text))
            {
                valida = false;
                errorProvider1.SetError(txtCantidad, "La cantidad de productos en el pedido excede el inventario disponible");
            }
            if (txtDescuento.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtDescuento, "Ingrese el descuento");
            }
            if (decimal.Parse(txtDescuento.Text) > 1 || decimal.Parse(txtDescuento.Text) < 0)
            {
                valida = false;
                errorProvider1.SetError(txtDescuento, "El descuento no puede ser mayor que 1 o menor que 0");
            }
            if (cboProducto.SelectedIndex > 0)
            {
                int numProd = int.Parse(cboProducto.SelectedValue.ToString());
                bool productoDuplicado = false;
                foreach (DataGridViewRow dgvr in DgvDetalle.Rows)
                {
                    if (int.Parse(dgvr.Cells["ProductoId"].Value.ToString()) == numProd)
                    {
                        productoDuplicado = true;
                        break;
                    }
                }
                if (productoDuplicado)
                {
                    valida = false;
                    errorProvider1.SetError(cboProducto, "No se puede tener un producto duplicado en el detalle del pedido");
                }
            }
            string total = txtTotal.Text;
            total = total.Replace("$", "");
            if (txtTotal.Text == "" || (decimal.Parse(total) + (decimal.Parse(txtPrecio.Text.Replace("$", "")) * int.Parse(txtCantidad.Text) * (1 - decimal.Parse(txtDescuento.Text))) == 0 ))
            {
                valida = false;
                errorProvider1.SetError(btnAgregar, "Ingrese el detalle del pedido");
            }
            return valida;
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

        private void txtBIdInicial_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdIni(txtBIdInicial, txtBIdFinal);
        }

        private void txtBIdFinal_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
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
            txtPrecio.Text = "$0.00";
            txtUInventario.Text = txtCantidad.Text = "0";
            BorrarMensajesError();
            if (cboCategoria.SelectedIndex > 0)
            {
                try
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                    SqlCommand cmd = new SqlCommand("Sp_Productos_Seleccionar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Categoria", cboCategoria.SelectedValue);
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataTable tbl = new DataTable();
                    dap.Fill(tbl);
                    cboProducto.DataSource = tbl;
                    cboProducto.DisplayMember = "Producto";
                    cboProducto.ValueMember = "Id";
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran {DgvPedidos.RowCount} registros en pedidos");
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
                Utils.ActualizarBarraDeEstado(this, $"Se muestran {DgvPedidos.RowCount} registros en pedidos");
            }
        }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            BorrarMensajesError();
            if (cboProducto.SelectedIndex > 0)
            {
                try
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                    SqlCommand cmd = new SqlCommand($"Select UnitPrice, UnitsInStock from Products Where ProductId = {cboProducto.SelectedValue}", cn);
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
                            txtPrecio.Text = "$0.00";
                            txtUInventario.Text = txtCantidad.Text = "0";
                            txtDescuento.Text = "0.00";
                        }
                        else
                            HabilitarControlesProducto();
                    }
                    else
                    {
                        DeshabilitarControlesProducto();
                        txtPrecio.Text = "$0.00";
                        txtUInventario.Text = txtCantidad.Text = "0";
                        txtDescuento.Text = "0.00";
                    }
                    rdr.Close();
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran {DgvPedidos.RowCount} registros en pedidos");
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
                txtPrecio.Text = "$0.00";
                txtUInventario.Text = txtCantidad.Text = "0";
                txtDescuento.Text = "0.00";
            }
        }

        private void DgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BorrarDatosPedido();
            DataGridViewRow dgvr = DgvPedidos.CurrentRow;
            txtId.Text = dgvr.Cells["Id"].Value.ToString();
            txtCliente.Text = dgvr.Cells["Cliente"].Value.ToString();
            LlenarDatosDetallePedido();
            HabilitarControles();
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
                        DgvDetalle.Rows.Add(new object[] { IdDetalle, pedidoDetalle.ProductName, pedidoDetalle.UnitPrice, pedidoDetalle.Quantity, pedidoDetalle.Discount, (pedidoDetalle.UnitPrice * pedidoDetalle.Quantity) * (1 - pedidoDetalle.Discount), "  Modificar  ", "  Eliminar  ", pedidoDetalle.ProductId });
                        ++IdDetalle;
                    } while (rdr.Read());
                }
                else
                    MessageBox.Show("No se encontraron detalles para el pedido especificado", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                rdr.Close();
                CalcularTotal();
                Utils.ActualizarBarraDeEstado(this, $"Se muestran {DgvPedidos.RowCount} registros en pedidos");
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

        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow dgvr in DgvDetalle.Rows)
            {
                decimal importe = (decimal)dgvr.Cells["Importe"].Value;
                total += importe;
            }
            txtTotal.Text = string.Format("{0:c}", total);
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

        private class PedidoDetalleDB
        {
            public int PedidoId { get; set; }

            public byte Add(PedidoDetalle pedidoDetalle)
            {
                // las excepciones generadas en este segmento de código son capturadas en un nivel superior, por eso no uso bloque try
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    SqlCommand cmd = new SqlCommand("Sp_PedidosDetalle_Insertar_V2", cn);
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
                return numRegs;
            }

            public byte Delete(PedidoDetalle pedidoDetalle)
            {
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    SqlCommand cmd = new SqlCommand("Sp_PedidosDetalle_Eliminar_V2", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("OrderId", PedidoId);
                    cmd.Parameters.AddWithValue("ProductId", pedidoDetalle.ProductId);
                    cn.Open();
                    numRegs = (byte)cmd.ExecuteNonQuery();
                    cn.Close();
                }
                return numRegs;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int numRegs = 0;
            BorrarMensajesError();
            if (ValidarControles())
            {
                try
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.insertandoRegistro);
                    DeshabilitarControles();
                    DeshabilitarControlesProducto();
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
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran {DgvPedidos.RowCount} registros de pedidos");
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
                    HabilitarControles();
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran {DgvPedidos.RowCount} registros de pedidos");
                    DgvDetalle.Focus();
                }
            }
        }

        private void BorrarDatosAgregarProducto()
        {
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
        }

        private void BorrarDatosDetallePedido()
        {
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
            txtTotal.Text = "$0.00";
            DgvDetalle.Rows.Clear();
        }

        private void DgvDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == DgvDetalle.Columns["Eliminar"].Index)
            {
                DataGridViewRow dgvr = DgvDetalle.CurrentRow;
                string productName = dgvr.Cells["Producto"].Value.ToString();
                int productId = (int)dgvr.Cells["ProductoId"].Value;
                int orderId = int.Parse(txtId.Text);
                EliminarProducto(productName, productId, orderId);
            }
            if (e.ColumnIndex == DgvDetalle.Columns["Modificar"].Index)
            {
                DataGridViewRow dgvr = DgvDetalle.CurrentRow;
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
                        BorrarDatosDetallePedido();
                        LlenarDatosDetallePedido();
                    }
                }
            }
            DgvDetalle.Focus();
        }

        private void EliminarProducto(string productName, int productId, int orderId)
        {
            int numRegs = 0;
            BorrarMensajesError();
            BorrarDatosAgregarProducto();
            try
            {
                DialogResult respuesta = MessageBox.Show($"¿Esta seguro de eliminar el producto: {productName} del pedido: {orderId}?", Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Yes)
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.eliminandoRegistro);
                    DeshabilitarControles();
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
                MessageBox.Show("Al tratar de devolver las unidades vendidas al inventario, la cantidad de unidades  excede las 32,767 unidades, rango máximo para un campo de tipo smallint", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                HabilitarControles();
                Utils.ActualizarBarraDeEstado(this, $"Se muestran {DgvPedidos.RowCount} registros de pedidos");
            }
        }
    }
}
