using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pac_Man.AI;
using Pac_Man.Animations;

namespace Pac_Man
{

    /// <summary>
    /// Tipo de personagem
    /// </summary>
    public enum TipoPersonagem
    {
        /// <summary>
        /// Jogador
        /// </summary>
        Player,
        /// <summary>
        /// Fantasma
        /// </summary>
        NPC
    }

    /// <summary>
    /// Direções
    /// </summary>
    public enum Direccao
    {
        /// <summary>
        /// Cima
        /// </summary>
        Cima,
        /// <summary>
        /// Baixo
        /// </summary>
        Baixo,
        /// <summary>
        /// Esquerda
        /// </summary>
        Esquerda,
        /// <summary>
        /// Direita
        /// </summary>
        Direita,
        /// <summary>
        /// teleport para Baixo
        /// </summary>
        teleportParaBaixo,
        /// <summary>
        /// Teleport para cima
        /// </summary>
        teleportParaCima
    }

    /// <summary>
    /// Classe Personagem
    /// </summary>
    public class Personagem
    {
        private List<Bomba> bombas;
        /// <summary>
        /// Devolve a lista de bombas colocadas por esta personagem
        /// </summary>
        /// <returns></returns>
        public List<Bomba> getBombas()
        {
            return bombas;
        }

        private Texture2D textura;
        /// <summary>
        /// Textura utilizada por esta personagem
        /// </summary>
        public Texture2D Textura
        {
            get { return textura; }
        }

        private Vector2 posicao;
        /// <summary>
        /// Posição da personagem no mundo
        /// </summary>
        public Vector2 Posicao
        {
            get { return posicao; }
            set { posicao = value; }
        }

        private Vector2 posicaoTarget;

        /// <summary>
        /// Nº de jogador (1/2)
        /// </summary>
        public int player;
        /// <summary>
        /// Devolve a posição para onde a personagem se está a mover
        /// </summary>
        /// <returns></returns>
        public Vector2 getPosicaoTarget()
        {
            return posicaoTarget;
        }

        private float rotacao;
        /// <summary>
        /// Rotação da personagem
        /// </summary>
        public float Rotacao
        {
            get { return rotacao; }
            set { rotacao = value; }
        }

        private float velocidade;
        /// <summary>
        /// Velocidade a que a personagem se move
        /// </summary>
        public float Velocidade
        {
            get { return velocidade; }
            set { velocidade = value; }
        }

        private SpriteEffects flip;

        private TipoPersonagem tipoPersonagem;
        /// <summary>
        /// Devolve o tipo de personagem
        /// </summary>
        /// <returns>Tipo de personagem</returns>
        public TipoPersonagem TipoPersonagem()
        {
            return tipoPersonagem;
        }

        //score
        private int score;
        /// <summary>
        /// Pontos deste jogador
        /// </summary>
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        //AI
        PathFinder pathFinder;
        List<Vector2> path;

        /// <summary>
        /// Cor da personagem
        /// </summary>
        public Color cor;

        int pathOffset;

        int contadorPortalEntrada;

        Texture2D teleport, deteleport;

        /// <summary>
        /// Construtor da personagem
        /// </summary>
        /// <param name="content">ContentManager</param>
        /// <param name="assetName">Nome da textura desta personagem</param>
        /// <param name="tipoPersonagem">Tipo de personagem (player / NPC)</param>
        /// <param name="mapa">Mapa</param>
        /// <param name="cor">Cor da personagem</param>
        /// <param name="pathOffset">Path Offset</param>
        /// <param name="player">Nº de player (1 ou 2)</param>
        public Personagem(ContentManager content, string assetName, TipoPersonagem tipoPersonagem, byte[,] mapa, Color cor, int pathOffset, int player)
        {
            bombas = new List<Bomba>();
            this.Rotacao = 0f;
            this.velocidade = 0.7f;
            this.Posicao = new Vector2(1, 2);
            this.posicaoTarget = posicao;
            this.textura = content.Load<Texture2D>(assetName);
            this.flip = SpriteEffects.None;
            this.tipoPersonagem = tipoPersonagem;
            if (tipoPersonagem == Pac_Man.TipoPersonagem.NPC)
            {
                pathFinder = new PathFinder(mapa, null, null);
            }
            this.cor = cor;
            this.path = new List<Vector2>();
            this.pathOffset = pathOffset;
            this.contadorPortalEntrada = 0;
            this.player = player;
        }
        /// <summary>
        /// Insere uma bomba na posição atual da personagem
        /// </summary>
        /// <param name="score">Score da personagem</param>
        /// <param name="content">Instância de ContentManager</param>
        /// <returns></returns>
        public int insereBomba(int score, ContentManager content)
        {

            if (score >= 100)
            {
                Bomba bomba = new Bomba(Color.White, posicao);
                bomba.Parent = this;
                bombas.Add(bomba);
                return (score - 25);
            }
            else if(score < 100)
            {
                Som.playErro(content);
            }
            return (score);
        }

        /// <summary>
        /// Teleporta a personagem para uma determinada localização
        /// </summary>
        /// <param name="posicao">Posição para onde se pretende teleportar</param>
        /// <returns></returns>
        public Personagem teleportTo(Vector2 posicao)
        {
            this.Posicao = posicao;
            this.posicaoTarget = posicao;
            
            return this;
        }

        /// <summary>
        /// Atualiza as bombas desta personagem
        /// </summary>
        /// <param name="tempoExplosao">Tempo explosão</param>
        /// <param name="mapa">Mapa</param>
        /// <param name="gameTime">GameTime</param>
        /// <param name="Content">ContentManager</param>
        public void UpdateBombs(float tempoExplosao, byte[,] mapa, GameTime gameTime, ContentManager Content)
        {

            foreach (Bomba bomb in bombas)
            {
                bomb.Update(gameTime, mapa, Content);
            }

        }

        /// <summary>
        /// Remove uma determinada bomba da lista de bombas
        /// </summary>
        /// <param name="bomba">Bomba a remover</param>
        public void removeBomba(Bomba bomba)
        {
            bombas.Remove(bomba);
        }

        /// <summary>
        /// Remove todas as bombas colocadas por este personagem do mapa
        /// </summary>
        /// <param name="mapa">Mapa</param>
        public void removeBombas(byte[,] mapa)
        {
            foreach (Bomba bomba in bombas)
            {
                mapa[(int)bomba.Posicao.X, (int)bomba.Posicao.Y] = 2;
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="pacmans">Lista de pacmans</param>
        /// <param name="mapa">Mapa</param>
        /// <param name="listaFantasmas">Lista de fantasmas</param>
        /// <param name="tempoExplosao">Tempo expçosão</param>
        /// <param name="Content">ContentManager</param>
        public void Update(GameTime gameTime, List<Personagem> pacmans, byte[,] mapa, List<Personagem> listaFantasmas, float tempoExplosao, ContentManager Content)
        {

            if (tipoPersonagem == Pac_Man.TipoPersonagem.NPC)
            {

                //Fantasmas

                if (Posicao == posicaoTarget)
                {

                    //Escolher o pacman mais perto
                    float distancia = float.MaxValue;
                    Vector2 posicaoPacman = Vector2.Zero;
                    foreach (Personagem pacman in pacmans)
                    {
                        if (Vector2.Distance(this.Posicao, pacman.Posicao) < distancia)
                        {
                            distancia = Vector2.Distance(this.Posicao, pacman.Posicao);
                            posicaoPacman = pacman.Posicao;
                        }
                    }
                    moverFantasma(posicaoPacman, mapa, listaFantasmas, this);
                }

            }
            else
            {

                //Player

                if (mapa[(int)Posicao.X, (int)Posicao.Y] == 5 && contadorPortalEntrada > 100)
                {
                    if (teleport == null)
                    {
                        teleport = Content.Load<Texture2D>("teleport");
                    }
                    if (deteleport == null)
                    {
                        deteleport = Content.Load<Texture2D>("de-teleport");
                    }
                    //Estamos em cima de um portal de entrada, teleport!
                    SpriteAnimationManager.addAnimation(teleport, 3, 5, false, new Vector2(Posicao.X + 0.3f, Posicao.Y + 0.3f), 20, 0);

                    Vector2 posicaosaida = Utils.posicaoPortalSaida(mapa);
                    this.teleportTo(posicaosaida);
                    Som.playTeleport(Content);
                    SpriteAnimationManager.addAnimation(deteleport, 2, 5, false, new Vector2(Posicao.X + 0.3f, Posicao.Y + 0.3f), 20, 200);
                    Utils.eliminarPortais(mapa);
                    contadorPortalEntrada = 0;

                }

                if (Utils.existePortal(mapa, 5))
                {
                    contadorPortalEntrada += gameTime.ElapsedGameTime.Milliseconds;
                }
            }

            //Comum a NPC's e Players

            if (Vector2.Distance(posicaoTarget, Posicao) < 0.1f)
            {
                //Se estamos suficientemente perto do target, fazer snap para o target
                Posicao = posicaoTarget;
            }
            if (Posicao != posicaoTarget)
            {
                //Ainda não chegámos ao target, lerpar
                posicao = Vector2.Lerp(posicao, posicaoTarget, Velocidade);
            }
        }

        private void moverFantasma(Vector2 posicaoPacman, byte[,] mapa, List<Personagem> listaFantasmas, Personagem fantasma)
        {
            path = pathFinder.FindPath(this.Posicao, posicaoPacman, mapa, listaFantasmas, this);
            if (path.Count > 0)
            {
                this.posicaoTarget = path.First();
            }

        }

        /// <summary>
        /// Move a personagem numa determinada direção
        /// </summary>
        /// <param name="direccao">Direção em que se pretende mover</param>
        public void moverPacMan(Direccao direccao)
        {
            if (Posicao == posicaoTarget)
            {
                switch (direccao)
                {
                    case Direccao.Cima:
                        this.posicaoTarget.Y -= 1;
                        break;
                    case Direccao.Baixo:
                        this.posicaoTarget.Y += 1;
                        break;
                    case Direccao.Esquerda:
                        this.posicaoTarget.X -= 1;
                        flip = SpriteEffects.FlipHorizontally;
                        break;
                    case Direccao.Direita:
                        this.posicaoTarget.X += 1;
                        flip = SpriteEffects.None;
                        break;
                    case Direccao.teleportParaBaixo:
                        this.posicaoTarget.Y = 19;
                        break;
                    case Direccao.teleportParaCima:
                        this.posicaoTarget.Y = 0;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Desenha no ecrã a personagem
        /// </summary>
        /// <param name="spriteBatch">Instância de SpriteBath</param>
        /// <param name="gameTime">GameTime</param>
        /// <param name="mapa">Mapa</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Byte[,] mapa)
        {
            spriteBatch.Draw(Textura, new Vector2(Camera.WorldPoint2Pixels(Posicao).X + Textura.Width / 4, Camera.WorldPoint2Pixels(Posicao).Y + Textura.Height / 4), null, cor, this.Rotacao, Vector2.Zero, 1f, flip, 0f);
            //plantar bombas
            foreach (Bomba bomb in bombas)
            {
                if (!bomb.Exploded)
                {
                    mapa[(int)bomb.Posicao.X, (int)bomb.Posicao.Y] = 6;
                }
            }
            /*
             * DEBUG
             * Desenha os caminhos calculados pelos fantasmas
             * 
             */
            //if (path.Count > 0)
            //{
            //    foreach (Vector2 posicao in path)
            //    {
            //        spriteBatch.Draw(Textura, new Vector2(posicao.X * 30 + Textura.Width / 4 + pathOffset, posicao.Y * 30 + Textura.Height / 4 + pathOffset), null, cor, this.Rotacao, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
            //    }

            //}

        }

        /// <summary>
        /// Destruir a textura
        /// </summary>
        public void Dispose()
        {
            this.textura.Dispose();
        }

    }
}
