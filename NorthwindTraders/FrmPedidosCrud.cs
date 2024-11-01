using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmPedidosCrud : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        bool eventoCargardo = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar
        int IdDetalle = 1;

        public FrmPedidosCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

    }
}
