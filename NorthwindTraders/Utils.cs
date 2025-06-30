using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace NorthwindTraders
{
    internal class Utils
    {
        #region VariablesGlobales
        public static string clbdd = "Consultando la base de datos... ";
        public static string oueclbdd = "Ocurrio un error con la base de datos:\n";
        public static string oue = "Ocurrio un error:\n";
        public static string nwtr = "Northwind Traders Ver 2.";
        public static string preguntaCerrar = "¿Esta seguro de querer cerrar el formulario?, si responde SI, se perderan los datos no guardados";
        public static string insertandoRegistro = "Insertando registro en la base de datos...";
        public static string modificandoRegistro = "Modificando registro en la base de datos...";
        public static string eliminandoRegistro = "Eliminando registro en la base de datos...";
        public static string errorRestriccionCF = "Error: No se puede eliminar el registro debido a una restricción de clave foránea";
        public static string errorClaveDuplicada = "Error: No se puede insertar una clave duplicada en el objeto. Infracción de la restricción PRIMARY KEY";
        public static string errorCriterioSelec = "Error: Proporcione los criterios de selección";
        public static string noDatos = "No se encontraron datos para mostrar en el reporte";
        #endregion

        public static string ComputeSha256Hash(string rawData)
        {
            // 1. Instancia el algoritmo
            using (SHA256 sha256 = SHA256.Create())
            {
                // 2. Convierte la cadena a bytes (UTF-8)
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // 3. Convierte cada byte a su representación hex (2 dígitos)
                var builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));

                // 4. Retorna el string hex completo
                return builder.ToString();
            }
        }

        public static DateTime? ObtenerFechaHora(DateTimePicker dtpFecha, DateTimePicker dtpHora)
        {
            if (!dtpFecha.Checked)
                return null;
            return dtpFecha.Value.Date.Add(dtpHora.Value.TimeOfDay);
        }

        public static void LlenarCbo(Form form, ComboBox cbo, string storedProcedure, string displayMember, string valueMember, SqlConnection cn, string parametroNombre, object parametroValor)
        {
            Utils.ActualizarBarraDeEstado(form, Utils.clbdd);
            try
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();
                using (SqlCommand cmd = new SqlCommand(storedProcedure, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(parametroNombre, parametroValor);
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    dap.Fill(dt);
                    cbo.DataSource = dt;
                    cbo.DisplayMember = displayMember;
                    cbo.ValueMember = valueMember;
                }
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(form, ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(form, ex);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
            Utils.ActualizarBarraDeEstado(form);
        }

        public static void LlenarCbo(Form form, ComboBox cbo, string storedProcedure, string displayMember, string valueMember, SqlConnection cn)
        {
            Utils.ActualizarBarraDeEstado(form, Utils.clbdd);
            try
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();
                using (SqlCommand cmd = new SqlCommand(storedProcedure, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    dap.Fill(dt);
                    cbo.DataSource = dt;
                    cbo.DisplayMember = displayMember;
                    cbo.ValueMember = valueMember;
                }
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(form, ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(form, ex);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
            Utils.ActualizarBarraDeEstado(form);
        }

        public static void ValidaTxtBIdIni(TextBox txtBIdIni, TextBox txtBIdFin)
        {
            int numBIdIni = 0, numBIdFin = 0;
            if (txtBIdIni.Text != "")
            {
                if (int.TryParse(txtBIdIni.Text, out int numTxtBIdIni))
                {
                    if (numTxtBIdIni == 0)
                    {
                        MessageBox.Show("El valor del Id inicial no puede ser cero", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtBIdIni.Text = "1";
                        txtBIdIni.Focus();
                        return;
                    }
                    numBIdIni = numTxtBIdIni;
                    if (txtBIdFin.Text == "")
                        txtBIdFin.Text = txtBIdIni.Text;
                }
                else
                    MessageBox.Show("Por favor ingrese un número valido", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (txtBIdFin.Text != "")
            {
                if (int.TryParse(txtBIdFin.Text, out int numTxtBIdFin))
                {
                    numBIdFin = numTxtBIdFin;
                }
                else
                    MessageBox.Show("Por favor ingrese un número valido", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (numBIdFin < numBIdIni)
                txtBIdFin.Text = txtBIdIni.Text;
        }

        public static void ValidaTxtBIdFin(TextBox txtBIdIni, TextBox txtBIdFin)
        {
            int numBIdIni = 0, numBIdFin = 0;
            if (txtBIdIni.Text != "")
            {
                if (int.TryParse(txtBIdIni.Text, out int numTxtBIdIni))
                {
                    numBIdIni = numTxtBIdIni;
                }
                else
                    MessageBox.Show("Por favor ingrese un número valido", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                txtBIdIni.Text = txtBIdFin.Text;
            }
            if (txtBIdFin.Text != "")
            {
                if (int.TryParse(txtBIdFin.Text, out int numTxtBIdFin))
                {
                    if (numTxtBIdFin == 0)
                    {
                        MessageBox.Show("El valor del Id final no puede ser cero", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtBIdFin.Text = "1";
                        txtBIdFin.Focus();
                        Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);
                        return;
                    }
                    numBIdFin = numTxtBIdFin;
                }
                else
                    MessageBox.Show("Por favor ingrese un número valido", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (numBIdIni > numBIdFin)
                txtBIdIni.Text = txtBIdFin.Text;
        }

        public static void ValidarDigitosConPunto(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar !='.')
                e.Handled = true;
            // valida que exista solo un punto decimal
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;
            // forzar que solo se capturen como máximo dos dígitos despues del punto decimal
            if (e.KeyChar != 8)
            {
                string numsDecimales = (sender as TextBox).Text + e.KeyChar;
                if ((sender as TextBox).Text.IndexOf('.') > -1)
                {
                    int posComienzo = (sender as TextBox).Text.IndexOf('.');
                    numsDecimales = numsDecimales.Substring(posComienzo, numsDecimales.Length - posComienzo);
                    if (numsDecimales.Length > 3)
                        e.Handled = true;
                }
            }
        }

        public static void ValidarDigitosSinPunto(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || (int)e.KeyChar == 8);
        }

        public static void ConfDgv(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.GradientActiveCaption;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.GradientActiveCaption;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.BackgroundColor = SystemColors.Control;
            dgv.RowHeadersVisible = false;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.AutoResizeColumns();
        }

        public static void GrbPaint(Form form, object sender, PaintEventArgs e)
        {
            GroupBox groupBox = sender as GroupBox;
            Utils.DrawGroupBox(form, groupBox, e.Graphics, Color.Black, Color.Black);
        }

        public static void GrbPaint2(Form form, object sender, PaintEventArgs e)
        {
            GroupBox groupBox = sender as GroupBox;
            Utils.DrawGroupBox(form, groupBox, e.Graphics, Color.Black, Color.LightSlateGray);
        }

        public static void MsgCatchOueclbdd(Form form, SqlException ex)
        {
            if (ex.Number == 53) // Error de conexión
                MessageBox.Show("No se pudo conectar a la base de datos.\n\nVerifique su conexión.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else 
                MessageBox.Show(Utils.oueclbdd + ex.Message, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Utils.ActualizarBarraDeEstado(form);
        }

        public static void MsgCatchOueclbdd(SqlException ex)
        {
            if (ex.Number == 53) // Error de conexión
                MessageBox.Show("No se pudo conectar a la base de datos.\n\nVerifique su conexión.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(Utils.oueclbdd + ex.Message, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            MDIPrincipal.ActualizarBarraDeEstado();
        }


        public static void MsgCatchOue(Form form, Exception ex)
        {
            MessageBox.Show(Utils.oue + ex.Message, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Utils.ActualizarBarraDeEstado(form);
        }

        public static void MsgCatchOue(Exception ex)
        {
            MessageBox.Show(Utils.oue + ex.Message, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        public static void MsgCatchErrorRestriccionCF(Form form)
        {
            MessageBox.Show(Utils.errorRestriccionCF, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Utils.ActualizarBarraDeEstado(form);
        }

        public static void MsgCatchErrorClaveDuplicada(Form form)
        {
            MessageBox.Show(Utils.errorClaveDuplicada, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Utils.ActualizarBarraDeEstado(form);
        }

        public static void ActualizarBarraDeEstado(Form form, string mensaje = "Listo.", bool error = false)
        {
            // se requiere en el archivo MDIPrincipal.cs declarar la propiedad:
            //public ToolStripStatusLabel ToolStripEstado
            //{
            //    get { return tsslEstado; }
            //    set { tsslEstado = value; }
            //}
            MDIPrincipal mDIPrincipal = (MDIPrincipal)form.MdiParent;
            if (mDIPrincipal != null) // esta comprobación se requiere para que no marque error en los formularios heredados en el tiempo de diseño.
            {
                if (mensaje != "Listo.")
                    if (error)
                        mDIPrincipal.ToolStripEstado.BackColor = Color.Red;
                    else
                        mDIPrincipal.ToolStripEstado.BackColor = SystemColors.ActiveCaption;
                else
                    mDIPrincipal.ToolStripEstado.BackColor = SystemColors.Control;
                if (error)
                {
                    mDIPrincipal.ToolStripEstado.ForeColor = Color.White;
                    mDIPrincipal.ToolStripEstado.Font = new Font(mDIPrincipal.ToolStripEstado.Font, FontStyle.Bold);
                }
                else
                {
                    mDIPrincipal.ToolStripEstado.ForeColor = SystemColors.ControlText;
                    mDIPrincipal.ToolStripEstado.Font = new Font(mDIPrincipal.ToolStripEstado.Font, FontStyle.Regular);
                }
                mDIPrincipal.ToolStripEstado.Text = mensaje;
                mDIPrincipal.Refresh();
            }
        }

        public static void ActualizarBarraDeEstadoPrincipal(Form form)
        {
            // por su logica, este metodo solo se debe ejecutar desde el metodo Utils.CerrarFormularios
            MDIPrincipal mDIPrincipal = (MDIPrincipal)form;
            if (mDIPrincipal != null)
            {
                mDIPrincipal.ToolStripEstado.BackColor = SystemColors.Control;
                mDIPrincipal.ToolStripEstado.Text = "Listo.";
                mDIPrincipal.Refresh();
            }
        }

        public static void DrawGroupBox(Form form, GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                                box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                                box.ClientRectangle.Width - 1,
                                                box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);
                // Clear text and border
                g.Clear(form.BackColor);
                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);
                // Drawing border
                // Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                // Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                // Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                // Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                // Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        public static void CerrarFormularios()
        {
            //Declaramos una lista de tipo Form
            List<Form> formularios = new List<Form>();
            //Recorremos Application.OpenForms el cual tiene la lista de formularios y metemos todos los forms en la lista que declarmos
            foreach (Form form in Application.OpenForms)
                formularios.Add(form);
            // recorremos la lista de formularios
            for (int i = 0; i < formularios.Count; i++)
            {
                // validamos que el nombre de los formularios sean distintos al unico formulario que queremos abierto
                if (formularios[i].Name != "MDIPrincipal")
                    formularios[i].Close();
                else
                    Utils.ActualizarBarraDeEstadoPrincipal(formularios[i]);
            }

        }

    }
}
