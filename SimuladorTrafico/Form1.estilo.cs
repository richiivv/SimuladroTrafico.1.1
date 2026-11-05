using System.Drawing.Drawing2D;
using System.Reflection;

namespace SimuladorTrafico
{
    /// <summary>
    /// Parte "visual" del formulario: colores, distribución de los controles,
    /// dibujo de la escena y de la gráfica. No contiene lógica de la simulación.
    /// </summary>
    public partial class FormPrincipal
    {
        #region Campos del estilo

        private PanelTarjeta tarjetaEstadisticas = null!;
        private PanelTarjeta tarjetaGrafica = null!;
        private PanelTarjeta tarjetaConfiguracion = null!;

        // Imagen del fondo de la escena (calles, manzanas...). Se dibuja una
        // sola vez y se reutiliza en cada tick.
        private Bitmap? fondoEscena;

        // Convierte píxeles "a 96 dpi" a los píxeles reales de la pantalla.
        private int Px(int valor)
        {
            return (int)Math.Round(valor * DeviceDpi / 96.0);
        }

        #endregion


        #region Aplicar estilo a los controles

        private void AplicarEstilo()
        {
            SuspendLayout();

            BackColor = Paleta.Fondo;
            DoubleBuffered = true;
            Text = "Simulador de tráfico";
            MinimumSize = new Size(Px(1000), Px(740));

            Paint += DibujarEncabezado;
            SizeChanged += (s, e) => DisponerControles();
            Load += (s, e) => DisponerControles();

            // Título
            lblTitulo.Text = "Simulador de tráfico";
            lblTitulo.Font = new Font("Segoe UI Semibold", 20f);
            lblTitulo.ForeColor = Paleta.Texto;
            lblTitulo.BackColor = Color.Transparent;

            // Botones
            EstiloBoton(btnIniciar, Paleta.Exito);
            EstiloBoton(btnPausar, Paleta.Alerta);
            EstiloBoton(btnDetener, Paleta.Peligro);
            EstiloBoton(btnReiniciar, Paleta.Neutro);
            EstiloBoton(btnAplicarConfiguracion, Paleta.Primario);
            btnAplicarConfiguracion.Text = "Aplicar cambios";

            // Paneles de dibujo
            panelSimulacion.BorderStyle = BorderStyle.None;
            panelSimulacion.BackColor = Paleta.Fondo;
            ActivarDobleBuffer(panelSimulacion);

            panelGrafica.BorderStyle = BorderStyle.None;
            panelGrafica.BackColor = Color.White;
            ActivarDobleBuffer(panelGrafica);

            // Tarjetas de la columna derecha
            tarjetaEstadisticas = new PanelTarjeta { Titulo = "Estadísticas" };
            tarjetaGrafica = new PanelTarjeta();
            tarjetaConfiguracion = new PanelTarjeta { Titulo = "Configuración del tráfico" };

            Controls.Add(tarjetaEstadisticas);
            Controls.Add(tarjetaGrafica);
            Controls.Add(tarjetaConfiguracion);

            // Textos iniciales con acentos (los mismos que usa "Reiniciar")
            lblVehiculos.Text = "Vehículos: 0";
            lblPasados.Text = "Vehículos que han pasado: 0";
            lblFlujo.Text = "Flujo: 0 vehículos/min";
            lblFlujoMaximo.Text = "Flujo máximo: 0 veh/min";
            lblFlujoPromedio.Text = "Flujo promedio general: 0 veh/min";

            // Estadísticas
            foreach (Label etiqueta in new[] { lblTiempo, lblVehiculos, lblPasados, lblFlujo, lblEspera })
            {
                EstiloEtiqueta(etiqueta, 10.5f, Paleta.Texto);
                tarjetaEstadisticas.Controls.Add(etiqueta);
            }

            // Gráfica
            tarjetaGrafica.Controls.Add(panelGrafica);

            foreach (Label etiqueta in new[] { lblFlujoMaximo, lblFlujoPromedio })
            {
                EstiloEtiqueta(etiqueta, 10f, Paleta.TextoSuave);
                tarjetaGrafica.Controls.Add(etiqueta);
            }

            // Configuración (se sacan los controles del GroupBox original)
            lblVerdeHorizontal.Enabled = true;

            foreach (Label etiqueta in new[] { lblVelocidadConfig, lblVerdeHorizontal, lblVerdeVertical })
            {
                EstiloEtiqueta(etiqueta, 10f, Paleta.Texto);
                tarjetaConfiguracion.Controls.Add(etiqueta);
            }

            foreach (NumericUpDown numero in new[] { numVelocidad, numVerdeHorizontal, numVerdeVertical })
            {
                numero.Font = new Font("Segoe UI", 10.5f);
                numero.BackColor = Color.White;
                numero.ForeColor = Paleta.Texto;
                numero.TextAlign = HorizontalAlignment.Center;
                numero.BorderStyle = BorderStyle.FixedSingle;
                tarjetaConfiguracion.Controls.Add(numero);
            }

            tarjetaConfiguracion.Controls.Add(btnAplicarConfiguracion);
            grpConfiguracion.Visible = false;

            // Botones con esquinas redondeadas
            foreach (Button boton in new[] { btnIniciar, btnPausar, btnDetener, btnReiniciar, btnAplicarConfiguracion })
            {
                RedondearControl(boton, 10f);
            }

            ResumeLayout(false);
            DisponerControles();
        }

        private static void EstiloBoton(Button boton, Color color)
        {
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.UseVisualStyleBackColor = false;
            boton.BackColor = color;
            boton.ForeColor = Color.White;
            boton.FlatAppearance.MouseOverBackColor = Dibujo.Aclarar(color, 18);
            boton.FlatAppearance.MouseDownBackColor = Dibujo.Oscurecer(color, 20);
            boton.Font = new Font("Segoe UI Semibold", 10.5f);
            boton.Cursor = Cursors.Hand;
        }

        private static void EstiloEtiqueta(Label etiqueta, float tamano, Color color)
        {
            etiqueta.AutoSize = true;
            etiqueta.Font = new Font("Segoe UI", tamano);
            etiqueta.ForeColor = color;
            etiqueta.BackColor = Color.White;
        }

        private static void RedondearControl(Control control, float radio)
        {
            control.Paint += (s, e) =>
            {
                Color fondo = control.Parent != null ? control.Parent.BackColor : Paleta.Fondo;
                Dibujo.RedondearEsquinas(e.Graphics, control.Width, control.Height, radio, fondo, Color.Empty);
            };
        }

        // Evita el parpadeo al redibujar un Panel cada 50 ms.
        private static void ActivarDobleBuffer(Control control)
        {
            typeof(Control)
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(control, true, null);
        }

        #endregion


        #region Distribución de los controles

        private void DisponerControles()
        {
            if (tarjetaEstadisticas == null)
            {
                return;
            }

            int w = ClientSize.Width;
            int h = ClientSize.Height;

            // Ventana minimizada o demasiado pequeña
            if (w < Px(500) || h < Px(300))
            {
                return;
            }

            int margen = Px(20);
            int hueco = Px(14);

            // Encabezado: icono, título y botones
            lblTitulo.Location = new Point(margen + Px(42), Px(14));

            int x = margen + Px(42) + lblTitulo.PreferredWidth + Px(40);

            foreach (Button boton in new[] { btnIniciar, btnPausar, btnDetener, btnReiniciar })
            {
                boton.Size = new Size(Px(104), Px(40));
                boton.Location = new Point(x, Px(12));
                x += Px(104) + Px(10);
            }

            // Zona principal
            int yTop = Px(72);
            int anchoLateral = Px(390);
            int xLateral = w - margen - anchoLateral;
            int bajo = h - margen;

            panelSimulacion.SetBounds(margen, yTop, xLateral - hueco - margen, bajo - yTop);

            int altoEstadisticas = Px(44) + 5 * Px(30) + Px(12);
            int altoConfiguracion = Px(50) + 3 * Px(42) + Px(40) + Px(20);
            int altoGrafica = Math.Max(
                Px(150),
                bajo - yTop - altoEstadisticas - altoConfiguracion - 2 * hueco);

            int yGrafica = yTop + altoEstadisticas + hueco;
            int yConfiguracion = yGrafica + altoGrafica + hueco;

            tarjetaEstadisticas.SetBounds(xLateral, yTop, anchoLateral, altoEstadisticas);
            tarjetaGrafica.SetBounds(xLateral, yGrafica, anchoLateral, altoGrafica);
            tarjetaConfiguracion.SetBounds(xLateral, yConfiguracion, anchoLateral, altoConfiguracion);

            // Tarjeta de estadísticas
            Label[] filas = { lblTiempo, lblVehiculos, lblPasados, lblFlujo, lblEspera };

            for (int i = 0; i < filas.Length; i++)
            {
                filas[i].Location = new Point(Px(18), Px(44) + i * Px(30));
            }

            // Tarjeta de la gráfica
            int altoTexto = Px(26);

            panelGrafica.SetBounds(
                Px(10),
                Px(10),
                anchoLateral - Px(20),
                altoGrafica - Px(10) - 2 * altoTexto - Px(14));

            lblFlujoMaximo.Location = new Point(Px(18), panelGrafica.Bottom + Px(4));
            lblFlujoPromedio.Location = new Point(Px(18), panelGrafica.Bottom + Px(4) + altoTexto);

            // Tarjeta de configuración
            FilaConfiguracion(lblVelocidadConfig, numVelocidad, Px(50), anchoLateral);
            FilaConfiguracion(lblVerdeHorizontal, numVerdeHorizontal, Px(50) + Px(42), anchoLateral);
            FilaConfiguracion(lblVerdeVertical, numVerdeVertical, Px(50) + 2 * Px(42), anchoLateral);

            btnAplicarConfiguracion.SetBounds(
                Px(18),
                Px(50) + 3 * Px(42),
                anchoLateral - Px(36),
                Px(40));
        }

        private void FilaConfiguracion(Label etiqueta, NumericUpDown numero, int y, int anchoTarjeta)
        {
            numero.Width = Px(84);
            numero.Location = new Point(anchoTarjeta - Px(18) - numero.Width, y);
            etiqueta.Location = new Point(Px(18), y + (numero.Height - etiqueta.Height) / 2);
        }

        // Icono de semáforo junto al título
        private void DibujarEncabezado(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF caja = new RectangleF(Px(20), Px(10), Px(30), Px(44));

            using (SolidBrush carcasa = new SolidBrush(Color.FromArgb(31, 41, 55)))
            {
                Dibujo.Rellenar(g, carcasa, caja, Px(8));
            }

            Color[] colores = { Paleta.Peligro, Paleta.Alerta, Paleta.Exito };
            float diametro = Px(10);
            float espacio = (caja.Height - 3 * diametro) / 4f;
            float cx = caja.X + (caja.Width - diametro) / 2f;

            for (int i = 0; i < 3; i++)
            {
                using (SolidBrush luz = new SolidBrush(colores[i]))
                {
                    g.FillEllipse(luz, cx, caja.Y + espacio * (i + 1) + diametro * i, diametro, diametro);
                }
            }
        }

        #endregion


        #region Escena de la simulación

        private void DibujarEscena(Graphics g)
        {
            int ancho = panelSimulacion.ClientSize.Width;
            int alto = panelSimulacion.ClientSize.Height;

            if (ancho <= 0 || alto <= 0)
            {
                return;
            }

            // El fondo solo se vuelve a dibujar si cambia el tamaño del panel
            if (fondoEscena == null || fondoEscena.Width != ancho || fondoEscena.Height != alto)
            {
                fondoEscena?.Dispose();
                fondoEscena = new Bitmap(ancho, alto);

                using (Graphics gf = Graphics.FromImage(fondoEscena))
                {
                    gf.SmoothingMode = SmoothingMode.AntiAlias;
                    DibujarFondoEscena(gf, ancho, alto);
                }
            }

            g.DrawImage(fondoEscena, 0, 0, ancho, alto);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (Vehiculo vehiculo in vehiculos)
            {
                vehiculo.Dibujar(g);
            }

            foreach (Vehiculo vehiculo in vehiculosVerticales)
            {
                vehiculo.Dibujar(g);
            }

            // Semáforos
            int xCalle = (ancho - 100) / 2;
            int yCalle = (alto - 100) / 2;

            DibujarSemaforo(g, xCalle - 40, yCalle - 66, semaforoHorizontal, true);
            DibujarSemaforo(g, xCalle + 100 + 8, yCalle + 100 + 8, semaforoVertical, false);

            // Esquinas redondeadas del panel
            Dibujo.RedondearEsquinas(g, ancho, alto, 16f, Paleta.Fondo, Paleta.Borde);
        }

        private void DibujarFondoEscena(Graphics g, int ancho, int alto)
        {
            const int anchoCalle = 100;
            const int altoCalle = 100;

            int xCalle = (ancho - anchoCalle) / 2;
            int yCalle = (alto - altoCalle) / 2;
            int xDerecha = xCalle + anchoCalle;
            int yAbajo = yCalle + altoCalle;

            g.Clear(Paleta.Acera);

            // Las cuatro manzanas (capa lisa de color pasto)
            Rectangle[] manzanas =
            {
                new Rectangle(0, 0, xCalle, yCalle),
                new Rectangle(xDerecha, 0, ancho - xDerecha, yCalle),
                new Rectangle(0, yAbajo, xCalle, alto - yAbajo),
                new Rectangle(xDerecha, yAbajo, ancho - xDerecha, alto - yAbajo)
            };

            for (int i = 0; i < manzanas.Length; i++)
            {
                Rectangle area = Rectangle.Inflate(manzanas[i], -14, -14);

                if (area.Width < 40 || area.Height < 40)
                {
                    continue;
                }

                using (SolidBrush pasto = new SolidBrush(Paleta.Manzana))
                {
                    Dibujo.Rellenar(g, pasto, area, 14f);
                }
            }

            // Calles
            using (SolidBrush asfalto = new SolidBrush(Paleta.Asfalto))
            {
                g.FillRectangle(asfalto, 0, yCalle, ancho, altoCalle);
                g.FillRectangle(asfalto, xCalle, 0, anchoCalle, alto);
            }

            // Líneas de borde de calzada
            using (Pen borde = new Pen(Color.FromArgb(90, 255, 255, 255), 2f))
            {
                g.DrawLine(borde, 0, yCalle + 7, xCalle, yCalle + 7);
                g.DrawLine(borde, xDerecha, yCalle + 7, ancho, yCalle + 7);
                g.DrawLine(borde, 0, yAbajo - 7, xCalle, yAbajo - 7);
                g.DrawLine(borde, xDerecha, yAbajo - 7, ancho, yAbajo - 7);

                g.DrawLine(borde, xCalle + 7, 0, xCalle + 7, yCalle);
                g.DrawLine(borde, xCalle + 7, yAbajo, xCalle + 7, alto);
                g.DrawLine(borde, xDerecha - 7, 0, xDerecha - 7, yCalle);
                g.DrawLine(borde, xDerecha - 7, yAbajo, xDerecha - 7, alto);
            }

            // Flechas que indican el sentido de circulación
            using (Pen flecha = new Pen(Color.FromArgb(75, 255, 255, 255), 3f))
            {
                flecha.StartCap = LineCap.Round;
                flecha.EndCap = LineCap.Round;
                flecha.LineJoin = LineJoin.Round;

                float yc = yCalle + altoCalle / 2f;

                for (float x = 80; x < xCalle - 70; x += 140)
                {
                    g.DrawLines(flecha, new[]
                    {
                        new PointF(x - 5, yc - 9), new PointF(x + 5, yc), new PointF(x - 5, yc + 9)
                    });
                }

                for (float x = xDerecha + 80; x < ancho - 30; x += 140)
                {
                    g.DrawLines(flecha, new[]
                    {
                        new PointF(x - 5, yc - 9), new PointF(x + 5, yc), new PointF(x - 5, yc + 9)
                    });
                }

                float xc = xCalle + anchoCalle / 2f;

                for (float y = yCalle - 80; y > 30; y -= 140)
                {
                    g.DrawLines(flecha, new[]
                    {
                        new PointF(xc - 9, y + 5), new PointF(xc, y - 5), new PointF(xc + 9, y + 5)
                    });
                }

                for (float y = yAbajo + 90; y < alto - 30; y += 140)
                {
                    g.DrawLines(flecha, new[]
                    {
                        new PointF(xc - 9, y + 5), new PointF(xc, y - 5), new PointF(xc + 9, y + 5)
                    });
                }
            }

            // Pasos peatonales en los cuatro lados del cruce
            using (SolidBrush cebra = new SolidBrush(Color.FromArgb(225, 255, 255, 255)))
            {
                for (int k = 0; k < 7; k++)
                {
                    int desplazamiento = 11 + k * 12;

                    g.FillRectangle(cebra, xCalle + 9, yCalle + desplazamiento, 14, 6);
                    g.FillRectangle(cebra, xDerecha - 9 - 14, yCalle + desplazamiento, 14, 6);
                    g.FillRectangle(cebra, xCalle + desplazamiento, yCalle + 9, 6, 14);
                    g.FillRectangle(cebra, xCalle + desplazamiento, yAbajo - 9 - 14, 6, 14);
                }
            }

            // Líneas de alto (donde realmente se detienen los vehículos)
            using (SolidBrush linea = new SolidBrush(Color.FromArgb(235, 255, 255, 255)))
            {
                g.FillRectangle(linea, xCalle + 1, yCalle + 4, 4, altoCalle - 8);
                g.FillRectangle(linea, xCalle + 4, yAbajo + 31, anchoCalle - 8, 4);
            }
        }

        private static void DibujarSemaforo(
            Graphics g, float x, float y, Semaforo semaforo, bool brazoAbajo)
        {
            // Brazo que une el semáforo con la acera
            using (SolidBrush brazo = new SolidBrush(Color.FromArgb(55, 65, 81)))
            {
                if (brazoAbajo)
                {
                    g.FillRectangle(brazo, x + 9, y + 56, 4, 10);
                }
                else
                {
                    g.FillRectangle(brazo, x - 8, y + 27, 10, 4);
                }
            }

            RectangleF caja = new RectangleF(x, y, 22, 58);

            // Sombra
            using (SolidBrush sombra = new SolidBrush(Color.FromArgb(40, 15, 23, 42)))
            {
                Dibujo.Rellenar(g, sombra, new RectangleF(x + 2, y + 3, 22, 58), 8f);
            }

            // Carcasa
            using (SolidBrush carcasa = new SolidBrush(Color.FromArgb(31, 41, 55)))
            using (Pen borde = new Pen(Color.FromArgb(17, 24, 39), 1.5f))
            {
                Dibujo.Rellenar(g, carcasa, caja, 8f);
                Dibujo.Contornear(g, borde, caja, 8f);
            }

            DibujarLuz(g, x + 4, y + 5, semaforo.EstaRojo,
                Color.FromArgb(239, 68, 68), Color.FromArgb(82, 45, 50));

            DibujarLuz(g, x + 4, y + 22, semaforo.EstaAmarillo,
                Color.FromArgb(251, 191, 36), Color.FromArgb(84, 72, 40));

            DibujarLuz(g, x + 4, y + 39, semaforo.EstaVerde,
                Color.FromArgb(34, 197, 94), Color.FromArgb(38, 74, 56));
        }

        private static void DibujarLuz(
            Graphics g, float x, float y, bool encendida, Color color, Color apagada)
        {
            const float diametro = 14f;

            if (encendida)
            {
                // Resplandor
                using (SolidBrush halo = new SolidBrush(Color.FromArgb(60, color)))
                {
                    g.FillEllipse(halo, x - 4, y - 4, diametro + 8, diametro + 8);
                }
            }

            using (SolidBrush luz = new SolidBrush(encendida ? color : apagada))
            {
                g.FillEllipse(luz, x, y, diametro, diametro);
            }

            if (encendida)
            {
                using (SolidBrush brillo = new SolidBrush(Color.FromArgb(110, 255, 255, 255)))
                {
                    g.FillEllipse(brillo, x + 3, y + 2.5f, 5, 4);
                }
            }
        }

        #endregion


        #region Gráfica de flujo

        private void DibujarGrafica(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            float f = DeviceDpi / 96f;
            int ancho = panelGrafica.ClientSize.Width;
            int alto = panelGrafica.ClientSize.Height;

            float margenIzquierdo = 40 * f;
            float margenDerecho = 14 * f;
            float margenSuperior = 40 * f;
            float margenInferior = 30 * f;

            float anchoUtil = ancho - margenIzquierdo - margenDerecho;
            float altoUtil = alto - margenSuperior - margenInferior;

            if (anchoUtil <= 0 || altoUtil <= 0)
            {
                return;
            }

            using (Font fuente = new Font("Segoe UI", 8f))
            using (Font titulo = new Font("Segoe UI Semibold", 10.5f))
            using (SolidBrush textoSuave = new SolidBrush(Paleta.TextoSuave))
            using (SolidBrush textoOscuro = new SolidBrush(Paleta.Texto))
            using (Pen cuadricula = new Pen(Color.FromArgb(232, 236, 242), 1f))
            using (StringFormat derecha = new StringFormat
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Center
            })
            using (StringFormat centro = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                // Título y unidad
                g.DrawString("Flujo vehicular", titulo, textoOscuro, 8 * f, 8 * f);

                g.DrawString(
                    "veh/min", fuente, textoSuave,
                    new RectangleF(0, 8 * f, ancho - 8 * f, 20 * f),
                    derecha);

                // Aún no hay suficientes muestras
                if (flujoHistorico.Count < 2 ||
                    tiempoHistorico.Count != flujoHistorico.Count)
                {
                    g.DrawString(
                        "Esperando datos...", fuente, textoSuave,
                        new RectangleF(margenIzquierdo, margenSuperior, anchoUtil, altoUtil),
                        centro);

                    return;
                }

                // Escala vertical
                float maximo = 0;

                foreach (float valor in flujoHistorico)
                {
                    if (valor > maximo)
                    {
                        maximo = valor;
                    }
                }

                float escalaMaxima =
                    Math.Max(5, (float)Math.Ceiling(maximo / 5.0) * 5);

                // Cuadrícula y etiquetas del eje vertical
                for (int i = 0; i <= 5; i++)
                {
                    float valorY = escalaMaxima * i / 5f;
                    float y = margenSuperior + altoUtil - altoUtil * i / 5f;

                    g.DrawLine(cuadricula, margenIzquierdo, y, ancho - margenDerecho, y);

                    g.DrawString(
                        valorY.ToString("0"), fuente, textoSuave,
                        new RectangleF(0, y - 9 * f, margenIzquierdo - 6 * f, 18 * f),
                        derecha);
                }

                // Eje horizontal: tiempo en segundos
                int tiempoInicial = tiempoHistorico[0];
                int tiempoFinal = tiempoHistorico[tiempoHistorico.Count - 1];
                int duracion = Math.Max(1, tiempoFinal - tiempoInicial);

                for (int i = 0; i <= 3; i++)
                {
                    float x = margenIzquierdo + anchoUtil * i / 3f;
                    int segundo = tiempoInicial + duracion * i / 3;

                    g.DrawString(
                        segundo + " s", fuente, textoSuave,
                        new RectangleF(x - 25 * f, margenSuperior + altoUtil + 6 * f, 50 * f, 16 * f),
                        centro);
                }

                // Puntos de la línea
                PointF[] puntos = new PointF[flujoHistorico.Count];

                for (int i = 0; i < puntos.Length; i++)
                {
                    float x = margenIzquierdo +
                        anchoUtil * (tiempoHistorico[i] - tiempoInicial) / (float)duracion;

                    float y = margenSuperior + altoUtil -
                        flujoHistorico[i] / escalaMaxima * altoUtil;

                    puntos[i] = new PointF(x, y);
                }

                // Área sombreada bajo la línea
                float baseY = margenSuperior + altoUtil;
                PointF[] area = new PointF[puntos.Length + 2];
                Array.Copy(puntos, area, puntos.Length);
                area[puntos.Length] = new PointF(puntos[puntos.Length - 1].X, baseY);
                area[puntos.Length + 1] = new PointF(puntos[0].X, baseY);

                using (LinearGradientBrush degradado = new LinearGradientBrush(
                    new RectangleF(margenIzquierdo, margenSuperior, anchoUtil, altoUtil),
                    Color.FromArgb(70, Paleta.Primario),
                    Color.FromArgb(4, Paleta.Primario),
                    90f))
                {
                    g.FillPolygon(degradado, area);
                }

                // Línea del eje inferior
                using (Pen eje = new Pen(Color.FromArgb(203, 213, 225), 1.5f))
                {
                    g.DrawLine(eje, margenIzquierdo, baseY, ancho - margenDerecho, baseY);
                }

                // Línea del flujo
                using (Pen linea = new Pen(Paleta.Primario, 2.5f))
                {
                    linea.LineJoin = LineJoin.Round;
                    g.DrawLines(linea, puntos);
                }

                // Marcador en el último punto
                PointF ultimo = puntos[puntos.Length - 1];

                using (Pen contorno = new Pen(Paleta.Primario, 2f))
                {
                    g.FillEllipse(Brushes.White, ultimo.X - 4, ultimo.Y - 4, 8, 8);
                    g.DrawEllipse(contorno, ultimo.X - 4, ultimo.Y - 4, 8, 8);
                }
            }
        }

        #endregion
    }
}