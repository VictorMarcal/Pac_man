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
    
    public enum TipoPersonagem
    {
        Player,
        NPC
    }

    public enum Direccao
    {
        Cima,
        Baixo,
        Esquerda,
        Direita
    }

    public class Personagem
    {
        float timer;
        private List<Bomba> bombas;

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
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        //AI
        PathFinder pathFinder;
        List<Vector2> path;

        Color cor;

        int pathOffset;

        int contadorPortalEntrada;

        Texture2D teleport, deteleport;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        /// <param name="assetName">Nome da textura desta personagem</param>
        public Personagem(ContentManager content, string assetName, TipoPersonagem tipoPersonagem, byte[,] mapa, Color cor, int pathOffset)
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
        }
        public int insereBomba(int score)
        {
            timer = 600f;
            if (score >= 100)
            {
                Bomba bomba = new Bomba(Color.White, posicao);
                bombas.Add(bomba);
                return (score - 100);
            }
            return (score);
        }

        public Personagem teleportTo(Vector2 posicao)
        {
            this.Posicao = posicao;
            this.posicaoTarget = posicao;
            return this;
        }

        public void UpdateBombs(float tempoExplosao, byte[,] mapa,GameTime gameTime, ContentManager Content)
        {
            
            foreach (Bomba bomb in bombas)
            {
                bomb.Update(gameTime, mapa, bomb.Posicao.X, bomb.Posicao.Y, Content);
            }
            
        }
        public void Update(GameTime gameTime, List<Personagem> pacmans, byte[,] mapa, List<Personagem> listaFantasmas,float tempoExplosao, ContentManager Content)
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
                    if(teleport == null){
                        teleport = Content.Load<Texture2D>("teleport");
                    }
                    if (deteleport == null)
                    {
                        deteleport = Content.Load<Texture2D>("de-teleport");
                    }
                    //Estamos em cima de um portal de entrada, teleport!
                    SpriteAnimationManager.addAnimation(teleport, 3, 5, false, Posicao, 15, 0);

                    Vector2 posicaosaida = Utils.posicaoPortalSaida(mapa);
                    this.teleportTo(posicaosaida);
                    SpriteAnimationManager.addAnimation(deteleport, 2, 5, false,Posicao, 10, 200);
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
                    default:
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime,Byte[,] mapa)
        {
            spriteBatch.Draw(Textura, new Vector2(Camera.WorldPoint2Pixels(Posicao).X + Textura.Width / 4, Camera.WorldPoint2Pixels(Posicao).Y + Textura.Height / 4), null, cor, this.Rotacao, Vector2.Zero, 1f, flip, 0f);
            //plantar bombas
            foreach (Bomba bomb in bombas)
            {
                if (bomb.Exploded == false)
                {
                    mapa[(int)bomb.Posicao.X, (int)bomb.Posicao.Y] = 6;
                }
                else if(bomb.Exploded==true)
                {
                    mapa[(int)bomb.Posicao.X, (int)bomb.Posicao.Y] = 2;
                    
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
