using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptClientesyProveedoresDirectorioxPais : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);

        public FrmRptClientesyProveedoresDirectorioxPais()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmRptClientesyProveedoresDirectorioxPais_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void FrmRptClientesyProveedoresDirectorioxPais_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
        }

        private void LlenarComboBox()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand("SP_CLIENTESPROVEEDORES_PAIS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                comboBox.DataSource = dt;
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

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex <= 0 | (!checkBoxClientes.Checked & !checkBoxProveedores.Checked))
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
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorPais_Rpt Order by Pais, Ciudad, NombreCompania", cn);
                    titulo = "» Reporte directorio de clientes y proveedores por país [ Todos los países ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & checkBoxProveedores.Checked )
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorPais_Rpt Where Pais = '{comboBox.SelectedValue.ToString()}' Order by Ciudad, NombreCompania", cn);
                    titulo = $"» Reporte directorio de clientes y proveedores por país [ País: {comboBox.SelectedValue.ToString()} ] «";
                }
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorPais_Rpt Where Relacion = 'Cliente' Order by Pais, Ciudad, NombreCompania", cn);
                    titulo = "» Reporte directorio de clientes por país [ Todos los países ] «";
                }
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorPais_Rpt Where Relacion = 'Proveedor' Order by Pais, Ciudad, NombreCompania", cn);
                    titulo = "» Reporte directorio de proveedores por país [ Todos los países ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorPais_Rpt Where Pais = '{comboBox.SelectedValue.ToString()}' And Relacion = 'Cliente' Order by Ciudad, NombreCompania", cn);
                    titulo = $"» Reporte directorio de clientes por país [ País: {comboBox.SelectedValue.ToString()} ] «";
                }
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand($"Select * from Vw_ClientesProveedores_DirectorioPorPais_Rpt Where Pais = '{comboBox.SelectedValue.ToString()}' And Relacion = 'Proveedor' Order by Ciudad, NombreCompania", cn);
                    titulo = $"» Reporte directorio de proveedores por país [ País: {comboBox.SelectedValue.ToString()} ] «";
                }
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                groupBox1.Text = titulo;
                Utils.ActualizarBarraDeEstado(this);
                if (dt.Rows.Count > 0)
                {
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rds);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
                    reportViewer1.LocalReport.Refresh();
                    reportViewer1.RefreshReport();
                }
                else
                {
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rds = new ReportDataSource("DataSet1", new DataTable());
                    reportViewer1.LocalReport.DataSources.Add(rds);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
                    reportViewer1.LocalReport.Refresh();
                    reportViewer1.RefreshReport();
                    MessageBox.Show(Utils.noDatos, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }
    }
}
