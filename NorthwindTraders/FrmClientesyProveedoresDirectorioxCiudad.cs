using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmClientesyProveedoresDirectorioxCiudad : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);

        public FrmClientesyProveedoresDirectorioxCiudad()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void grbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmClientesyProveedoresDirectorioxCiudad_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            Utils.ConfDgv(Dgv);
        }

        private void LlenarComboBox()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand("SP_CLIENTESPROVEEDORES_CIUDAD", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                comboBox.DataSource = tbl;
                comboBox.DisplayMember = "CiudadPaís";
                comboBox.ValueMember = "Ciudad";
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
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad Order by Ciudad, País, [Nombre de compañía]", cn);
                    titulo = "» Directorio de clientes y proveedores por ciudad [ Todas las ciudades ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorCiudad Where Ciudad = '{comboBox.SelectedValue.ToString()}' Order by País, [Nombre de compañía]", cn);
                    titulo = $"» Directorio de clientes y proveedores por ciudad [ Ciudad: {comboBox.SelectedValue.ToString()} ] «";
                }
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad Where Relación = 'Cliente' Order by Ciudad, País, [Nombre de compañía]", cn);
                    titulo = "» Directorio de clientes por ciudad [ Todas las ciudades ] «";
                }
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad Where Relación = 'Proveedor' Order by Ciudad, País, [Nombre de compañía]", cn);
                    titulo = "» Directorio de proveedores por ciudad [ Todas las ciudades ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorCiudad Where Ciudad = '{comboBox.SelectedValue.ToString()}' And Relación = 'Cliente' Order by País, [Nombre de compañía]", cn);
                    titulo = $"» Directorio de clientes por ciudad [ Ciudad: {comboBox.SelectedValue.ToString()} ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorCiudad Where Ciudad = '{comboBox.SelectedValue.ToString()}' And Relación = 'Proveedor' Order by País, [Nombre de compañía]", cn);
                    titulo = $"» Directorio de proveedores por ciudad [ Ciudad: {comboBox.SelectedValue.ToString()} ] «";
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
        }

        private void FrmClientesyProveedoresDirectorioxCiudad_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == 0 | (!checkBoxClientes.Checked & !checkBoxProveedores.Checked))
            {
                MessageBox.Show(Utils.errorCriterioSelec, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LlenarDgv();
        }
    }
}
