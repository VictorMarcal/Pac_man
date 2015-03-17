﻿#region Using Statements
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
        List<Personagem> fantasmas;

        SpriteFont myFont;
        int score=0;
        Texture2D dummyTexture;

        float ultimoMovimento = 0f;
        float time;
        int gametime;
        bool bombaLargada = false;
        float tempoExpulão;
        int numerodeBombasimplantadas=0;
        Vector2 PosiçãoBomba;
        Texture2D bomba;

        Texture2D portal_saida;
        Texture2D portal_entrada;

        /*
         * 0 - Caminho / Comida
         * 1 - Parede
         * 2 - ???
         * 3 - Caminho sem comida ?
         * 4 - Portal de saida
         * 5 - Portal de entrada
         * 6 - Bomba
        */
        
        KeyboardState teclado;
        byte[,] mapa ={{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                        {1,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,1},
                        {1,0,1,0,0,1,1,0,1,1,0,1,0,1,1,0,0,1,0,1},
                        {1,0,1,0,0,1,1,0,1,1,0,1,0,1,1,0,0,1,0,1},
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
                        {1,0,1,0,0,1,1,0,1,0,0,0,0,1,1,0,0,1,0,1},
                        {1,0,1,0,0,0,0,0,1,0,1,1,0,0,0,0,0,1,0,1},
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

            fantasmas = new List<Personagem>();

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

            pacman = new Personagem(Content, "pac2", TipoPersonagem.Player, mapa, Color.Yellow, 0);

            Personagem fantasma = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Green, 1).teleportTo(new Vector2(11, 12));
            fantasma.Velocidade = 0.5f;
            fantasmas.Add(fantasma);

            Personagem fantasma2 = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Red, 6).teleportTo(new Vector2(11, 10));
            fantasma2.Velocidade = 0.5f;
            fantasmas.Add(fantasma2);

            Personagem fantasma3 = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Blue, 4).teleportTo(new Vector2(11, 8));
            fantasma3.Velocidade = 0.5f;
            fantasmas.Add(fantasma3);
            
            comida = Content.Load<Texture2D>("comida");
            sem_comida = Content.Load<Texture2D>("sem_comida");

            portal_saida = Content.Load<Texture2D>("portal_saida");
            portal_entrada = Content.Load<Texture2D>("portal_entrada");

            bomba = Content.Load<Texture2D>("Bomb");

            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            myFont = Content.Load<SpriteFont>("MyFont");
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
            bomba.Dispose();
            
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
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            teclado = Keyboard.GetState();
            gametime = (int)time;

            if (ultimoMovimento > 0.09f)
            {
                if (teclado.IsKeyDown(Keys.W) &&
                    !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y - 1))
                    && teclado.IsKeyUp(Keys.S) 
                    && teclado.IsKeyUp(Keys.A) 
                    && teclado.IsKeyUp(Keys.D))
                {
                    pacman.moverPacMan(Direccao.Cima);
                }
                if (teclado.IsKeyDown(Keys.A) &&
                    !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X - 1, pacman.Posicao.Y)) 
                    && teclado.IsKeyUp(Keys.S)
                    && teclado.IsKeyUp(Keys.W)
                    && teclado.IsKeyUp(Keys.D))
                {
                    pacman.moverPacMan(Direccao.Esquerda);
                }
                if (teclado.IsKeyDown(Keys.D) &&
                    !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X + 1, pacman.Posicao.Y))
                    && teclado.IsKeyUp(Keys.S)
                    && teclado.IsKeyUp(Keys.A)
                    && teclado.IsKeyUp(Keys.W))
                {
                    pacman.moverPacMan(Direccao.Direita);
                }
                if (teclado.IsKeyDown(Keys.S) &&
                    !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y + 1))
                    && teclado.IsKeyUp(Keys.W)
                    && teclado.IsKeyUp(Keys.A)
                    && teclado.IsKeyUp(Keys.D))
                {
                    pacman.moverPacMan(Direccao.Baixo);
                }
                if (teclado.IsKeyDown(Keys.Space))
                {
                    if (Utils.existePortal(mapa, 4) && !Utils.existePortal(mapa, 5) && Utils.posicaoPortalSaida(mapa) != pacman.Posicao)
                    {
                        //Já existe portal de saída, vamos colocar um portal de entrada
                        mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 5;
                    }
                    else if (!Utils.existePortal(mapa, 4))
                    {
                        //Ainda não existe portal de saida, vamos colocar portal de saida
                        mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 4;
                    }
                }
                if (teclado.IsKeyDown(Keys.B))
                {
                    if (score > 100&& numerodeBombasimplantadas==0)
                    {
                        
                        // posição da bomba passa a ser igual à posição do pac neste instante de tempo!!
                        PosiçãoBomba = new Vector2(pacman.Posicao.X, pacman.Posicao.Y);
                        mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 6;
                       
                        bombaLargada = true;
                        numerodeBombasimplantadas = 1;
                        
                    }
                }
                if (bombaLargada == true) 
                {
                    tempoExpulão += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                pacman.Update(gameTime, pacman.Posicao, mapa, fantasmas);

                foreach (Personagem fantasma in fantasmas)
                {
                    fantasma.Update(gameTime, pacman.Posicao, mapa, fantasmas);
                }

                comer();
                Bomba();
                
                Console.WriteLine("valor de x {0}", pacman.Posicao.X);
                Console.WriteLine("valor de y {0}", pacman.Posicao.Y);
                ultimoMovimento = 0;

                base.Update(gameTime);
                
            }

        }

        
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //desenhar o mapa
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    switch (mapa[x, y])
                    {
                        case 0:
                            spriteBatch.Draw(comida, new Vector2((x * 30) + 12, (y * 30) + 12), Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(bloco, new Vector2(x * 30, y * 30), Color.White);
                            break;
                        case 4:
                            spriteBatch.Draw(portal_saida, new Vector2(x * 30, y * 30), Color.White);
                            break;
                        case 5:
                            spriteBatch.Draw(portal_entrada, new Vector2(x * 30, y * 30), Color.White);
                            break;
                        case 6:
                            spriteBatch.Draw(bomba, new Vector2(x*30, y*30), Color.White);
                            break;
                        default:
                            break;
                    }
                }
            }

            pacman.Draw(spriteBatch, gameTime);

            foreach (Personagem fantasma in fantasmas)
            {
                fantasma.Draw(spriteBatch, gameTime);
            }

            //desenhar texto e mostrar pontuaçao
            spriteBatch.DrawString(myFont, "Score", new Vector2(650, 10), Color.Yellow);
            spriteBatch.DrawString(myFont, score+"", new Vector2(680, 50), Color.Yellow);
            spriteBatch.DrawString(myFont, "Game Time", new Vector2(620, 150), Color.Yellow);
            spriteBatch.DrawString(myFont, gametime + "sec", new Vector2(680, 190), Color.Yellow);

            

            spriteBatch.End();

            base.Draw(gameTime);

        }
        

        //metodo para eliminar comida apos pacman passar por cima
        private void comer()
        {
           
            if(mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y]==0)
            {
                mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 2;
                score+=10;
            }
        }
        


        private void Bomba()
        {
            // expressões que difinem o acontecimento na vizinhança da bomba
            if (tempoExpulão >= 0.5f)
            {

                //explosão em cruz
                mapa[(int)PosiçãoBomba.X - 1, (int)PosiçãoBomba.Y] = 2;
                mapa[(int)PosiçãoBomba.X + 1, (int)PosiçãoBomba.Y] = 2;
                mapa[(int)PosiçãoBomba.X, (int)PosiçãoBomba.Y - 1] = 2;
                mapa[(int)PosiçãoBomba.X, (int)PosiçãoBomba.Y + 1] = 2;
                mapa[(int)PosiçãoBomba.X, (int)PosiçãoBomba.Y] = 2;
                score -= 100;
                bombaLargada = false;
                tempoExpulão = 0;
                numerodeBombasimplantadas = 0;
            }
                    
                    
                
            
        }
       
    }
    
}
