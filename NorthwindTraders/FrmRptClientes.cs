using System;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptClientes : Form
    {
        public FrmRptClientes()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void FrmRptClientes_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'northwindDataSet.Customers' Puede moverla o quitarla según sea necesario.
            this.customersTableAdapter.Fill(this.northwindDataSet.Customers);

            this.reportViewer1.RefreshReport();
        }
    }
}
