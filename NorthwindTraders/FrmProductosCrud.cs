using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmProductosCrud : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        bool EventoCargado = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv ojo no quitar

        public FrmProductosCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmProductosCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarCboCategoria();
            LlenarCboProveedor();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            txtProducto.ReadOnly = txtCantidadxU.ReadOnly = txtPrecio.ReadOnly = true;
            txtUInventario.ReadOnly = txtUPedido.ReadOnly = txtPPedido.ReadOnly = true;
            chkbDescontinuado.Enabled = false;
            cboCategoria.Enabled = cboProveedor.Enabled = false;
        }

        private void HabilitarControles()
        {
            txtProducto.ReadOnly = txtCantidadxU.ReadOnly = txtPrecio.ReadOnly = false;
            txtUInventario.ReadOnly = txtUPedido.ReadOnly = txtPPedido.ReadOnly = false;
            chkbDescontinuado.Enabled = true;
            cboCategoria.Enabled = cboProveedor.Enabled = true;
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
                DataTable tbl1 = tbl.Clone();
                DataRow dr;
                foreach (DataRow dataRow in tbl.Rows)
                {
                    dr = tbl1.NewRow();
                    dr["Id"] = Convert.ToInt32(dataRow["Id"]);
                    dr["Categoria"] = dataRow["Categoria"].ToString();
                    tbl1.Rows.Add(dr);
                }
                cboBCategoria.DataSource = tbl1;
                cboBCategoria.DisplayMember = "Categoria";
                cboBCategoria.ValueMember = "Id";
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
                DataTable tbl1 = tbl.Clone();
                DataRow dr;
                foreach (DataRow dataRow in tbl.Rows)
                {
                    dr = tbl1.NewRow();
                    dr["Id"] = Convert.ToInt32(dataRow["Id"]);
                    dr["Proveedor"] = dataRow["Proveedor"].ToString();
                    tbl1.Rows.Add(dr);
                }
                cboBProveedor.DataSource = tbl1;
                cboBProveedor.DisplayMember = "Proveedor";
                cboBProveedor.ValueMember = "Id";
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

        private void LlenarDgv(object sender)
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd;
                if (sender == null)
                {
                    cmd = new SqlCommand("Sp_Productos_All", cn);
                    cmd.Parameters.AddWithValue("top100", 0);
                }
                else
                {
                    cmd = new SqlCommand("Sp_Productos_Buscar_V2", cn);
                    cmd.Parameters.AddWithValue("IdIni", txtBIdIni.Text);
                    cmd.Parameters.AddWithValue("IdFin", txtBIdFin.Text);
                    cmd.Parameters.AddWithValue("Producto", txtBProducto.Text);
                    cmd.Parameters.AddWithValue("Categoria", cboBCategoria.SelectedValue);
                    cmd.Parameters.AddWithValue("Proveedor", cboBProveedor.SelectedValue);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                Dgv.DataSource = tbl;
                Utils.ConfDgv(Dgv);
                ConfDgv();
                if (sender == null)
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran los últimos {Dgv.RowCount} productos registrados");
                else
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosProducto();
            BorrarMensajesError();
            BorrarDatosBusqueda();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(null);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosProducto();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void BorrarDatosProducto()
        {
            txtId.Text = txtProducto.Text = txtCantidadxU.Text = txtPrecio.Text = "";
            txtUInventario.Text = txtUPedido.Text = txtPPedido.Text = "";
            chkbDescontinuado.Checked = false;
            cboCategoria.SelectedIndex = cboProveedor.SelectedIndex = 0;
        }

        private void BorrarMensajesError()
        {
            errorProvider1.SetError(cboCategoria, "");
            errorProvider1.SetError(cboProveedor, "");
            errorProvider1.SetError(txtProducto, "");
            errorProvider1.SetError(txtPrecio, "");
            errorProvider1.SetError(txtUInventario, "");
            errorProvider1.SetError(txtUPedido, "");
            errorProvider1.SetError(txtPPedido, "");
        }

        private void BorrarDatosBusqueda()
        {
            txtBIdIni.Text = txtBIdFin.Text = txtBProducto.Text = "";
            cboBCategoria.SelectedIndex = cboBProveedor.SelectedIndex = 0;
        }

        private bool ValidarControles()
        {
            bool valida = true;
            if (cboCategoria.SelectedIndex == 0 || cboCategoria.SelectedIndex == -1)
            {
                valida = false;
                errorProvider1.SetError(cboCategoria, "Seleccione una categoría");
            }
            if (cboProveedor.SelectedIndex == 0 || cboProveedor.SelectedIndex == -1)
            {
                valida = false;
                errorProvider1.SetError(cboProveedor, "Seleccione un proveedor");
            }
            if (txtProducto.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtProducto, "Ingrese producto");
            }
            if (txtPrecio.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtPrecio, "Ingrese precio");
            }
            if (txtUInventario.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtUInventario, "Ingrese unidades en inventario");
            }
            if (txtUPedido.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtUPedido, "Ingrese unidades en pedido");
            }
            if (txtPPedido.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtPPedido, "Ingrese punto de pedido");
            }
            return valida;
        }

        private void txtBIdIni_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdFin_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdIni_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);

        private void txtBIdFin_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtBIdIni, txtBIdFin);

        private void FrmProductosCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpConsultar)
            {
                if (cboCategoria.SelectedIndex != 0 || cboProveedor.SelectedIndex != 0 || txtId.Text != "" || txtProducto.Text != "" || txtCantidadxU.Text != "" || txtPrecio.Text != "" || txtUInventario.Text != "" || txtUPedido.Text != "" || txtPPedido.Text != "")
                {
                    DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (respuesta == DialogResult.No)
                        e.Cancel = true;
                }
            }
        }

        private void FrmProductosCrud_FormClosed(object sender, FormClosedEventArgs e) => Utils.ActualizarBarraDeEstado(this);

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpRegistrar)
            {
                DeshabilitarControles();
                DataGridViewRow dgvr = Dgv.CurrentRow;
                txtId.Text = dgvr.Cells["Id"].Value.ToString();
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE ProductId = @Id", cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("Id", txtId.Text);
                    if (cn.State != ConnectionState.Open) cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //int indexCategoria = cboCategoria.FindStringExact(reader["IdCategoria"].ToString());
                            //cboCategoria.SelectedIndex = indexCategoria;
                            //int indexProveedor = cboProveedor.FindStringExact(reader["IdProveedor"].ToString());
                            //cboProveedor.SelectedIndex = indexProveedor;
                            txtId.Tag = reader["RowVersion"];
                            cboCategoria.SelectedValue = reader["CategoryId"].ToString() == "" ? 0 : (int)reader["CategoryId"];
                            cboProveedor.SelectedValue = reader["SupplierId"].ToString() == "" ? 0 : (int)reader["SupplierId"];
                            txtProducto.Text = reader["ProductName"].ToString();
                            if (reader["QuantityPerUnit"].ToString() == "")
                                txtCantidadxU.Text = "";
                            else
                                txtCantidadxU.Text = reader["QuantityPerUnit"].ToString();
                            txtPrecio.Text = reader["UnitPrice"].ToString();
                            txtUInventario.Text = reader["UnitsInStock"].ToString();
                            txtUPedido.Text = reader["UnitsOnOrder"].ToString();
                            txtPPedido.Text = reader["ReorderLevel"].ToString();
                            chkbDescontinuado.Checked = (bool)reader["Discontinued"];
                        }
                        else
                        {
                            MessageBox.Show($"No se encontró el producto con Id: {txtId.Text}, es posible que otro usuario lo haya eliminado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (cn.State == ConnectionState.Open) cn.Close();
                            ActualizaDgv();
                            return;
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
                finally
                {
                    if (cn.State == ConnectionState.Open) cn.Close();
                }
                if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                    btnOperacion.Enabled = true;
            }
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarDatosProducto();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (EventoCargado)
                {
                    Dgv.CellClick -= new DataGridViewCellEventHandler(Dgv_CellClick);
                    EventoCargado = false;
                }
                BorrarDatosBusqueda();
                HabilitarControles();
                btnOperacion.Text = "Registrar producto";
                btnOperacion.Visible = true;
                btnOperacion.Enabled = true;
            }
            else
            {
                if (!EventoCargado)
                {
                    Dgv.CellClick += new DataGridViewCellEventHandler(Dgv_CellClick);
                    EventoCargado = true;
                }
                DeshabilitarControles();
                btnOperacion.Enabled = false;
                if (tabcOperacion.SelectedTab == tbpConsultar)
                {
                    btnOperacion.Visible = false;
                    btnOperacion.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    btnOperacion.Text = "Modificar producto";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar producto";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                }
            }
        }

        private void txtUInventario_Validating(object sender, CancelEventArgs e)
        {
            if (txtUInventario.Text.Trim() != "")
            {
                if (int.Parse(txtUInventario.Text) > 32767)
                {
                    errorProvider1.SetError(txtUInventario, "La cantidad no puede ser mayor a 32767");
                    e.Cancel = true;
                }
                else
                    errorProvider1.SetError(txtUInventario, "");
            }
        }

        private void txtUPedido_Validating(object sender, CancelEventArgs e)
        {
            if (txtUPedido.Text.Trim() != "")
            {
                if (int.Parse(txtUPedido.Text) > 32767)
                {
                    errorProvider1.SetError(txtUPedido, "La cantidad no puede ser mayor a 32767");
                    e.Cancel = true;
                }
                else
                    errorProvider1.SetError(txtUPedido, "");
            }
        }

        private void txtPPedido_Validating(object sender, CancelEventArgs e)
        {
            if (txtPPedido.Text.Trim() != "")
            {
                if (int.Parse(txtPPedido.Text) > 32767)
                {
                    errorProvider1.SetError(txtPPedido, "La cantidad no puede ser mayor a 32767");
                    e.Cancel = true;
                }
                else
                    errorProvider1.SetError(txtPPedido, "");
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosConPunto(sender, e);

        private void txtUInventario_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtUPedido_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtPPedido_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void btnOperacion_Click(object sender, EventArgs e)
        {
            int numRegs = 0;
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (ValidarControles())
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.insertandoRegistro);
                    DeshabilitarControles();
                    btnOperacion.Enabled = false;
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Productos_Insertar", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Categoria", cboCategoria.SelectedValue);
                        cmd.Parameters.AddWithValue("Proveedor", cboProveedor.SelectedValue);
                        cmd.Parameters.AddWithValue("Producto", txtProducto.Text);
                        if (txtCantidadxU.Text == "")
                            cmd.Parameters.AddWithValue("Cantidad", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Cantidad", txtCantidadxU.Text);
                        cmd.Parameters.AddWithValue("Precio", txtPrecio.Text);
                        cmd.Parameters.AddWithValue("UInventario", txtUInventario.Text);
                        cmd.Parameters.AddWithValue("UPedido", txtUPedido.Text);
                        cmd.Parameters.AddWithValue("PPedido", txtPPedido.Text);
                        cmd.Parameters.AddWithValue("Descontinuado", chkbDescontinuado.Checked);
                        cmd.Parameters.AddWithValue("Id", 0);
                        cmd.Parameters["Id"].Direction = ParameterDirection.Output;
                        if (cn.State != ConnectionState.Open) cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                        {
                            txtId.Text = cmd.Parameters["Id"].Value.ToString();
                            MessageBox.Show($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} se registró satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} NO fue registrado en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        if (cn.State == ConnectionState.Open) cn.Close();
                    }
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                    LlenarCombos();
                    ActualizaDgv();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpModificar)
            {
                if (ValidarControles())
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.modificandoRegistro);
                    DeshabilitarControles();
                    btnOperacion.Enabled = false;
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Productos_Actualizar_V3", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", txtId.Text);
                        cmd.Parameters.AddWithValue("Categoria", cboCategoria.SelectedValue);
                        cmd.Parameters.AddWithValue("Proveedor", cboProveedor.SelectedValue);
                        cmd.Parameters.AddWithValue("Producto", txtProducto.Text);
                        if (txtCantidadxU.Text == "")
                            cmd.Parameters.AddWithValue("Cantidad", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Cantidad", txtCantidadxU.Text);
                        cmd.Parameters.AddWithValue("Precio", txtPrecio.Text);
                        cmd.Parameters.AddWithValue("UInventario", txtUInventario.Text);
                        cmd.Parameters.AddWithValue("UPedido", txtUPedido.Text);
                        cmd.Parameters.AddWithValue("PPedido", txtPPedido.Text);
                        cmd.Parameters.AddWithValue("Descontinuado", chkbDescontinuado.Checked);
                        cmd.Parameters.AddWithValue("RowVersion", txtId.Tag);
                        if (cn.State != ConnectionState.Open) cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                            MessageBox.Show($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} se modificó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} NO fue modificado en la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        if (cn.State == ConnectionState.Open) cn.Close();
                    }
                    LlenarCombos();
                    ActualizaDgv();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpEliminar)
            {
                if (txtId.Text == "")
                {
                    MessageBox.Show("Seleccione el producto a eliminar", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult respuesta = MessageBox.Show($"¿Está seguro de eliminar el producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text}?", Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Yes)
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.eliminandoRegistro);
                    btnOperacion.Enabled = false;
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Productos_Eliminar_V3", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", txtId.Text);
                        cmd.Parameters.AddWithValue("RowVersion", txtId.Tag);
                        if (cn.State != ConnectionState.Open) cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                            MessageBox.Show($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} se eliminó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} NO se eliminó en la base de datos, es posible que otro usuario de la red lo haya modificado o eliminado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (SqlException ex) when (ex.Number == 547)
                    {
                        Utils.MsgCatchErrorRestriccionCF(this);
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
                        if (cn.State == ConnectionState.Open) cn.Close();
                    }
                    LlenarCombos();
                    ActualizaDgv();
                }
            }
        }

        private void ActualizaDgv() => btnLimpiar.PerformClick();

        private void LlenarCombos()
        {
            LlenarCboCategoria();
            LlenarCboProveedor();
        }
    }
}
