﻿using Microsoft.ReportingServices.RdlExpressions.ExpressionHostObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmPedidosCrud : Form
    {
        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        private TabPage lastSelectedTab;
        bool EventoCargardo = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar
        int IdDetalle = 1;
        bool PedidoGenerado = false;

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
                if (cboCliente.SelectedIndex > 0 || cboEmpleado.SelectedIndex > 0 || cboTransportista.SelectedIndex > 0 || cboCategoria.SelectedIndex > 0 || cboProducto.SelectedIndex > 0 || dgvDetalle.RowCount > 0) 
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
            dtpHoraRequerido.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpHoraEnvio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
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
            txtUInventario.Text = "0";
        }

        private void DeshabilitarControles()
        {
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = cboCategoria.Enabled = cboProducto.Enabled = false;
            dtpPedido.Enabled = dtpHoraPedido.Enabled = dtpRequerido.Enabled = dtpHoraRequerido.Enabled = dtpEnvio.Enabled = dtpHoraEnvio.Enabled = false;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = true;
            txtCantidad.Enabled = txtDescuento.Enabled = false;
            btnAgregar.Enabled = btnGenerar.Enabled = false;
        }

        private void HabilitarControles()
        {
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = cboCategoria.Enabled = cboProducto.Enabled = true;
            dtpPedido.Enabled = dtpRequerido.Enabled = dtpEnvio.Enabled = true;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = false;
            btnAgregar.Enabled = btnGenerar.Enabled = true;
        }

        private void HabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = true;
        }

        private void DeshabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = false;
        }

        private bool ValidarControles()
        {
            bool valida = true;
            if (cboCliente.SelectedIndex == 0)
            {
                valida = false;
                errorProvider1.SetError(cboCliente, "Ingrese el cliente");
            }
            if (cboEmpleado.SelectedIndex == 0)
            {
                valida = false;
                errorProvider1.SetError(cboEmpleado, "Ingrese el empleado");
            }
            if (dtpPedido.Checked == false)
            {
                valida = false;
                errorProvider1.SetError(dtpPedido, "Ingrese la fecha de pedido");
            }
            if (cboTransportista.SelectedIndex == 0)
            {
                valida = false;
                errorProvider1.SetError(cboTransportista, "Ingrese la compañía transportista");
            }
            string total = txtTotal.Text;
            total = total.Replace("$", "");
            if (txtTotal.Text == "" || decimal.Parse(total) == 0)
            {
                valida = false;
                errorProvider1.SetError(btnAgregar, "Ingrese el detalle del pedido");
                errorProvider1.SetError(txtTotal, "El total del pedido no puede ser cero");
            }
            if (cboProducto.SelectedIndex > 0)
            {
                valida = false;
                errorProvider1.SetError(cboProducto, "Ha seleccionado un producto y no lo ha agregado al pedido");
            }
            return valida;
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

            dgvPedidos.Columns["Fecha requerido"].HeaderText = "Fecha de entrega";
            dgvPedidos.Columns["Dirigido a"].HeaderText = "Enviar a";
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
            txtId.Tag = null;
            cboCliente.SelectedIndex = cboEmpleado.SelectedIndex = cboTransportista.SelectedIndex = cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            dtpPedido.Value = dtpRequerido.Value = dtpEnvio.Value = DateTime.Now;
            dtpHoraPedido.Value = DateTime.Now;
            dtpHoraRequerido.Value = dtpHoraEnvio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpRequerido.Checked = dtpEnvio.Checked =  false;
            txtDirigidoa.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCP.Text = txtPais.Text = "";
            txtFlete.Text = txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
            txtTotal.Text = "$0.00";
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
            txtUInventario.Text = "0";
            txtCantidad.Text = "0";
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
                Utils.ActualizarBarraDeEstado(this, $"Se muestran {dgvPedidos.RowCount} registros en pedidos");
            }
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex > 0)
            {
                try
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                    SqlCommand cmd = new SqlCommand($"Select Top 1 ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry from Orders Where CustomerId = '{cboCliente.SelectedValue}' order by OrderId Desc", cn);
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
                        txtDirigidoa.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCP.Text = txtPais.Text = "";
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
                txtDirigidoa.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCP.Text = txtPais.Text = "";
        }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProducto.SelectedIndex > 0)
            {
                try
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                    SqlCommand cmd = new SqlCommand($"Select UnitPrice, UnitsInStock from Products Where ProductId = {cboProducto.SelectedValue}", cn);
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
                            txtUInventario.Text = "0";
                            txtCantidad.Text = "0";
                            txtDescuento.Text = "0.00";
                        }
                        else
                            HabilitarControlesProducto();
                    }
                    else
                    {
                        DeshabilitarControlesProducto();
                        txtPrecio.Text = "$0.00";
                        txtUInventario.Text = "0";
                        txtCantidad.Text = "0";
                        txtDescuento.Text = "0.00";
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
                txtPrecio.Text = "$0.00";
                txtUInventario.Text = "0";
                txtCantidad.Text = "0";
                txtDescuento.Text = "0.00";
            }
        }

        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
            {
                decimal importe = decimal.Parse(dgvr.Cells["Importe"].Value.ToString());
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
            if (txtDescuento.Text.Trim() == "")
                txtDescuento.Text = "0.00";
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            if (txtCantidad.Text.Trim() == "" || int.Parse(txtCantidad.Text) == 0) txtCantidad.Text = "1";
        }

        private void txtFlete_Enter(object sender, EventArgs e)
        {
            if (txtFlete.Text.Contains("$")) txtFlete.Text = txtFlete.Text.Replace("$", "");
            if (decimal.Parse(txtFlete.Text) == 0) txtFlete.Text = "";
        }

        private void txtFlete_Leave(object sender, EventArgs e)
        {
            if (txtFlete.Text.Trim() == "") txtFlete.Text = "0.00";
            decimal flete = decimal.Parse(txtFlete.Text.Trim());
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
            if (txtCantidad.Text.Trim() == "" || int.Parse(txtCantidad.Text) == 0)
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
            dgvDetalle.Rows.Add(new object[] { IdDetalle, cboProducto.Text, txtPrecio.Text, txtCantidad.Text, txtDescuento.Text, ((decimal.Parse(txtPrecio.Text) * decimal.Parse(txtCantidad.Text)) * (1 - decimal.Parse(txtDescuento.Text))).ToString(), "Eliminar", cboProducto.SelectedValue });
            CalcularTotal();
            ++IdDetalle;
            cboCategoria.SelectedIndex = cboProducto.SelectedIndex = 0;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
            cboCategoria.Focus();
        }

        private void dgvDetalle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("c");
            if (e.ColumnIndex == 3 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("n0");
            if (e.ColumnIndex == 4 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("n2");
            if (e.ColumnIndex == 5 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("c");
        }

        private void dgvDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvDetalle.Columns["Eliminar"].Index)
                return;
            dgvDetalle.Rows.RemoveAt(e.RowIndex);
            CalcularTotal();
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
            if (txtCantidad.Text.Trim() != "")
            {
                if (int.Parse(txtCantidad.Text.Replace(",", "")) > 32767)
                {
                    errorProvider1.SetError(txtCantidad, "La cantidad no puede ser mayor a 32767");
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
            lastSelectedTab = e.TabPage;  // actualizar la pestaña actual
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
                btnGenerar.Text = "Generar pedido";
                btnGenerar.Visible = true;
                btnGenerar.Enabled = true;
                btnAgregar.Visible = true;
                btnAgregar.Enabled = true;
                dgvDetalle.Columns["Eliminar"].Visible = true;
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
                btnGenerar.Enabled = false;
                dgvDetalle.Columns["Eliminar"].Visible = false;
                grbProducto.Enabled = false;
                if (tabcOperacion.SelectedTab == tabpConsultar)
                {
                    btnGenerar.Visible = false;
                    btnAgregar.Visible = false;
                    btnNota.Visible = true;
                    btnNota.Enabled = false;
                    btnNuevo.Visible= false;
                    btnNuevo.Enabled= false;
                }
                else if (tabcOperacion.SelectedTab == tabpModificar)
                {
                    PedidoGenerado = false;
                    btnGenerar.Text = "Modificar pedido";
                    btnGenerar.Visible = true;
                    btnAgregar.Visible = false;
                    btnNota.Visible = true; 
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                    btnNuevo.Enabled = false;
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
            if (tabcOperacion.SelectedTab != tabpRegistrar)
            {
                BorrarDatosPedido();
                DataGridViewRow dgvr = dgvPedidos.CurrentRow;
                txtId.Text = dgvr.Cells["Id"].Value.ToString();
                LlenarDatosPedido();
                LlenarDatosDetallePedido();
                DeshabilitarControles();
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
                    btnNota.Visible = true;
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                }
                else if (tabcOperacion.SelectedTab == tabpEliminar)
                {
                    btnGenerar.Enabled = true;
                    btnNota.Visible = false;
                    btnNuevo.Visible = false;
                }
            }
        }

        private void LlenarDatosPedido()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_Pedidos_Listar1", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("PedidoId", txtId.Text);
                        cn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
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
                                decimal flete;
                                if (decimal.TryParse(txtFlete.Text, out flete))
                                    txtFlete.Text = flete.ToString("c2");
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
                                byte[] rowVersion = (byte[])rdr["RowVersion"];
                                txtId.Tag = rowVersion;
                            }
                            rdr.Close();
                        }
                    }
                }
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
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_DetallePedidos_Productos_Listar1", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("PedidoId", txtId.Text);
                        cn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                        {
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
                                    pedidoDetalle.RowVersion = (byte[])rdr["RowVersion"];
                                    dgvDetalle.Rows.Add(new object[] { IdDetalle, pedidoDetalle.ProductName, pedidoDetalle.UnitPrice, pedidoDetalle.Quantity, pedidoDetalle.Discount, (pedidoDetalle.UnitPrice * pedidoDetalle.Quantity) * (1 - pedidoDetalle.Discount), "Eliminar", pedidoDetalle.ProductId, pedidoDetalle.RowVersion });
                                    ++IdDetalle;
                                } while (rdr.Read());
                            }
                            else
                                MessageBox.Show("No se encontraron detalles para el pedido especificado", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            rdr.Close();
                        }
                    }
                }
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
            public byte[] RowVersion { get; set; }
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
            public string ShipPostalCode { get; set;}
            public string ShipCountry { get; set; }
            public byte[] RowVersion { get; set; }
        }

        private class PedidosDB
        {
            public int PedidoId { get; set; }

            public byte Add(Pedido pedido, List<PedidoDetalle> lst, TextBox textBox)
            {
                // las excepciones generadas en este segmento de código son capturadas en un nivel superior, por eso no uso bloque try
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Id", typeof(int));
                    dt.Columns.Add("ProductId", typeof(int));
                    dt.Columns.Add("UnitPrice", typeof(decimal));
                    dt.Columns.Add("Quantity", typeof(int));
                    dt.Columns.Add("Discount", typeof(float));
                    int i = 1;
                    foreach (var item in lst)
                    {
                        dt.Rows.Add(i, item.ProductId, item.UnitPrice, item.Quantity, item.Discount);
                        i++;
                    }
                    using (SqlCommand cmd = new SqlCommand("Sp_Pedidos_Insertar_V3", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("OrderId", 0);
                        cmd.Parameters["OrderId"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("CustomerId", pedido.CustomerId);
                        cmd.Parameters.AddWithValue("EmployeeId", pedido.EmployeeId);
                        //if (pedido.OrderDate == null) cmd.Parameters.AddWithValue("OrderDate", DBNull.Value);
                        //else cmd.Parameters.AddWithValue("OrderDate", pedido.OrderDate);
                        //if (pedido.RequiredDate == null) cmd.Parameters.AddWithValue("RequiredDate", DBNull.Value);
                        //else cmd.Parameters.AddWithValue("RequiredDate", pedido.RequiredDate);
                        //if (pedido.ShippedDate == null) cmd.Parameters.AddWithValue("ShippedDate", DBNull.Value);
                        //else cmd.Parameters.AddWithValue("ShippedDate", pedido.ShippedDate);
                        cmd.Parameters.AddWithValue("OrderDate", pedido.OrderDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("RequiredDate", pedido.RequiredDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("ShippedDate", pedido.ShippedDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("ShipVia", pedido.ShipVia);
                        cmd.Parameters.AddWithValue("Freight", pedido.Freight);
                        //if (pedido.ShipName.Trim() == "") cmd.Parameters.AddWithValue("ShipName", DBNull.Value);
                        //else cmd.Parameters.AddWithValue("ShipName", pedido.ShipName);
                        //if (pedido.ShipAddress.Trim() == "") cmd.Parameters.AddWithValue("ShipAddress", DBNull.Value);
                        //else cmd.Parameters.AddWithValue("ShipAddress", pedido.ShipAddress);
                        //if (pedido.ShipCity.Trim() == "") cmd.Parameters.AddWithValue("ShipCity", DBNull.Value);
                        //else cmd.Parameters.AddWithValue("ShipCity", pedido.ShipCity);
                        //if (pedido.ShipRegion.Trim() == "") cmd.Parameters.AddWithValue("ShipRegion", DBNull.Value);
                        //else cmd.Parameters.AddWithValue("ShipRegion", pedido.ShipRegion);
                        //if (pedido.ShipPostalCode.Trim() == "") cmd.Parameters.AddWithValue("ShipPostalCode", DBNull.Value);
                        //else cmd.Parameters.AddWithValue("ShipPostalCode", pedido.ShipPostalCode);
                        //if (pedido.ShipCountry.Trim() == "") cmd.Parameters.AddWithValue("ShipCountry", DBNull.Value);
                        //else cmd.Parameters.AddWithValue("ShipCountry", pedido.ShipCountry);
                        cmd.Parameters.AddWithValue("ShipName", string.IsNullOrWhiteSpace(pedido.ShipName) ? DBNull.Value : (object)pedido.ShipName);
                        cmd.Parameters.AddWithValue("ShipAddress", string.IsNullOrWhiteSpace(pedido.ShipAddress) ? DBNull.Value : (object)pedido.ShipAddress);
                        cmd.Parameters.AddWithValue("ShipCity", string.IsNullOrWhiteSpace(pedido.ShipCity) ? DBNull.Value : (object)pedido.ShipCity);
                        cmd.Parameters.AddWithValue("ShipRegion", string.IsNullOrWhiteSpace(pedido.ShipRegion) ? DBNull.Value : (object)pedido.ShipRegion);
                        cmd.Parameters.AddWithValue("ShipPostalCode", string.IsNullOrWhiteSpace(pedido.ShipPostalCode) ? DBNull.Value : (object)pedido.ShipPostalCode);
                        cmd.Parameters.AddWithValue("ShipCountry", string.IsNullOrWhiteSpace(pedido.ShipCountry) ? DBNull.Value : (object)pedido.ShipCountry);
                        var sqlParameter = new SqlParameter("lstOrderDetails", SqlDbType.Structured);
                        sqlParameter.TypeName = "dbo.OrderDetails";
                        sqlParameter.Value = dt;
                        cmd.Parameters.Add(sqlParameter);
                        SqlParameter RowVersion = new SqlParameter("RowVersion", SqlDbType.Timestamp, 8)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(RowVersion);
                        cn.Open();
                        numRegs = (byte)cmd.ExecuteNonQuery();
                        PedidoId = (int)cmd.Parameters["OrderId"].Value;
                        textBox.Text = PedidoId.ToString();
                        textBox.Tag = (byte[])cmd.Parameters["RowVersion"].Value;
                        cn.Close();
                    }
                }
                return numRegs;
            }

            public byte Update(Pedido pedido, string cliente, TextBox textBox)
            {
                // las excepciones generadas en este segmento de código son capturadas en un nivel superior, por eso no uso bloque try
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    SqlCommand cmd = new SqlCommand("Sp_Pedidos_Actualizar_V2", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("OrderId", pedido.OrderId);
                    //cmd.Parameters.AddWithValue("CustomerId", pedido.CustomerId);
                    //cmd.Parameters.AddWithValue("EmployeeId", pedido.EmployeeId);
                    //if (pedido.OrderDate == null) cmd.Parameters.AddWithValue("OrderDate", DBNull.Value);
                    //else cmd.Parameters.AddWithValue("OrderDate", pedido.OrderDate);
                    //if (pedido.RequiredDate == null) cmd.Parameters.AddWithValue("RequiredDate", DBNull.Value);
                    //else cmd.Parameters.AddWithValue("RequiredDate", pedido.RequiredDate);
                    //if (pedido.ShippedDate == null) cmd.Parameters.AddWithValue("ShippedDate", DBNull.Value);
                    //else cmd.Parameters.AddWithValue("ShippedDate", pedido.ShippedDate);
                    //cmd.Parameters.AddWithValue("ShipVia", pedido.ShipVia);
                    //cmd.Parameters.AddWithValue("Freight", pedido.Freight);
                    //if (pedido.ShipName.Trim() == "") cmd.Parameters.AddWithValue("ShipName", DBNull.Value);
                    //else cmd.Parameters.AddWithValue("ShipName", pedido.ShipName);
                    //if (pedido.ShipAddress.Trim() == "") cmd.Parameters.AddWithValue("ShipAddress", DBNull.Value);
                    //else cmd.Parameters.AddWithValue("ShipAddress", pedido.ShipAddress);
                    //if (pedido.ShipCity.Trim() == "") cmd.Parameters.AddWithValue("ShipCity", DBNull.Value);
                    //else cmd.Parameters.AddWithValue("ShipCity", pedido.ShipCity);
                    //if (pedido.ShipRegion.Trim() == "") cmd.Parameters.AddWithValue("ShipRegion", DBNull.Value);
                    //else cmd.Parameters.AddWithValue("ShipRegion", pedido.ShipRegion);
                    //if (pedido.ShipPostalCode.Trim() == "") cmd.Parameters.AddWithValue("ShipPostalCode", DBNull.Value);
                    //else cmd.Parameters.AddWithValue("ShipPostalCode", pedido.ShipPostalCode);
                    //if (pedido.ShipCountry.Trim() == "") cmd.Parameters.AddWithValue("ShipCountry", DBNull.Value);
                    //else cmd.Parameters.AddWithValue("ShipCountry", pedido.ShipCountry);
                    cmd.Parameters.AddWithValue("@OrderId", pedido.OrderId);
                    cmd.Parameters.AddWithValue("@CustomerId", pedido.CustomerId);
                    cmd.Parameters.AddWithValue("@EmployeeId", pedido.EmployeeId);
                    cmd.Parameters.AddWithValue("@OrderDate", pedido.OrderDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequiredDate", pedido.RequiredDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ShippedDate", pedido.ShippedDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ShipVia", pedido.ShipVia);
                    cmd.Parameters.AddWithValue("@Freight", pedido.Freight);
                    cmd.Parameters.AddWithValue("@ShipName", string.IsNullOrWhiteSpace(pedido.ShipName) ? (object)DBNull.Value : pedido.ShipName);
                    cmd.Parameters.AddWithValue("@ShipAddress", string.IsNullOrWhiteSpace(pedido.ShipAddress) ? (object)DBNull.Value : pedido.ShipAddress);
                    cmd.Parameters.AddWithValue("@ShipCity", string.IsNullOrWhiteSpace(pedido.ShipCity) ? (object)DBNull.Value : pedido.ShipCity);
                    cmd.Parameters.AddWithValue("@ShipRegion", string.IsNullOrWhiteSpace(pedido.ShipRegion) ? (object)DBNull.Value : pedido.ShipRegion);
                    cmd.Parameters.AddWithValue("@ShipPostalCode", string.IsNullOrWhiteSpace(pedido.ShipPostalCode) ? (object)DBNull.Value : pedido.ShipPostalCode);
                    cmd.Parameters.AddWithValue("@ShipCountry", string.IsNullOrWhiteSpace(pedido.ShipCountry) ? (object)DBNull.Value : pedido.ShipCountry);
                    cn.Open();
                    // Con ExecuteScalar obtenemos el primer valor de la primera columna que regresa nuestro SELECT,
                    // que en este caso es el valor del rowversion (devuelto como un arreglo de bytes: byte[])
                    object rowVersionObj = cmd.ExecuteScalar();
                    // Asignamos el valor obtenido a txtId.Tag. 
                    // Si necesitas mostrarlo, considera convertirlo (por ejemplo a hexadecimal) para su representación.
                    textBox.Tag = rowVersionObj;
                    // Si rowVersionObj no es nulo se asume que la actualización fue exitosa.
                    if (rowVersionObj != null)
                        numRegs = 1;
                    cn.Close();
                }
                return numRegs;
            }

            public byte Delete(Pedido pedido)
            {
                byte numRegs = 0;
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    SqlCommand cmd = new SqlCommand("Sp_Pedidos_Eliminar_V2", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("OrderId", pedido.OrderId);
                    cn.Open();
                    numRegs = (byte)cmd.ExecuteNonQuery();
                    cn.Close();
                }
                return numRegs;
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            int numRegs = 0;
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tabpRegistrar)
            {
                try
                {
                    if (ValidarControles())
                    {
                        Utils.ActualizarBarraDeEstado(this, Utils.insertandoRegistro);
                        DeshabilitarControles();
                        btnGenerar.Enabled = false;
                        List<PedidoDetalle> lstDetalle = new List<PedidoDetalle>();
                        // llenado de elementos hijos
                        foreach(DataGridViewRow dgvr in dgvDetalle.Rows)
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
                        //if (!dtpPedido.Checked) pedido.OrderDate = null;
                        //else pedido.OrderDate = Convert.ToDateTime(dtpPedido.Value.ToShortDateString() + " " + dtpHoraPedido.Value.ToLongTimeString());
                        //if (!dtpRequerido.Checked) pedido.RequiredDate = null;
                        //else pedido.RequiredDate = Convert.ToDateTime(dtpRequerido.Value.ToShortDateString() + " " + dtpHoraRequerido.Value.ToLongTimeString());
                        //if (!dtpEnvio.Checked) pedido.ShippedDate = null;
                        //else pedido.ShippedDate = Convert.ToDateTime(dtpEnvio.Value.ToShortDateString() + " " + dtpHoraEnvio.Value.ToLongTimeString());
                        if (dtpPedido != null && dtpHoraPedido != null)
                            pedido.OrderDate = Utils.ObtenerFechaHora(dtpPedido, dtpHoraPedido);
                        if (dtpRequerido != null && dtpHoraRequerido != null)
                            pedido.RequiredDate = Utils.ObtenerFechaHora(dtpRequerido, dtpHoraRequerido);
                        if (dtpEnvio != null && dtpHoraEnvio != null)
                            pedido.ShippedDate = Utils.ObtenerFechaHora(dtpEnvio, dtpHoraEnvio);
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
                        numRegs = pedidosDB.Add(pedido, lstDetalle, txtId);
                        if (numRegs > 0) MessageBox.Show($"El pedido con Id: {txtId.Text} del Cliente: {cboCliente.Text}, se registró satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("No se pudo realizar el registro, es posible que el pedido ya exista", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    BorrarDatosBusqueda();
                    LlenarDgvPedidos(null);
                    dgvDetalle.Rows.Clear();
                    dgvDetalle.Columns["Eliminar"].Visible = false;
                    LlenarDatosDetallePedido();
                }
            }
            else if (tabcOperacion.SelectedTab == tabpModificar)
            {
                try
                {
                    if (ValidarControles())
                    {
                        if (!chkRowVersion())
                        {
                            MessageBox.Show("El registro ha sido modificado por otro usuario de la red, no se realizará la actualización del registro, vuelva a cargar el registro para que se muestre el pedido con los datos proporcionados por el otro usuario", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        Utils.ActualizarBarraDeEstado(this, Utils.modificandoRegistro);
                        DeshabilitarControles();
                        btnGenerar.Enabled = false;
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
                        numRegs = pedidosDB.Update(pedido, cboCliente.Text, txtId);
                        if (numRegs > 0)
                            MessageBox.Show($"El pedido con Id: {pedido.OrderId} del Cliente: {cboCliente.Text}, se actualizó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("No se pudo realizar la modificación, es posible que el registro se haya eliminado previamente por otro usuario de la red", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (numRegs > 0)
                {
                    PedidoGenerado = true;
                    btnNota.Enabled = true;
                    btnNota.Visible = true;
                    btnNuevo.Visible = false;
                    LlenarDgvPedidos(null);
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
                    if (!chkRowVersion())
                    {
                        MessageBox.Show("El registro ha sido modificado por otro usuario de la red, no se realizará la eliminación del registro, vuelva a cargar el registro para que se muestre el pedido con los datos proporcionados por el otro usuario", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    Utils.ActualizarBarraDeEstado(this, Utils.eliminandoRegistro);
                    btnGenerar.Enabled = false;
                    try
                    {
                        Pedido pedido = new Pedido();
                        pedido.OrderId = int.Parse(txtId.Text);
                        PedidosDB pedidosDB = new PedidosDB();
                        numRegs = pedidosDB.Delete(pedido);
                        if (numRegs > 0)
                            MessageBox.Show($"El pedido con Id: {pedido.OrderId} del Cliente: {cboCliente.Text}, se eliminó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("No se pudo realizar la eliminación, es posible que el registro haya sido eliminado previamente por otro usuario de la red", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        LlenarDgvPedidos(null);
                    }
                }
                else
                {
                    BorrarDatosPedido();
                    btnGenerar.Enabled = false;
                }
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

        private void btnNota_Click(object sender, EventArgs e)
        {
            if (!chkRowVersion())
            {
                MessageBox.Show("El registro ha sido modificado por otro usuario de la red, se mostrará la nota de remisión con los datos proporcionados por el otro usuario", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            FrmNotaRemision0 frmNotaRemision0 = new FrmNotaRemision0();
            frmNotaRemision0.Id = int.Parse(txtId.Text);
            frmNotaRemision0.ShowDialog();
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
            dgvDetalle.Columns["Eliminar"].Visible = true;
            IdDetalle = 1;
        }

        private bool chkRowVersion()
        {
            bool rowVersionOk = true;
            if (txtId.Tag != null)
            {
                byte[] rowVersion = (byte[])txtId.Tag;
                byte[] rowVersionActual = null;
                try
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                    using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                    {
                        using (SqlCommand cmd = new SqlCommand("Sp_Pedidos_Listar1", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("PedidoId", txtId.Text);
                            cn.Open();
                            using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                            {
                                if (rdr.Read())
                                {
                                    rowVersionActual = (byte[])rdr["RowVersion"];
                                    if (!rowVersion.SequenceEqual(rowVersionActual))
                                    {
                                        rowVersionOk = false;
                                    }
                                }
                                else
                                {
                                    rowVersionOk = false;
                                }
                                rdr.Close();
                            }
                        }
                    }
                    if (rowVersionOk)
                    {
                        // Comprobamos primero que todos los registros del gridview existan en la DB y tengan el mismo rowversion
                        using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                        {
                            cn.Open();
                            foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
                            {
                                int numPedido = int.Parse(txtId.Text);
                                int numProducto = int.Parse(dgvr.Cells["ProductoId"].Value.ToString());
                                byte[] rowVersionDetalle = (byte[])dgvr.Cells["RowVersion"].Value;
                                using (SqlCommand cmd = new SqlCommand("Sp_DetallePedidos_ChkRowVersion", cn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("PedidoId", numPedido);
                                    cmd.Parameters.AddWithValue("ProductoId", numProducto);
                                    using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                                    { 
                                        if (rdr.Read())
                                        {
                                            byte[] rowVersionDetalleEnDB = (byte[])rdr["RowVersion"];
                                            if (!rowVersionDetalle.SequenceEqual(rowVersionDetalleEnDB))
                                            {
                                                rowVersionOk = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            rowVersionOk = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (rowVersionOk)
                        { 
                            // Comprobamos segundo que todos los registros de la DB existan en el DataGridView y tengan el mismo rowversion
                            using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                            {
                                cn.Open();
                                using (SqlCommand cmd = new SqlCommand("Sp_DetallePedidos_Productos_Listar1", cn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("PedidoId", txtId.Text);
                                    using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                                    {
                                        PedidoDetalle pedidoDetalle;
                                        if (rdr.Read())
                                        {
                                            do
                                            {
                                                pedidoDetalle = new PedidoDetalle();
                                                pedidoDetalle.ProductId = (int)rdr["Id Producto"];
                                                pedidoDetalle.RowVersion = (byte[])rdr["RowVersion"];
                                                bool registroEncontradoEnGrid = false;
                                                foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
                                                {
                                                    if (pedidoDetalle.ProductId == int.Parse(dgvr.Cells["ProductoId"].Value.ToString()))
                                                    {
                                                        registroEncontradoEnGrid = true;
                                                        byte[] rowVersionGrid = (byte[])dgvr.Cells["RowVersion"].Value;
                                                        if (!rowVersionGrid.SequenceEqual(pedidoDetalle.RowVersion))
                                                        {
                                                            rowVersionOk = false;
                                                        }
                                                        break;
                                                    }
                                                }
                                                if (!registroEncontradoEnGrid)
                                                    rowVersionOk = false;
                                                if (!rowVersionOk)
                                                    break;
                                            } while (rdr.Read());
                                        }
                                        else
                                        {
                                            rowVersionOk = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
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
            return rowVersionOk;
        }
    }
}
