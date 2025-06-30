﻿namespace NorthwindTraders
{
    partial class FrmUsuariosCrud
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
            this.tabcOperacion = new System.Windows.Forms.TabControl();
            this.tbpConsultar = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tbpRegistrar = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.tbpModificar = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tbpEliminar = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.grbUsuarios = new System.Windows.Forms.GroupBox();
            this.Dgv = new System.Windows.Forms.DataGridView();
            this.grbBuscar = new System.Windows.Forms.GroupBox();
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
            this.grbUsuario = new System.Windows.Forms.GroupBox();
            this.btnTogglePwd1 = new System.Windows.Forms.Button();
            this.chkbEstatus = new System.Windows.Forms.CheckBox();
            this.lblFechaModificacion = new System.Windows.Forms.Label();
            this.lblFechaCaptura = new System.Windows.Forms.Label();
            this.txtConfirmarPwd = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtNombres = new System.Windows.Forms.TextBox();
            this.txtMaterno = new System.Windows.Forms.TextBox();
            this.txtPaterno = new System.Windows.Forms.TextBox();
            this.btnOperacion = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabcOperacion.SuspendLayout();
            this.tbpConsultar.SuspendLayout();
            this.tbpRegistrar.SuspendLayout();
            this.tbpModificar.SuspendLayout();
            this.tbpEliminar.SuspendLayout();
            this.grbUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).BeginInit();
            this.grbBuscar.SuspendLayout();
            this.grbUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabcOperacion
            // 
            this.tabcOperacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabcOperacion.Controls.Add(this.tbpConsultar);
            this.tabcOperacion.Controls.Add(this.tbpRegistrar);
            this.tabcOperacion.Controls.Add(this.tbpModificar);
            this.tabcOperacion.Controls.Add(this.tbpEliminar);
            this.tabcOperacion.Location = new System.Drawing.Point(16, 8);
            this.tabcOperacion.Name = "tabcOperacion";
            this.tabcOperacion.SelectedIndex = 0;
            this.tabcOperacion.Size = new System.Drawing.Size(960, 56);
            this.tabcOperacion.TabIndex = 1;
            this.tabcOperacion.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabcOperacion_Selected);
            // 
            // tbpConsultar
            // 
            this.tbpConsultar.Controls.Add(this.label1);
            this.tbpConsultar.Location = new System.Drawing.Point(4, 22);
            this.tbpConsultar.Name = "tbpConsultar";
            this.tbpConsultar.Padding = new System.Windows.Forms.Padding(3);
            this.tbpConsultar.Size = new System.Drawing.Size(952, 30);
            this.tbpConsultar.TabIndex = 0;
            this.tbpConsultar.Text = "   Consultar usuario   ";
            this.tbpConsultar.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(373, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Busque el usuario y seleccionelo en la lista que se muestra para ver su detalle";
            // 
            // tbpRegistrar
            // 
            this.tbpRegistrar.Controls.Add(this.label2);
            this.tbpRegistrar.Location = new System.Drawing.Point(4, 22);
            this.tbpRegistrar.Name = "tbpRegistrar";
            this.tbpRegistrar.Padding = new System.Windows.Forms.Padding(3);
            this.tbpRegistrar.Size = new System.Drawing.Size(952, 30);
            this.tbpRegistrar.TabIndex = 1;
            this.tbpRegistrar.Text = "   Registrar un usuario   ";
            this.tbpRegistrar.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Proporcione los datos del usuario a registrar.";
            // 
            // tbpModificar
            // 
            this.tbpModificar.Controls.Add(this.label3);
            this.tbpModificar.Location = new System.Drawing.Point(4, 22);
            this.tbpModificar.Name = "tbpModificar";
            this.tbpModificar.Padding = new System.Windows.Forms.Padding(3);
            this.tbpModificar.Size = new System.Drawing.Size(952, 30);
            this.tbpModificar.TabIndex = 2;
            this.tbpModificar.Text = "   Modificar un usuario   ";
            this.tbpModificar.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(457, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Busque al usuario y seleccionelo en la lista que se muestra, para que pueda modif" +
    "icar sus datos";
            // 
            // tbpEliminar
            // 
            this.tbpEliminar.Controls.Add(this.label4);
            this.tbpEliminar.Location = new System.Drawing.Point(4, 22);
            this.tbpEliminar.Name = "tbpEliminar";
            this.tbpEliminar.Padding = new System.Windows.Forms.Padding(3);
            this.tbpEliminar.Size = new System.Drawing.Size(952, 30);
            this.tbpEliminar.TabIndex = 3;
            this.tbpEliminar.Text = "   Eliminar un usuario   ";
            this.tbpEliminar.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(330, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Busque al usuario a eliminar y seleccionelo en la lista que se muestra";
            // 
            // grbUsuarios
            // 
            this.grbUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbUsuarios.Controls.Add(this.Dgv);
            this.grbUsuarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbUsuarios.Location = new System.Drawing.Point(16, 64);
            this.grbUsuarios.Name = "grbUsuarios";
            this.grbUsuarios.Size = new System.Drawing.Size(952, 240);
            this.grbUsuarios.TabIndex = 2;
            this.grbUsuarios.TabStop = false;
            this.grbUsuarios.Text = "»   Usuarios:   «";
            this.grbUsuarios.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // Dgv
            // 
            this.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv.Location = new System.Drawing.Point(3, 16);
            this.Dgv.Name = "Dgv";
            this.Dgv.RowHeadersWidth = 51;
            this.Dgv.Size = new System.Drawing.Size(946, 221);
            this.Dgv.TabIndex = 0;
            this.Dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_CellClick);
            this.Dgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Dgv_CellFormatting);
            // 
            // grbBuscar
            // 
            this.grbBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grbBuscar.Controls.Add(this.txtBUsuario);
            this.grbBuscar.Controls.Add(this.label10);
            this.grbBuscar.Controls.Add(this.txtBNombres);
            this.grbBuscar.Controls.Add(this.txtBMaterno);
            this.grbBuscar.Controls.Add(this.btnBuscar);
            this.grbBuscar.Controls.Add(this.btnLimpiar);
            this.grbBuscar.Controls.Add(this.txtBPaterno);
            this.grbBuscar.Controls.Add(this.txtBIdFin);
            this.grbBuscar.Controls.Add(this.txtBIdIni);
            this.grbBuscar.Controls.Add(this.label9);
            this.grbBuscar.Controls.Add(this.label8);
            this.grbBuscar.Controls.Add(this.label7);
            this.grbBuscar.Controls.Add(this.label6);
            this.grbBuscar.Controls.Add(this.label5);
            this.grbBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbBuscar.Location = new System.Drawing.Point(16, 312);
            this.grbBuscar.Name = "grbBuscar";
            this.grbBuscar.Size = new System.Drawing.Size(208, 298);
            this.grbBuscar.TabIndex = 3;
            this.grbBuscar.TabStop = false;
            this.grbBuscar.Text = "»   Buscar un usuario:   «";
            this.grbBuscar.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // txtBUsuario
            // 
            this.txtBUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBUsuario.Location = new System.Drawing.Point(14, 223);
            this.txtBUsuario.MaxLength = 50;
            this.txtBUsuario.Name = "txtBUsuario";
            this.txtBUsuario.Size = new System.Drawing.Size(180, 20);
            this.txtBUsuario.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 205);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Usuario:";
            // 
            // txtBNombres
            // 
            this.txtBNombres.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBNombres.Location = new System.Drawing.Point(14, 180);
            this.txtBNombres.MaxLength = 50;
            this.txtBNombres.Name = "txtBNombres";
            this.txtBNombres.Size = new System.Drawing.Size(180, 20);
            this.txtBNombres.TabIndex = 8;
            // 
            // txtBMaterno
            // 
            this.txtBMaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBMaterno.Location = new System.Drawing.Point(14, 141);
            this.txtBMaterno.MaxLength = 50;
            this.txtBMaterno.Name = "txtBMaterno";
            this.txtBMaterno.Size = new System.Drawing.Size(180, 20);
            this.txtBMaterno.TabIndex = 7;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(104, 252);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(90, 23);
            this.btnBuscar.TabIndex = 6;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.Location = new System.Drawing.Point(8, 251);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(88, 23);
            this.btnLimpiar.TabIndex = 5;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // txtBPaterno
            // 
            this.txtBPaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBPaterno.Location = new System.Drawing.Point(14, 97);
            this.txtBPaterno.MaxLength = 50;
            this.txtBPaterno.Name = "txtBPaterno";
            this.txtBPaterno.Size = new System.Drawing.Size(180, 20);
            this.txtBPaterno.TabIndex = 2;
            // 
            // txtBIdFin
            // 
            this.txtBIdFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBIdFin.Location = new System.Drawing.Point(72, 47);
            this.txtBIdFin.MaxLength = 10;
            this.txtBIdFin.Name = "txtBIdFin";
            this.txtBIdFin.Size = new System.Drawing.Size(100, 20);
            this.txtBIdFin.TabIndex = 1;
            this.txtBIdFin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBIdFin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBIdFin_KeyPress);
            this.txtBIdFin.Leave += new System.EventHandler(this.txtBIdFin_Leave);
            // 
            // txtBIdIni
            // 
            this.txtBIdIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBIdIni.Location = new System.Drawing.Point(72, 21);
            this.txtBIdIni.MaxLength = 10;
            this.txtBIdIni.Name = "txtBIdIni";
            this.txtBIdIni.Size = new System.Drawing.Size(100, 20);
            this.txtBIdIni.TabIndex = 0;
            this.txtBIdIni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBIdIni.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBIdIni_KeyPress);
            this.txtBIdIni.Leave += new System.EventHandler(this.txtBIdIni_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 163);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Nombres:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Apellido materno:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Apellido paterno:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Id final:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Id inicial:";
            // 
            // grbUsuario
            // 
            this.grbUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbUsuario.Controls.Add(this.btnTogglePwd1);
            this.grbUsuario.Controls.Add(this.chkbEstatus);
            this.grbUsuario.Controls.Add(this.lblFechaModificacion);
            this.grbUsuario.Controls.Add(this.lblFechaCaptura);
            this.grbUsuario.Controls.Add(this.txtConfirmarPwd);
            this.grbUsuario.Controls.Add(this.txtPwd);
            this.grbUsuario.Controls.Add(this.txtUsuario);
            this.grbUsuario.Controls.Add(this.txtNombres);
            this.grbUsuario.Controls.Add(this.txtMaterno);
            this.grbUsuario.Controls.Add(this.txtPaterno);
            this.grbUsuario.Controls.Add(this.btnOperacion);
            this.grbUsuario.Controls.Add(this.label20);
            this.grbUsuario.Controls.Add(this.label19);
            this.grbUsuario.Controls.Add(this.label18);
            this.grbUsuario.Controls.Add(this.label17);
            this.grbUsuario.Controls.Add(this.label16);
            this.grbUsuario.Controls.Add(this.label15);
            this.grbUsuario.Controls.Add(this.label14);
            this.grbUsuario.Controls.Add(this.label13);
            this.grbUsuario.Controls.Add(this.label12);
            this.grbUsuario.Controls.Add(this.txtId);
            this.grbUsuario.Controls.Add(this.label11);
            this.grbUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grbUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbUsuario.Location = new System.Drawing.Point(248, 312);
            this.grbUsuario.Margin = new System.Windows.Forms.Padding(2);
            this.grbUsuario.Name = "grbUsuario";
            this.grbUsuario.Size = new System.Drawing.Size(724, 298);
            this.grbUsuario.TabIndex = 4;
            this.grbUsuario.TabStop = false;
            this.grbUsuario.Text = "»  Usuario:   «";
            this.grbUsuario.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // btnTogglePwd1
            // 
            this.btnTogglePwd1.AutoSize = true;
            this.btnTogglePwd1.Enabled = false;
            this.btnTogglePwd1.Image = global::NorthwindTraders.Properties.Resources.mostrarCh;
            this.btnTogglePwd1.Location = new System.Drawing.Point(467, 143);
            this.btnTogglePwd1.Margin = new System.Windows.Forms.Padding(0);
            this.btnTogglePwd1.Name = "btnTogglePwd1";
            this.btnTogglePwd1.Size = new System.Drawing.Size(35, 30);
            this.btnTogglePwd1.TabIndex = 21;
            this.toolTip1.SetToolTip(this.btnTogglePwd1, "Mostrar/Ocultar contraseña");
            this.btnTogglePwd1.UseVisualStyleBackColor = true;
            this.btnTogglePwd1.Click += new System.EventHandler(this.btnTogglePwd1_Click);
            // 
            // chkbEstatus
            // 
            this.chkbEstatus.AutoSize = true;
            this.chkbEstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbEstatus.Location = new System.Drawing.Point(309, 237);
            this.chkbEstatus.Margin = new System.Windows.Forms.Padding(2);
            this.chkbEstatus.Name = "chkbEstatus";
            this.chkbEstatus.Size = new System.Drawing.Size(15, 14);
            this.chkbEstatus.TabIndex = 20;
            this.chkbEstatus.UseVisualStyleBackColor = true;
            // 
            // lblFechaModificacion
            // 
            this.lblFechaModificacion.AutoSize = true;
            this.lblFechaModificacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaModificacion.Location = new System.Drawing.Point(307, 213);
            this.lblFechaModificacion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFechaModificacion.Name = "lblFechaModificacion";
            this.lblFechaModificacion.Size = new System.Drawing.Size(41, 13);
            this.lblFechaModificacion.TabIndex = 19;
            this.lblFechaModificacion.Text = "label22";
            // 
            // lblFechaCaptura
            // 
            this.lblFechaCaptura.AutoSize = true;
            this.lblFechaCaptura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaCaptura.Location = new System.Drawing.Point(307, 189);
            this.lblFechaCaptura.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFechaCaptura.Name = "lblFechaCaptura";
            this.lblFechaCaptura.Size = new System.Drawing.Size(41, 13);
            this.lblFechaCaptura.TabIndex = 18;
            this.lblFechaCaptura.Text = "label21";
            // 
            // txtConfirmarPwd
            // 
            this.txtConfirmarPwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmarPwd.Location = new System.Drawing.Point(306, 161);
            this.txtConfirmarPwd.Margin = new System.Windows.Forms.Padding(2);
            this.txtConfirmarPwd.MaxLength = 20;
            this.txtConfirmarPwd.Name = "txtConfirmarPwd";
            this.txtConfirmarPwd.Size = new System.Drawing.Size(151, 20);
            this.txtConfirmarPwd.TabIndex = 17;
            this.txtConfirmarPwd.UseSystemPasswordChar = true;
            this.txtConfirmarPwd.TextChanged += new System.EventHandler(this.txtConfirmarPwd_TextChanged);
            this.txtConfirmarPwd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtConfirmarPwd_MouseUp);
            // 
            // txtPwd
            // 
            this.txtPwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPwd.Location = new System.Drawing.Point(306, 136);
            this.txtPwd.Margin = new System.Windows.Forms.Padding(2);
            this.txtPwd.MaxLength = 20;
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(151, 20);
            this.txtPwd.TabIndex = 16;
            this.txtPwd.UseSystemPasswordChar = true;
            this.txtPwd.TextChanged += new System.EventHandler(this.txtPwd_TextChanged);
            this.txtPwd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtPwd_MouseUp);
            // 
            // txtUsuario
            // 
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(306, 112);
            this.txtUsuario.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsuario.MaxLength = 20;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(151, 20);
            this.txtUsuario.TabIndex = 15;
            // 
            // txtNombres
            // 
            this.txtNombres.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombres.Location = new System.Drawing.Point(306, 88);
            this.txtNombres.Margin = new System.Windows.Forms.Padding(2);
            this.txtNombres.MaxLength = 80;
            this.txtNombres.Name = "txtNombres";
            this.txtNombres.Size = new System.Drawing.Size(264, 20);
            this.txtNombres.TabIndex = 14;
            // 
            // txtMaterno
            // 
            this.txtMaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaterno.Location = new System.Drawing.Point(306, 61);
            this.txtMaterno.Margin = new System.Windows.Forms.Padding(2);
            this.txtMaterno.MaxLength = 50;
            this.txtMaterno.Name = "txtMaterno";
            this.txtMaterno.Size = new System.Drawing.Size(264, 20);
            this.txtMaterno.TabIndex = 13;
            // 
            // txtPaterno
            // 
            this.txtPaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPaterno.Location = new System.Drawing.Point(306, 37);
            this.txtPaterno.Margin = new System.Windows.Forms.Padding(2);
            this.txtPaterno.MaxLength = 50;
            this.txtPaterno.Name = "txtPaterno";
            this.txtPaterno.Size = new System.Drawing.Size(264, 20);
            this.txtPaterno.TabIndex = 12;
            // 
            // btnOperacion
            // 
            this.btnOperacion.AutoSize = true;
            this.btnOperacion.Location = new System.Drawing.Point(384, 261);
            this.btnOperacion.Margin = new System.Windows.Forms.Padding(2);
            this.btnOperacion.Name = "btnOperacion";
            this.btnOperacion.Size = new System.Drawing.Size(185, 23);
            this.btnOperacion.TabIndex = 11;
            this.btnOperacion.Text = "Registrar usuario";
            this.btnOperacion.UseVisualStyleBackColor = true;
            this.btnOperacion.Visible = false;
            this.btnOperacion.Click += new System.EventHandler(this.btnOperacion_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(164, 213);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(139, 13);
            this.label20.TabIndex = 10;
            this.label20.Text = "Fecha de modificación:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(190, 189);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(111, 13);
            this.label19.TabIndex = 9;
            this.label19.Text = "Fecha de captura:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(245, 237);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 13);
            this.label18.TabIndex = 8;
            this.label18.Text = "Estatus:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(168, 164);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(131, 13);
            this.label17.TabIndex = 7;
            this.label17.Text = "Confirmar contraseña:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(223, 140);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(75, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "Contraseña:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(243, 116);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(54, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "Usuario:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(228, 91);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "Nombre(s):";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(194, 67);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(105, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Apellido materno:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(196, 42);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(103, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Apellido paterno:";
            // 
            // txtId
            // 
            this.txtId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtId.Location = new System.Drawing.Point(306, 13);
            this.txtId.Margin = new System.Windows.Forms.Padding(2);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(76, 20);
            this.txtId.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(275, 18);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(22, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Id:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // FrmUsuariosCrud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 621);
            this.ControlBox = false;
            this.Controls.Add(this.grbUsuario);
            this.Controls.Add(this.grbBuscar);
            this.Controls.Add(this.grbUsuarios);
            this.Controls.Add(this.tabcOperacion);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmUsuariosCrud";
            this.Text = "» Mantenimiento de usuarios «";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUsuariosCrud_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmUsuariosCrud_FormClosed);
            this.Load += new System.EventHandler(this.FrmUsuariosCrud_Load);
            this.tabcOperacion.ResumeLayout(false);
            this.tbpConsultar.ResumeLayout(false);
            this.tbpConsultar.PerformLayout();
            this.tbpRegistrar.ResumeLayout(false);
            this.tbpRegistrar.PerformLayout();
            this.tbpModificar.ResumeLayout(false);
            this.tbpModificar.PerformLayout();
            this.tbpEliminar.ResumeLayout(false);
            this.tbpEliminar.PerformLayout();
            this.grbUsuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).EndInit();
            this.grbBuscar.ResumeLayout(false);
            this.grbBuscar.PerformLayout();
            this.grbUsuario.ResumeLayout(false);
            this.grbUsuario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabcOperacion;
        private System.Windows.Forms.TabPage tbpConsultar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tbpRegistrar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tbpModificar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tbpEliminar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grbUsuarios;
        private System.Windows.Forms.DataGridView Dgv;
        private System.Windows.Forms.GroupBox grbBuscar;
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
        private System.Windows.Forms.TextBox txtBNombres;
        private System.Windows.Forms.TextBox txtBMaterno;
        private System.Windows.Forms.TextBox txtBUsuario;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox grbUsuario;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnOperacion;
        private System.Windows.Forms.TextBox txtPaterno;
        private System.Windows.Forms.TextBox txtConfirmarPwd;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtNombres;
        private System.Windows.Forms.TextBox txtMaterno;
        private System.Windows.Forms.CheckBox chkbEstatus;
        private System.Windows.Forms.Label lblFechaModificacion;
        private System.Windows.Forms.Label lblFechaCaptura;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnTogglePwd1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}