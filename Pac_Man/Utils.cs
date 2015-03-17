using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    public static class Utils
    {
        /// <summary>
        /// Devolve true caso já exista um portal no mapa
        /// </summary>
        /// <param name="mapa">Mapa</param>
        /// <returns>True or False</returns>
        public static bool existePortal(byte[,] mapa, int tipo){
            for(int x = 0; x < mapa.GetLength(0); x++)
            {
                for (int y = 0; y < mapa.GetLength(1); y++)
                {
                    if (mapa[x, y] == tipo)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Devolve a posicao no mapa do portal de saida
        /// </summary>
        /// <param name="mapa">Mapa</param>
        /// <returns>Vector2</returns>
        public static Vector2 posicaoPortalSaida(byte[,] mapa)
        {
            Vector2 posicao = Vector2.Zero;
            for (int x = 0; x < mapa.GetLength(0); x++)
            {
                for (int y = 0; y < mapa.GetLength(1); y++)
                {
                    if (mapa[x, y] == 4)
                    {
                        posicao.X = x;
                        posicao.Y = y;
                    }
                }
            }
            return posicao;
        }

        /// <summary>
        /// Elimina os portais do mapa
        /// </summary>
        /// <param name="mapa">Mapa</param>
        public static void eliminarPortais(byte[,] mapa)
        {
            for (int x = 0; x < mapa.GetLength(0); x++)
            {
                for (int y = 0; y < mapa.GetLength(1); y++)
                {
                    if (mapa[x, y] == 4 || mapa[x, y] == 5)
                    {
                        mapa[x, y] = 0;
                    }
                }
            }
        }
    }
}
