using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    public static class Camera
    {

        static private GraphicsDeviceManager graphics;
        /// <summary>
        /// Instância do graphics device
        /// </summary>
        static public GraphicsDeviceManager Graphics
        {
            set { graphics = value; }
        }

        static private float worldWidth;
        /// <summary>
        /// Largura do mundo
        /// </summary>
        static public float WorldWith
        {
            get { return worldWidth; }
            set { worldWidth = value; }
        }

        static private float ratio;

        static private Vector2 target;
        /// <summary>
        /// Coordenadas virtuais centrais da camara
        /// </summary>
        static public Vector2 Target
        {
            set { target = value; }
        }

        //Última largura em pixels que a janela teve
        static private int lastSeenPixelWidth = 0;

        static private Vector2 shake;
        static private int shakeAmount;

        static public void addShake(int valor)
        {
            shakeAmount += valor;
        }

        static private void calcularRatio()
        {
            if (Camera.lastSeenPixelWidth != Camera.graphics.PreferredBackBufferWidth)
            {
                //Só fazemos a divisão (pesada) se os valores tiverem alterado
                Camera.ratio = (Camera.graphics.PreferredBackBufferWidth - 200) / Camera.WorldWith;
                Camera.lastSeenPixelWidth = Camera.graphics.PreferredBackBufferWidth;
            }
        }

        /// <summary>
        /// Traduz coordenadas do mundo virtual para coordenadas em pixeis
        /// </summary>
        /// <param name="point">Coordenada no mundo virtual</param>
        /// <returns>Coordenada em pixeis</returns>
        static public Vector2 WorldPoint2Pixels(Vector2 point)
        {
            Camera.calcularRatio();
            Vector2 pixelPoint = new Vector2();

            //Calcular pixels em relação ao target da camara (centro)
            pixelPoint.X = (int)((point.X - target.X) * Camera.ratio + 0.5f);
            pixelPoint.Y = (int)((point.Y - target.Y) * Camera.ratio + 0.5f);

            //projetar pixeis calculados para o canto inferior esquerdo do ecrã
            pixelPoint.X += lastSeenPixelWidth / 2;
            pixelPoint.Y += Camera.graphics.PreferredBackBufferHeight / 2;

            //inverter coordenadas Y
            //pixelPoint.Y = Camera.graphics.PreferredBackBufferHeight - pixelPoint.Y;

            return pixelPoint + shake;
        }

        static public void Update(Random random)
        {
            if (shakeAmount > 0)
            {
                int denominador = 20;
                shake.X = random.Next(-(shakeAmount / denominador), shakeAmount / denominador);
                shake.Y = random.Next(-(shakeAmount / denominador), shakeAmount / denominador);
                shakeAmount -= shakeAmount / denominador;
            }
            else
            {
                shake = Vector2.Zero;
                shakeAmount = 0;
            }
        }

    }
}
