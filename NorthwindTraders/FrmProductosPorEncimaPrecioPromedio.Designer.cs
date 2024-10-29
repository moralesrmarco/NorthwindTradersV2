namespace NorthwindTraders
{
    partial class FrmProductosPorEncimaPrecioPromedio
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
            this.Grb.Location = new System.Drawing.Point(16, 16);
            this.Grb.Name = "Grb";
            this.Grb.Size = new System.Drawing.Size(768, 424);
            this.Grb.TabIndex = 0;
            this.Grb.TabStop = false;
            this.Grb.Text = "»   Listado de productos con el precio por encima del precio promedio:   «";
            this.Grb.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // Dgv
            // 
            this.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv.Location = new System.Drawing.Point(3, 18);
            this.Dgv.Name = "Dgv";
            this.Dgv.Size = new System.Drawing.Size(762, 403);
            this.Dgv.TabIndex = 0;
            // 
            // FrmProductosPorEncimaPrecioPromedio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.Grb);
            this.Name = "FrmProductosPorEncimaPrecioPromedio";
            this.Text = "» Productos por encima del precio promedio «";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmProductosPorEncimaPrecioPromedio_FormClosed);
            this.Load += new System.EventHandler(this.FrmProductosPorEncimaPrecioPromedio_Load);
            this.Grb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Grb;
        private System.Windows.Forms.DataGridView Dgv;
    }
}