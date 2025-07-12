using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmPermisosCrud : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);

        public FrmPermisosCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void GrbPaint2(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmPermisosCrud_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmPermisosCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarListBoxCatalogo();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            listBoxCatalogo.Enabled = false;
            listBoxConcedidos.Enabled = false;
            listBoxCatalogo.Visible = false;
            listBoxConcedidos.Visible = false;
            txtUsuario.Visible = false;
            txtId.Visible = false;
            txtNombre.Visible = false;
            BtnAgregar.Enabled = BtnQuitar.Enabled = BtnAgregarTodos.Enabled = BtnQuitarTodos.Enabled = false;
        }

        private void HabilitarControles()
        {
            listBoxCatalogo.Enabled = true;
            listBoxConcedidos.Enabled = true;
            listBoxCatalogo.Visible = true;
            listBoxConcedidos.Visible = true;
            txtUsuario.Visible = true;
            txtId.Visible = true;
            txtNombre.Visible = true;
            BtnAgregar.Enabled = BtnQuitar.Enabled = BtnAgregarTodos.Enabled = BtnQuitarTodos.Enabled = true;
        }

        private void LlenarListBoxCatalogo()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                SqlCommand cmd = new SqlCommand("Select PermisoId, Descripción from CatalogoPermisos Where Estatus = 1 Order By PermisoId", cn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                listBoxCatalogo.DataSource = dt;
                listBoxCatalogo.DisplayMember = "Descripción";
                listBoxCatalogo.ValueMember = "PermisoId";
                listBoxCatalogo.SelectedIndex = -1;
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void LlenarDgv(object sender)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                SqlCommand cmd;
                if (sender == null)
                {
                    cmd = new SqlCommand("Select Top 20 Id, Paterno, Materno, Nombres, Usuario, FechaCaptura, FechaModificacion, Estatus from Usuarios Where Estatus = 1 order by Id Desc", cn);
                    cmd.CommandType = CommandType.Text;
                }
                else
                {
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cn;

                    // 1) Base de la consulta
                    var sql = new StringBuilder();
                    sql.AppendLine("Select ");
                    sql.AppendLine("Id, Paterno, Materno, Nombres, Usuario, FechaCaptura, FechaModificacion, Estatus ");
                    sql.AppendLine("From Usuarios ");
                    sql.AppendLine("Where 1 = 1"); // Facilita agregar AND condicionales
                    // 2) Filtros opcionales
                    if (!string.IsNullOrWhiteSpace(txtBIdIni.Text.Trim()) && Convert.ToInt32(txtBIdIni.Text) > 0)
                    {
                        sql.AppendLine(" And Id Between @IdIni And @IdFin ");
                        cmd.Parameters.AddWithValue("@IdIni", Convert.ToInt32(txtBIdIni.Text));
                        cmd.Parameters.AddWithValue("@IdFin", Convert.ToInt32(txtBIdFin.Text));
                    }
                    if (!string.IsNullOrWhiteSpace(txtBPaterno.Text.Trim()))
                    {
                        sql.AppendLine(" And Paterno Like @Paterno ");
                        cmd.Parameters.AddWithValue("@Paterno", $"%{txtBPaterno.Text.Trim()}%");
                    }
                    if (!string.IsNullOrWhiteSpace(txtBMaterno.Text.Trim()))
                    {
                        sql.AppendLine(" And Materno Like @Materno ");
                        cmd.Parameters.AddWithValue("@Materno", $"%{txtBMaterno.Text.Trim()}%");
                    }
                    if (!string.IsNullOrWhiteSpace(txtBNombres.Text.Trim()))
                    {
                        sql.AppendLine(" And Nombres Like @Nombres ");
                        cmd.Parameters.AddWithValue("@Nombres", $"%{txtBNombres.Text.Trim()}%");
                    }
                    if (!string.IsNullOrWhiteSpace(txtBUsuario.Text.Trim()))
                    {
                        sql.AppendLine(" And Usuario Like @Usuario ");
                        cmd.Parameters.AddWithValue("@Usuario", $"%{txtBUsuario.Text.Trim()}%");
                    }
                    sql.AppendLine(" And Estatus = 1 ");
                    // 3) Ordenamiento
                    sql.AppendLine(" Order By Paterno, Materno, Nombres, Usuario");
                    cmd.CommandText = sql.ToString();
                }
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);

                // Agrega una nueva columna "EstatusTexto" de tipo string
                dt.Columns.Add("EstatusTexto", typeof(string));

                // Llena la nueva columna con el texto equivalente
                foreach (DataRow row in dt.Rows)
                {
                    bool estatus = Convert.ToBoolean(row["Estatus"]);
                    row["EstatusTexto"] = estatus ? "Activo" : "Inactivo";
                }

                // Opcional: eliminar la columna original si ya no la necesitas
                dt.Columns.Remove("Estatus");

                // Opcional: renombrar la columna nueva para mantener el nombre original
                dt.Columns["EstatusTexto"].ColumnName = "Estatus";

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
        }

        private void ConfDgv()
        {
            Dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Paterno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Materno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Nombres"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Usuario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["FechaCaptura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["FechaModificacion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Estatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["Usuario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["FechaCaptura"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["FechaModificacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Estatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Dgv.Columns["Paterno"].HeaderText = "Apellido Paterno";
            Dgv.Columns["Materno"].HeaderText = "Apellido Materno";
            Dgv.Columns["FechaCaptura"].HeaderText = "Fecha de creación";
            Dgv.Columns["FechaModificacion"].HeaderText = "Fecha de modificación";

            Dgv.Columns["FechaCaptura"].DefaultCellStyle.Format = "dd/MMMM/yyyy\nhh:mm:ss tt";
            Dgv.Columns["FechaModificacion"].DefaultCellStyle.Format = "dd/MMMM/yyyy\nhh:mm:ss tt";
        }

        private void txtBIdIni_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdFin_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdIni_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);

        private void txtBIdFin_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtBIdIni, txtBIdFin);

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosPermisos();
            BorrarDatosBusqueda();
            DeshabilitarControles();
            LlenarDgv(null);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosPermisos();
            DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void BorrarDatosPermisos()
        {
            listBoxConcedidos.DataSource = null;
            listBoxConcedidos.Items.Clear();
        }

        private void BorrarDatosBusqueda()
        {
            txtBIdIni.Text = string.Empty;
            txtBIdFin.Text = string.Empty;
            txtBPaterno.Text = string.Empty;
            txtBMaterno.Text = string.Empty;
            txtBNombres.Text = string.Empty;
            txtBUsuario.Text = string.Empty;
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            HabilitarControles();
            if (e.RowIndex >= 0 && e.RowIndex < Dgv.RowCount)
            {
                DataGridViewRow row = Dgv.Rows[e.RowIndex];
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtUsuario.Text = row.Cells["Usuario"].Value.ToString();
                txtNombre.Text = $"{row.Cells["Paterno"].Value} {row.Cells["Materno"].Value} {row.Cells["Nombres"].Value}";
                LlenarListBoxConcedidos();
            }
        }

        private void LlenarListBoxConcedidos()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                SqlCommand cmd = new SqlCommand("Select P.PermisoId, CP.Descripción from Permisos P Inner join CatalogoPermisos CP On P.PermisoId = CP.PermisoId Where P.UsuarioId = @UsuarioId And CP.Estatus = 1 Order By P.PermisoId", cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UsuarioId", Convert.ToInt32(txtId.Text));
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                listBoxConcedidos.DataSource = dt;
                listBoxConcedidos.DisplayMember = "Descripción";
                listBoxConcedidos.ValueMember = "PermisoId";
                listBoxConcedidos.SelectedIndex = -1;
                MDIPrincipal.ActualizarBarraDeEstado();

            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void BtnAgregarTodos_Click(object sender, EventArgs e)
        {
            MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
            var existentes = new HashSet<int>(listBoxConcedidos.Items.OfType<DataRowView>().Select(drv => (int)drv["PermisoId"]));
            if (cn.State != ConnectionState.Open)
                cn.Open();
            using (var tran = cn.BeginTransaction())
            using (var cmd = cn.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.CommandText = "Insert Into Permisos (UsuarioId, PermisoId) Values (@UsuarioId, @PermisoId)";
                cmd.Parameters.AddWithValue("@UsuarioId", txtId.Text);  
                cmd.Parameters.Add("@PermisoId", SqlDbType.Int);
                try
                {
                    foreach (DataRowView drv in listBoxCatalogo.Items)
                    {
                        int permisoId = Convert.ToInt32(drv["PermisoId"]);
                        if (existentes.Contains(permisoId))
                        {
                            // Ya existe, lo ignoramos
                            continue;
                        }
                        cmd.Parameters["@PermisoId"].Value = permisoId;
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    LlenarListBoxConcedidos();
                    MessageBox.Show($"Se concedieron todos los permisos al usuario {txtUsuario.Text}", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Utils.MsgCatchOue(ex);
                }
            }
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        private void BtnQuitarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                SqlCommand cmd = new SqlCommand("Delete From Permisos Where UsuarioId = @UsuarioId", cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UsuarioId", Convert.ToInt32(txtId.Text));
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                int filasAfectadas = cmd.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    LlenarListBoxConcedidos();
                    MessageBox.Show($"Se eliminaron {filasAfectadas} permisos concedidos al usuario {txtUsuario.Text}", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"No se encontraron permisos concedidos al usuario {txtUsuario.Text}", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MDIPrincipal.ActualizarBarraDeEstado();
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

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (listBoxCatalogo.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un permiso del catálogo para agregarlo.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            using (var tran = cn.BeginTransaction())
            using(var cmd = cn.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.CommandText = "Insert Into Permisos (UsuarioId, PermisoId) Values (@UsuarioId, @PermisoId)";
                cmd.Parameters.AddWithValue("@UsuarioId", Convert.ToInt32(txtId.Text));
                cmd.Parameters.Add("@PermisoId", SqlDbType.Int);
                try
                {
                    int permisoId = Convert.ToInt32(listBoxCatalogo.SelectedValue);
                    cmd.Parameters["@PermisoId"].Value = permisoId;
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    LlenarListBoxConcedidos();
                }
                catch (SqlException ex) when (ex.Number == 2627) // Violación de clave única
                {
                    // Ya existe, no hacemos nada
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Utils.MsgCatchOue(ex);
                }
            }
            if (cn.State == ConnectionState.Open)
                cn.Close();
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        private void BtnQuitar_Click(object sender, EventArgs e)
        {
            if (listBoxConcedidos.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un permiso concedido para eliminarlo.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            using (var tran = cn.BeginTransaction())
            using (var cmd = cn.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.CommandText = "Delete From Permisos Where UsuarioId = @UsuarioId And PermisoId = @PermisoId";
                cmd.Parameters.AddWithValue("@UsuarioId", Convert.ToInt32(txtId.Text));
                cmd.Parameters.Add("@PermisoId", SqlDbType.Int);
                try
                {
                    int permisoId = Convert.ToInt32(listBoxConcedidos.SelectedValue);
                    cmd.Parameters["@PermisoId"].Value = permisoId;
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    LlenarListBoxConcedidos();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Utils.MsgCatchOue(ex);
                }
            }
            if (cn.State == ConnectionState.Open)
                cn.Close();
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string estado = e.Value.ToString();
            if (estado == "Activo")
            {
                e.CellStyle.BackColor = Color.LightGreen;
                e.CellStyle.ForeColor = Color.Black;
            }
            else if (estado == "Inactivo")
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White;
            }
        }
    }
}
