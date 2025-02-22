using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmClientesyProveedoresDirectorio : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);

        public FrmClientesyProveedoresDirectorio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmClientesyProveedoresDirectorio_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void FrmClientesyProveedoresDirectorio_Load(object sender, EventArgs e)
        {
            Utils.ConfDgv(Dgv);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (!checkBoxClientes.Checked & !checkBoxProveedores.Checked)
            {
                MessageBox.Show(Utils.errorCriterioSelec, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LlenarDgv();
        }

        private void LlenarDgv()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand();
                if (checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad Order by Relación, [Nombre de compañía]", cn);
                    groupBox1.Text = "» Directorio de clientes y proveedores «";
                }
                else if (checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad Where Relación = 'Cliente' Order by [Nombre de compañía]", cn);
                    groupBox1.Text = "» Directorio de clientes «";
                }
                else if (!checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad Where Relación = 'Proveedor' Order by [Nombre de compañía]", cn);
                    groupBox1.Text = "» Directorio de proveedores «";
                }
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                Dgv.DataSource = dt;
                ConfDgv();
                Utils.ActualizarBarraDeEstado(this, $"Se encontraron {dt.Rows.Count} registros");
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
            Dgv.Columns["Ciudad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["País"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Relación"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Teléfono"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Región"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Código postal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Fax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["País"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Relación"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Teléfono"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Región"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Código postal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Fax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Dgv.Columns["Nombre de compañía"].DisplayIndex = 0;
            Dgv.Columns["Nombre de contacto"].DisplayIndex = 1;
            Dgv.Columns["Relación"].DisplayIndex = 2;
            Dgv.Columns["Domicilio"].DisplayIndex = 3;
            Dgv.Columns["Ciudad"].DisplayIndex = 4;
            Dgv.Columns["Región"].DisplayIndex = 5;
            Dgv.Columns["Código postal"].DisplayIndex = 6;
            Dgv.Columns["País"].DisplayIndex = 7;
            Dgv.Columns["Teléfono"].DisplayIndex = 8;
            Dgv.Columns["Fax"].DisplayIndex = 9;
        }
    }
}
