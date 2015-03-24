using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man.Animations
{
    class SpriteAnimation : SpriteManager
    {

        private float timeElapsed;
        public bool isLooping = false;

        private float timeToUpdate = 0.05f;
        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }

        public SpriteAnimation(Texture2D textura, int largura, int altura)
            : base(textura, largura, altura)
        {

        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                if (frameIndex < rectangulos.Length - 1)
                {
                    frameIndex++;
                }else if (isLooping){
                    frameIndex = 0;
                }
            }
        }
    }
}
