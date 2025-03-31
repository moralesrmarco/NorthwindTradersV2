using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmEmpleadosCrud : Form
    {
        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        bool EventoCargado = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar
        OpenFileDialog openFileDialog;

        public FrmEmpleadosCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void grbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmEmpleadosCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarCboPais();
            LlenarCboReportaA();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            txtNombres.ReadOnly = txtApellidos.ReadOnly = txtTitulo.ReadOnly = txtTitCortesia.ReadOnly = true;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = true;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtExtension.ReadOnly = true;
            dtpFNacimiento.Enabled = dtpFContratacion.Enabled = false;
            txtNotas.ReadOnly = false;
            cboReportaA.Enabled = false;
            picFoto.Enabled = false;
            btnCargar.Enabled = false;
        }

        private void HabilitarControles()
        {
            txtNombres.ReadOnly = txtApellidos.ReadOnly = txtTitulo.ReadOnly = false;
            txtTitCortesia.ReadOnly = false;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = false;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtExtension.ReadOnly = false;
            txtNotas.ReadOnly = false;
            dtpFNacimiento.Enabled = dtpFContratacion.Enabled = cboReportaA.Enabled = true;
            picFoto.Enabled = true;
            //btnCargar.Enabled = true;  // no se debe habilitar este control para los registros 1 al nueve
        }

        private void LlenarCboPais()
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Empleados_Pais", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                cboBPais.DataSource = tbl;
                cboBPais.DisplayMember = "País";
                cboBPais.ValueMember = "Id";
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

        private void LlenarCboReportaA()
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Empleados_Nombres_V2", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                cboReportaA.DataSource = tbl;
                cboReportaA.DisplayMember = "Nombre";
                cboReportaA.ValueMember = "Id";
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
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            try
            {
                SqlCommand cmd;
                if (sender == null)
                {
                    cmd = new SqlCommand("Sp_Empleados_Listar_V2", cn);
                    cmd.Parameters.AddWithValue("top100", 0);
                }
                else
                {
                    cmd = new SqlCommand("Sp_Empleados_Buscar_V3", cn);
                    cmd.Parameters.AddWithValue("IdIni", txtBIdIni.Text);
                    cmd.Parameters.AddWithValue("IdFin", txtBIdFin.Text);
                    cmd.Parameters.AddWithValue("Nombres", txtBNombres.Text);
                    cmd.Parameters.AddWithValue("Apellidos", txtBApellidos.Text);
                    cmd.Parameters.AddWithValue("Titulo", txtBTitulo.Text);
                    cmd.Parameters.AddWithValue("Domicilio", txtBDomicilio.Text);
                    cmd.Parameters.AddWithValue("Ciudad", txtBCiudad.Text);
                    cmd.Parameters.AddWithValue("Region", txtBRegion.Text);
                    cmd.Parameters.AddWithValue("CodigoP", txtBCodigoP.Text);
                    cmd.Parameters.AddWithValue("Pais", cboBPais.SelectedValue);
                    cmd.Parameters.AddWithValue("Telefono", txtBTelefono.Text);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                dgv.DataSource = tbl;
                Utils.ConfDgv(dgv);
                ConfDgvEmpleados(dgv);
                if (sender == null)
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran los últimos {dgv.RowCount} empleados registrados");
                else
                    Utils.ActualizarBarraDeEstado(this, $"Se encontraron {dgv.RowCount} registros");
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

        private void ConfDgvEmpleados(DataGridView dgv)
        {
            dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Foto"].Width = 20;
            dgv.Columns["Foto"].DefaultCellStyle.Padding = new Padding(2, 2, 2, 2);
            ((DataGridViewImageColumn)dgv.Columns["Foto"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgv.Columns["Título"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Fecha de nacimiento"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Ciudad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["País"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Reporta a"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Título"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Fecha de nacimiento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Ciudad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["País"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Reporta a"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Fecha de nacimiento"].DefaultCellStyle.Format = "dd \" de \"MMM\" de \"yyyy";
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosBusqueda();
            BorrarDatosEmpleado();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
        }

        private void BorrarMensajesError()
        {
            errorProvider1.SetError(txtId, "");
            errorProvider1.SetError(txtNombres, "");
            errorProvider1.SetError(txtApellidos, "");
            errorProvider1.SetError(txtTitulo, "");
            errorProvider1.SetError(txtTitCortesia, "");
            errorProvider1.SetError(txtDomicilio, "");
            errorProvider1.SetError(txtCiudad, "");
            errorProvider1.SetError(txtPais, "");
            errorProvider1.SetError(txtTelefono, "");
            errorProvider1.SetError(btnCargar, "");
            errorProvider1.SetError(dtpFNacimiento, "");
            errorProvider1.SetError(dtpFContratacion, "");
            errorProvider1.SetError(cboReportaA, "");
        }

        private void BorrarDatosBusqueda()
        {
            txtBIdIni.Text = txtBIdFin.Text = txtBNombres.Text = txtBApellidos.Text = string.Empty;
            txtBTitulo.Text = txtBDomicilio.Text = txtBCiudad.Text = string.Empty;
            txtBRegion.Text = txtBCodigoP.Text = txtBTelefono.Text = string.Empty;
            cboBPais.SelectedIndex = 0;
        }

        private void BorrarDatosEmpleado()
        {
            txtId.Text = txtNombres.Text = txtApellidos.Text = txtTitulo.Text = string.Empty;
            txtTitCortesia.Text = txtDomicilio.Text = txtCiudad.Text = string.Empty;
            txtRegion.Text = txtCodigoP.Text = txtTelefono.Text = txtPais.Text = string.Empty;
            txtExtension.Text = txtNotas.Text = string.Empty;
            cboReportaA.SelectedIndex = -1;
            picFoto.Image = Properties.Resources.FotoPerfil;
            dtpFNacimiento.Value = dtpFNacimiento.MinDate;
            dtpFContratacion.Value = dtpFContratacion.MinDate;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosEmpleado();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
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

        private bool ValidarControles()
        {
            bool valida = true;
            if (txtNombres.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtNombres, "Ingrese el nombre");
            }
            if (txtApellidos.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtApellidos, "Ingrese el apellido");
            }
            if (txtTitulo.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtTitulo, "Ingrese el título");
            }
            if (txtTitCortesia.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtTitCortesia, "Ingrese el título de cortesia");
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
            if (picFoto.Image == null)
            {
                valida = false;
                errorProvider1.SetError(btnCargar, "Ingrese la foto");
            }
            if (dtpFNacimiento.Value == new DateTime(1753, 1, 1))
            {
                valida = false;
                errorProvider1.SetError(dtpFNacimiento, "Ingrese la fecha de nacimiento");
            }
            if (dtpFContratacion.Value == new DateTime(1753, 1, 1))
            {
                valida = false;
                errorProvider1.SetError(dtpFContratacion, "Ingrese la fecha de contratación");
            }
            if (cboReportaA.SelectedValue.ToString() == "-1")
            {
                valida = false;
                errorProvider1.SetError(cboReportaA, "Seleccione a quien reporta el empleado");
            }
            return valida;
        }

        private void FrmEmpleadosCrud_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void FrmEmpleadosCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpListar)
                if (txtId.Text != "" || txtNombres.Text != "" || txtApellidos.Text != "" || txtTitulo.Text != "" || txtTitCortesia.Text != "" || txtDomicilio.Text != "" || txtCiudad.Text != "" || txtRegion.Text != "" || txtCodigoP.Text != "" || txtPais.Text != "" || txtTelefono.Text != "" || txtExtension.Text != "" || dtpFNacimiento.Value != dtpFNacimiento.MinDate || dtpFContratacion.Value != dtpFContratacion.MinDate || txtNotas.Text.Trim() != "" || cboReportaA.SelectedIndex > 0)
                {
                    DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (respuesta == DialogResult.No)
                        e.Cancel = true;
                }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpRegistrar)
            {
                DeshabilitarControles();
                DataGridViewRow dgvr = dgv.CurrentRow;
                txtId.Text = dgvr.Cells["Id"].Value.ToString();
                try
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT Employees.EmployeeID AS Id, Employees.FirstName AS Nombres, Employees.LastName AS Apellidos, Employees.Title AS Título, Employees.TitleOfCourtesy AS [Título de cortesia], Employees.BirthDate AS [Fecha de nacimiento], Employees.HireDate AS [Fecha de contratación], Employees.Address AS Domicilio, Employees.City AS Ciudad, Employees.Region AS Región, Employees.PostalCode AS [Código postal], Employees.Country AS País, Employees.HomePhone AS Teléfono, Employees.Extension AS Extensión, Employees.Photo AS Foto, Employees.Notes AS Notas, Employees.ReportsTo AS Reportaa, Employees_1.LastName + N', ' + Employees_1.FirstName AS [Reporta a], Employees.RowVersion FROM Employees LEFT OUTER JOIN Employees AS Employees_1 ON Employees.ReportsTo = Employees_1.EmployeeID Where Employees.EmployeeID = @Id", cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("Id", txtId.Text);
                    if (cn.State != ConnectionState.Open) cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            txtId.Tag = rdr["RowVersion"];
                            txtNombres.Text = rdr["Nombres"].ToString();
                            txtApellidos.Text = rdr["Apellidos"].ToString();
                            txtTitulo.Text = rdr["Título"].ToString();
                            txtTitCortesia.Text = rdr["Título de cortesia"].ToString();
                            txtDomicilio.Text = rdr["Domicilio"].ToString();
                            txtCiudad.Text = rdr["Ciudad"].ToString();
                            txtRegion.Text = rdr["Región"].ToString();
                            txtCodigoP.Text = rdr["Código postal"].ToString();
                            txtPais.Text = rdr["País"].ToString();
                            txtTelefono.Text = rdr["Teléfono"].ToString();
                            txtExtension.Text = rdr["Extensión"].ToString();
                            if (rdr["Fecha de nacimiento"] != DBNull.Value)
                                dtpFNacimiento.Value = DateTime.Parse(rdr["Fecha de nacimiento"].ToString());
                            else
                                dtpFNacimiento.Value = dtpFNacimiento.MinDate;
                            if (rdr["Fecha de contratación"] != DBNull.Value)
                                dtpFContratacion.Value = DateTime.Parse(rdr["Fecha de contratación"].ToString());
                            else
                                dtpFContratacion.Value = dtpFContratacion.MinDate;
                            if (rdr["Foto"] != DBNull.Value)
                            {
                                byte[] foto = (byte[])rdr["Foto"];
                                MemoryStream ms;
                                if (int.Parse(txtId.Text) <= 9)
                                {
                                    ms = new MemoryStream(foto, 78, foto.Length - 78);
                                    btnCargar.Enabled = false; // no se permite modificar porque desconozco el formato de la imagen.
                                }
                                else
                                {
                                    ms = new MemoryStream(foto);
                                    btnCargar.Enabled = true;
                                }
                                picFoto.Image = Image.FromStream(ms);
                            }
                            else
                                picFoto.Image = null;
                            txtNotas.Text = rdr["Notas"].ToString();
                            if (rdr["Reportaa"] != DBNull.Value)
                                cboReportaA.SelectedValue = rdr["Reportaa"].ToString();
                            else
                                cboReportaA.SelectedValue = 0;
                        }
                        else 
                        { 
                            MessageBox.Show($"No se encontró el empleado con Id: {txtId.Text}, es posible que otro usuario lo haya eliminado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (tabcOperacion.SelectedTab == tbpListar)
                {
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = true;
                    btnCargar.Visible = false;
                }
                if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    HabilitarControles();
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = true;
                    btnCargar.Visible = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Enabled = true;
                    btnOperacion.Visible = true;
                    btnCargar.Visible = false;
                }
            }
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarDatosEmpleado();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (EventoCargado)
                {
                    dgv.CellClick -= new DataGridViewCellEventHandler(dgv_CellClick);
                    EventoCargado = false;
                }
                BorrarDatosBusqueda();
                HabilitarControles();
                btnOperacion.Text = "Registrar empleado";
                btnOperacion.Visible = true;
                btnOperacion.Enabled = true;
                btnCargar.Enabled = true;
                btnCargar.Visible = true;
                cboReportaA.SelectedValue = -1;
            }
            else
            {
                if (!EventoCargado)
                {
                    dgv.CellClick += new DataGridViewCellEventHandler(dgv_CellClick);
                    EventoCargado = true;
                }
                DeshabilitarControles();
                btnOperacion.Enabled = false;
                btnCargar.Enabled = false;
                if (tabcOperacion.SelectedTab == tbpListar)
                {
                    btnOperacion.Text = "Imprimir empleado";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = true;
                    btnCargar.Visible = false;
                    btnCargar.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    btnOperacion.Text = "Modificar empleado";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                    btnCargar.Visible = true;
                    btnCargar.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar empleado";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                    btnCargar.Visible = false;
                    btnCargar.Enabled = false;
                }
            }
        }

        private void btnOperacion_Click(object sender, EventArgs e)
        {
            int numRegs = 0;
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpListar)
            {
                FrmRptEmpleado frmRptEmpleado = new FrmRptEmpleado();
                frmRptEmpleado.Owner = this;
                frmRptEmpleado.Id = int.Parse(txtId.Text);
                frmRptEmpleado.ShowDialog();
            }
            else if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (ValidarControles())
                {
                    Utils.ActualizarBarraDeEstado(this, Utils.insertandoRegistro);
                    DeshabilitarControles();
                    btnOperacion.Enabled = false;
                    byte[] byteFoto = null;
                    Image image = picFoto.Image;
                    ImageConverter converter = new ImageConverter();
                    byteFoto = (byte[])converter.ConvertTo(image, typeof(byte[]));
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Empleados_Insertar", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", 0);
                        cmd.Parameters["Id"].Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("Nombres", txtNombres.Text);
                        cmd.Parameters.AddWithValue("Apellidos", txtApellidos.Text);
                        cmd.Parameters.AddWithValue("Titulo", txtTitulo.Text);
                        cmd.Parameters.AddWithValue("TitCortesia", txtTitCortesia.Text);
                        cmd.Parameters.AddWithValue("FNacimiento", dtpFNacimiento.Value);
                        cmd.Parameters.AddWithValue("FContratacion", dtpFContratacion.Value);
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
                        if (txtExtension.Text == "")
                            cmd.Parameters.AddWithValue("Extension", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Extension", txtExtension.Text);
                        if (txtNotas.Text == "")
                            cmd.Parameters.AddWithValue("Notas", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Notas", txtNotas.Text);
                        if (int.Parse(cboReportaA.SelectedValue.ToString()) == 0)
                            //|| int.Parse(cboReportaA.SelectedValue.ToString()) == -1)
                            cmd.Parameters.AddWithValue("Reportaa", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Reportaa", cboReportaA.SelectedValue);
                        cmd.Parameters.AddWithValue("Foto", byteFoto);
                        if (cn.State != ConnectionState.Open) cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                        {
                            txtId.Text = cmd.Parameters["Id"].Value.ToString();
                            MessageBox.Show($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} se registró satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} NO fue registrado en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    byte[] byteFoto = null;
                    Image image = picFoto.Image;
                    ImageConverter converter = new ImageConverter();
                    byteFoto = (byte[])converter.ConvertTo(image, typeof(byte[]));
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Empleados_Actualizar_V3", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", txtId.Text);
                        cmd.Parameters.AddWithValue("Nombres", txtNombres.Text);
                        cmd.Parameters.AddWithValue("Apellidos", txtApellidos.Text);
                        cmd.Parameters.AddWithValue("Titulo", txtTitulo.Text);
                        cmd.Parameters.AddWithValue("TitCortesia", txtTitCortesia.Text);
                        cmd.Parameters.AddWithValue("FNacimiento", dtpFNacimiento.Value);
                        cmd.Parameters.AddWithValue("FContratacion", dtpFContratacion.Value);
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
                        if (txtExtension.Text == "")
                            cmd.Parameters.AddWithValue("Extension", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Extension", txtExtension.Text);
                        if (txtNotas.Text == "")
                            cmd.Parameters.AddWithValue("Notas", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Notas", txtNotas.Text);
                        if (int.Parse(cboReportaA.SelectedValue.ToString()) == 0)
                            cmd.Parameters.AddWithValue("Reportaa", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("Reportaa", cboReportaA.SelectedValue);
                        cmd.Parameters.AddWithValue("Foto", byteFoto);
                        cmd.Parameters.AddWithValue("RowVersion", txtId.Tag);
                        if (cn.State != ConnectionState.Open) cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                            MessageBox.Show($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} se modificó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} NO fue modificado en la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Seleccione el empleado a eliminar", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult respuesta = MessageBox.Show($"¿Esta seguro de eliminar el empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text}?", Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Yes)
                {
                    btnOperacion.Enabled = false;
                    Utils.ActualizarBarraDeEstado(this, Utils.eliminandoRegistro);
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Empleados_Eliminar_V3", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", txtId.Text);
                        cmd.Parameters.AddWithValue("RowVersion", txtId.Tag);
                        if (cn.State != ConnectionState.Open) cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                            MessageBox.Show($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} se eliminó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} NO se eliminó en la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnCargar_Click(object sender, EventArgs e)
        {
            // Mostrar el cuadro de diálogo OpenFileDialog
            //La instrucción siguiente es para que nos muestre todos los tipos juntos
            openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Archivos de imagen (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.InitialDirectory = "c:\\Imágenes\\";
            //La instrucción siguiente es para que nos muestre varias filas en el openfiledialog que nos permita abrir por un tipo especifico
            openFileDialog.Filter = "Archivos jpg (*.jpg)|*.jpg|Archivos jpeg (*.jpeg)|*.jpeg|Archivos png (*.png)|*.png|Archivos bmp (*.bmp)|*.bmp";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Cargar la imagen seleccionada en un objeto Image
                Image image = Image.FromFile(openFileDialog.FileName);

                // Mostrar la imagen en un control PictureBox
                picFoto.Image = image;
                errorProvider1.SetError(btnCargar, "");
            }
        }

        private void ActualizaDgv()
        {
            LlenarDgv(null);
            btnLimpiar.PerformClick();
        }

        private void LlenarCombos()
        {
            LlenarCboPais();
            LlenarCboReportaA();
        }
    }
}
