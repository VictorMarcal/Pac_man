﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pac_Man.AI;

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

        //AI
        PathFinder pathFinder;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        /// <param name="assetName">Nome da textura desta personagem</param>
        public Personagem(ContentManager content, string assetName, TipoPersonagem tipoPersonagem, byte[,] mapa)
        {
            this.Rotacao = 0f;
            this.velocidade = 0.7f;
            this.Posicao = new Vector2(1, 2);
            this.posicaoTarget = posicao;
            this.textura = content.Load<Texture2D>(assetName);
            this.flip = SpriteEffects.None;
            this.tipoPersonagem = tipoPersonagem;
            if (tipoPersonagem == Pac_Man.TipoPersonagem.NPC)
            {
                pathFinder = new PathFinder(mapa);
            }
        }

        public Personagem teleportTo(Vector2 posicao)
        {
            this.Posicao = posicao;
            this.posicaoTarget = posicao;
            return this;
        }

        public void Update(GameTime gameTime, Vector2 posicaoPacman, List<Personagem> listaFantasmas)
        {
            if (tipoPersonagem == Pac_Man.TipoPersonagem.NPC)
            {
                if (Posicao == posicaoTarget)
                {
                    moverFantasma(posicaoPacman, listaFantasmas);
                }
                
            }
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

        private void moverFantasma(Vector2 posicaoPacman, List<Personagem> listaFantamas)
        {
            this.posicaoTarget = pathFinder.FindPath(this.Posicao, posicaoPacman, listaFantamas, this).First();
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
