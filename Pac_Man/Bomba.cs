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
    /// <summary>
    /// Classe Bomba
    /// </summary>
    public class Bomba
    {
        private float timer;
        private bool exploded;
        /// <summary>
        /// Indica se a bomba já explodiu
        /// </summary>
        public bool Exploded
        {
            get { return exploded;}
            set { exploded = value; }
        }

        private Personagem parent;

        /// <summary>
        /// Personagem que colocou a bomba
        /// </summary>
        public Personagem Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        
        private Vector2 posicao;
        /// <summary>
        /// Posição da bomba
        /// </summary>
        public Vector2 Posicao
        {
            get { return posicao; }
            set { posicao = value; }
        }
        
        /// <summary>
        /// Construtor da bomba
        /// </summary>
        /// <param name="cor">Cor da bomba</param>
        /// <param name="posicao">Posição</param>
        public Bomba( Color cor, Vector2 posicao)
        {
            this.Posicao = posicao;
            this.Exploded = false;
            this.timer = 200f;
        }

        private Texture2D explosao;
        
        /// <summary>
        /// Atualiza a bomba
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="mapa">Mapa</param>
        /// <param name="Content">ContentManager</param>
        public void Update(GameTime gameTime, byte[,] mapa, ContentManager Content)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (this.exploded == false) 
            { 
                Som.playAvisoBomba(Content); 
            }
            
             if (timer<=0f)
            {
                Som.playExplosao(Content);

                if (this.posicao.Y == 0)
                {
                    mapa[(int)this.posicao.X , (int)this.posicao.Y + 1] = 2;
                    inserirExplosao((int)this.posicao.X, (int)this.posicao.Y + 1, Content, 300);
                    inserirExplosao((int)this.posicao.X, 19, Content, 300);

                }
                else 
                    if (this.posicao.Y == 19)
                {
                    mapa[(int)this.posicao.X, (int)this.posicao.Y - 1] = 2;
                    inserirExplosao((int)this.posicao.X, (int)this.posicao.Y - 1, Content, 300);
                    inserirExplosao((int)this.posicao.X, 0, Content, 300);

                }

                
                else if (this.posicao.X == 1) 
                {
                    mapa[(int)this.posicao.X + 1, (int)this.posicao.Y] = 2;
                    inserirExplosao((int)this.posicao.X + 1, (int)this.posicao.Y, Content, 300);
                    if (this.posicao.Y == 1) 
                    {
                        mapa[(int)this.posicao.X, (int)this.posicao.Y + 1] = 2;
                        inserirExplosao((int)this.posicao.X, (int)this.posicao.Y + 1, Content, 300);
                    }
                    if (this.posicao.Y == 18) 
                    {
                        mapa[(int)this.posicao.X, (int)this.posicao.Y - 1] = 2;
                        inserirExplosao((int)this.posicao.X - 1, (int)this.posicao.Y, Content, 300);
                    }
                     if (this.posicao.Y != 1 && this.posicao.Y != 18) 
                    {
                        mapa[(int)this.posicao.X, (int)this.posicao.Y - 1] = 2;
                        inserirExplosao((int)this.posicao.X, (int)this.posicao.Y - 1, Content, 300);
                        mapa[(int)this.posicao.X, (int)this.posicao.Y + 1] = 2;
                        inserirExplosao((int)this.posicao.X, (int)this.posicao.Y + 1, Content, 300);
                    }

                }
                else if (this.posicao.X == 18) 
                {
                    mapa[(int)this.posicao.X - 1, (int)this.posicao.Y] = 2;
                    inserirExplosao((int)this.posicao.X - 1, (int)this.posicao.Y, Content, 300);
                     if (this.posicao.Y == 1) 
                    {
                        mapa[(int)this.posicao.X, (int)this.posicao.Y + 1] = 2;
                        inserirExplosao((int)this.posicao.X, (int)this.posicao.Y + 1, Content, 300);
                    }
                     if (this.posicao.Y == 18) 
                    {
                        mapa[(int)this.posicao.X, (int)this.posicao.Y - 1] = 2;
                        inserirExplosao((int)this.posicao.X, (int)this.posicao.Y - 1, Content, 300);
                    }
                     if (this.posicao.Y != 1 && this.posicao.Y != 18) 
                    {
                        mapa[(int)this.posicao.X, (int)this.posicao.Y - 1] = 2;
                        inserirExplosao((int)this.posicao.X, (int)this.posicao.Y - 1, Content, 300);
                        mapa[(int)this.posicao.X, (int)this.posicao.Y + 1] = 2;
                        inserirExplosao((int)this.posicao.X, (int)this.posicao.Y + 1, Content, 300);
                    }
                
                }
                else if (this.posicao.Y == 1) 
                {
                    mapa[(int)this.posicao.X, (int)this.posicao.Y + 1] = 2;
                    inserirExplosao((int)this.posicao.X, (int)this.posicao.Y + 1, Content, 300);
                    if (this.posicao.X == 1) 
                    {
                        mapa[(int)this.posicao.X + 1, (int)this.posicao.Y] = 2;
                        inserirExplosao((int)this.posicao.X + 1, (int)this.posicao.Y, Content, 300);
                    }
                    if (this.posicao.X == 18) 
                    {
                        mapa[(int)this.posicao.X - 1, (int)this.posicao.Y] = 2;
                        inserirExplosao((int)this.posicao.X - 1, (int)this.posicao.Y, Content, 300);
                    }
                    if (this.posicao.X != 1 && this.posicao.X != 18) 
                    {
                        mapa[(int)this.posicao.X - 1, (int)this.posicao.Y] = 2;
                        inserirExplosao((int)this.posicao.X - 1, (int)this.posicao.Y, Content, 300);
                        mapa[(int)this.posicao.X + 1, (int)this.posicao.Y] = 2;
                        inserirExplosao((int)this.posicao.X + 1, (int)this.posicao.Y, Content, 300);
                    }
                    

                }
                else if (this.posicao.Y == 18)
                {
                    mapa[(int)this.posicao.X, (int)this.posicao.Y - 1] = 2;
                    inserirExplosao((int)this.posicao.X, (int)this.posicao.Y - 1, Content, 300);
                    if (this.posicao.X == 1) 
                    {
                        mapa[(int)this.posicao.X + 1, (int)this.posicao.Y] = 2;
                        inserirExplosao((int)this.posicao.X + 1, (int)this.posicao.Y, Content, 300);
                    }
                    if (this.posicao.X == 18) 
                    {
                        mapa[(int)this.posicao.X - 1, (int)this.posicao.Y] = 2;
                        inserirExplosao((int)this.posicao.X - 1, (int)this.posicao.Y, Content, 300);
                    }
                    if (this.posicao.X != 1 && this.posicao.X != 18) 
                    {
                        mapa[(int)this.posicao.X - 1, (int)this.posicao.Y] = 2;
                        inserirExplosao((int)this.posicao.X - 1, (int)this.posicao.Y, Content, 300);
                        mapa[(int)this.posicao.X + 1, (int)this.posicao.Y] = 2;
                        inserirExplosao((int)this.posicao.X + 1, (int)this.posicao.Y, Content, 300);
                    }

                }
                else 
                {
                    
                    mapa[(int)this.posicao.X, (int)this.posicao.Y + 1] = 2;
                    inserirExplosao((int)this.posicao.X, (int)this.posicao.Y + 1, Content, 300);
                    mapa[(int)this.posicao.X, (int)this.posicao.Y - 1] = 2;
                    inserirExplosao((int)this.posicao.X, (int)this.posicao.Y - 1, Content, 300);
                    mapa[(int)this.posicao.X + 1, (int)this.posicao.Y] = 2;
                    inserirExplosao((int)this.posicao.X + 1, (int)this.posicao.Y, Content, 300);
                    mapa[(int)this.posicao.X - 1, (int)this.posicao.Y] = 2;
                    inserirExplosao((int)this.posicao.X - 1, (int)this.posicao.Y, Content, 300);

                }

                
                this.Exploded = true;

                inserirExplosao((int)Posicao.X, (int)Posicao.Y, Content, 0);
                Camera.addShake(300);
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
                    new Vector2(x + 0.50f, y + 0.25f), 35, maxDelay);
        }
         
    }
}
