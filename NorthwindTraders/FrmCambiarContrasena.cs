using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmCambiarContrasena : Form
    {

        public string UsuarioLogueado;
        bool _imagenMostrada = true;

        public FrmCambiarContrasena()
        {
            InitializeComponent();
        }

        private void btnTogglePwd_Click(object sender, EventArgs e)
        {
            _imagenMostrada = !_imagenMostrada;
            txtPwd.UseSystemPasswordChar = !txtPwd.UseSystemPasswordChar;
            txtNewPwd.UseSystemPasswordChar = !txtNewPwd.UseSystemPasswordChar;
            txtConfirmarPwd.UseSystemPasswordChar = !txtConfirmarPwd.UseSystemPasswordChar;
            btnTogglePwd.Image = _imagenMostrada ? Properties.Resources.mostrarCh : Properties.Resources.ocultarCh;

        }

        private void FrmCambiarContrasena_Load(object sender, EventArgs e)
        {
            txtUsuario.Text = UsuarioLogueado;
            txtPwd.Focus();
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            PonerNoVisibleBtnTogglePwd();
            if (!ValidarNuevaContrasena())
                return;
            try
            {
                string pwdHasheada = Utils.ComputeSha256Hash(txtNewPwd.Text.Trim());
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (var cmd = new SqlCommand("UPDATE Usuarios SET Password = @password WHERE Usuario = @usuario AND Estatus = 1", cn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                        cmd.Parameters.AddWithValue("@password", pwdHasheada);
                        cn.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Contraseña cambiada correctamente.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo cambiar la contraseña. Verifique que su cuenta esté activa.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
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

        private bool ValidarNuevaContrasena()
        {
            errorProvider1.Clear();
            bool valida = true;
            txtPwd.Text = txtPwd.Text.Trim();
            if (string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                errorProvider1.SetError(txtPwd, "Debe ingresar su contraseña actual");
                valida = false;
            }
            if (valida)
            {
                try
                {
                    string pwdHasheada = Utils.ComputeSha256Hash(txtPwd.Text.Trim());
                    using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                    {
                        using (var cmd = new SqlCommand("SELECT COUNT(0) FROM Usuarios WHERE Usuario = @usuario AND Password = @password AND Estatus = 1", cn))
                        {
                            cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                            cmd.Parameters.AddWithValue("@password", pwdHasheada);
                            cn.Open();
                            int count = (int)cmd.ExecuteScalar();
                            if (count == 0)
                            {
                                errorProvider1.SetError(txtPwd, "La contraseña actual es incorrecta");
                                valida = false;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Utils.MsgCatchOueclbdd(ex);
                    valida = false;
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                    valida = false;
                }
                txtNewPwd.Text = txtNewPwd.Text.Trim();
                txtConfirmarPwd.Text = txtConfirmarPwd.Text.Trim();
                if (string.IsNullOrWhiteSpace(txtNewPwd.Text.Trim()))
                {
                    errorProvider1.SetError(txtNewPwd, "La nueva contraseña es obligatoria");
                    valida = false;
                }
                if (string.IsNullOrWhiteSpace(txtConfirmarPwd.Text.Trim()))
                {
                    errorProvider1.SetError(txtConfirmarPwd, "La confirmación de la contraseña es obligatoria");
                    valida = false;
                }
                if (valida)
                {
                    // Validar que las contraseñas coincidan
                    if (txtNewPwd.Text != txtConfirmarPwd.Text)
                    {
                        errorProvider1.SetError(txtNewPwd, "La nueva contraseña y la confirmación de la contraseña no coinciden");
                        errorProvider1.SetError(txtConfirmarPwd, "La nueva contraseña y la confirmación de la contraseña no coinciden");
                        valida = false;
                    }
                }
            }
            return valida;
        }

        private void PonerNoVisibleBtnTogglePwd()
        {
            txtPwd.UseSystemPasswordChar = txtNewPwd.UseSystemPasswordChar = txtConfirmarPwd.UseSystemPasswordChar = true;
            btnTogglePwd.Image = Properties.Resources.mostrarCh;
        }
    }
}
