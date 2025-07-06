namespace NorthwindTraders
{
    partial class FrmPermisosCrud
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
            this.components = new System.ComponentModel.Container();
            this.GrbUsuarios = new System.Windows.Forms.GroupBox();
            this.Dgv = new System.Windows.Forms.DataGridView();
            this.GrbBuscar = new System.Windows.Forms.GroupBox();
            this.txtBUsuario = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBNombres = new System.Windows.Forms.TextBox();
            this.txtBMaterno = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.txtBPaterno = new System.Windows.Forms.TextBox();
            this.txtBIdFin = new System.Windows.Forms.TextBox();
            this.txtBIdIni = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.GrbPermisos = new System.Windows.Forms.GroupBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxConcedidos = new System.Windows.Forms.ListBox();
            this.GrbCatalogoPermisos = new System.Windows.Forms.GroupBox();
            this.listBoxCatalogo = new System.Windows.Forms.ListBox();
            this.BtnAgregar = new System.Windows.Forms.Button();
            this.BtnQuitar = new System.Windows.Forms.Button();
            this.BtnAgregarTodos = new System.Windows.Forms.Button();
            this.BtnQuitarTodos = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip4 = new System.Windows.Forms.ToolTip(this.components);
            this.GrbUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).BeginInit();
            this.GrbBuscar.SuspendLayout();
            this.GrbPermisos.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.GrbCatalogoPermisos.SuspendLayout();
            this.SuspendLayout();
            // 
            // GrbUsuarios
            // 
            this.GrbUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrbUsuarios.Controls.Add(this.Dgv);
            this.GrbUsuarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrbUsuarios.Location = new System.Drawing.Point(17, 19);
            this.GrbUsuarios.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GrbUsuarios.Name = "GrbUsuarios";
            this.GrbUsuarios.Padding = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.GrbUsuarios.Size = new System.Drawing.Size(950, 240);
            this.GrbUsuarios.TabIndex = 0;
            this.GrbUsuarios.TabStop = false;
            this.GrbUsuarios.Text = "»   Usuarios:   «";
            this.GrbUsuarios.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // Dgv
            // 
            this.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv.Location = new System.Drawing.Point(11, 25);
            this.Dgv.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Dgv.Name = "Dgv";
            this.Dgv.RowHeadersWidth = 51;
            this.Dgv.RowTemplate.Height = 24;
            this.Dgv.Size = new System.Drawing.Size(928, 203);
            this.Dgv.TabIndex = 0;
            this.Dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_CellClick);
            // 
            // GrbBuscar
            // 
            this.GrbBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.GrbBuscar.Controls.Add(this.txtBUsuario);
            this.GrbBuscar.Controls.Add(this.label10);
            this.GrbBuscar.Controls.Add(this.txtBNombres);
            this.GrbBuscar.Controls.Add(this.txtBMaterno);
            this.GrbBuscar.Controls.Add(this.btnBuscar);
            this.GrbBuscar.Controls.Add(this.btnLimpiar);
            this.GrbBuscar.Controls.Add(this.txtBPaterno);
            this.GrbBuscar.Controls.Add(this.txtBIdFin);
            this.GrbBuscar.Controls.Add(this.txtBIdIni);
            this.GrbBuscar.Controls.Add(this.label9);
            this.GrbBuscar.Controls.Add(this.label8);
            this.GrbBuscar.Controls.Add(this.label7);
            this.GrbBuscar.Controls.Add(this.label6);
            this.GrbBuscar.Controls.Add(this.label5);
            this.GrbBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrbBuscar.Location = new System.Drawing.Point(20, 276);
            this.GrbBuscar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GrbBuscar.Name = "GrbBuscar";
            this.GrbBuscar.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GrbBuscar.Size = new System.Drawing.Size(208, 284);
            this.GrbBuscar.TabIndex = 1;
            this.GrbBuscar.TabStop = false;
            this.GrbBuscar.Text = "»   Buscar un usuario:   «";
            this.GrbBuscar.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // txtBUsuario
            // 
            this.txtBUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBUsuario.Location = new System.Drawing.Point(16, 226);
            this.txtBUsuario.MaxLength = 20;
            this.txtBUsuario.Name = "txtBUsuario";
            this.txtBUsuario.Size = new System.Drawing.Size(180, 20);
            this.txtBUsuario.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 208);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Usuario:";
            // 
            // txtBNombres
            // 
            this.txtBNombres.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBNombres.Location = new System.Drawing.Point(16, 184);
            this.txtBNombres.MaxLength = 80;
            this.txtBNombres.Name = "txtBNombres";
            this.txtBNombres.Size = new System.Drawing.Size(180, 20);
            this.txtBNombres.TabIndex = 22;
            // 
            // txtBMaterno
            // 
            this.txtBMaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBMaterno.Location = new System.Drawing.Point(16, 145);
            this.txtBMaterno.MaxLength = 50;
            this.txtBMaterno.Name = "txtBMaterno";
            this.txtBMaterno.Size = new System.Drawing.Size(180, 20);
            this.txtBMaterno.TabIndex = 21;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(106, 255);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(90, 23);
            this.btnBuscar.TabIndex = 20;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.Location = new System.Drawing.Point(10, 254);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(88, 23);
            this.btnLimpiar.TabIndex = 19;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // txtBPaterno
            // 
            this.txtBPaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBPaterno.Location = new System.Drawing.Point(16, 100);
            this.txtBPaterno.MaxLength = 50;
            this.txtBPaterno.Name = "txtBPaterno";
            this.txtBPaterno.Size = new System.Drawing.Size(180, 20);
            this.txtBPaterno.TabIndex = 18;
            // 
            // txtBIdFin
            // 
            this.txtBIdFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBIdFin.Location = new System.Drawing.Point(74, 50);
            this.txtBIdFin.MaxLength = 10;
            this.txtBIdFin.Name = "txtBIdFin";
            this.txtBIdFin.Size = new System.Drawing.Size(100, 20);
            this.txtBIdFin.TabIndex = 17;
            this.txtBIdFin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBIdFin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBIdFin_KeyPress);
            // 
            // txtBIdIni
            // 
            this.txtBIdIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBIdIni.Location = new System.Drawing.Point(74, 24);
            this.txtBIdIni.MaxLength = 10;
            this.txtBIdIni.Name = "txtBIdIni";
            this.txtBIdIni.Size = new System.Drawing.Size(100, 20);
            this.txtBIdIni.TabIndex = 11;
            this.txtBIdIni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBIdIni.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBIdIni_KeyPress);
            this.txtBIdIni.Leave += new System.EventHandler(this.txtBIdIni_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 167);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Nombres:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Apellido materno:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Apellido paterno:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Id final:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Id inicial:";
            // 
            // GrbPermisos
            // 
            this.GrbPermisos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrbPermisos.Controls.Add(this.BtnQuitarTodos);
            this.GrbPermisos.Controls.Add(this.BtnAgregarTodos);
            this.GrbPermisos.Controls.Add(this.BtnQuitar);
            this.GrbPermisos.Controls.Add(this.BtnAgregar);
            this.GrbPermisos.Controls.Add(this.txtNombre);
            this.GrbPermisos.Controls.Add(this.label3);
            this.GrbPermisos.Controls.Add(this.txtId);
            this.GrbPermisos.Controls.Add(this.txtUsuario);
            this.GrbPermisos.Controls.Add(this.label2);
            this.GrbPermisos.Controls.Add(this.label1);
            this.GrbPermisos.Controls.Add(this.groupBox1);
            this.GrbPermisos.Controls.Add(this.GrbCatalogoPermisos);
            this.GrbPermisos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrbPermisos.Location = new System.Drawing.Point(254, 276);
            this.GrbPermisos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GrbPermisos.Name = "GrbPermisos";
            this.GrbPermisos.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GrbPermisos.Size = new System.Drawing.Size(711, 284);
            this.GrbPermisos.TabIndex = 2;
            this.GrbPermisos.TabStop = false;
            this.GrbPermisos.Text = "» Permisos «";
            this.GrbPermisos.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(228, 48);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtNombre.MaxLength = 180;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.ReadOnly = true;
            this.txtNombre.Size = new System.Drawing.Size(290, 20);
            this.txtNombre.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Nombre:";
            // 
            // txtId
            // 
            this.txtId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtId.Location = new System.Drawing.Point(62, 48);
            this.txtId.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(98, 20);
            this.txtId.TabIndex = 6;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(157, 22);
            this.txtUsuario.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtUsuario.MaxLength = 20;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(138, 20);
            this.txtUsuario.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Id:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Usuario seleccionado: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxConcedidos);
            this.groupBox1.Location = new System.Drawing.Point(326, 80);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(202, 197);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "» Permisos concedidos «";
            this.groupBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint2);
            // 
            // listBoxConcedidos
            // 
            this.listBoxConcedidos.FormattingEnabled = true;
            this.listBoxConcedidos.Location = new System.Drawing.Point(27, 30);
            this.listBoxConcedidos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBoxConcedidos.Name = "listBoxConcedidos";
            this.listBoxConcedidos.Size = new System.Drawing.Size(150, 147);
            this.listBoxConcedidos.TabIndex = 0;
            // 
            // GrbCatalogoPermisos
            // 
            this.GrbCatalogoPermisos.Controls.Add(this.listBoxCatalogo);
            this.GrbCatalogoPermisos.Location = new System.Drawing.Point(30, 80);
            this.GrbCatalogoPermisos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GrbCatalogoPermisos.Name = "GrbCatalogoPermisos";
            this.GrbCatalogoPermisos.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GrbCatalogoPermisos.Size = new System.Drawing.Size(202, 197);
            this.GrbCatalogoPermisos.TabIndex = 1;
            this.GrbCatalogoPermisos.TabStop = false;
            this.GrbCatalogoPermisos.Text = "» Catálogo de permisos «";
            this.GrbCatalogoPermisos.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint2);
            // 
            // listBoxCatalogo
            // 
            this.listBoxCatalogo.FormattingEnabled = true;
            this.listBoxCatalogo.Location = new System.Drawing.Point(22, 30);
            this.listBoxCatalogo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBoxCatalogo.Name = "listBoxCatalogo";
            this.listBoxCatalogo.Size = new System.Drawing.Size(155, 147);
            this.listBoxCatalogo.TabIndex = 0;
            // 
            // BtnAgregar
            // 
            this.BtnAgregar.Location = new System.Drawing.Point(239, 128);
            this.BtnAgregar.Name = "BtnAgregar";
            this.BtnAgregar.Size = new System.Drawing.Size(75, 23);
            this.BtnAgregar.TabIndex = 9;
            this.BtnAgregar.Text = "»»";
            this.toolTip1.SetToolTip(this.BtnAgregar, "Agregar permiso");
            this.BtnAgregar.UseVisualStyleBackColor = true;
            this.BtnAgregar.Click += new System.EventHandler(this.BtnAgregar_Click);
            // 
            // BtnQuitar
            // 
            this.BtnQuitar.Location = new System.Drawing.Point(240, 155);
            this.BtnQuitar.Name = "BtnQuitar";
            this.BtnQuitar.Size = new System.Drawing.Size(75, 23);
            this.BtnQuitar.TabIndex = 10;
            this.BtnQuitar.Text = "««";
            this.toolTip2.SetToolTip(this.BtnQuitar, "Quitar permiso");
            this.BtnQuitar.UseVisualStyleBackColor = true;
            this.BtnQuitar.Click += new System.EventHandler(this.BtnQuitar_Click);
            // 
            // BtnAgregarTodos
            // 
            this.BtnAgregarTodos.Location = new System.Drawing.Point(240, 185);
            this.BtnAgregarTodos.Name = "BtnAgregarTodos";
            this.BtnAgregarTodos.Size = new System.Drawing.Size(75, 23);
            this.BtnAgregarTodos.TabIndex = 11;
            this.BtnAgregarTodos.Text = "»»»»»»";
            this.toolTip3.SetToolTip(this.BtnAgregarTodos, "Agregar todos los permisos");
            this.BtnAgregarTodos.UseVisualStyleBackColor = true;
            this.BtnAgregarTodos.Click += new System.EventHandler(this.BtnAgregarTodos_Click);
            // 
            // BtnQuitarTodos
            // 
            this.BtnQuitarTodos.Location = new System.Drawing.Point(240, 215);
            this.BtnQuitarTodos.Name = "BtnQuitarTodos";
            this.BtnQuitarTodos.Size = new System.Drawing.Size(75, 23);
            this.BtnQuitarTodos.TabIndex = 12;
            this.BtnQuitarTodos.Text = "««««««";
            this.toolTip4.SetToolTip(this.BtnQuitarTodos, "Quitar todos los permisos");
            this.BtnQuitarTodos.UseVisualStyleBackColor = true;
            this.BtnQuitarTodos.Click += new System.EventHandler(this.BtnQuitarTodos_Click);
            // 
            // FrmPermisosCrud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 579);
            this.ControlBox = false;
            this.Controls.Add(this.GrbPermisos);
            this.Controls.Add(this.GrbBuscar);
            this.Controls.Add(this.GrbUsuarios);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmPermisosCrud";
            this.Padding = new System.Windows.Forms.Padding(15, 16, 15, 16);
            this.ShowIcon = false;
            this.Text = "» Mantenimiento de permisos «";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPermisosCrud_FormClosed);
            this.Load += new System.EventHandler(this.FrmPermisosCrud_Load);
            this.GrbUsuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).EndInit();
            this.GrbBuscar.ResumeLayout(false);
            this.GrbBuscar.PerformLayout();
            this.GrbPermisos.ResumeLayout(false);
            this.GrbPermisos.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.GrbCatalogoPermisos.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GrbUsuarios;
        private System.Windows.Forms.DataGridView Dgv;
        private System.Windows.Forms.GroupBox GrbBuscar;
        private System.Windows.Forms.TextBox txtBUsuario;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBNombres;
        private System.Windows.Forms.TextBox txtBMaterno;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.TextBox txtBPaterno;
        private System.Windows.Forms.TextBox txtBIdFin;
        private System.Windows.Forms.TextBox txtBIdIni;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox GrbPermisos;
        private System.Windows.Forms.GroupBox GrbCatalogoPermisos;
        private System.Windows.Forms.ListBox listBoxCatalogo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxConcedidos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnQuitarTodos;
        private System.Windows.Forms.Button BtnAgregarTodos;
        private System.Windows.Forms.Button BtnQuitar;
        private System.Windows.Forms.Button BtnAgregar;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip4;
    }
}