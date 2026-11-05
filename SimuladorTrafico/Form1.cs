using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;



namespace SimuladorTrafico
{
    #region Constructor
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();

            // Aplica colores, tarjetas y distribución (Form1_estilo.cs)
            AplicarEstilo();

            // Dibujo nuevo de la escena y de la gráfica (Form1_estilo.cs)
            panelSimulacion.Paint += (s, e) => DibujarEscena(e.Graphics);
            panelGrafica.Paint += (s, e) => DibujarGrafica(e.Graphics);

            temporizador.Interval = 50;
            temporizador.Tick += Temporizador_Tick;
        }
    #endregion


        #region Entorno
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion



        #region Movimiento y actualización del tráfico

        private void Temporizador_Tick(object? sender, EventArgs e)
        {
            #region Control  de los semáforos

            contadorSemaforo++;

            // Duraciones del ciclo
            int finVerdeHorizontal = verdeHorizontal;
            int finAmarilloHorizontal = finVerdeHorizontal + 40;
            int finSeguridadHorizontal = finAmarilloHorizontal + 20;

            int finVerdeVertical = finSeguridadHorizontal + verdeVertical;
            int finAmarilloVertical = finVerdeVertical + 40;
            int finCiclo = finAmarilloVertical + 20;

            // Reiniciar ciclo
            if (contadorSemaforo >= finCiclo)
            {
                contadorSemaforo = 0;
            }

            // Horizontal verde
            if (contadorSemaforo < finVerdeHorizontal)
            {
                semaforoHorizontal.PonerVerde();
                semaforoVertical.PonerRojo();
            }
            // Horizontal amarillo
            else if (contadorSemaforo < finAmarilloHorizontal)
            {
                semaforoHorizontal.PonerAmarillo();
                semaforoVertical.PonerRojo();
            }
            // Ambos rojos
            else if (contadorSemaforo < finSeguridadHorizontal)
            {
                semaforoHorizontal.PonerRojo();
                semaforoVertical.PonerRojo();
            }
            // Vertical verde
            else if (contadorSemaforo < finVerdeVertical)
            {
                semaforoHorizontal.PonerRojo();
                semaforoVertical.PonerVerde();
            }
            // Vertical amarillo
            else if (contadorSemaforo < finAmarilloVertical)
            {
                semaforoHorizontal.PonerRojo();
                semaforoVertical.PonerAmarillo();
            }
            // Ambos rojos
            else
            {
                semaforoHorizontal.PonerRojo();
                semaforoVertical.PonerRojo();
            }

            #endregion


            #region Generación de vehículos

            contadorGeneracion++;

            if (contadorGeneracion >= 30)
            {
                float y = panelSimulacion.Height / 2 - 10;
                bool espacioDisponible = true;

                foreach (Vehiculo vehiculo in vehiculos)
                {
                    if (vehiculo.X < 80)
                    {
                        espacioDisponible = false;
                        break;
                    }
                }

                if (espacioDisponible)
                {
                    Vehiculo nuevo = new Vehiculo(
                        0,
                        y,
                        velocidadVehiculos,
                        Paleta.CocheRojo
                    );

                    vehiculos.Add(nuevo);
                    totalVehiculosGenerados++;
                    contadorGeneracion = 0;
                }
            }

            contadorGeneracionVertical++;

            if (contadorGeneracionVertical >= 40)
            {
                bool hayVehiculoCerca = false;

                foreach (Vehiculo vehiculo in vehiculosVerticales)
                {
                    if (vehiculo.Y > panelSimulacion.Height - 80)
                    {
                        hayVehiculoCerca = true;
                        break;
                    }
                }

                if (!hayVehiculoCerca)
                {
                    Vehiculo nuevoVehiculo = new Vehiculo(
                        panelSimulacion.Width / 2 - 10,
                        panelSimulacion.Height,
                        velocidadVehiculos,
                        Paleta.CocheAzul,
                        true
                    );

                    vehiculosVerticales.Add(nuevoVehiculo);
                    totalVehiculosGenerados++;
                    contadorGeneracionVertical = 0;
                }
            }

            #endregion



            #region Movimiento, distancia entre coches y semáforo

            vehiculos.Sort((a, b) => b.X.CompareTo(a.X));

            for (int i = 0; i < vehiculos.Count; i++)
            {
                Vehiculo vehiculo = vehiculos[i];

                // Velocidad normal
                vehiculo.Velocidad = velocidadVehiculos;

                // Mantener distancia con el vehículo delantero
                if (i > 0)
                {
                    Vehiculo delantero = vehiculos[i - 1];

                    float distancia = delantero.X - (vehiculo.X + 37);

                    if (distancia <= 8)
                    {
                        vehiculo.Velocidad = 0;
                    }
                    else if (distancia < 30)
                    {
                        vehiculo.Velocidad = 1;
                    }
                }


                // Posición de la línea de alto
                float lineaAlto = panelSimulacion.Width / 2 - 50;

                // Comprobar si el vehículo todavía está antes de la línea
                bool antesDeLaLinea = vehiculo.X < lineaAlto;

                // Comportamiento del semáforo amarillo
                if (semaforoHorizontal.EstaAmarillo && antesDeLaLinea)
                {
                    // Reducir la velocidad al acercarse a la línea
                    if (vehiculo.X + 37 >= lineaAlto - 30)
                    {
                        vehiculo.Velocidad = 1;
                    }
                }

                // Comportamiento del semáforo rojo
                if (semaforoHorizontal.EstaRojo && antesDeLaLinea)
                {
                    if (vehiculo.X + 37 >= lineaAlto)
                    {
                        vehiculo.Velocidad = 0;
                    }
                }


                // Detener vehículos antes de la línea si no está en verde
                if (!semaforoHorizontal.EstaVerde &&
                    vehiculo.X < lineaAlto &&
                    vehiculo.X + 37 >= lineaAlto)
                {
                    vehiculo.Velocidad = 0;
                }

                // Registrar tiempo de espera
                if (vehiculo.Velocidad == 0)
                {
                    vehiculo.TiempoEspera++;

                    if (!vehiculo.YaContoEspera)
                    {
                        vehiculo.YaContoEspera = true;
                        vehiculosQueEsperaron++;
                    }

                    totalTiempoEspera++;
                }

                // Mover el vehículo una sola vez
                vehiculo.Mover();
            }


            // Movimiento de los vehículos verticales
            vehiculosVerticales.Sort((a, b) => a.Y.CompareTo(b.Y));

            for (int i = 0; i < vehiculosVerticales.Count; i++)
            {
                Vehiculo vehiculo = vehiculosVerticales[i];

                // Velocidad normal
                vehiculo.Velocidad = velocidadVehiculos;

                // Mantener distancia de seguridad
                if (i > 0)
                {
                    Vehiculo delantero = vehiculosVerticales[i - 1];

                    float distancia = vehiculo.Y - (delantero.Y + 38);

                    if (distancia <= 8)
                    {
                        vehiculo.Velocidad = 0;
                    }
                    else if (distancia < 30)
                    {
                        vehiculo.Velocidad = 1;
                    }
                }

                // Línea de alto vertical
                float lineaAltoVertical = panelSimulacion.Height / 2 + 50;

                // Comprobar si todavía está antes de la línea
                bool antesDeLaLinea = vehiculo.Y > lineaAltoVertical;

                // Semáforo amarillo
                if (semaforoVertical.EstaAmarillo && antesDeLaLinea)
                {
                    if (vehiculo.Y <= lineaAltoVertical + 30)
                    {
                        vehiculo.Velocidad = 1;
                    }
                }

                // Semáforo rojo
                if (semaforoVertical.EstaRojo && antesDeLaLinea)
                {
                    if (vehiculo.Y <= lineaAltoVertical + 38)
                    {
                        vehiculo.Velocidad = 0;
                    }
                }

                // Detener antes de la línea si no está en verde
                if (!semaforoVertical.EstaVerde &&
                    vehiculo.Y > lineaAltoVertical &&
                    vehiculo.Y <= lineaAltoVertical + 38)
                {
                    vehiculo.Velocidad = 0;
                }


                // Registrar tiempo de espera de vehículos verticales
                if (vehiculo.Velocidad == 0)
                {
                    vehiculo.TiempoEspera++;

                    if (!vehiculo.YaContoEspera)
                    {
                        vehiculo.YaContoEspera = true;
                        vehiculosQueEsperaron++;
                    }

                    totalTiempoEspera++;
                }

                // Mover el vehículo una sola vez
                vehiculo.Mover();
            }

            // Eliminar vehículos verticales que salen del panel
            for (int i = vehiculosVerticales.Count - 1; i >= 0; i--)
            {
                if (vehiculosVerticales[i].Y < -40)
                {
                    vehiculosVerticales.RemoveAt(i);
                    vehiculosPasados++;
                }
            }

            // Eliminar vehículos que salen del panel
            for (int i = vehiculos.Count - 1; i >= 0; i--)
            {
                if (vehiculos[i].X > panelSimulacion.Width)
                {
                    vehiculos.RemoveAt(i);
                    vehiculosPasados++;
                }
            }

            #endregion

            #region Tiempo y espera

            tiempoTranscurrido++;


            // Calcular el flujo vehicular por minuto
            double flujoVehicular = 0;

            if (tiempoTranscurrido > 0)
            {
                flujoVehicular =
                    vehiculosPasados * 60.0 / (tiempoTranscurrido / 20.0);
            }

            double esperaPromedio = 0;

            if (vehiculosQueEsperaron > 0)
            {
                esperaPromedio =
                    totalTiempoEspera / (double)vehiculosQueEsperaron / 20.0;
            }

            #endregion


            #region Estadisticas

            // Actualizar estadísticas
            lblVehiculos.Text = "Vehículos: " + vehiculos.Count;
            lblTiempo.Text = "Tiempo: " + (tiempoTranscurrido / 20) + " s";
            lblPasados.Text = "Vehículos que han pasado: " + vehiculosPasados;
            lblFlujo.Text = "Flujo: " +
         flujoVehicular.ToString("0.0") + " vehículos/min";
            lblEspera.Text = "Espera promedio: " +
            esperaPromedio.ToString("0.0") + " s";

            // Redibujar el panel
            panelSimulacion.Invalidate();

            #endregion


            #region Panel Grafica


            contadorGrafica++;

            if (contadorGrafica >= 20)
            {
                // Guardar las salidas de este segundo
                int nuevosVehiculos =
                    vehiculosPasados - vehiculosPasadosAnterior;

                historialSalidas.Add(nuevosVehiculos);

                // Conservar las últimas 5 mediciones de segundos
                if (historialSalidas.Count > 5)
                {
                    historialSalidas.RemoveAt(0);
                }

                // Calcular el flujo promedio de los últimos segundos
                int totalSalidas = 0;

                foreach (int salidas in historialSalidas)
                {
                    totalSalidas += salidas;
                }

                float flujoActual =
                    totalSalidas * 60f / historialSalidas.Count;

                flujoHistorico.Add(flujoActual);
                tiempoHistorico.Add(tiempoTranscurrido / 20);

                vehiculosPasadosAnterior = vehiculosPasados;
                contadorGrafica = 0;

                // Mantener un máximo de 30 muestras en la gráfica
                if (flujoHistorico.Count > 30)
                {
                    flujoHistorico.RemoveAt(0);
                    tiempoHistorico.RemoveAt(0);
                }

                panelGrafica.Invalidate();

                // Actualizar indicadores de flujo

                if (flujoHistorico.Count > 0)
                {
                    float flujoMaximo = flujoHistorico[0];

                    foreach (float flujo in flujoHistorico)
                    {
                        if (flujo > flujoMaximo)
                        {
                            flujoMaximo = flujo;
                        }
                    }

                    double tiempoSegundos = tiempoTranscurrido / 20.0;

                    double flujoPromedioGeneral = 0;

                    if (tiempoSegundos > 0)
                    {
                        flujoPromedioGeneral =
                            vehiculosPasados * 60.0 / tiempoSegundos;
                    }

                    lblFlujoMaximo.Text =
                        "Flujo máximo: " + flujoMaximo.ToString("0.0")
                        + " veh/min";

                    lblFlujoPromedio.Text =
                        "Flujo promedio general: "
                        + flujoPromedioGeneral.ToString("0.0")
                        + " veh/min";
                }
                else
                {
                    lblFlujoMaximo.Text = "Flujo máximo: 0 veh/min";
                    lblFlujoPromedio.Text = "Flujo promedio general: 0 veh/min";
                }

            }

            #endregion



        }

        #endregion



        #region Variables y configuración inicial
        private void FormPrincipal_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            temporizador.Stop();
            fondoEscena?.Dispose();
            base.OnFormClosed(e);
        }


        //Tiempo y vel.
        private int verdeHorizontal = 100;
        private int verdeVertical = 100;
        private int velocidadVehiculos = 3;

        //Semaforos
        private Semaforo semaforoHorizontal = new Semaforo();
        private int contadorSemaforo = 0;
        private Semaforo semaforoVertical = new Semaforo();
        private int contadorSemaforoVertical = 0;

        //Vehiculos
        private List<Vehiculo> vehiculos = new List<Vehiculo>();
        private List<Vehiculo> vehiculosVerticales = new List<Vehiculo>();

        //Temporizador
        private System.Windows.Forms.Timer temporizador = new System.Windows.Forms.Timer();
        private int tiempoTranscurrido = 0;
        private int vehiculosPasados = 0;

        //Generadores de veh.
        private int contadorGeneracion = 0;
        private int contadorGeneracionVertical = 0;

        // Métricas del trafico
        private int totalVehiculosGenerados = 0;
        private int totalTiempoEspera = 0;
        private int vehiculosQueEsperaron = 0;
        private int vehiculosDetenidos = 0;
        private Random aleatorio = new Random();

        //Datos grafica
        private int contadorGrafica = 0;
        private int vehiculosPasadosAnterior = 0;
        private List<float> flujoHistorico = new List<float>();
        private List<int> tiempoHistorico = new List<int>();
        private List<int> historialSalidas = new List<int>();

        private int tiempoUltimaMuestra = 0;




        #region Botones
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            temporizador.Start();
        }

        private void btnDetener_Click(object sender, EventArgs e)
        {
            temporizador.Stop();

        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            temporizador.Stop();

            // Eliminar todos los vehículos
            vehiculos.Clear();
            vehiculosVerticales.Clear();

            // Reiniciar contadores
            tiempoTranscurrido = 0;
            vehiculosPasados = 0;
            contadorGeneracion = 0;
            contadorGeneracionVertical = 0;
            contadorSemaforo = 0;

            // Reiniciar métricas
            totalVehiculosGenerados = 0;
            totalTiempoEspera = 0;
            vehiculosQueEsperaron = 0;

            // Actualizar etiquetas
            lblVehiculos.Text = "Vehículos: 0";
            lblTiempo.Text = "Tiempo: 0 s";
            lblPasados.Text = "Vehículos que han pasado: 0";
            lblFlujo.Text = "Flujo: 0 vehículos/min";
            lblEspera.Text = "Espera promedio: 0 s";

            // Redibujar el panel
            panelSimulacion.Invalidate();

            //Reinicira grafica
            flujoHistorico.Clear();
            tiempoHistorico.Clear();
            contadorGrafica = 0;
            vehiculosPasadosAnterior = 0;
            historialSalidas.Clear();

            panelGrafica.Invalidate();

        }

        private void lblVehiculos_Click(object sender, EventArgs e)
        {

        }

        private void grpConfiguracion_Enter(object sender, EventArgs e)
        {

        }

        private void lblVerdeHorizontal_Click(object sender, EventArgs e)
        {

        }

        private void btnAplicarConfiguracion_Click(object sender, EventArgs e)
        {
            // Aplicar los tiempos de los semáforos
            verdeHorizontal = (int)numVerdeHorizontal.Value * 20;
            verdeVertical = (int)numVerdeVertical.Value * 20;

            // Aplicar la velocidad de los vehículos
            velocidadVehiculos = (int)numVelocidad.Value;

            MessageBox.Show(
                "Configuración aplicada correctamente.\n\n" +
                "Velocidad: " + velocidadVehiculos + "\n" +
                "Verde horizontal: " + numVerdeHorizontal.Value + " segundos\n" +
                "Verde vertical: " + numVerdeVertical.Value + " segundos",
                "Simulador de tráfico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnPausar_Click(object sender, EventArgs e)
        {
            if (temporizador.Enabled)
            {
                temporizador.Stop();
                btnPausar.Text = "Reanudar";
            }
            else
            {
                temporizador.Start();
                btnPausar.Text = "Pausar";
            }
        }
        #endregion



    }

        #endregion


}