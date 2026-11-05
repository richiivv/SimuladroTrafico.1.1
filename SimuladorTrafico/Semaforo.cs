using System;
using System.Collections.Generic;
using System.Text;


namespace SimuladorTrafico
{
    public class Semaforo
    {
        public bool EstaVerde { get; private set; }
        public bool EstaAmarillo { get; private set; }

 public bool EstaRojo
        {
            get { return !EstaVerde && !EstaAmarillo; }
        }

        public Semaforo()
        {
            EstaVerde = true;
            EstaAmarillo = false;
        }

        public void CambiarEstado()
        {
            if (EstaVerde)
            {
                EstaVerde = false;
                EstaAmarillo = true;
            }
            else if (EstaAmarillo)
            {
                EstaAmarillo = false;
                EstaVerde = false;
            }
            else
            {
                EstaVerde = true;
                EstaAmarillo = false;
            }
        }
        public void PonerVerde()
        {
            EstaVerde = true;
            EstaAmarillo = false;
        }

        public void PonerAmarillo()
        {
            EstaVerde = false;
            EstaAmarillo = true;
        }

        public void PonerRojo()
        {
            EstaVerde = false;
            EstaAmarillo = false;
        }

    }

}
