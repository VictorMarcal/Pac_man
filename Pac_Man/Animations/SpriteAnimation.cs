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
        private float timeElapsedDelay;
        private bool isLooping;
        public bool alive;
        private int maxDelay;

        private float timeToUpdate = 0.05f;
        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }

        public SpriteAnimation(Texture2D textura, int largura, int altura, bool looping, Vector2 posicao, int maxDelay)
            : base(textura, largura, altura, posicao)
        {
            this.alive = true;
            this.isLooping = looping;
            FramesPerSecond = 30;
            this.maxDelay = maxDelay;
            this.frameIndex = 0;
        }

        public void Update(GameTime gameTime)
        {

            if (timeElapsedDelay > maxDelay)
            {
                this.draw = true;
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timeElapsed > timeToUpdate)
                {
                    timeElapsed -= timeToUpdate;
                    if (frameIndex < rectangulos.Length - 1)
                    {
                        frameIndex++;
                    }
                    else if (isLooping)
                    {
                        frameIndex = 0;
                    }
                    else
                    {
                        frameIndex = rectangulos.Length - 1;
                        this.alive = false;
                    }
                }
            }
            else
            {
                timeElapsedDelay += gameTime.ElapsedGameTime.Milliseconds;
            }
        }
    }
}
