using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private float rotacao;
        /// <summary>
        /// Rotação da personagem
        /// </summary>
        public float Rotacao
        {
            get { return rotacao; }
            set { rotacao = value; }
        }

        private int velocidade;
        /// <summary>
        /// Velocidade a que a personagem se move
        /// </summary>
        public int Velocidade
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

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        /// <param name="assetName">Nome da textura desta personagem</param>
        public Personagem(ContentManager content, string assetName, TipoPersonagem tipoPersonagem)
        {
            this.Rotacao = 0f;
            this.velocidade = 1;
            this.Posicao = new Vector2(1, 2);
            this.posicaoTarget = posicao;
            this.textura = content.Load<Texture2D>(assetName);
            this.flip = SpriteEffects.None;
            this.tipoPersonagem = tipoPersonagem;
        }

        public Personagem teleportTo(Vector2 posicao)
        {
            this.Posicao = posicao;
            this.posicaoTarget = posicao;
            return this;
        }

        public void Update(GameTime gameTime, Vector2 posicaoPacman)
        {
            if (tipoPersonagem == Pac_Man.TipoPersonagem.NPC)
            {
                moverFantasma(posicaoPacman);
            }
            if (Vector2.Distance(posicaoTarget, Posicao) < 0.1f)
            {
                //Se estamos suficientemente perto do target, fazer snap para o target
                Posicao = posicaoTarget;
            }
            if (Posicao != posicaoTarget)
            {
                //Ainda não chegámos ao target, lerpar
                posicao = Vector2.Lerp(posicao, posicaoTarget, 0.7f);
            }
        }

        private void moverFantasma(Vector2 posicaoPacman)
        {
            if (Posicao == posicaoTarget)
            {
                float difX = posicaoPacman.X - Posicao.X;
                float difY = posicaoPacman.Y - Posicao.Y;
                if (Math.Abs(difX) > Math.Abs(difY))
                {
                    //Vamos andar na lateral
                    if (difX > 0)
                    {
                        //Andar para a esquerda
                        this.posicaoTarget.X += Velocidade;
                        flip = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        //Andar para a direita
                        this.posicaoTarget.X -= Velocidade;
                        flip = SpriteEffects.None;
                    }
                }
                else
                {
                    //Vamos andar na vertical
                    if (difY > 0)
                    {
                        //Andar para baixo
                        this.posicaoTarget.Y += Velocidade;
                    }
                    else
                    {
                        //Andar para cima
                        this.posicaoTarget.Y -= Velocidade;
                    }
                }
            }
        }

        public void moverPacMan(Direccao direccao)
        {
            if (Posicao == posicaoTarget)
            {
                switch (direccao)
                {
                    case Direccao.Cima:
                        this.posicaoTarget.Y -= Velocidade;
                        break;
                    case Direccao.Baixo:
                        this.posicaoTarget.Y += Velocidade;
                        break;
                    case Direccao.Esquerda:
                        this.posicaoTarget.X -= Velocidade;
                        flip = SpriteEffects.FlipHorizontally;
                        break;
                    case Direccao.Direita:
                        this.posicaoTarget.X += Velocidade;
                        flip = SpriteEffects.None;
                        break;
                    default:
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Textura, new Vector2(Posicao.X * 30 + Textura.Width / 4, Posicao.Y * 30 + Textura.Height / 4), null, Color.White, this.Rotacao, Vector2.Zero, 1f, flip, 0f);
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
