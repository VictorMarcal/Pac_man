using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    public static class Colisoes
    {

        private static List<Personagem> listaTempPersonagens;

        public static bool paredeEncontrada(byte[,] mapa, Vector2 posicaoFutura)
        {
            int posiçãoX = (int)Math.Round((30 * posicaoFutura.X) * 20 / 600);
            int posiçãoY = (int)Math.Round((30 * posicaoFutura.Y) * 20 / 600);
            if (mapa[posiçãoX, posiçãoY] == 1 || mapa[posiçãoX,posiçãoY]== 3 || mapa[posiçãoX,posiçãoY]== 6)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Devolve uma lista de fantasmas que foram mortos por uma bomba
        /// </summary>
        /// <param name="posicaoBomba">Posição central da bomba</param>
        /// <param name="fantasmas">Lista de fantasmas</param>
        /// <returns></returns>
        public static List<Personagem> bombaFantasma(Vector2 posicaoBomba, List<Personagem> fantasmas)
        {
            if (listaTempPersonagens == null)
            {
                listaTempPersonagens = new List<Personagem>();
            }
            else
            {
                listaTempPersonagens.Clear();
            }
            foreach (Personagem fantasma in fantasmas)
            {
                for (int i = 1; i < 3; i++)
                {
                    if (Math.Round(posicaoBomba.X) == Math.Round(fantasma.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(fantasma.Posicao.Y)
                        || Math.Round(posicaoBomba.X + i) == Math.Round(fantasma.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(fantasma.Posicao.Y)
                        || Math.Round(posicaoBomba.X - i) == Math.Round(fantasma.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(fantasma.Posicao.Y)
                        || Math.Round(posicaoBomba.X) == Math.Round(fantasma.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(fantasma.Posicao.Y + i)
                        || Math.Round(posicaoBomba.X) == Math.Round(fantasma.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(fantasma.Posicao.Y - i))
                    {
                        //Este fantasma está ao alcance da bomba, bye bye
                        listaTempPersonagens.Add(fantasma);
                    } 
                }
            }
            return listaTempPersonagens;
        }

    }
}
