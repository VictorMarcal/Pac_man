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
    public abstract class SpriteManager
    {

        protected Texture2D textura;
        public Vector2 posicao = Vector2.Zero;
        public Color cor = Color.White;
        public Vector2 origem;
        public float rotacao = 0f;
        public float escala = 1f;
        public SpriteEffects spriteEffect;
        protected Rectangle[] rectangulos;
        protected int frameIndex = 0;

        public SpriteManager(Texture2D textura, int linhas, int colunas)
        {
            this.textura = textura;
            int altura = textura.Width / linhas;
            int largura = textura.Height / colunas;
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura, posicao, rectangulos[frameIndex], cor, rotacao, origem, escala, spriteEffect, 0f);
        }
    }
}
