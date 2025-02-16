using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmClientesyProveedoresDirectorioxPais : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);

        public FrmClientesyProveedoresDirectorioxPais()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmClientesyProveedoresDirectorioxPais_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            Utils.ConfDgv(Dgv);
        }

        private void LlenarComboBox()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand("SP_CLIENTESPROVEEDORES_PAIS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                comboBox.DataSource = tbl;
                comboBox.DisplayMember = "País";
                comboBox.ValueMember = "IdPaís";
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

        private void LlenarDgv()
        {
            try
            {
                string titulo = string.Empty;
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand();
                if (comboBox.SelectedValue.ToString() == "aaaaa" & checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorPais Order by País, Ciudad, [Nombre de compañía]", cn);
                    titulo = "» Directorio de clientes y proveedores por país [ Todos los países ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorPais Where País = '{comboBox.SelectedValue.ToString()}' Order by Ciudad, [Nombre de compañía]", cn);
                    titulo = $"» Directorio de clientes y proveedores por país [ País: {comboBox.SelectedValue.ToString()} ] «";
                }
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorPais Where Relación = 'Cliente' Order by País, Ciudad, [Nombre de compañía]", cn);
                    titulo = "» Directorio de clientes por país [ Todos los países ] «";
                }
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorPais Where Relación = 'Proveedor' Order by País, Ciudad, [Nombre de compañía]", cn);
                    titulo = "» Directorio de proveedores por país [ Todos los países ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorPais Where País = '{comboBox.SelectedValue.ToString()}' And Relación = 'Cliente' Order by Ciudad, [Nombre de compañía]", cn);
                    titulo = $"» Directorio de clientes por país [ País: {comboBox.SelectedValue.ToString()} ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorPais Where País = '{comboBox.SelectedValue.ToString()}' And Relación = 'Proveedor' Order by Ciudad, [Nombre de compañía]", cn);
                    titulo = $"» Directorio de proveedores por país [ País: {comboBox.SelectedValue.ToString()} ] «";
                }
                cmd.CommandType = CommandType.Text;
                Grb.Text = titulo;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                Dgv.DataSource = tbl;
                ConfDgv();
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == 0 | (!checkBoxClientes.Checked & !checkBoxProveedores.Checked))
            {
                MessageBox.Show(Utils.errorCriterioSelec, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LlenarDgv();
        }

        private void ConfDgv()
        {
            Dgv.Columns["País"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Ciudad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Relación"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Teléfono"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Región"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Código postal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Fax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["Ciudad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Relación"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Teléfono"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Región"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Código postal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Fax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void FrmClientesyProveedoresDirectorioxPais_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }
    }
}
