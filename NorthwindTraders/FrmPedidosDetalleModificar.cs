using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmPedidosDetalleModificar : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);

        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public decimal Precio { get; set; }
        public short Cantidad { get; set; }
        public decimal Descuento { get; set; }
        public decimal Importe { get; set; }
        public short? UInventario { get; set; }
        public short CantidadOld { get; set; }
        public decimal DescuentoOld { get; set; }

        public FrmPedidosDetalleModificar()
        {
            InitializeComponent();
        }

        private void FrmPedidosDetalleModificar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (short.Parse(txtCantidad.Text) != CantidadOld || decimal.Parse(txtDescuento.Text) != DescuentoOld)
            {
                DialogResult respuesta = MessageBox.Show(Utils.preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPedidosDetalleModificar_Load(object sender, EventArgs e)
        {
            ObtenerUInventario();
            txtPedido.Text = PedidoId.ToString();
            txtProducto.Text = Producto;
            txtPrecio.Text = Precio.ToString("c");
            txtUinventario.Text = string.Format("{0:n0}", UInventario);
            txtCantidad.Text = Cantidad.ToString("n0");
            txtDescuento.Text = Descuento.ToString("n2");
            txtImporte.Text = Importe.ToString("c");
            CantidadOld = Cantidad;
            DescuentoOld = Descuento;
        }

        private void ObtenerUInventario()
        {
            try
            {
                //Utils.ActualizarBarraDeEstado(this.ParentForm.ParentForm, Utils.clbdd);
                SqlCommand cmd = new SqlCommand($"Select Top 1 UnitsInStock From Products Where ProductId = {ProductoId}", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (rdr.Read())
                    UInventario = (short)rdr["UnitsInStock"];
                else
                    UInventario = null;
                rdr.Close();
                //Utils.ActualizarBarraDeEstado(this);
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
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            BorrarMensajesError();
            if (ValidarControles())
                CalcularImporte();
        }

        private void txtDescuento_Leave(object sender, EventArgs e)
        {
            BorrarMensajesError();
            if (ValidarControles())
                CalcularImporte();
        }

        private void BorrarMensajesError()
        {
            errorProvider1.SetError(txtCantidad, "");
            errorProvider1.SetError(txtDescuento, "");
        }

        private bool ValidarControles()
        {
            bool valida = true;
            try
            {
                if (txtCantidad.Text == "" || short.Parse(txtCantidad.Text.Replace(",", "")) == 0)
                {
                    valida = false;
                    errorProvider1.SetError(txtCantidad, "Ingrese la cantidad");
                }
            }
            catch (Exception ex) when (int.Parse(txtCantidad.Text) + int.Parse(txtUinventario.Text) > 32767)
            {
                valida = false;
                errorProvider1.SetError(txtCantidad, "La cantidad más las unidades en inventario, no puede ser mayor a 32,767");
                return valida;
            }
            if (UInventario != null)
            {
                if (short.Parse(txtCantidad.Text.Replace(",", "")) > short.Parse(txtUinventario.Text.Replace(",", "")))
                {
                    valida = false;
                    errorProvider1.SetError(txtCantidad, "La cantidad de productos en el pedido excede el inventario disponible");
                }
            }
            else
            {
                valida = false;
                errorProvider1.SetError(txtCantidad, "Es posible que el producto haya sido eliminado por otro usuario en la red");
            }
            if (txtDescuento.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtDescuento, "Ingrese el descuento");
            }
            if (decimal.Parse(txtDescuento.Text) > 1 || decimal.Parse(txtDescuento.Text) < 0)
            {
                valida = false;
                errorProvider1.SetError(txtDescuento, "El descuento no puede ser mayor que 1 o menor que 0");
            }
            return valida;
        }

        private void CalcularImporte()
        {
            Cantidad = short.Parse(txtCantidad.Text);
            Descuento = decimal.Parse(txtDescuento.Text);
            Importe = (Precio * Cantidad) * (1 - Descuento);
            txtImporte.Text = Importe.ToString("c");
        }
    }
}
