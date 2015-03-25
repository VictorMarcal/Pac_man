using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man.Animations
{
    public static class SpriteAnimationManager
    {
        private static List<SpriteAnimation> listAnimations;
        private static Random random;

        public static void Initialize()
        {
            random = new Random();
            listAnimations = new List<SpriteAnimation>();
        }

        public static void addAnimation(Texture2D textura, int largura, int altura, bool looping, Vector2 posicao, int fps, int maxDelay)
        {
            listAnimations.Add(new SpriteAnimation(textura, largura, altura, looping, posicao, fps, random.Next(Math.Min(100, maxDelay), maxDelay)));
        }

        public static void Update(GameTime gameTime)
        {
            //Limpar a lista de animações mortas
            listAnimations.RemoveAll(x => x.alive == false);

            foreach (SpriteAnimation animation in listAnimations)
            {
                animation.Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (SpriteAnimation animation in listAnimations)
            {
                animation.Draw(spriteBatch);
            }
        }
    }
}
