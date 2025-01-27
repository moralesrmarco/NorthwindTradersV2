using System;
using System.Data;
using System.Data.SqlClient;
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
            if (int.Parse(txtCantidad.Text.Replace(",", "")) != CantidadOld || decimal.Parse(txtDescuento.Text) != DescuentoOld)
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
                Utils.ActualizarBarraDeEstado(this.Owner, Utils.clbdd);
                SqlCommand cmd = new SqlCommand($"Select Top 1 UnitsInStock From Products Where ProductId = {ProductoId}", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (rdr.Read())
                    UInventario = (short)rdr["UnitsInStock"];
                else
                    UInventario = null;
                rdr.Close();
                Utils.ActualizarBarraDeEstado(this.Owner);
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
            if (ValidarControles())
                CalcularImporte();
        }

        private void txtDescuento_Leave(object sender, EventArgs e)
        {
            if (ValidarControles())
                CalcularImporte();
        }

        private bool ValidarControles()
        {
            txtCantidad.Text = txtCantidad.Text.Replace(",", "");
            bool valida = true;
            btnModificar.Enabled = false;
            errorProvider1.Clear();

            int cantidadInt = 0, unidadesInventario = 0;
            short cantidad = 0, diferencia = 0;
            decimal descuento = 0;

            //Validar txtCantidad 
            if (!short.TryParse(txtCantidad.Text.Replace(",", ""), out cantidad))
            {
                valida = false;
                errorProvider1.SetError(txtCantidad, "Ingrese una cantidad valida, la cantidad debe ser mayor que 1 y menor que 32,767");
            }
            if (valida && cantidad == 0)
            {
                valida = false;
                errorProvider1.SetError(txtCantidad, "Ingrese la cantidad");
            }
            // Validar descuento
            if (string.IsNullOrWhiteSpace(txtDescuento.Text) || !decimal.TryParse(txtDescuento.Text, out descuento))
            {
                valida = false;
                errorProvider1.SetError(txtDescuento, "Ingrese el descuento");
            }
            else if (descuento > 1 || descuento < 0)
            {
                valida = false;
                errorProvider1.SetError(txtDescuento, "El descuento no puede ser mayor que 1 o menor que 0");
            }
            if (valida)
            {
                // Calcula la diferencia de cantidad
                diferencia = (short)(cantidad - CantidadOld);

                // Validar cantidad y unidades en inventario sean números válidos
                if (!int.TryParse(txtCantidad.Text.Replace(",", ""), out cantidadInt) || !int.TryParse(txtUinventario.Text.Replace(",", ""), out unidadesInventario))
                {
                    valida = false;
                    errorProvider1.SetError(txtCantidad, "Ingrese una cantidad válida");
                }
                // Verificar disponibilidad en el inventario
                if (valida && UInventario != null)
                {
                    // Aquí manejamos el caso de devolver productos al inventario
                    if (diferencia < 0)
                    {
                        // La validación es correcta al devolver productos
                        if (unidadesInventario + Math.Abs(diferencia) > 32767)
                        {
                            valida = false;
                            errorProvider1.SetError(txtCantidad, "La cantidad de producto devuelto mas las unidades en inventario exceden los 32,767 unidades");
                        }
                    }
                    // Aquí manejamos el caso de retirar productos del inventario
                    else if (diferencia > 0)
                    {
                        if (diferencia > unidadesInventario)
                        {
                            valida = false;
                            errorProvider1.SetError(txtCantidad, "La cantidad de productos en el pedido excede el inventario disponible");
                        }
                    }
                }
                else if (UInventario == null)
                {
                    valida = false;
                    errorProvider1.SetError(txtCantidad, "Es posible que el producto haya sido eliminado por otro usuario en la red");
                }
            }

            // Habilitar el botón Modificar si las cantidades y descuentos son válidos y han cambiado
            if (valida && (cantidad != CantidadOld || descuento != DescuentoOld))
                btnModificar.Enabled = true;
            return valida;
        }

        private void CalcularImporte()
        {
            Cantidad = short.Parse(txtCantidad.Text.Replace(",", ""));
            Descuento = decimal.Parse(txtDescuento.Text);
            Importe = (Precio * Cantidad) * (1 - Descuento);
            txtImporte.Text = Importe.ToString("c");
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosConPunto(sender, e);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            byte numRegs = 0;
            // No se realiza la validación porque ya se han realizado previamente en el evento leave de 
            // txtdescuento y txtcantidad
            try
            {
                btnModificar.Enabled = false;
                Utils.ActualizarBarraDeEstado(this.Owner, Utils.modificandoRegistro);
                SqlCommand cmd = new SqlCommand("Sp_PedidosDetalle_Actualizar_V2", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("OrderId", PedidoId);
                cmd.Parameters.AddWithValue("ProductId", ProductoId);
                cmd.Parameters.AddWithValue("Quantity", txtCantidad.Text);
                cmd.Parameters.AddWithValue("Discount", txtDescuento.Text);
                cmd.Parameters.AddWithValue("QuantityOld", CantidadOld);
                cmd.Parameters.AddWithValue("DiscountOld", DescuentoOld); // en realidad en esta etapa ya no se utiliza el descuentoold, se deja por claritad en la logica.
                cn.Open();
                numRegs = (byte)cmd.ExecuteNonQuery();
                if (numRegs == 0 )
                    MessageBox.Show("No se pudo realizar la modificación, es posible que el registro se haya eliminado previamente por otro usuario de la red", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(this.Owner, ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(this.Owner, ex);
            }
            finally
            {
                cn.Close();
            }
            if (numRegs > 0)
            {
                // Las siguientes dos lineas son necesarias para que se permita cerrar la ventana. 
                // ya que se validan las variables en FrmPedidosDetalleModificar_FormClosing
                CantidadOld = short.Parse(txtCantidad.Text);
                DescuentoOld = decimal.Parse(txtDescuento.Text);
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
