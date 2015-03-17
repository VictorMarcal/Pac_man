/*
Classe adaptada a partir do tutorial encontrado em:
 * http://xnatd.blogspot.pt/2011/06/pathfinding-tutorial-part-1.html
*/

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man.AI
{
    public class PathFinder
    {
        /// <summary>
        /// Guarda uma lista de posções do mapa em que se pode andar
        /// </summary>
        private SearchNode[,] searchNodes;

        /// <summary>
        /// Largura do mapa
        /// </summary>
        private int largura;

        /// <summary>
        /// Altura do mapa
        /// </summary>
        private int altura;

        //Reutilizado para não criar Vector2 em runtime
        private Vector2 posicao;

        /// <summary>
        /// Lista de nós que estão disponíveis para procurar
        /// </summary>
        private List<SearchNode> openList = new List<SearchNode>();

        /// <summary>
        /// Lista de nós que já foram estudados
        /// </summary>
        private List<SearchNode> closedList = new List<SearchNode>();

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="mapa">A matriz do mapa</param>
        public PathFinder(byte[,] mapa)
        {
            this.largura = mapa.GetLength(0);
            this.altura = mapa.GetLength(1);
            InicializarSearchNodes(mapa);
        }

        /// <summary>
        /// Divide a matriz do mapa num grapho de nodes
        /// </summary>
        /// <param name="mapa"></param>
        private void InicializarSearchNodes(byte[,] mapa)
        {
            searchNodes = new SearchNode[largura, altura];

            //Vamos criar um nó para cada um dos espaços do mapa
            for (int x = 0; x < largura; x++)
            {
                for (int y = 0; y < altura; y++)
                {
                    //Criar um nó para representar este espaço
                    SearchNode node = new SearchNode();
                    posicao.X = x;
                    posicao.Y = y;
                    node.Posicao = posicao;

                    //Só podemos andar nos espaços da matriz que têm 0
                    node.Caminho = mapa[x, y] == 0;

                    //Só queremos guardar na lista caminhos em que se pode andar
                    if (node.Caminho)
                    {
                        node.Vizinhos = new SearchNode[4];
                        searchNodes[x, y] = node;
                    }
                    
                }
            }

            //Para cada um dos nós que criámos, vamos ligá-lo aos vizinhos
            for(int x = 0; x < largura; x++){
                for (int y = 0; y < altura; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    //Só nos interessam os nós em que podemos andar
                    if (node == null || !node.Caminho)
                    {
                        continue;
                    }

                    //Criamos um array de todos os possíveis vizinhos que este nó pode ter
                    Vector2[] vizinhos = new Vector2[]{
                        new Vector2(x, y - 1), //Acima
                        new Vector2(x, y + 1), //Abaixo
                        new Vector2(x - 1, y), //Esquerda
                        new Vector2(x + 1, y) //Direita
                    };

                    //Iteramos por cada um dos possíveis vizinhos
                    for (int i = 0; i < vizinhos.Length; i++)
                    {
                        Vector2 position = vizinhos[i];

                        //Confirmar se este vizinho faz parte do mapa
                        if (position.X < 0 || position.X > largura - 1
                            || position.Y < 0 || position.Y > altura - 1)
                        {
                            continue;
                        }

                        SearchNode vizinho = searchNodes[(int)position.X, (int)position.Y];

                        //Só nos interessam os vizinhos em que se pode andar
                        if (vizinho == null || !vizinho.Caminho)
                        {
                            continue;
                        }

                        //Guardar uma referência para o vizinho
                        node.Vizinhos[i] = vizinho;

                    }
                }
            }

        }

        /// <summary>
        /// Devolve uma estimativa da distância entre dois pontos
        /// </summary>
        /// <param name="ponto1">Ponto de origem</param>
        /// <param name="ponto2">Objetivo</param>
        /// <returns></returns>
        private float Heuristic(Vector2 ponto1, Vector2 ponto2)
        {
            return Math.Abs(ponto1.X - ponto2.X) +
                   Math.Abs(ponto1.Y - ponto2.Y);
        }

        /// <summary>
        /// Faz reset ao estado dos nós
        /// </summary>
        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < largura; x++)
            {
                for (int y = 0; y < altura; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanciaViajada = float.MaxValue;
                    node.DistanciaAlvo = float.MaxValue;
                }
            }
        }

        /// <summary>
        /// Devolve o nó com distância mais pequena ao objetivo
        /// </summary>
        public SearchNode FindBestNode()
        {
            SearchNode currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            // Find the closest node to the goal.
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanciaAlvo < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanciaAlvo;
                }
            }
            return currentTile;
        }

        /// <summary>
        /// Usa o campo parent dos nodes para refazer
        /// um caminho do end node para o start node
        /// </summary>
        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            closedList.Add(endNode);

            SearchNode parentTile = endNode.Parent;

            // Trace back through the nodes using the parent fields
            // to find the best path.
            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            List<Vector2> finalPath = new List<Vector2>();

            // Reverse the path and transform into world space.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(closedList[i].Posicao.X ,
                                          closedList[i].Posicao.Y));
            }

            return finalPath;
        }

        /// <summary>
        /// Finds the optimal path from one point to another.
        /// </summary>
        public List<Vector2> FindPath(Vector2 startPoint, Vector2 endPoint)
        {
            // Only try to find a path if the start and end points are different.
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            /////////////////////////////////////////////////////////////////////
            // Step 1 : Clear the Open and Closed Lists and reset each node’s F 
            //          and G values in case they are still set from the last 
            //          time we tried to find a path. 
            /////////////////////////////////////////////////////////////////////
            ResetSearchNodes();

            // Store references to the start and end nodes for convenience.
            SearchNode startNode = searchNodes[(int)startPoint.X, (int)startPoint.Y];
            SearchNode endNode = searchNodes[(int)endPoint.X, (int)endPoint.Y];

            /////////////////////////////////////////////////////////////////////
            // Step 2 : Set the start node’s G value to 0 and its F value to the 
            //          estimated distance between the start node and goal node 
            //          (this is where our H function comes in) and add it to the 
            //          Open List. 
            /////////////////////////////////////////////////////////////////////
            startNode.InOpenList = true;

            startNode.DistanciaAlvo = Heuristic(startPoint, endPoint);
            startNode.DistanciaViajada = 0;

            openList.Add(startNode);

            /////////////////////////////////////////////////////////////////////
            // Setp 3 : While there are still nodes to look at in the Open list : 
            /////////////////////////////////////////////////////////////////////
            while (openList.Count > 0)
            {
                /////////////////////////////////////////////////////////////////
                // a) : Loop through the Open List and find the node that 
                //      has the smallest F value.
                /////////////////////////////////////////////////////////////////
                SearchNode currentNode = FindBestNode();

                /////////////////////////////////////////////////////////////////
                // b) : If the Open List empty or no node can be found, 
                //      no path can be found so the algorithm terminates.
                /////////////////////////////////////////////////////////////////
                if (currentNode == null)
                {
                    break;
                }

                /////////////////////////////////////////////////////////////////
                // c) : If the Active Node is the goal node, we will 
                //      find and return the final path.
                /////////////////////////////////////////////////////////////////
                if (currentNode == endNode)
                {
                    // Trace our path back to the start.
                    return FindFinalPath(startNode, endNode);
                }

                /////////////////////////////////////////////////////////////////
                // d) : Else, for each of the Active Node’s neighbours :
                /////////////////////////////////////////////////////////////////
                for (int i = 0; i < currentNode.Vizinhos.Length; i++)
                {
                    SearchNode neighbor = currentNode.Vizinhos[i];

                    //////////////////////////////////////////////////
                    // i) : Make sure that the neighbouring node can 
                    //      be walked across. 
                    //////////////////////////////////////////////////
                    if (neighbor == null || neighbor.Caminho == false)
                    {
                        continue;
                    }

                    //////////////////////////////////////////////////
                    // ii) Calculate a new G value for the neighbouring node.
                    //////////////////////////////////////////////////
                    float distanceTraveled = currentNode.DistanciaViajada + 1;

                    // An estimate of the distance from this node to the end node.
                    float heuristic = Heuristic(neighbor.Posicao, endPoint);

                    //////////////////////////////////////////////////
                    // iii) If the neighbouring node is not in either the Open 
                    //      List or the Closed List : 
                    //////////////////////////////////////////////////
                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        // (1) Set the neighbouring node’s G value to the G value 
                        //     we just calculated.
                        neighbor.DistanciaViajada = distanceTraveled;
                        // (2) Set the neighbouring node’s F value to the new G value + 
                        //     the estimated distance between the neighbouring node and
                        //     goal node.
                        neighbor.DistanciaAlvo = distanceTraveled + heuristic;
                        // (3) Set the neighbouring node’s Parent property to point at the Active 
                        //     Node.
                        neighbor.Parent = currentNode;
                        // (4) Add the neighbouring node to the Open List.
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    //////////////////////////////////////////////////
                    // iv) Else if the neighbouring node is in either the Open 
                    //     List or the Closed List :
                    //////////////////////////////////////////////////
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        // (1) If our new G value is less than the neighbouring 
                        //     node’s G value, we basically do exactly the same 
                        //     steps as if the nodes are not in the Open and 
                        //     Closed Lists except we do not need to add this node 
                        //     the Open List again.
                        if (neighbor.DistanciaViajada > distanceTraveled)
                        {
                            neighbor.DistanciaViajada = distanceTraveled;
                            neighbor.DistanciaAlvo = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////
                // e) Remove the Active Node from the Open List and add it to the 
                //    Closed List
                /////////////////////////////////////////////////////////////////
                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            // No path could be found.
            return new List<Vector2>();
        }

    }
}
