using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptClientesyProveedoresDirectorioxCiudad : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);

        public FrmRptClientesyProveedoresDirectorioxCiudad()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void FrmRptClientesyProveedoresDirectorioxCiudad_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
        }

        private void LlenarComboBox()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand("SP_CLIENTESPROVEEDORES_CIUDAD", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                comboBox.DataSource = dt;
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

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmRptClientesyProveedoresDirectorioxCiudad_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == 0 | (checkBoxClientes.Checked == false & checkBoxProveedores.Checked == false))
            {
                MessageBox.Show(Utils.errorCriterioSelec, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string titulo = string.Empty;
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand();
                if (comboBox.SelectedValue.ToString() == "aaaaa" & checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad_Rpt Order by Ciudad, Pais, NombreCompania", cn);
                    titulo = "» Reporte directorio de clientes y proveedores por ciudad [ Todas las ciudades ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorCiudad_Rpt Where Ciudad = '{comboBox.SelectedValue.ToString()}' Order by Pais, NombreCompania", cn);
                    titulo = $"» Reporte directorio de clientes y proveedores por ciudad [ Ciudad: {comboBox.SelectedValue.ToString()} ] «";
                }
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorCiudad_Rpt Where Relacion = 'Cliente' Order by Ciudad, Pais, NombreCompania", cn);
                    titulo = "» Reporte directorio de clientes por ciudad [ Todas las ciudades ] «";
                }
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorCiudad_Rpt Where Relacion = 'Proveedor' Order by Ciudad, Pais, NombreCompania", cn);
                    titulo = "» Reporte directorio de proveedores por ciudad [ Todas las ciudades ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorCiudad_Rpt Where Ciudad = '{comboBox.SelectedValue.ToString()}' And Relacion = 'Cliente' Order by Pais, NombreCompania", cn);
                    titulo = $"» Reporte directorio de clientes por ciudad [ Ciudad: {comboBox.SelectedValue.ToString()} ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorCiudad_Rpt Where Ciudad = '{comboBox.SelectedValue.ToString()}' And Relacion = 'Proveedor' Order by Pais, NombreCompania", cn);
                    titulo = $"» Reporte directorio de proveedores por ciudad [ Ciudad: {comboBox.SelectedValue.ToString()} ] «";
                }
                groupBox1.Text = titulo;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                Utils.ActualizarBarraDeEstado(this);
                ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                // Crear y configurar el parámetro
                ReportParameter parameter = new ReportParameter("titulo", titulo);
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { parameter });
                reportViewer1.RefreshReport();
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
    }
}
