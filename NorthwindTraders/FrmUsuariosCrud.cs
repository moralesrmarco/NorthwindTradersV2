using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmUsuariosCrud : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        bool EventoCargado = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv ojo no quitar
        bool _imagenMostrada = true;
        string passHasheadaOld = string.Empty; // Variable para almacenar la contraseña hasheada antes de modificarla
        string usuarioOld = string.Empty; // Variable para almacenar el usuario antes de modificarlo
        string paternoOld, maternoOld, nombresOld, pwdOld, pwdConfirmarOld; // Variables para almacenar los datos del usuario antes de modificarlo
        bool estatusOld; // Variable para almacenar el estatus del usuario antes de modificarlo

        public FrmUsuariosCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmUsuariosCrud_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmUsuariosCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpConsultar & tabcOperacion.SelectedTab != tbpEliminar)
            {
                if (paternoOld != txtPaterno.Text || maternoOld != txtMaterno.Text || nombresOld != txtNombres.Text || usuarioOld != txtUsuario.Text || pwdOld != txtPwd.Text || pwdConfirmarOld != txtConfirmarPwd.Text || estatusOld != chkbEstatus.Checked)
                {
                    DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (respuesta == DialogResult.No)
                        e.Cancel = true; // Cancela el cierre del formulario
                }
            }
        }

        private void FrmUsuariosCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            txtPaterno.ReadOnly = txtMaterno.ReadOnly = txtNombres.ReadOnly = txtUsuario.ReadOnly = txtPwd.ReadOnly = txtConfirmarPwd.ReadOnly = true;
            lblFechaCaptura.Text = lblFechaModificacion.Text = string.Empty;
            chkbEstatus.Enabled = false;
            btnTogglePwd1.Enabled = false;
        }

        private void HabilitarControles()
        {
            txtPaterno.ReadOnly = txtMaterno.ReadOnly = txtNombres.ReadOnly = txtUsuario.ReadOnly = txtPwd.ReadOnly = txtConfirmarPwd.ReadOnly = false;
            chkbEstatus.Enabled = true;
            btnTogglePwd1.Enabled = true;
        }

        private void LlenarDgv(object sender)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                SqlCommand cmd;
                if (sender == null)
                {
                    cmd = new SqlCommand("SELECT Top 20 Id, Paterno, Materno, Nombres, Usuario, Password, FechaCaptura, FechaModificacion, Estatus FROM Usuarios ORDER BY Id DESC", cn);
                    cmd.CommandType = CommandType.Text;
                }
                else
                {
                    cmd = new SqlCommand("Sp_Usuarios_Buscar", cn);
                    cmd.Parameters.AddWithValue("@IdIni", txtBIdIni.Text);
                    cmd.Parameters.AddWithValue("@IdFin", txtBIdFin.Text);
                    cmd.Parameters.AddWithValue("@Paterno", txtBPaterno.Text);
                    cmd.Parameters.AddWithValue("@Materno", txtBMaterno.Text);
                    cmd.Parameters.AddWithValue("@Nombres", txtBNombres.Text);
                    cmd.Parameters.AddWithValue("@Usuario", txtBUsuario.Text);
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                Dgv.DataSource = dt;
                Utils.ConfDgv(Dgv);
                ConfDgv();
                if (sender == null)
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran los últimos {Dgv.RowCount} usuarios registrados");
                else
                    MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {Dgv.RowCount} registros");
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }

        private void ConfDgv()
        {
            Dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Paterno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Materno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Nombres"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Usuario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Password"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["FechaCaptura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["FechaModificacion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Estatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["Usuario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Password"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["FechaCaptura"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["FechaModificacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Dgv.Columns["Paterno"].HeaderText = "Apellido Paterno";
            Dgv.Columns["Materno"].HeaderText = "Apellido Materno";
            Dgv.Columns["Password"].HeaderText = "Contraseña";
            Dgv.Columns["FechaCaptura"].HeaderText = "Fecha de captura";
            Dgv.Columns["FechaModificacion"].HeaderText = "Fecha de modificación";

            Dgv.Columns["FechaCaptura"].DefaultCellStyle.Format = "dd/MMMM/yyyy\nhh:mm:ss tt";
            Dgv.Columns["FechaModificacion"].DefaultCellStyle.Format = "dd/MMMM/yyyy\nhh:mm:ss tt";
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosUsuario();
            BorrarMensajesError();
            BorrarDatosBusqueda();
            PonerNoVisibleBtnTogglePwd1();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(null);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosUsuario();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void BorrarDatosUsuario()
        {
            txtId.Text = txtPaterno.Text = txtMaterno.Text = txtNombres.Text = txtUsuario.Text = txtPwd.Text = txtConfirmarPwd.Text = string.Empty;
            chkbEstatus.Checked = false;
            lblFechaCaptura.Text = lblFechaModificacion.Text = string.Empty;
        }

        private void BorrarMensajesError() => errorProvider1.Clear();

        private void BorrarDatosBusqueda()
        {
            txtBIdIni.Text = txtBIdFin.Text = txtBPaterno.Text = txtBMaterno.Text = txtBNombres.Text = txtBUsuario.Text = string.Empty;
        }

        private bool ValidarControles()
        {
            bool valida = true;
            if (string.IsNullOrWhiteSpace(txtNombres.Text))
            {
                errorProvider1.SetError(txtNombres, "El nombre es obligatorio");
                valida = false;
            }
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                errorProvider1.SetError(txtUsuario, "El usuario es obligatorio");
                valida = false;
            }
            if (string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                errorProvider1.SetError(txtPwd, "La contraseña es obligatoria");
                valida = false;
            }
            if (string.IsNullOrWhiteSpace(txtConfirmarPwd.Text))
            {
                errorProvider1.SetError(txtConfirmarPwd, "La confirmación de la contraseña es obligatoria");
                valida = false;
            }
            if (valida)
            {
                // Validar que el usuario no exista en la base de datos
                using (var cmd = new SqlCommand("Select Count(0) from Usuarios Where Usuario = @Usuario", cn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text.Trim());
                    if (cn.State != ConnectionState.Open)
                        cn.Open();
                    int numRegs = (int)cmd.ExecuteScalar();
                    cn.Close();
                    if (numRegs > 0)
                    {
                        errorProvider1.SetError(txtUsuario, "El usuario ya existe, por favor elige otro");
                        valida = false;
                    }
                    // Validar que las contraseñas coincidan
                    if (txtPwd.Text != txtConfirmarPwd.Text)
                    {
                        errorProvider1.SetError(txtPwd, "Las contraseñas no coinciden");
                        errorProvider1.SetError(txtConfirmarPwd, "Las contraseñas no coinciden");
                        valida = false;
                    }
                }
            }
            return valida;
        }

        private void txtBIdIni_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdFin_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdIni_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);

        private void txtBIdFin_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtBIdIni, txtBIdFin);

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpRegistrar) 
            {
                PonerNoVisibleBtnTogglePwd1();
                DeshabilitarControles();
                DataGridViewRow dgvr = Dgv.CurrentRow;
                if (dgvr != null)
                {
                    txtId.Text = dgvr.Cells["Id"].Value.ToString();
                    paternoOld = txtPaterno.Text = dgvr.Cells["Paterno"].Value.ToString();
                    maternoOld = txtMaterno.Text = dgvr.Cells["Materno"].Value.ToString();
                    nombresOld =txtNombres.Text = dgvr.Cells["Nombres"].Value.ToString();
                    usuarioOld = txtUsuario.Text = dgvr.Cells["Usuario"].Value.ToString();

                    pwdOld = txtPwd.Text = dgvr.Cells["Password"].Value.ToString();

                    pwdConfirmarOld = txtConfirmarPwd.Text = txtPwd.Text; // Para que coincidan al editar
                    if (dgvr.Cells["FechaCaptura"].Value != null)
                        lblFechaCaptura.Text = Convert.ToDateTime(dgvr.Cells["FechaCaptura"].Value).ToString("dd/MMMM/yyyy hh:mm:ss tt");
                    else
                        lblFechaCaptura.Text = "Nulo";
                    if (dgvr.Cells["FechaModificacion"].Value != null)
                        lblFechaModificacion.Text = Convert.ToDateTime(dgvr.Cells["FechaModificacion"].Value).ToString("dd/MMMM/yyyy hh:mm:ss tt");
                    else
                        lblFechaModificacion.Text = "Nulo";
                    estatusOld = chkbEstatus.Checked = Convert.ToBoolean(dgvr.Cells["Estatus"].Value);
                    passHasheadaOld = txtPwd.Text.Trim(); // Almacena la contraseña hasheada antes de modificarla
                }
                if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                    btnTogglePwd1.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                    btnOperacion.Enabled = true;
            }
        }

        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Dgv.Columns[e.ColumnIndex].Name == "Password" && e.Value != null)
                e.Value = new string('●', 10);
                //e.Value = new string('●', e.Value.ToString().Length); // Oculta la contraseña con asteriscos
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarDatosUsuario();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (EventoCargado)
                {
                    Dgv.CellClick -= Dgv_CellClick; // Desvincula el evento para evitar que se ejecute al cambiar de pestaña
                    EventoCargado = false; // Cambia el estado para evitar que se ejecute nuevamente
                }
                paternoOld = maternoOld = nombresOld = usuarioOld = pwdOld = pwdConfirmarOld = string.Empty; // Limpia las variables de los datos del usuario
                estatusOld = false; // Limpia la variable del estatus del usuario
                BorrarDatosBusqueda();
                HabilitarControles();
                btnOperacion.Text = "Registrar usuario";
                btnOperacion.Visible = true;
                btnOperacion.Enabled = true;
            }
            else
            {
                if (!EventoCargado)
                {
                    Dgv.CellClick += Dgv_CellClick; // Vuelve a vincular el evento al cambiar de pestaña
                    EventoCargado = true; // Cambia el estado para permitir que se ejecute nuevamente
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
                    btnOperacion.Text = "Modificar usuario";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar usuario";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                }
            }
        }

        private void btnOperacion_Click(object sender, EventArgs e)
        {
            int numRegs = 0;
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (ValidarControles())
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
                    DeshabilitarControles();
                    PonerNoVisibleBtnTogglePwd1();
                    btnOperacion.Enabled = false;
                    try
                    {
                        string passHasheada = Utils.ComputeSha256Hash(txtPwd.Text.Trim());
                        SqlCommand cmd = new SqlCommand("Sp_Usuarios_Insertar", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Paterno", txtPaterno.Text.Trim());
                        cmd.Parameters.AddWithValue("@Materno", txtMaterno.Text.Trim());
                        cmd.Parameters.AddWithValue("@Nombres", txtNombres.Text.Trim());
                        cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", passHasheada);
                        cmd.Parameters.AddWithValue("@FechaCaptura", DateTime.Now);
                        cmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now); 
                        cmd.Parameters.AddWithValue("@Estatus", chkbEstatus.Checked);
                        cmd.Parameters.AddWithValue("@Id", 0);
                        cmd.Parameters["@Id"].Direction = ParameterDirection.Output;
                        if (cn.State != ConnectionState.Open)
                            cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                        {
                            txtId.Text = cmd.Parameters["@Id"].Value.ToString();
                            lblFechaCaptura.Text = cmd.Parameters["@FechaCaptura"].Value.ToString() == null ? "Nulo" : Convert.ToDateTime(cmd.Parameters["@FechaCaptura"].Value).ToString("dd/MMMM/yyyy hh:mm:ss tt");
                            lblFechaModificacion.Text = cmd.Parameters["@FechaModificacion"].Value.ToString() == null ? "Nulo" : Convert.ToDateTime(cmd.Parameters["@FechaModificacion"].Value).ToString("dd/MMMM/yyyy hh:mm:ss tt");
                            MessageBox.Show($"El usuario con Id: {txtId.Text} y Nombre: {txtPaterno.Text} {txtMaterno.Text} {txtNombres.Text} se registró satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"El usuario con Id: {txtId.Text} y Nombre: {txtPaterno.Text} {txtMaterno.Text} {txtNombres.Text} NO fue registrado en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601) // Error de clave duplicada
                    {
                        errorProvider1.SetError(txtUsuario, "El usuario ya existe, por favor elige otro");
                        return;
                    }
                    catch (SqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    finally
                    {
                        if (cn.State == ConnectionState.Open)
                            cn.Close();
                    }
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                    btnLimpiar.PerformClick();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpModificar)
            {
                if (txtUsuario.Text.Trim() == usuarioOld & ValidarControles1())
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.modificandoRegistro);
                    DeshabilitarControles();
                    PonerNoVisibleBtnTogglePwd1();
                    btnOperacion.Enabled = false;
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Update Usuarios Set Paterno = @Paterno, Materno = @Materno, Nombres = @Nombres, Usuario = @Usuario, Password = @Password, FechaModificacion = @FechaModificacion, Estatus = @Estatus Where Id = @Id", cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Paterno", txtPaterno.Text.Trim());
                        cmd.Parameters.AddWithValue("@Materno", txtMaterno.Text.Trim());
                        cmd.Parameters.AddWithValue("@Nombres", txtNombres.Text.Trim());
                        cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text.Trim());
                        if (txtPwd.Text.Trim() != passHasheadaOld)
                        {
                            string passHasheada = Utils.ComputeSha256Hash(txtPwd.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", passHasheada);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Password", passHasheadaOld);
                        }
                        cmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Estatus", chkbEstatus.Checked);
                        cmd.Parameters.AddWithValue("@Id", txtId.Text.Trim());
                        if (cn.State != ConnectionState.Open) cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                        {
                            lblFechaModificacion.Text = Convert.ToDateTime(cmd.Parameters["@FechaModificacion"].Value).ToString("dd/MMMM/yyyy hh:mm:ss tt");
                            MessageBox.Show($"El usuario con Id: {txtId.Text} y Nombre: {txtPaterno.Text} {txtMaterno.Text} {txtNombres.Text} se modificó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"El usuario con Id: {txtId.Text} y Nombre: {txtPaterno.Text} {txtMaterno.Text} {txtNombres.Text} NO se modificó en la base de datos, es posible que otro usuario de la red lo haya modificado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (SqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    finally
                    {
                        if (cn.State == ConnectionState.Open) cn.Close();
                    }
                    btnLimpiar.PerformClick();
                } 
                else if (txtUsuario.Text.Trim() != usuarioOld & ValidarControles())
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.modificandoRegistro);
                    DeshabilitarControles();
                    PonerNoVisibleBtnTogglePwd1();
                    btnOperacion.Enabled = false;
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Update Usuarios Set Paterno = @Paterno, Materno = @Materno, Nombres = @Nombres, Usuario = @Usuario, Password = @Password, FechaModificacion = @FechaModificacion, Estatus = @Estatus Where Id = @Id", cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Paterno", txtPaterno.Text.Trim());
                        cmd.Parameters.AddWithValue("@Materno", txtMaterno.Text.Trim());
                        cmd.Parameters.AddWithValue("@Nombres", txtNombres.Text.Trim());
                        cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text.Trim());
                        if (txtPwd.Text.Trim() != passHasheadaOld)
                        {
                            string passHasheada = Utils.ComputeSha256Hash(txtPwd.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", passHasheada);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Password", passHasheadaOld);
                        }
                        cmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Estatus", chkbEstatus.Checked);
                        cmd.Parameters.AddWithValue("@Id", txtId.Text.Trim());
                        if (cn.State != ConnectionState.Open) cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                        {
                            lblFechaModificacion.Text = Convert.ToDateTime(cmd.Parameters["@FechaModificacion"].Value).ToString("dd/MMMM/yyyy hh:mm:ss tt");
                            MessageBox.Show($"El usuario con Id: {txtId.Text} y Nombre: {txtPaterno.Text} {txtMaterno.Text} {txtNombres.Text} se modificó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"El usuario con Id: {txtId.Text} y Nombre: {txtPaterno.Text} {txtMaterno.Text} {txtNombres.Text} NO se modificó en la base de datos, es posible que otro usuario de la red lo haya modificado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (SqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    finally
                    {
                        if (cn.State == ConnectionState.Open) cn.Close();
                    }
                    btnLimpiar.PerformClick();
                }

            }
            else if (tabcOperacion.SelectedTab == tbpEliminar)
            {
                if (txtId.Text == "")
                {
                    MessageBox.Show("Seleccione el usuario a eliminar", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult respuesta = MessageBox.Show($"¿Está seguro de eliminar el usuario con Id: {txtId.Text} y Nombre: {txtPaterno.Text} {txtMaterno.Text} {txtNombres.Text}, tenga en cuenta que también se eliminaran los permisos que se le hayan concedido en el sistema?", Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Yes)
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                    btnOperacion.Enabled = false;
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Delete from Usuarios where Id = @Id", cn);
                        cmd.Parameters.AddWithValue("@Id", txtId.Text);
                        if (cn.State != ConnectionState.Open) cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                            MessageBox.Show($"El usuario con Id: {txtId.Text} y Nombre: {txtPaterno.Text} {txtMaterno.Text} {txtNombres.Text} se eliminó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"El usuario con Id: {txtId.Text} y Nombre: {txtPaterno.Text} {txtMaterno.Text} {txtNombres.Text} NO se eliminó en la base de datos, es posible que otro usuario de la red lo haya eliminado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (SqlException ex) { Utils.MsgCatchOueclbdd(ex); }
                    catch (Exception ex) { Utils.MsgCatchOue(ex); }
                    finally
                    {
                        if (cn.State == ConnectionState.Open) cn.Close();
                    }
                    btnLimpiar.PerformClick();
                }
            }
        }

        private void btnTogglePwd1_Click(object sender, EventArgs e)
        {
            _imagenMostrada = !_imagenMostrada;
            txtPwd.UseSystemPasswordChar = !txtPwd.UseSystemPasswordChar;
            txtConfirmarPwd.UseSystemPasswordChar = !txtConfirmarPwd.UseSystemPasswordChar;
            btnTogglePwd1.Image = _imagenMostrada ? Properties.Resources.mostrarCh : Properties.Resources.ocultarCh;
        }

        private void PonerNoVisibleBtnTogglePwd1()
        {
            txtPwd.UseSystemPasswordChar = txtConfirmarPwd.UseSystemPasswordChar = true;
            btnTogglePwd1.Image = Properties.Resources.mostrarCh;
        }

        private bool ValidarControles1()
        {
            bool valida = true;
            if (string.IsNullOrWhiteSpace(txtNombres.Text))
            {
                errorProvider1.SetError(txtNombres, "El nombre es obligatorio");
                valida = false;
            }
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                errorProvider1.SetError(txtUsuario, "El usuario es obligatorio");
                valida = false;
            }
            if (string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                errorProvider1.SetError(txtPwd, "La contraseña es obligatoria");
                valida = false;
            }
            if (string.IsNullOrWhiteSpace(txtConfirmarPwd.Text))
            {
                errorProvider1.SetError(txtConfirmarPwd, "La confirmación de la contraseña es obligatoria");
                valida = false;
            }
            if (valida)
            {
                // Validar que las contraseñas coincidan
                if (txtPwd.Text != txtConfirmarPwd.Text)
                {
                    errorProvider1.SetError(txtPwd, "Las contraseñas no coinciden");
                    errorProvider1.SetError(txtConfirmarPwd, "Las contraseñas no coinciden");
                    valida = false;
                }
            }
            return valida;
        }

        private void txtPwd_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void txtConfirmarPwd_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void txtPwd_TextChanged(object sender, EventArgs e)
        {
            if (tabcOperacion.SelectedTab == tbpModificar)
                if (txtPwd.Text.Trim() != passHasheadaOld)
                    btnTogglePwd1.Enabled = true;
                else
                    btnTogglePwd1.Enabled = false;
        }

        private void txtConfirmarPwd_TextChanged(object sender, EventArgs e)
        {
            if (tabcOperacion.SelectedTab == tbpModificar)
                if (txtConfirmarPwd.Text.Trim() != passHasheadaOld)
                    btnTogglePwd1.Enabled = true;
                else
                    btnTogglePwd1.Enabled = false;
        }
    }
}
