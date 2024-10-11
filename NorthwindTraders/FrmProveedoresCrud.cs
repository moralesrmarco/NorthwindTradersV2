using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmProveedoresCrud : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        bool EventoCargado = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar

        public FrmProveedoresCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmProveedoresCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarCboPais();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            txtCompañia.ReadOnly = txtContacto.ReadOnly = txtTitulo.ReadOnly = true;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = true;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtFax.ReadOnly = true;
        }

        private void HabilitarControles()
        {
            txtCompañia.ReadOnly = txtContacto.ReadOnly = txtTitulo.ReadOnly = false;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = false;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtFax.ReadOnly = false;
        }

        private void LlenarCboPais()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand("SP_PROVEEDORES_PAIS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                cboBPais.DataSource = tbl;
                cboBPais.DisplayMember = "País";
                cboBPais.ValueMember = "Id";
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

        private void LlenarDgv(object sender)
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd;
                if (sender == null)
                {
                    cmd = new SqlCommand("Sp_Proveedores_Listar", cn);
                    cmd.Parameters.AddWithValue("top100", 0);
                }
                else
                {
                    cmd = new SqlCommand("Sp_Proveedores_Buscar_V2", cn);
                    cmd.Parameters.AddWithValue("IdIni", txtBIdIni.Text);
                    cmd.Parameters.AddWithValue("IdFin", txtBIdFin.Text);
                    cmd.Parameters.AddWithValue("Compañia", txtBCompañia.Text);
                    cmd.Parameters.AddWithValue("Contacto", txtBContacto.Text);
                    cmd.Parameters.AddWithValue("Domicilio", txtBDomicilio.Text);
                    cmd.Parameters.AddWithValue("Ciudad", txtBCiudad.Text);
                    cmd.Parameters.AddWithValue("Region", txtBRegion.Text);
                    cmd.Parameters.AddWithValue("CodigoP", txtBCodigoP.Text);
                    cmd.Parameters.AddWithValue("Pais", cboBPais.SelectedValue);
                    cmd.Parameters.AddWithValue("Telefono", txtBTelefono.Text);
                    cmd.Parameters.AddWithValue("Fax", txtBFax.Text);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                Dgv.DataSource = tbl;
                Utils.ConfDgv(Dgv);
                ConfDgv();
                if (sender == null)
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran los últimos {Dgv.RowCount} proveedores registrados");
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
            Dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Título de contacto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Ciudad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Región"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Código postal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["País"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Teléfono"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Fax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["Ciudad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Región"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Código postal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["País"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosBusqueda();
            BorrarDatosProveedor();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosProveedor();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void BorrarMensajesError()
        {
            errorProvider1.SetError(txtCompañia, "");
            errorProvider1.SetError(txtContacto, "");
            errorProvider1.SetError(txtTitulo, "");
            errorProvider1.SetError(txtDomicilio, "");
            errorProvider1.SetError(txtCiudad, "");
            errorProvider1.SetError(txtPais, "");
            errorProvider1.SetError(txtTelefono, "");
        }

        private void BorrarDatosBusqueda()
        {
            txtBIdIni.Text = txtBIdFin.Text = "";
            txtBCompañia.Text = txtContacto.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCodigoP.Text = "";
            cboBPais.SelectedIndex = 0;
            txtBTelefono.Text = txtBFax.Text = "";
        }

        private bool ValidarControles()
        {
            bool valida = true;
            if (txtCompañia.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtCompañia, "Ingrese el nombre de la compañía");
            }
            if (txtContacto.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtContacto, "Ingrese el nombre de contacto");
            }
            if (txtTitulo.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtTitulo, "Ingrese el título del contacto");
            }
            if (txtDomicilio.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtDomicilio, "Ingrese el domicilio");
            }
            if (txtCiudad.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtCiudad, "Ingrese la ciudad");
            }
            if (txtPais.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtPais, "Ingrese el país");
            }
            if (txtTelefono.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtTelefono, "Ingrese el teléfono");
            }
            return valida;
        }

        private void BorrarDatosProveedor()
        {
            txtId.Text = txtCompañia.Text = txtContacto.Text = txtTitulo.Text = "";
            txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCodigoP.Text = "";
            txtPais.Text = txtTelefono.Text = txtFax.Text = "";
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpRegistrar)
            {
                DeshabilitarControles();
                DataGridViewRow dgvr = Dgv.CurrentRow;
                txtId.Text = dgvr.Cells["Id"].Value.ToString();
                txtCompañia.Text = dgvr.Cells["Nombre de Compañía"].Value.ToString();
                txtContacto.Text = dgvr.Cells["Nombre de contacto"].Value.ToString();
                txtTitulo.Text = dgvr.Cells["Título de contacto"].Value.ToString();
                txtDomicilio.Text = dgvr.Cells["Domicilio"].Value.ToString();
                txtCiudad.Text = dgvr.Cells["Ciudad"].Value.ToString();
                txtRegion.Text = dgvr.Cells["Región"].Value.ToString();
                txtCodigoP.Text = dgvr.Cells["Código postal"].Value.ToString();
                txtPais.Text = dgvr.Cells["País"].Value.ToString();
                txtTelefono.Text = dgvr.Cells["Teléfono"].Value.ToString();
                txtFax.Text = dgvr.Cells["Fax"].Value.ToString();
            }
            if (tabcOperacion.SelectedTab == tbpModificar)
            {
                HabilitarControles();
                btnOperacion.Enabled = true;
            }
            else if (tabcOperacion.SelectedTab == tbpEliminar)
                btnOperacion.Enabled = true;
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosProveedor();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (EventoCargado)
                {
                    Dgv.CellClick -= new DataGridViewCellEventHandler(Dgv_CellClick);
                    EventoCargado = false;
                }
                BorrarDatosBusqueda();
                HabilitarControles();
                btnOperacion.Text = "Registrar proveedor";
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
                    btnOperacion.Visible = false;
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    btnOperacion.Text = "Modificar proveedor";
                    btnOperacion.Visible = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar proveedor";
                    btnOperacion.Visible = true;
                }
            }
        }

        private void FrmProveedoresCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpConsultar)
            {
                if (txtId.Text != "" || txtCompañia.Text != "" || txtContacto.Text != "" || txtTitulo.Text != "" || txtDomicilio.Text != "" || txtCiudad.Text != "" || txtRegion.Text != "" || txtCodigoP.Text != "" || txtPais.Text != "" || txtTelefono.Text != "" || txtFax.Text != "")
                {
                    DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (respuesta == DialogResult.No)
                        e.Cancel = true;
                }
            }
        }

        private void FrmProveedoresCrud_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

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
                        SqlCommand cmd = new SqlCommand("Sp_Proveedores_Insertar", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", 0);
                        cmd.Parameters["Id"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("Compañia", txtCompañia.Text);
                        cmd.Parameters.AddWithValue("Contacto", txtContacto.Text);
                        cmd.Parameters.AddWithValue("Titulo", txtTitulo.Text);
                        cmd.Parameters.AddWithValue("Domicilio", txtDomicilio.Text);
                        cmd.Parameters.AddWithValue("Ciudad", txtCiudad.Text);
                        if (txtRegion.Text == "")
                            cmd.Parameters.AddWithValue("Region", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Region", txtRegion.Text);
                        if (txtCodigoP.Text == "")
                            cmd.Parameters.AddWithValue("CodigoP", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("CodigoP", txtCodigoP.Text);
                        cmd.Parameters.AddWithValue("Pais", txtPais.Text);
                        cmd.Parameters.AddWithValue("Telefono", txtTelefono.Text);
                        if (txtFax.Text == "")
                            cmd.Parameters.AddWithValue("Fax", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Fax", txtFax.Text);
                        cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                        {
                            txtId.Text = cmd.Parameters["Id"].Value.ToString();
                            MessageBox.Show($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} se registró satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} NO fue registrado en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    LlenarCboPais();
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                    if (numRegs > 0)
                        BuscaReg();
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
                        SqlCommand cmd = new SqlCommand("Sp_Proveedores_Actualizar", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", txtId.Text);
                        cmd.Parameters.AddWithValue("Compañia", txtCompañia.Text);
                        cmd.Parameters.AddWithValue("Contacto", txtContacto.Text);
                        cmd.Parameters.AddWithValue("Titulo", txtTitulo.Text);
                        cmd.Parameters.AddWithValue("Domicilio", txtDomicilio.Text);
                        cmd.Parameters.AddWithValue("Ciudad", txtCiudad.Text);
                        if (txtRegion.Text == "")
                            cmd.Parameters.AddWithValue("Region", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Region", txtRegion.Text);
                        if (txtCodigoP.Text == "")
                            cmd.Parameters.AddWithValue("CodigoP", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("CodigoP", txtCodigoP.Text);
                        cmd.Parameters.AddWithValue("Pais", txtPais.Text);
                        cmd.Parameters.AddWithValue("Telefono", txtTelefono.Text);
                        if (txtFax.Text == "")
                            cmd.Parameters.AddWithValue("Fax", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Fax", txtFax.Text);
                        cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                            MessageBox.Show($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} se modificó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} NO fue modificado en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    LlenarCboPais();
                    if (numRegs > 0)
                        BuscaReg();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpEliminar)
            {
                if (txtId.Text == "")
                {
                    MessageBox.Show("Seleccione el proveedor a eliminar", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult respuesta = MessageBox.Show($"¿Esta seguro de eliminar el proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text}?", Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Yes)
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.eliminandoRegistro);
                    btnOperacion.Enabled = false;
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Proveedores_Eliminar", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", txtId.Text);
                        cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                            MessageBox.Show($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} se eliminó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} NO fue eliminado en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    LlenarCboPais();
                    if (numRegs > 0)
                        BuscaReg();
                }
            }
        }

        private void BuscaReg()
        {
            BorrarDatosBusqueda();
            txtBIdIni.Text = txtBIdFin.Text = txtId.Text;
            btnBuscar.PerformClick();
            btnLimpiar.PerformClick();
        }

        private void txtBIdIni_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtBIdFin_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtBIdIni_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);
        }

        private void txtBIdFin_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdFin(txtBIdIni, txtBIdFin);
        }
    }
}
