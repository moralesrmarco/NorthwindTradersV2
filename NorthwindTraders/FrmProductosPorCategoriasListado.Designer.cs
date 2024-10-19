namespace NorthwindTraders
{
    partial class FrmProductosPorCategoriasListado
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
            this.GrbListado = new System.Windows.Forms.GroupBox();
            this.DgvListado = new System.Windows.Forms.DataGridView();
            this.GrbListado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvListado)).BeginInit();
            this.SuspendLayout();
            // 
            // GrbListado
            // 
            this.GrbListado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrbListado.Controls.Add(this.DgvListado);
            this.GrbListado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrbListado.Location = new System.Drawing.Point(16, 16);
            this.GrbListado.Name = "GrbListado";
            this.GrbListado.Size = new System.Drawing.Size(952, 592);
            this.GrbListado.TabIndex = 0;
            this.GrbListado.TabStop = false;
            this.GrbListado.Text = "»   Listado de productos por categorías:   «";
            this.GrbListado.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // DgvListado
            // 
            this.DgvListado.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvListado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvListado.Location = new System.Drawing.Point(3, 18);
            this.DgvListado.Name = "DgvListado";
            this.DgvListado.Size = new System.Drawing.Size(946, 571);
            this.DgvListado.TabIndex = 0;
            // 
            // FrmProductosPorCategoriasListado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 621);
            this.ControlBox = false;
            this.Controls.Add(this.GrbListado);
            this.Name = "FrmProductosPorCategoriasListado";
            this.Text = "» Listado de productos por categorías: «";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmProductosPorCategoriasListado_FormClosed);
            this.Load += new System.EventHandler(this.FrmProductosPorCategoriasListado_Load);
            this.GrbListado.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvListado)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GrbListado;
        private System.Windows.Forms.DataGridView DgvListado;
    }
}