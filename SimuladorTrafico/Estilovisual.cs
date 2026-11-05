using System.Drawing.Drawing2D;

namespace SimuladorTrafico
{
    /// <summary>
    /// Colores de toda la aplicación. Si quieres cambiar el "look",
    /// empieza por aquí.
    /// </summary>
    internal static class Paleta
    {
        // Interfaz
        public static readonly Color Fondo = Color.FromArgb(243, 245, 249);
        public static readonly Color Borde = Color.FromArgb(226, 231, 239);
        public static readonly Color Texto = Color.FromArgb(17, 24, 39);
        public static readonly Color TextoSuave = Color.FromArgb(100, 116, 139);

        public static readonly Color Primario = Color.FromArgb(59, 130, 246);
        public static readonly Color Exito = Color.FromArgb(16, 185, 129);
        public static readonly Color Alerta = Color.FromArgb(245, 158, 11);
        public static readonly Color Peligro = Color.FromArgb(239, 68, 68);
        public static readonly Color Neutro = Color.FromArgb(100, 116, 139);

        // Escena
        public static readonly Color Acera = Color.FromArgb(228, 233, 240);
        public static readonly Color Manzana = Color.FromArgb(207, 228, 203);
        public static readonly Color Asfalto = Color.FromArgb(55, 65, 81);

        // Vehículos
        public static readonly Color CocheRojo = Color.FromArgb(244, 91, 91);
        public static readonly Color CocheAzul = Color.FromArgb(59, 130, 246);
    }


    /// <summary>
    /// Pequeñas ayudas de dibujo (rectángulos redondeados, colores, etc.).
    /// </summary>
    internal static class Dibujo
    {
        public static GraphicsPath RectRedondeado(RectangleF r, float radio)
        {
            GraphicsPath ruta = new GraphicsPath();
            float d = Math.Min(radio * 2f, Math.Min(r.Width, r.Height));

            if (d <= 0f)
            {
                ruta.AddRectangle(r);
                return ruta;
            }

            ruta.AddArc(r.X, r.Y, d, d, 180, 90);
            ruta.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            ruta.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            ruta.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            ruta.CloseFigure();
            return ruta;
        }

        public static void Rellenar(Graphics g, Brush pincel, RectangleF r, float radio)
        {
            using (GraphicsPath ruta = RectRedondeado(r, radio))
            {
                g.FillPath(pincel, ruta);
            }
        }

        public static void Contornear(Graphics g, Pen lapiz, RectangleF r, float radio)
        {
            using (GraphicsPath ruta = RectRedondeado(r, radio))
            {
                g.DrawPath(lapiz, ruta);
            }
        }

        public static Color Aclarar(Color c, int cantidad)
        {
            return Color.FromArgb(
                c.A,
                Math.Min(255, c.R + cantidad),
                Math.Min(255, c.G + cantidad),
                Math.Min(255, c.B + cantidad));
        }

        public static Color Oscurecer(Color c, int cantidad)
        {
            return Color.FromArgb(
                c.A,
                Math.Max(0, c.R - cantidad),
                Math.Max(0, c.G - cantidad),
                Math.Max(0, c.B - cantidad));
        }

        /// <summary>
        /// Redondea las esquinas de un control pintando encima del color de
        /// fondo del padre. Se usa al final del evento Paint.
        /// Si "borde" es Color.Empty no se dibuja contorno.
        /// </summary>
        public static void RedondearEsquinas(
            Graphics g, int ancho, int alto, float radio, Color fondo, Color borde)
        {
            SmoothingMode anterior = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF area = new RectangleF(0, 0, ancho, alto);

            using (GraphicsPath esquinas = new GraphicsPath(FillMode.Alternate))
            using (GraphicsPath forma = RectRedondeado(area, radio))
            using (SolidBrush pincel = new SolidBrush(fondo))
            {
                esquinas.AddRectangle(new RectangleF(-2, -2, ancho + 4, alto + 4));
                esquinas.AddPath(forma, false);
                g.FillPath(pincel, esquinas);
            }

            if (borde != Color.Empty)
            {
                RectangleF areaBorde = new RectangleF(0.5f, 0.5f, ancho - 1f, alto - 1f);

                using (Pen lapiz = new Pen(borde, 1f))
                {
                    Contornear(g, lapiz, areaBorde, radio);
                }
            }

            g.SmoothingMode = anterior;
        }
    }


    /// <summary>
    /// Panel blanco con esquinas redondeadas y un título opcional.
    /// Se usa como "tarjeta" para agrupar controles.
    /// </summary>
    /// <summary>
    /// Panel blanco con esquinas redondeadas y un título opcional.
    /// Se usa como "tarjeta" para agrupar controles.
    /// </summary>
    internal class PanelTarjeta : Panel
    {
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public string Titulo { get; set; } = "";

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public float Radio { get; set; } = 14f;

        public PanelTarjeta()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            BackColor = Color.White;
        }
    

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            float f = DeviceDpi / 96f;

            g.Clear(Parent != null ? Parent.BackColor : Paleta.Fondo);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF area = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);

            using (SolidBrush relleno = new SolidBrush(BackColor))
            using (Pen borde = new Pen(Paleta.Borde, 1f))
            {
                Dibujo.Rellenar(g, relleno, area, Radio);
                Dibujo.Contornear(g, borde, area, Radio);
            }

            if (string.IsNullOrEmpty(Titulo))
            {
                return;
            }

            // Barrita de color junto al título
            using (SolidBrush acento = new SolidBrush(Paleta.Primario))
            {
                Dibujo.Rellenar(
                    g, acento,
                    new RectangleF(18f * f, 15f * f, 4f * f, 18f * f),
                    2f * f);
            }

            using (Font fuente = new Font("Segoe UI Semibold", 11f))
            using (SolidBrush texto = new SolidBrush(Paleta.Texto))
            {
                g.DrawString(Titulo, fuente, texto, 28f * f, 12f * f);
            }
        }
    }
}