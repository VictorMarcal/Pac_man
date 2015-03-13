using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{

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
        
        

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        /// <param name="assetName">Nome da textura desta personagem</param>
        public Personagem(ContentManager content, string assetName)
        {
            this.Rotacao = 0f;
            this.velocidade = 1;
            this.Posicao = new Vector2(1, 2);
            posicaoTarget = posicao;
            this.textura = content.Load<Texture2D>(assetName);
        }

        public void Update(GameTime gameTime)
        {
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
                        break;
                    case Direccao.Direita:
                        this.posicaoTarget.X += Velocidade;
                        break;
                    default:
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Texture2D dummyTexture)
        {
            spriteBatch.Draw(Textura, new Vector2(Posicao.X * 30 + Textura.Width / 4, Posicao.Y * 30 + Textura.Height / 4), Color.White);
            //spriteBatch.Draw(dummyTexture, new Rectangle((int)Posicao.X * 30, (int)Posicao.Y * 30, 30, 30), Color.Yellow);
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
