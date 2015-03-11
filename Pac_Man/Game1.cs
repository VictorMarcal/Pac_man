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
        Texture2D comida;
        Texture2D sem_comida;

        Personagem pacman;


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
            graphics.PreferredBackBufferWidth = 600;
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

            pacman = new Personagem(Content, "pac2");

            comida = Content.Load<Texture2D>("comida");
            sem_comida = Content.Load<Texture2D>("sem_comida");

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
            pacman.Dispose();
            comida.Dispose();
            sem_comida.Dispose();
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


            if (teclado.IsKeyDown(Keys.W) && 
                !paredeEncontrada(new Vector2(pacman.Posicao.X, pacman.Posicao.Y - pacman.Velocidade * 10)))
            {
                pacman.moverPacMan(Direccao.Cima);
            }
            if (teclado.IsKeyDown(Keys.A) &&
                !paredeEncontrada(new Vector2(pacman.Posicao.X - pacman.Velocidade * 10, pacman.Posicao.Y)))
            {
                pacman.moverPacMan(Direccao.Esquerda);
            }
            if (teclado.IsKeyDown(Keys.D) &&
                !paredeEncontrada(new Vector2(pacman.Posicao.X + pacman.Velocidade * 10, pacman.Posicao.Y)))
            {
                pacman.moverPacMan(Direccao.Direita);
            }
            if (teclado.IsKeyDown(Keys.S) &&
                !paredeEncontrada(new Vector2(pacman.Posicao.X, pacman.Posicao.Y + pacman.Velocidade * 10)))
            {
                pacman.moverPacMan(Direccao.Baixo);
            }

            comer();

            // condiçoes que permitem o pacaman passar pelo tunel


            base.Update(gameTime);
            Console.WriteLine("valor de x {0}", pacman.Posicao.X);
            Console.WriteLine("valor de y {0}", pacman.Posicao.Y);

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

            pacman.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            base.Draw(gameTime);

        }
        //METODO PARA DETETAR PAREDES
        private bool paredeEncontrada(Vector2 posicaoFutura)
        {

            int posiçãoX = (int)Math.Round((30 * posicaoFutura.X) * 20 / 600);
            int posiçãoY = (int)Math.Round((30 * posicaoFutura.Y) * 20 / 600);
            if (mapa[posiçãoX, posiçãoY] == 1)
            {
                return true;
            }

            return false;

        }

        //metodo para eliminar comida apos pacman passar por cima

        private void comer()
        {
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] == 0)
                    {
                        mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 2;
                    }
                }
            }
        }

    }
    
}
