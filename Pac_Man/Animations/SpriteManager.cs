/*
Código adaptado a partir de:
http://coderplex.blogspot.pt/2010/04/2d-animation-part-2-sprite-manager.html
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man.Animations
{
    /// <summary>
    /// Classe SpriteManager
    /// </summary>
    public abstract class SpriteManager
    {
        /// <summary>
        /// Textura da sprite
        /// </summary>
        protected Texture2D textura;
        private Vector2 posicao;
        /// <summary>
        /// Cor a utilizar aquando do desenho
        /// </summary>
        public Color cor = Color.White;
        /// <summary>
        /// Origem da sprite
        /// </summary>
        public Vector2 origem;
        /// <summary>
        /// Rotação
        /// </summary>
        public float rotacao = 0f;
        /// <summary>
        /// Escala
        /// </summary>
        public float escala = 1f;
        /// <summary>
        /// Rodada?
        /// </summary>
        public SpriteEffects spriteEffect;
        /// <summary>
        /// Rectângulo da sprite
        /// </summary>
        protected Rectangle[] rectangulos;
        /// <summary>
        /// Index do frame que está a ser desenhado
        /// </summary>
        protected int frameIndex = 0;
        /// <summary>
        /// Indica se a sprite deve ser desenhada
        /// </summary>
        protected bool draw;
        int altura, largura;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="textura">Textura / spritesheet</param>
        /// <param name="linhas">Nº de linhas da animação</param>
        /// <param name="colunas">Nº de colunas da animação</param>
        /// <param name="posicao">Posição</param>
        public SpriteManager(Texture2D textura, int linhas, int colunas, Vector2 posicao)
        {
            this.textura = textura;
            this.posicao = posicao;
            largura = textura.Width / colunas;
            altura = textura.Height / linhas;
            rectangulos = new Rectangle[linhas * colunas];
            int contador = 0;
            for (int i = 0; i < linhas; i++)
            {
                for (int j = 0; j < colunas; j++)
                {
                    rectangulos[contador] = new Rectangle(j * largura, i * altura, largura, altura);
                    contador++;
                }  
            }
            this.draw = false;
        }

        /// <summary>
        /// Desenha uma sprite
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.draw)
                spriteBatch.Draw(textura, new Vector2(Camera.WorldPoint2Pixels(posicao).X - (largura / 2),
                        Camera.WorldPoint2Pixels(posicao).Y - (altura / 2)), rectangulos[frameIndex], cor, rotacao, origem, escala, spriteEffect, 0f);
        }
    }
}
