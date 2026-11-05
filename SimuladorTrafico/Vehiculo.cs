using System.Drawing;

namespace SimuladorTrafico
{
    public class Vehiculo : IMovible
    {
        #region Propiedades del vehículo

        public bool EsVertical { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Velocidad { get; set; }
        public Color ColorVehiculo { get; set; }
        public int TiempoEspera { get; set; }
        public bool YaContoEspera { get; set; }

        #endregion


        #region  Constructor

        public Vehiculo(
            float x,
            float y,
            float velocidad,
            Color color,
            bool esVertical = false)
        {
            X = x;
            Y = y;
            Velocidad = velocidad;
            ColorVehiculo = color;
            EsVertical = esVertical;
            TiempoEspera = 0;
            YaContoEspera = false;
        }

        #endregion


        #region Dibujo del vehículo

        public void Dibujar(Graphics g)
        {
            // Dibujar vehículo vertical
            if (EsVertical)
            {
                // Carrocería principal
                using (SolidBrush carroceriaV = new SolidBrush(ColorVehiculo))
                {
                    g.FillRectangle(carroceriaV, X, Y, 20, 38);
                }

                // Contorno de la carrocería
                g.DrawRectangle(
                    Pens.Black,
                    X, Y, 20, 38
                );

                // Ventana delantera
                g.FillRectangle(
                    Brushes.LightBlue,
                    X + 3, Y + 5, 14, 8
                );

                // Ventana trasera
                g.FillRectangle(
                    Brushes.LightBlue,
                    X + 3, Y + 24, 14, 8
                );

                // Separación entre ventanas
                g.DrawLine(
                    Pens.Black,
                    X + 3, Y + 19,
                    X + 17, Y + 19
                );

                // Llantas del lado izquierdo
                g.FillRectangle(Brushes.Black, X - 2, Y + 5, 4, 8);
                g.FillRectangle(Brushes.Black, X - 2, Y + 25, 4, 8);

                // Llantas del lado derecho
                g.FillRectangle(Brushes.Black, X + 18, Y + 5, 4, 8);
                g.FillRectangle(Brushes.Black, X + 18, Y + 25, 4, 8);

                // Luces delanteras
                g.FillRectangle(Brushes.Yellow, X + 2, Y, 5, 3);
                g.FillRectangle(Brushes.Yellow, X + 13, Y, 5, 3);

                // Luces traseras
                g.FillRectangle(Brushes.Red, X + 2, Y + 35, 5, 3);
                g.FillRectangle(Brushes.Red, X + 13, Y + 35, 5, 3);

                return;
            }

            // Dibujar ruedas horizontales
            using (SolidBrush ruedas = new SolidBrush(Color.Black))
            {
                g.FillEllipse(ruedas, X + 6, Y, 8, 5);
                g.FillEllipse(ruedas, X + 25, Y, 8, 5);
                g.FillEllipse(ruedas, X + 6, Y + 15, 8, 5);
                g.FillEllipse(ruedas, X + 25, Y + 15, 8, 5);
            }

            // Carrocería
            using (SolidBrush carroceria = new SolidBrush(ColorVehiculo))
            {
                g.FillRectangle(carroceria, X + 2, Y + 3, 34, 14);
            }

            // Contorno
            using (Pen contorno = new Pen(Color.Black, 1.5f))
            {
                g.DrawRectangle(contorno, X + 2, Y + 3, 34, 14);
            }

            // Techo
            using (SolidBrush techo = new SolidBrush(Dibujo.Oscurecer(ColorVehiculo, 25)))
            {
                g.FillRectangle(techo, X + 10, Y + 4, 16, 12);
            }

            // Ventanas
            using (SolidBrush ventanas = new SolidBrush(Color.LightCyan))
            {
                g.FillRectangle(ventanas, X + 12, Y + 5, 12, 4);
                g.FillRectangle(ventanas, X + 12, Y + 11, 12, 3);
            }

            // Faros delanteros
            using (SolidBrush faros = new SolidBrush(Color.Yellow))
            {
                g.FillRectangle(faros, X + 34, Y + 5, 3, 3);
                g.FillRectangle(faros, X + 34, Y + 12, 3, 3);
            }

            // Luces traseras
            using (SolidBrush luces = new SolidBrush(Color.Red))
            {
                g.FillRectangle(luces, X + 2, Y + 5, 2, 3);
                g.FillRectangle(luces, X + 2, Y + 12, 2, 3);
            }
        }

        #endregion


        #region Movimiento del vehículo

        public void Mover()
        {
            if (EsVertical)
            {
                Y -= Velocidad;
            }
            else
            {
                X += Velocidad;
            }
        }

        #endregion
    }
}