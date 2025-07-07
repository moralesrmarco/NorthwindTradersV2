using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmLogin : Form
    {

        public bool IsAuthenticated { get; private set; } = false;
        public string UsuarioLogueado;
        public int IdUsuarioLogueado;
        bool _imagenMostrada = true;
        byte numeroIntentos = 0;

        public FrmLogin()
        {
            InitializeComponent();
            this.Text = Utils.nwtr;
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            var usuario = txtUsuario.Text.Trim();
            var pass = txtPwd.Text.Trim();
            if (ValidateUser(usuario, pass))
            {
                IsAuthenticated = true;
                UsuarioLogueado = usuario;
                this.Close();
            }
            else
            {
                numeroIntentos++;
                if (numeroIntentos >= 3)
                {
                    MessageBox.Show("Demasiados intentos fallidos. La aplicación se cerrará.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                MessageBox.Show("Error de autenticación.\nUsuario o contraseña incorrectos.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPwd.Clear();
                txtPwd.Focus();
            }
        }

        private bool ValidateUser(string usuario, string pass)
        {
            string hashed = Utils.ComputeSha256Hash(pass);
            using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
            {
                using (var cmd = new SqlCommand("SELECT Id FROM Usuarios WHERE Usuario = @usuario AND Password = @password AND Estatus = 1", cn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@password", hashed);
                    cn.Open();
                    //int count = (int)cmd.ExecuteScalar();
                    //return count > 0;
                    IdUsuarioLogueado = (int)cmd.ExecuteScalar();
                    return IdUsuarioLogueado > 0;
                }
            }
        }

        private void btnTogglePwd_Click(object sender, EventArgs e)
        {
            _imagenMostrada = !_imagenMostrada;
            txtPwd.UseSystemPasswordChar = !txtPwd.UseSystemPasswordChar;
            btnTogglePwd.Image = _imagenMostrada ? Properties.Resources.mostrarCh : Properties.Resources.ocultarCh;
        }
    }
}
