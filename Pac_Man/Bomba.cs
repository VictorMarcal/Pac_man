using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        

        public void Update(GameTime gameTime, byte[,] mapa,float posBombaX, float posBombaY)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
             if (timer<=0f)
            {
                if (posBombaX == 1) 
                {
                    mapa[(int)posBombaX + 1, (int)posBombaY] = 2;
                    if (posBombaY == 1) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                    }
                    else if (posBombaY == 18) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                    }
                    else if (posBombaY != 1 && posBombaY != 18) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                        mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                    }

                }
                else if (posBombaX == 18) 
                {
                     mapa[(int)posBombaX - 1, (int)posBombaY] = 2;
                    if (posBombaY == 1) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                    }
                    else if (posBombaY == 18) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                    }
                    else if (posBombaY != 1 && posBombaY != 18) 
                    {
                        mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                        mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                    }
                
                }
                else if (posBombaY == 1) 
                {
                    mapa[(int)posBombaX , (int)posBombaY+1] = 2;
                    mapa[(int)posBombaX-1, (int)posBombaY ] = 2;
                    mapa[(int)posBombaX+1, (int)posBombaY ] = 2;

                }
                else if (posBombaY == 18)
                {
                    mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                    mapa[(int)posBombaX - 1, (int)posBombaY] = 2;
                    mapa[(int)posBombaX + 1, (int)posBombaY] = 2;

                }
                else 
                {
                    
                    mapa[(int)posBombaX, (int)posBombaY + 1] = 2;
                    mapa[(int)posBombaX, (int)posBombaY - 1] = 2;
                    mapa[(int)posBombaX + 1, (int)posBombaY] = 2;
                    mapa[(int)posBombaX - 1, (int)posBombaY] = 2;

                }

                
                this.Exploded = true;
      
            }
        }
         
    }
}
