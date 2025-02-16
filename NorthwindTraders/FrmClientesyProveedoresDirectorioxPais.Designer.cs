namespace NorthwindTraders
{
    partial class FrmClientesyProveedoresDirectorioxPais
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Grb = new System.Windows.Forms.GroupBox();
            this.Dgv = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.checkBoxProveedores = new System.Windows.Forms.CheckBox();
            this.checkBoxClientes = new System.Windows.Forms.CheckBox();
            this.Grb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // Grb
            // 
            this.Grb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grb.Controls.Add(this.Dgv);
            this.Grb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Grb.Location = new System.Drawing.Point(16, 56);
            this.Grb.Name = "Grb";
            this.Grb.Padding = new System.Windows.Forms.Padding(10);
            this.Grb.Size = new System.Drawing.Size(952, 552);
            this.Grb.TabIndex = 0;
            this.Grb.TabStop = false;
            this.Grb.Text = "»   Directorio de clientes y proveedores por país   «";
            this.Grb.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // Dgv
            // 
            this.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv.Location = new System.Drawing.Point(10, 25);
            this.Dgv.Name = "Dgv";
            this.Dgv.Size = new System.Drawing.Size(932, 517);
            this.Dgv.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(72, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Buscar por país:";
            // 
            // comboBox
            // 
            this.comboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(200, 20);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(259, 24);
            this.comboBox.TabIndex = 2;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(764, 20);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 3;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // checkBoxProveedores
            // 
            this.checkBoxProveedores.AutoSize = true;
            this.checkBoxProveedores.Checked = true;
            this.checkBoxProveedores.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxProveedores.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxProveedores.Location = new System.Drawing.Point(614, 22);
            this.checkBoxProveedores.Name = "checkBoxProveedores";
            this.checkBoxProveedores.Size = new System.Drawing.Size(116, 20);
            this.checkBoxProveedores.TabIndex = 21;
            this.checkBoxProveedores.Text = "Proveedores";
            this.checkBoxProveedores.UseVisualStyleBackColor = true;
            // 
            // checkBoxClientes
            // 
            this.checkBoxClientes.AutoSize = true;
            this.checkBoxClientes.Checked = true;
            this.checkBoxClientes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxClientes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxClientes.Location = new System.Drawing.Point(498, 22);
            this.checkBoxClientes.Name = "checkBoxClientes";
            this.checkBoxClientes.Size = new System.Drawing.Size(82, 20);
            this.checkBoxClientes.TabIndex = 20;
            this.checkBoxClientes.Text = "Clientes";
            this.checkBoxClientes.UseVisualStyleBackColor = true;
            // 
            // FrmClientesyProveedoresDirectorioxPais
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 621);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxProveedores);
            this.Controls.Add(this.checkBoxClientes);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Grb);
            this.Name = "FrmClientesyProveedoresDirectorioxPais";
            this.Text = "» Directorio de clientes y proveedores por país «";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmClientesyProveedoresDirectorioxPais_FormClosed);
            this.Load += new System.EventHandler(this.FrmClientesyProveedoresDirectorioxPais_Load);
            this.Grb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox Grb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView Dgv;
        private System.Windows.Forms.CheckBox checkBoxProveedores;
        private System.Windows.Forms.CheckBox checkBoxClientes;
    }
}