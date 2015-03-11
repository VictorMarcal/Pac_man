#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Pac_Man
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bloco;
        Texture2D pac_man;
        Texture2D comida;
        float posiçãoPacX = 13f;
        float posiçãoPacY = 9f;
        float posiçãoTabX;
        float posiçãoTabY;


        float ultimoMovimento = 0f;
        KeyboardState teclado;
        byte[,] mapa ={{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                        {1,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,1},
                        {1,0,1,1,0,1,1,0,1,1,0,1,0,1,1,0,1,1,0,1},
                        {1,0,1,1,0,1,1,0,1,1,0,1,0,1,1,0,1,1,0,1},
                        {1,0,1,1,0,1,1,0,0,0,0,1,0,1,1,0,1,1,0,1},
                        {1,0,0,0,0,0,0,0,1,1,0,1,0,0,0,0,0,0,0,1},
                        {1,0,1,1,1,1,1,0,1,1,0,1,0,1,1,1,1,1,0,1},
                        {1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1},
                        {1,1,1,0,1,0,1,1,1,1,1,1,1,1,0,1,0,1,1,1},
                        {0,0,0,0,1,0,1,2,2,2,2,2,2,1,0,1,0,0,0,0},
                        {1,1,1,0,1,0,1,1,1,1,1,1,1,1,0,1,0,1,1,1},
                        {1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1},
                        {1,0,1,1,1,1,1,0,1,0,1,1,0,1,1,1,1,1,0,1},
                        {1,0,0,0,0,0,0,0,1,0,1,1,0,0,0,0,0,0,0,1},
                        {1,0,1,1,0,1,1,0,1,0,1,1,0,1,1,0,1,1,0,1},
                        {1,0,1,1,0,1,1,0,1,0,0,0,0,1,1,0,1,1,0,1},
                        {1,0,1,1,0,0,0,0,1,0,1,1,0,0,0,0,1,1,0,1},
                        {1,0,1,1,0,1,1,0,1,0,1,1,0,1,1,0,1,1,0,1},
                        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}};

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = true;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            teclado = new KeyboardState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bloco = Content.Load<Texture2D>("parede");
            pac_man = Content.Load<Texture2D>("pac2");
            comida = Content.Load<Texture2D>("comida");


            // TODO: use this.Content to load your game content here


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            bloco.Dispose();
            pac_man.Dispose();
            comida.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            ultimoMovimento += (float)gameTime.ElapsedGameTime.TotalSeconds;
            teclado = Keyboard.GetState();
            if (posiçãoTabY < 0)
            {
                posiçãoTabY = 30f;
            }



            if (teclado.IsKeyDown(Keys.W) && !paredeEncontradaCima())
            {
                posiçãoPacY -= 0.1f;
            }
            if (teclado.IsKeyDown(Keys.A) && !paredeEncontradaEsquerda())
            {
                posiçãoPacX -= 0.1f;
            }
            if (teclado.IsKeyDown(Keys.D) && !paredeEncontradaDireita())
            {
                posiçãoPacX += 0.1f;
            }
            if (teclado.IsKeyDown(Keys.S) && !paredeEncontradaBaixo())
            {
                posiçãoPacY += 0.1f;
            }
            

            // condiçoes que permitem o pacaman passar pelo tunel

            
            base.Update(gameTime);
            Console.WriteLine("valor de x {0}", posiçãoPacX);
            Console.WriteLine("valor de y {0}", posiçãoPacY);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //desenhar o mapa
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (mapa[x, y] == 1)
                    {
                        spriteBatch.Draw(bloco, new Vector2(x * 30, y * 30), Color.White);
                    }
                    if (mapa[x, y] == 0)
                    {
                        spriteBatch.Draw(comida, new Vector2((x * 30) + 12, (y * 30) + 12), Color.White);

                    }
                }
            }
            spriteBatch.Draw(pac_man, new Vector2((posiçãoPacX * 20), (posiçãoPacY * 20)), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);

        }
        //METODO PARA DETETAR PAREDES À DIREITA
        private bool paredeEncontradaDireita()
        {


            posiçãoTabX = (20 * posiçãoPacX) / 30;
            posiçãoTabY = (20 * posiçãoPacY) / 30;
            int posiçãoX = (int)Math.Round(posiçãoTabX + 0.33);
            int posiçãoY = (int)Math.Round(posiçãoTabY);
            if (mapa[posiçãoX, posiçãoY] == 1)
            {
                return true;
            }

            return false;

        }
        //METODO PARA DETETAR PAREDES À ESQUERDA
        private bool paredeEncontradaEsquerda()
        {
            posiçãoTabX = (20 * posiçãoPacX) / 30;
            posiçãoTabY = (20 * posiçãoPacY) / 30;
            int posiçãoX = (int)Math.Round(posiçãoTabX - 0.66);
            int posiçãoY = (int)Math.Round(posiçãoTabY);
            if (mapa[posiçãoX, posiçãoY] == 1)
            {
                return true;
            }

            return false;

        }
        //METODO PARA DETETAR PAREDES EM CIMA
        private bool paredeEncontradaCima()
        {
            posiçãoTabX = (20 * posiçãoPacX) / 30;
            posiçãoTabY = (20 * posiçãoPacY) / 30;
            int posiçãoX = (int)Math.Round(posiçãoTabX);
            int posiçãoY = (int)Math.Round(posiçãoTabY - 0.66);
            if (mapa[posiçãoX, posiçãoY] == 1)
            {
                return true;
            }

            return false;

        }
        //METODO PARA DETETAR PAREDES EM CIMA
        private bool paredeEncontradaBaixo()
        {
            posiçãoTabX = (20 * posiçãoPacX) / 30;
            posiçãoTabY = (20 * posiçãoPacY) / 30;
            int posiçãoX = (int)Math.Round(posiçãoTabX);
            int posiçãoY = (int)Math.Round(posiçãoTabY + 0.33);
            if (mapa[posiçãoX, posiçãoY] == 1)
            {
                return true;
            }

            return false;

        }

        
    
}
