using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man.Animations
{
    /// <summary>
    /// Classe SpriteAnimationManager
    /// </summary>
    public static class SpriteAnimationManager
    {
        private static List<SpriteAnimation> listAnimations;
        private static Random random;

        /// <summary>
        /// Inicializa a lista de animações
        /// </summary>
        public static void Initialize()
        {
            random = new Random();
            listAnimations = new List<SpriteAnimation>();
        }

        /// <summary>
        /// Adiciona uma animação à lista de animações
        /// </summary>
        /// <param name="textura">Spritesheet da animação</param>
        /// <param name="largura">Largura da spritesheet</param>
        /// <param name="altura">Altura da spritesheet</param>
        /// <param name="looping">Loop?</param>
        /// <param name="posicao">Posição da animação</param>
        /// <param name="fps">FPS pretendidos</param>
        /// <param name="maxDelay">Delay até começar a animação</param>
        public static void addAnimation(Texture2D textura, int largura, int altura, bool looping, Vector2 posicao, int fps, int maxDelay)
        {
            listAnimations.Add(new SpriteAnimation(textura, largura, altura, looping, posicao, fps, random.Next(Math.Min(100, maxDelay), maxDelay)));
        }

        /// <summary>
        /// Elimina todas as animações da lista de animações
        /// </summary>
        public static void resetAnimations()
        {
            listAnimations.Clear();
        }

        /// <summary>
        /// Update das animações na lista
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public static void Update(GameTime gameTime)
        {
            //Limpar a lista de animações mortas
            listAnimations.RemoveAll(x => x.alive == false);

            foreach (SpriteAnimation animation in listAnimations)
            {
                animation.Update(gameTime);
            }
        }

        /// <summary>
        /// Desenha as animações da lista
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (SpriteAnimation animation in listAnimations)
            {
                animation.Draw(spriteBatch);
            }
        }
    }
}
