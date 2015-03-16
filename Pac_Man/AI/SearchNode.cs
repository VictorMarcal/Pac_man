/*
 Classe adaptada a partir do tutorial encontrado em:
 http://xnatd.blogspot.pt/2011/06/pathfinding-tutorial-part-1.html
*/

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man.AI
{
    public class SearchNode
    {

        private Vector2 posicao;
        /// <summary>
        /// Posicao no mapa
        /// </summary>
        public Vector2 Posicao
        {
            get { return posicao; }
            set { posicao = value; }
        }

        private bool caminho;
        /// <summary>
        /// Indica se esta posição no mapa é ou não caminho
        /// </summary>
        public bool Caminho
        {
            get { return caminho; }
            set { caminho = value; }
        }

        private SearchNode[] vizinhos;


        private SearchNode parent;
        /// <summary>
        /// Referência para nó que colocou este nó na lista aberta.
        /// Utilizado para refazer o caminho deste o alvo até à origem
        /// </summary>
        public SearchNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// Permite verificar se este nó está na openList
        /// </summary>
        public bool InOpenList;

        /// <summary>
        /// Permite verificar se este nó está na closedList
        /// </summary>
        public bool InClosedList;

        private float distanciaAlvo;
        /// <summary>
        /// Distancia aproximada desde o start node até ao goal node se o percurso passa por este nó (F)
        /// </summary>
        public float DistanciaAlvo
        {
            get { return distanciaAlvo; }
            set { distanciaAlvo = value; }
        }

        private float distanciaViajada;
        /// <summary>
        /// Distancia percorrida desde o nó de origem (G)
        /// </summary>
        public float DistanciaViajada
        {
            get { return distanciaViajada; }
            set { distanciaViajada = value; }
        }

        /// <summary>
        /// Contêm os 4 vizinhos deste nó (cima, baixo, esquerda, direita)
        /// </summary>
        public SearchNode[] Vizinhos
        {
            get { return vizinhos; }
            set { vizinhos = value; }
        }

    }
}
