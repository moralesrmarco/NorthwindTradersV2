using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmProveedoresProductos : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        BindingSource bsProveedores = new BindingSource();
        BindingSource bsProductos = new BindingSource();

        public FrmProveedoresProductos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmProveedoresProductos_Load(object sender, EventArgs e)
        {
            DgvProveedores.DataSource = bsProveedores;
            DgvProductos.DataSource = bsProductos;
            GetData();
            Utils.ConfDgv(DgvProveedores);
            Utils.ConfDgv(DgvProductos);
            ConfDgvProveedores();
            ConfDgvProductos();
        }

        private void GetData()
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                DataSet ds = new DataSet();
                ds.Locale = System.Globalization.CultureInfo.InvariantCulture;
                SqlDataAdapter dapProveedores = new SqlDataAdapter("Sp_Proveedores_Listar", cn);
                dapProveedores.Fill(ds, "Proveedores");
                SqlDataAdapter dapProductos = new SqlDataAdapter("Sp_Productos_All", cn);
                dapProductos.Fill(ds, "Productos");
                // en la siguiente instrucción se deben de proporcionar los nombres de los campos (alias) que devuelve el store procedure
                DataRelation dataRelation = new DataRelation("ProveedoresProductos", ds.Tables["Proveedores"].Columns["Id"], ds.Tables["Productos"].Columns["IdProveedor"]);
                ds.Relations.Add(dataRelation);
                bsProveedores.DataSource = ds;
                bsProveedores.DataMember = "Proveedores";
                bsProductos.DataSource = bsProveedores;
                bsProductos.DataMember = "ProveedoresProductos";
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

        private void ConfDgvProveedores()
        {
            DgvProveedores.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Título de contacto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Ciudad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Región"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Código postal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["País"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Teléfono"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Fax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            DgvProveedores.Columns["Ciudad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProveedores.Columns["Región"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProveedores.Columns["Código postal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProveedores.Columns["País"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void ConfDgvProductos()
        {
            DgvProductos.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProductos.Columns["Precio"].DefaultCellStyle.Format = "c";
            DgvProductos.Columns["Unidades en inventario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProductos.Columns["Unidades en pedido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProductos.Columns["Punto de pedido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DgvProductos.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProductos.Columns["Precio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProductos.Columns["Unidades en inventario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgvProductos.Columns["Unidades en pedido"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgvProductos.Columns["Punto de pedido"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DgvProductos.Columns["Descontinuado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProductos.Columns["Categoría"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            DgvProductos.Columns["Proveedor"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            DgvProductos.Columns["IdCategoria"].Visible = false;
            DgvProductos.Columns["IdProveedor"].Visible = false;
        }

        private void FrmProveedoresProductos_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void DgvProveedores_SelectionChanged(object sender, EventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this, $"Se encontraron {DgvProveedores.RowCount} registros en proveedores y {DgvProductos.RowCount} registros de productos; del proveedor {DgvProveedores.CurrentRow.Cells["Nombre de compañía"].Value}");
        }

        private void DgvProveedores_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this, $"Se encontraron {DgvProveedores.RowCount} registros en proveedores");
        }
    }
}
