using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmCategoriasCrud : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        bool EventoCargado = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar
        OpenFileDialog openFileDialog;

        public FrmCategoriasCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            splitContainer1.SplitterDistance = 250;
        }

        private void FrmCategoriasCrud_Load(object sender, EventArgs e)
        {
            // Establecer el tamaño inicial
            splitContainer1.SplitterDistance = 250;

            // Asociar el evento
            splitContainer1.SplitterMoved += new SplitterEventHandler(splitContainer1_SplitterMoved);

            DeshabilitarControles();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            txtCategoria.ReadOnly = txtDescripcion.ReadOnly = true;
            picFoto.Enabled = false;
        }

        private void HabilitarControles()
        {
            txtCategoria.ReadOnly = txtDescripcion.ReadOnly = false;
            picFoto.Enabled = true;
        }

        private void LlenarDgv(object sender)
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            try
            {
                SqlCommand cmd;
                if (sender == null)
                {
                    cmd = new SqlCommand("Sp_Categorias_Listar", cn);
                    cmd.Parameters.AddWithValue("top100", 0);
                }
                else
                {
                    cmd = new SqlCommand("Sp_Categorias_Buscar_V2", cn);
                    cmd.Parameters.AddWithValue("IdIni", txtBIdIni.Text);
                    cmd.Parameters.AddWithValue("IdFin", txtBIdFin.Text);
                    cmd.Parameters.AddWithValue("Categoria", txtBCategoria.Text);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                Dgv.DataSource = tbl;
                Utils.ConfDgv(Dgv);
                ConfDgv();
                if (sender == null)
                    Utils.ActualizarBarraDeEstado(this, $"Se muestran las últimas {Dgv.RowCount} categorías registradas");
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

        private void ConfDgv()
        {
            Dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Categoría"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Foto"].Width = 50;
            Dgv.Columns["Foto"].DefaultCellStyle.Padding = new Padding(4, 4, 4, 4);
            ((DataGridViewImageColumn)Dgv.Columns["Foto"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosCategoria();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosBusqueda();
            BorrarDatosCategoria();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
        }

        private void BorrarMensajesError()
        {
            errorProvider1.SetError(txtCategoria, "");
            errorProvider1.SetError(txtDescripcion, "");
            errorProvider1.SetError(btnCargar, "");
        }

        private bool ValidarControles()
        {
            bool valida = true;
            if (txtCategoria.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtCategoria, "Ingrese la categoría");
            }
            if (txtDescripcion.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtDescripcion, "Ingrese la descripción");
            }
            if (picFoto.Image == null)
            {
                valida = false;
                errorProvider1.SetError(btnCargar, "Ingrese la imagen");
            }
            return valida;
        }

        private void BorrarDatosCategoria()
        {
            txtCategoria.Text = txtDescripcion.Text = txtId.Text = "";
            picFoto.Image = null;
            picFoto.BackgroundImage = Properties.Resources.Categorias;
        }

        private void BorrarDatosBusqueda()
        {
            txtBCategoria.Text = txtBIdIni.Text = txtBIdFin.Text = "";
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpRegistrar)
            {
                DeshabilitarControles();
                DataGridViewRow dgvr = Dgv.CurrentRow;
                txtId.Text = dgvr.Cells["Id"].Value.ToString();
                txtCategoria.Text = dgvr.Cells["Categoría"].Value.ToString();
                txtDescripcion.Text = dgvr.Cells["Descripción"].Value.ToString();
                if (dgvr.Cells["Foto"].Value != DBNull.Value)
                {
                    byte[] foto = (byte[])dgvr.Cells["Foto"].Value;
                    MemoryStream ms;
                    if (int.Parse(txtId.Text) <= 8)
                    {
                        ms = new MemoryStream(foto, 78, foto.Length - 78);
                        btnCargar.Enabled = false; // no se permite modificar porque desconozco el formato de la imagen
                    }
                    else
                    {
                        ms = new MemoryStream(foto);
                        btnCargar.Enabled = true;
                    }
                    picFoto.Image = Image.FromStream(ms);
                    picFoto.BackgroundImage = null;
                }
                else
                {
                    picFoto.Image = null;
                    picFoto.BackgroundImage = Properties.Resources.Categorias;
                }
                if (tabcOperacion.SelectedTab == tbpConsultar)
                {
                    btnCargar.Visible = false;
                    btnOperacion.Visible = false;
                }
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    HabilitarControles();
                    btnOperacion.Visible = true;
                    btnCargar.Visible = true;
                    btnOperacion.Enabled = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnCargar.Visible = false;
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = true;
                }
            }
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarDatosCategoria();
            BorrarMensajesError();
            picFoto.BackgroundImage = Properties.Resources.Categorias;
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (EventoCargado)
                {
                    Dgv.CellClick -= new DataGridViewCellEventHandler(Dgv_CellClick);
                    EventoCargado = false;
                }
                HabilitarControles();
                btnOperacion.Text = "Registrar categoría";
                btnOperacion.Visible = true;
                btnOperacion.Enabled = true;
                btnCargar.Visible = true;
                btnCargar.Enabled = true;
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
                btnCargar.Enabled = false;
                if (tabcOperacion.SelectedTab == tbpConsultar)
                {
                    btnOperacion.Visible = false;
                    btnCargar.Visible = false;
                }
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    btnOperacion.Text = "Modificar categoría";
                    btnOperacion.Enabled = false;
                    btnOperacion.Visible = true;
                    btnCargar.Visible = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar categoría";
                    btnOperacion.Enabled = false;
                    btnOperacion.Visible = true;
                    btnCargar.Visible = false;
                }
            }
        }

        private void FrmCategoriasCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpConsultar)
            {
                if (txtId.Text != "" || txtCategoria.Text != "" || txtDescripcion.Text != "" || picFoto.Image != null)
                {
                    DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (respuesta == DialogResult.No)
                        e.Cancel = true;
                }
            }
        }

        private void FrmCategoriasCrud_FormClosed(object sender, FormClosedEventArgs e)
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
                    btnCargar.Enabled = false;
                    byte[] fileFoto = null;
                    Image image = picFoto.Image;
                    ImageConverter converter = new ImageConverter();
                    fileFoto = (byte[])converter.ConvertTo(image, typeof(byte[]));
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Categorias_Insertar", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Categoria", txtCategoria.Text);
                        cmd.Parameters.AddWithValue("Descripcion", txtDescripcion.Text);
                        cmd.Parameters.AddWithValue("Foto", fileFoto);
                        cmd.Parameters.AddWithValue("Id", 0);
                        cmd.Parameters["Id"].Direction = ParameterDirection.Output;
                        cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                        {
                            txtId.Text = cmd.Parameters["Id"].Value.ToString();
                            MessageBox.Show($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} se registró satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"La categoria con Id: {txtId.Text} y Nombre: {txtCategoria.Text} NO fue registrada en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                    btnCargar.Enabled = true;
                    picFoto.BackgroundImage = Properties.Resources.Categorias;
                    if (numRegs > 0)
                        BuscaReg();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpModificar)
            {
                if (ValidarControles())
                {
                    btnOperacion.Enabled = false;
                    btnCargar.Enabled = false;
                    Utils.ActualizarBarraDeEstado(this, Utils.modificandoRegistro);
                    DeshabilitarControles();
                    byte[] fileFoto = null;
                    Image image = picFoto.Image;
                    ImageConverter converter = new ImageConverter();
                    fileFoto = (byte[])converter.ConvertTo(image, typeof(byte[]));
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Categorias_Actualizar", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", txtId.Text);
                        cmd.Parameters.AddWithValue("Categoria", txtCategoria.Text);
                        cmd.Parameters.AddWithValue("Descripcion", txtDescripcion.Text);
                        cmd.Parameters.AddWithValue("Foto", fileFoto);
                        cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                            MessageBox.Show($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} se modificó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} NO fue modificada en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    picFoto.BackgroundImage = Properties.Resources.Categorias;
                    if (numRegs > 0)
                        BuscaReg();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpEliminar)
            {
                if (txtId.Text == "")
                {
                    MessageBox.Show("Seleccione la categoría a eliminar", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult respuesta = MessageBox.Show($"¿Esta seguro de eliminar la categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text}?", Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Yes)
                {
                    btnOperacion.Enabled = false;
                    Utils.ActualizarBarraDeEstado(this, Utils.eliminandoRegistro);
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Categorias_Eliminar", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Id", txtId.Text);
                        cn.Open();
                        numRegs = cmd.ExecuteNonQuery();
                        if (numRegs > 0)
                            MessageBox.Show($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} se eliminó satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} NO se eliminó en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        cn.Close();
                    }
                    if (numRegs > 0)
                    {
                        picFoto.BackgroundImage = Properties.Resources.Categorias;
                        BuscaReg();
                    }
                        
                }
            }
        }

        private void BuscaReg()
        {
            txtBIdIni.Text = txtBIdFin.Text = txtId.Text;
            btnBuscar.PerformClick();
            btnLimpiar.PerformClick();
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
                picFoto.BackgroundImage = null;
                errorProvider1.SetError(btnCargar, "");
            }
        }
    }
}
