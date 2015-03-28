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

        public static bool paredeEncontrada(byte[,] mapa, Vector2 posicaoFutura, Personagem pacman)
        {
            int posiçãoX = (int)Math.Round((30 * posicaoFutura.X) * 20 / 600);
            int posiçãoY = (int)Math.Round((30 * posicaoFutura.Y) * 20 / 600);

            if (posiçãoY < 0)
            {
                pacman.teleportTo(new Vector2(posiçãoX, 19));
                return false;

            }
            if (posiçãoY > 19)
            {
                pacman.teleportTo(new Vector2(posiçãoX, 0));
                return false;
            }

            else if (mapa[posiçãoX, posiçãoY] == 1 || mapa[posiçãoX, posiçãoY] == 3 || mapa[posiçãoX, posiçãoY] == 6)
            {
                return true;
            }



            return false;
        }
        /* public Vector2 colisao(Vector2 posicaofutura) 
         {

         }*/

        /// <summary>
        /// Devolve uma lista de fantasmas que foram mortos por uma bomba
        /// </summary>
        /// <param name="posicaoBomba">Posição central da bomba</param>
        /// <param name="fantasmas">Lista de fantasmas</param>
        /// <returns></returns>
        public static List<Personagem> bombaFantasmaPacman(Vector2 posicaoBomba, List<Personagem> fantasmas, List<Personagem> pacmans)
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
            foreach (Personagem pacman in pacmans)
            {
                for (int i = 1; i < 2; i++)
                {
                    if (Math.Round(posicaoBomba.X) == Math.Round(pacman.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(pacman.Posicao.Y)
                        || Math.Round(posicaoBomba.X + i) == Math.Round(pacman.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(pacman.Posicao.Y)
                        || Math.Round(posicaoBomba.X - i) == Math.Round(pacman.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(pacman.Posicao.Y)
                        || Math.Round(posicaoBomba.X) == Math.Round(pacman.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(pacman.Posicao.Y + i)
                        || Math.Round(posicaoBomba.X) == Math.Round(pacman.Posicao.X) && Math.Round(posicaoBomba.Y) == Math.Round(pacman.Posicao.Y - i))
                    {
                        //Este fantasma está ao alcance da bomba, bye bye
                        listaTempPersonagens.Add(pacman);
                    }
                }
            }

            return listaTempPersonagens;
        }

        public static List<Personagem> fantasmaPacman(List<Personagem> fantasmas, List<Personagem> pacmans)
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
                foreach (Personagem pacman in pacmans)
                {
                    if (Math.Round(fantasma.Posicao.X) == Math.Round(pacman.Posicao.X) 
                        && Math.Round(fantasma.Posicao.Y) == Math.Round(pacman.Posicao.Y))
                    {
                        //Este fantasma está a tocar num pacman!
                        listaTempPersonagens.Add(pacman);
                    }
                }
            }
            return listaTempPersonagens;
        }

    }
}
