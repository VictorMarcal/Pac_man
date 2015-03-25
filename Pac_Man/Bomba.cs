using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pac_Man.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    class Bomba
    {
        private float timer;
        private bool exploded;
        public bool Exploded
        {
            get { return exploded;}
            set { exploded = value; }
        }
        //textura da bomba
        private Texture2D textura;
        public Texture2D Textura
        {
            get { return textura; }
        }
        //posicao da bomba
        private Vector2 posicao;
        public Vector2 Posicao
        {
            get { return posicao; }
            set { posicao = value; }
        }
        //construtor
        public Bomba( Color cor, Vector2 posicao)
        {
            this.Posicao = posicao;
            this.Exploded = false;
            this.timer = 600f;
            
        }

        private Texture2D explosao;
        

        public void Update(GameTime gameTime, byte[,] mapa,float posBombaX, float posBombaY, ContentManager Content)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            
             if (timer<=0f)
            {
                if (posBombaX == 1) 
                {
                    mapa[(int)posBombaX + 1, (int)posBombaY] = 2;
                    inserirExplosao((int)posBombaX + 1, (int)posBombaY, Content, 300);
                    if (posBombaY == 1) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                        inserirExplosao((int)posBombaX, (int)posBombaY + 1, Content, 300);
                    }
                     if (posBombaY == 18) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                        inserirExplosao((int)posBombaX - 1, (int)posBombaY, Content, 300);
                    }
                     if (posBombaY != 1 && posBombaY != 18) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                        inserirExplosao((int)posBombaX, (int)posBombaY - 1, Content, 300);
                        mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                        inserirExplosao((int)posBombaX, (int)posBombaY + 1, Content, 300);
                    }

                }
                else if (posBombaX == 18) 
                {
                     mapa[(int)posBombaX - 1, (int)posBombaY] = 2;
                     inserirExplosao((int)posBombaX - 1, (int)posBombaY, Content, 300);
                    if (posBombaY == 1) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                        inserirExplosao((int)posBombaX, (int)posBombaY + 1, Content, 300);
                    }
                     if (posBombaY == 18) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                        inserirExplosao((int)posBombaX, (int)posBombaY - 1, Content, 300);
                    }
                     if (posBombaY != 1 && posBombaY != 18) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                        inserirExplosao((int)posBombaX, (int)posBombaY - 1, Content, 300);
                        mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                        inserirExplosao((int)posBombaX, (int)posBombaY + 1, Content, 300);
                    }
                
                }
                else if (posBombaY == 1) 
                {
                    mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                    inserirExplosao((int)posBombaX, (int)posBombaY + 1, Content, 300);
                    if (posBombaX == 1) 
                    {                  
                        mapa[(int)posBombaX + 1, (int)posBombaY] = 2;
                        inserirExplosao((int)posBombaX + 1, (int)posBombaY, Content, 300);
                    }
                    if (posBombaX == 18) 
                    {
                        mapa[(int)posBombaX - 1, (int)posBombaY] = 2;
                        inserirExplosao((int)posBombaX - 1, (int)posBombaY, Content, 300);
                    }
                    if (posBombaX != 1 && posBombaX != 18) 
                    {
                        mapa[(int)posBombaX - 1, (int)posBombaY] = 2;
                        inserirExplosao((int)posBombaX - 1, (int)posBombaY, Content, 300);
                        mapa[(int)posBombaX + 1, (int)posBombaY] = 2;
                        inserirExplosao((int)posBombaX + 1, (int)posBombaY, Content, 300);
                    }
                    

                }
                else if (posBombaY == 18)
                {
                    mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                    inserirExplosao((int)posBombaX, (int)posBombaY - 1, Content, 300);
                    if (posBombaX == 1) 
                    {
                        mapa[(int)posBombaX + 1, (int)posBombaY] = 2;
                        inserirExplosao((int)posBombaX + 1, (int)posBombaY, Content, 300);
                    }
                    if (posBombaX == 18) 
                    {
                        mapa[(int)posBombaX - 1, (int)posBombaY] = 2;
                        inserirExplosao((int)posBombaX - 1, (int)posBombaY, Content, 300);
                    }
                    if (posBombaX != 1 && posBombaX != 18) 
                    {
                        mapa[(int)posBombaX - 1, (int)posBombaY] = 2;
                        inserirExplosao((int)posBombaX - 1, (int)posBombaY, Content, 300);
                        mapa[(int)posBombaX + 1, (int)posBombaY] = 2;
                        inserirExplosao((int)posBombaX + 1, (int)posBombaY, Content, 300);

                    }

                }
                else 
                {
                    
                    mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                    inserirExplosao((int)posBombaX, (int)posBombaY + 1, Content, 300);
                    mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                    inserirExplosao((int)posBombaX, (int)posBombaY - 1, Content, 300);
                    mapa[(int)posBombaX + 1, (int)posBombaY] = 2;
                    inserirExplosao((int)posBombaX + 1, (int)posBombaY, Content, 300);
                    mapa[(int)posBombaX - 1, (int)posBombaY] = 2;
                    inserirExplosao((int)posBombaX - 1, (int)posBombaY, Content, 300);

                }

                
                this.Exploded = true;

                inserirExplosao((int)Posicao.X, (int)Posicao.Y, Content, 0);
                timer = float.MaxValue;
            }
        }

        private void inserirExplosao(int x, int y, ContentManager Content, int maxDelay)
        {
            if (explosao == null)
            {
                explosao = Content.Load<Texture2D>("explosao");
            }
            SpriteAnimationManager.addAnimation(explosao, 9, 9, false,
                    new Vector2(x * 30 - (explosao.Width / 9 / 4) - 10, y * 30 - (explosao.Height / 9 / 4)), 35, maxDelay);
        }
         
    }
}
