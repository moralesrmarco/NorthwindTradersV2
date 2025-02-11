using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptEmpleado : Form
    {
        //SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);

        public int Id { get; set; }

        public FrmRptEmpleado()
        {
            InitializeComponent();
        }

        private void FrmRptEmpleado_Load(object sender, EventArgs e)
        {
            this.employeeTableAdapter.Fill(this.northwindDataSet.Employee, Id);
            this.reportViewer1.RefreshReport();
        }
    }
}
