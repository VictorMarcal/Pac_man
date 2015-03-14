using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    public static class Colisoes
    {

        public static bool paredeEncontrada(byte[,] mapa, Vector2 posicaoFutura)
        {

            int posiçãoX = (int)Math.Round((30 * posicaoFutura.X) * 20 / 600);
            int posiçãoY = (int)Math.Round((30 * posicaoFutura.Y) * 20 / 600);
            if (mapa[posiçãoX, posiçãoY] == 1)
            {
                return true;
            }

            return false;

        }

    }
}
