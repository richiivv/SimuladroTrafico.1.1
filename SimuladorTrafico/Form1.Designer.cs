namespace SimuladorTrafico
{
    partial class FormPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            btnDetener = new Button();
            btnReiniciar = new Button();
            btnIniciar = new Button();
            panelSimulacion = new Panel();
            lblTiempo = new Label();
            lblVehiculos = new Label();
            lblPasados = new Label();
            lblEspera = new Label();
            lblFlujo = new Label();
            grpConfiguracion = new GroupBox();
            btnAplicarConfiguracion = new Button();
            lblVerdeVertical = new Label();
            lblVerdeHorizontal = new Label();
            lblVelocidadConfig = new Label();
            numVerdeVertical = new NumericUpDown();
            numVerdeHorizontal = new NumericUpDown();
            numVelocidad = new NumericUpDown();
            btnPausar = new Button();
            panelGrafica = new Panel();
            lblFlujoMaximo = new Label();
            lblFlujoPromedio = new Label();
            grpConfiguracion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numVerdeVertical).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numVerdeHorizontal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numVelocidad).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(52, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(177, 20);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "SIMULADOR DE TRAFICO";
            // 
            // btnDetener
            // 
            btnDetener.Location = new Point(207, 60);
            btnDetener.Name = "btnDetener";
            btnDetener.Size = new Size(108, 46);
            btnDetener.TabIndex = 1;
            btnDetener.Text = "Detener";
            btnDetener.UseVisualStyleBackColor = true;
            btnDetener.Click += btnDetener_Click;
            // 
            // btnReiniciar
            // 
            btnReiniciar.Location = new Point(389, 60);
            btnReiniciar.Name = "btnReiniciar";
            btnReiniciar.Size = new Size(121, 46);
            btnReiniciar.TabIndex = 1;
            btnReiniciar.Text = "Reiniciar";
            btnReiniciar.UseVisualStyleBackColor = true;
            btnReiniciar.Click += btnReiniciar_Click;
            // 
            // btnIniciar
            // 
            btnIniciar.Location = new Point(27, 60);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(103, 46);
            btnIniciar.TabIndex = 1;
            btnIniciar.Text = "Iniciar";
            btnIniciar.UseVisualStyleBackColor = true;
            btnIniciar.Click += btnIniciar_Click;
            // 
            // panelSimulacion
            // 
            panelSimulacion.BackColor = SystemColors.ControlDark;
            panelSimulacion.BorderStyle = BorderStyle.FixedSingle;
            panelSimulacion.Location = new Point(12, 112);
            panelSimulacion.Name = "panelSimulacion";
            panelSimulacion.Size = new Size(1331, 725);
            panelSimulacion.TabIndex = 2;
            panelSimulacion.Paint += panel1_Paint;
            // 
            // lblTiempo
            // 
            lblTiempo.AutoSize = true;
            lblTiempo.Location = new Point(1658, 86);
            lblTiempo.Name = "lblTiempo";
            lblTiempo.Size = new Size(85, 20);
            lblTiempo.TabIndex = 0;
            lblTiempo.Text = "Tiempo: 0 s";
            // 
            // lblVehiculos
            // 
            lblVehiculos.AutoSize = true;
            lblVehiculos.Location = new Point(1449, 86);
            lblVehiculos.Name = "lblVehiculos";
            lblVehiculos.Size = new Size(86, 20);
            lblVehiculos.TabIndex = 0;
            lblVehiculos.Text = "Vehiculos: 0";
            lblVehiculos.Click += lblVehiculos_Click;
            // 
            // lblPasados
            // 
            lblPasados.AutoSize = true;
            lblPasados.Location = new Point(1511, 227);
            lblPasados.Name = "lblPasados";
            lblPasados.Size = new Size(196, 20);
            lblPasados.TabIndex = 0;
            lblPasados.Text = "Vehiculos que han pasado: 0";
            // 
            // lblEspera
            // 
            lblEspera.AutoSize = true;
            lblEspera.Location = new Point(1658, 153);
            lblEspera.Name = "lblEspera";
            lblEspera.Size = new Size(148, 20);
            lblEspera.TabIndex = 0;
            lblEspera.Text = "Espera promedio: 0 s";
            // 
            // lblFlujo
            // 
            lblFlujo.AutoSize = true;
            lblFlujo.Location = new Point(1449, 153);
            lblFlujo.Name = "lblFlujo";
            lblFlujo.Size = new Size(146, 20);
            lblFlujo.TabIndex = 0;
            lblFlujo.Text = "Flujo: 0 vehiculo/min";
            // 
            // grpConfiguracion
            // 
            grpConfiguracion.Controls.Add(btnAplicarConfiguracion);
            grpConfiguracion.Controls.Add(lblVerdeVertical);
            grpConfiguracion.Controls.Add(lblVerdeHorizontal);
            grpConfiguracion.Controls.Add(lblVelocidadConfig);
            grpConfiguracion.Controls.Add(numVerdeVertical);
            grpConfiguracion.Controls.Add(numVerdeHorizontal);
            grpConfiguracion.Controls.Add(numVelocidad);
            grpConfiguracion.Location = new Point(1468, 617);
            grpConfiguracion.Name = "grpConfiguracion";
            grpConfiguracion.Size = new Size(321, 220);
            grpConfiguracion.TabIndex = 3;
            grpConfiguracion.TabStop = false;
            grpConfiguracion.Text = "Configuracion del trafico";
            grpConfiguracion.Enter += grpConfiguracion_Enter;
            // 
            // btnAplicarConfiguracion
            // 
            btnAplicarConfiguracion.Location = new Point(73, 185);
            btnAplicarConfiguracion.Name = "btnAplicarConfiguracion";
            btnAplicarConfiguracion.Size = new Size(180, 29);
            btnAplicarConfiguracion.TabIndex = 4;
            btnAplicarConfiguracion.Text = "Aplicar Cambios";
            btnAplicarConfiguracion.UseVisualStyleBackColor = true;
            btnAplicarConfiguracion.Click += btnAplicarConfiguracion_Click;
            // 
            // lblVerdeVertical
            // 
            lblVerdeVertical.AutoSize = true;
            lblVerdeVertical.Location = new Point(37, 135);
            lblVerdeVertical.Name = "lblVerdeVertical";
            lblVerdeVertical.Size = new Size(122, 20);
            lblVerdeVertical.TabIndex = 4;
            lblVerdeVertical.Text = "Verde vertical (s):";
            // 
            // lblVerdeHorizontal
            // 
            lblVerdeHorizontal.AutoSize = true;
            lblVerdeHorizontal.Enabled = false;
            lblVerdeHorizontal.Location = new Point(37, 81);
            lblVerdeHorizontal.Name = "lblVerdeHorizontal";
            lblVerdeHorizontal.Size = new Size(141, 20);
            lblVerdeHorizontal.TabIndex = 4;
            lblVerdeHorizontal.Text = "Verde horizontal (s):";
            lblVerdeHorizontal.Click += lblVerdeHorizontal_Click;
            // 
            // lblVelocidadConfig
            // 
            lblVelocidadConfig.AutoSize = true;
            lblVelocidadConfig.Location = new Point(37, 36);
            lblVelocidadConfig.Name = "lblVelocidadConfig";
            lblVelocidadConfig.Size = new Size(78, 20);
            lblVelocidadConfig.TabIndex = 4;
            lblVelocidadConfig.Text = "Velocidad:";
            // 
            // numVerdeVertical
            // 
            numVerdeVertical.Location = new Point(213, 135);
            numVerdeVertical.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numVerdeVertical.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numVerdeVertical.Name = "numVerdeVertical";
            numVerdeVertical.Size = new Size(73, 27);
            numVerdeVertical.TabIndex = 5;
            numVerdeVertical.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // numVerdeHorizontal
            // 
            numVerdeHorizontal.Location = new Point(213, 81);
            numVerdeHorizontal.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numVerdeHorizontal.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numVerdeHorizontal.Name = "numVerdeHorizontal";
            numVerdeHorizontal.Size = new Size(73, 27);
            numVerdeHorizontal.TabIndex = 5;
            numVerdeHorizontal.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // numVelocidad
            // 
            numVelocidad.Location = new Point(149, 29);
            numVelocidad.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numVelocidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numVelocidad.Name = "numVelocidad";
            numVelocidad.Size = new Size(73, 27);
            numVelocidad.TabIndex = 5;
            numVelocidad.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // btnPausar
            // 
            btnPausar.Location = new Point(606, 60);
            btnPausar.Name = "btnPausar";
            btnPausar.Size = new Size(136, 45);
            btnPausar.TabIndex = 4;
            btnPausar.Text = "Pausar";
            btnPausar.UseVisualStyleBackColor = true;
            btnPausar.Click += btnPausar_Click;
            // 
            // panelGrafica
            // 
            panelGrafica.BackColor = Color.White;
            panelGrafica.Location = new Point(1406, 286);
            panelGrafica.Name = "panelGrafica";
            panelGrafica.Size = new Size(400, 220);
            panelGrafica.TabIndex = 5;
            // 
            // lblFlujoMaximo
            // 
            lblFlujoMaximo.AutoSize = true;
            lblFlujoMaximo.Location = new Point(1523, 523);
            lblFlujoMaximo.Name = "lblFlujoMaximo";
            lblFlujoMaximo.Size = new Size(172, 20);
            lblFlujoMaximo.TabIndex = 6;
            lblFlujoMaximo.Text = "Flujo maximo: 0 veh/min";
            // 
            // lblFlujoPromedio
            // 
            lblFlujoPromedio.AutoSize = true;
            lblFlujoPromedio.Location = new Point(1523, 566);
            lblFlujoPromedio.Name = "lblFlujoPromedio";
            lblFlujoPromedio.Size = new Size(184, 20);
            lblFlujoPromedio.TabIndex = 6;
            lblFlujoPromedio.Text = "Flujo promedio: 0 veh/min";
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1832, 867);
            Controls.Add(lblFlujoPromedio);
            Controls.Add(lblFlujoMaximo);
            Controls.Add(panelGrafica);
            Controls.Add(btnPausar);
            Controls.Add(grpConfiguracion);
            Controls.Add(panelSimulacion);
            Controls.Add(btnReiniciar);
            Controls.Add(btnIniciar);
            Controls.Add(btnDetener);
            Controls.Add(lblEspera);
            Controls.Add(lblFlujo);
            Controls.Add(lblTiempo);
            Controls.Add(lblVehiculos);
            Controls.Add(lblPasados);
            Controls.Add(lblTitulo);
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Simulador de Trafico";
            WindowState = FormWindowState.Maximized;
            Load += FormPrincipal_Load;
            grpConfiguracion.ResumeLayout(false);
            grpConfiguracion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numVerdeVertical).EndInit();
            ((System.ComponentModel.ISupportInitialize)numVerdeHorizontal).EndInit();
            ((System.ComponentModel.ISupportInitialize)numVelocidad).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Button btnDetener;
        private Button btnReiniciar;
        private Button btnIniciar;
        private Panel panelSimulacion;
        private Label lblTiempo;
        private Label lblVehiculos;
        private Label lblPasados;
        private Label lblEspera;
        private Label lblFlujo;
        private GroupBox grpConfiguracion;
        private Button btnAplicarConfiguracion;
        private Label lblVerdeVertical;
        private Label lblVerdeHorizontal;
        private Label lblVelocidadConfig;
        private NumericUpDown numVerdeVertical;
        private NumericUpDown numVerdeHorizontal;
        private NumericUpDown numVelocidad;
        private Button btnPausar;
        private Panel panelGrafica;
        private Label lblFlujoMaximo;
        private Label lblFlujoPromedio;
    }
}
