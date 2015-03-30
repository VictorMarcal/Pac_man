#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Pac_Man.Animations;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
        Texture2D paredeFerro;
        Texture2D comida;
        Texture2D sem_comida;

        List<Personagem> fantasmas;
        List<Personagem> pacmans;
        SoundEffect somComer;
        SoundEffect somExplosao;
        SoundEffect somAviso;
        SoundEffect somTeleport;

        SpriteFont myFont;
        Texture2D dummyTexture;

        float ultimoMovimento = 0f;
        float time;
        int gametime;
        float contador;
        float tempoExpulão;
        int numerodeBombasimplantadas = 0;
        bool proximaBombaPac1;
        bool proximaBombaPac2;
        Texture2D bomba;

        Texture2D portal_saida;
        Texture2D portal_entrada;

        Random random;

        List<Personagem> listaTempPersonagens;
        List<Bomba> listaTempBombas;
        bool musicaPlay;
        enum GameStatus
        {
            inicio,
            jogo,
            GameOver,
            ganhou
            
        };
        GameStatus status;
        Texture2D corMenu;

        KeyboardState teclado;
        byte[,] mapa;

        /// <summary>
        /// Classe principal
        /// </summary>
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
            pacmans = new List<Personagem>();
            listaTempPersonagens = new List<Personagem>();
            listaTempBombas = new List<Bomba>();

            SpriteAnimationManager.Initialize();

            random = new Random();

            Camera.Graphics = graphics;
            Camera.Target = new Vector2(13.45f, 10.1f);
            Camera.WorldWith = 20;

            status = GameStatus.inicio;
            corMenu = new Texture2D(graphics.GraphicsDevice, 1, 1,false, SurfaceFormat.Color);
            corMenu.SetData<Color>(new Color[] { Color.Blue });
            musicaPlay = false;
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

            mapa = loadMapa();

            bloco = Content.Load<Texture2D>("parede");
            paredeFerro = Content.Load<Texture2D>("parede2");

            loadPacmans();

            loadFantasmas();

            comida = Content.Load<Texture2D>("comida");
            sem_comida = Content.Load<Texture2D>("sem_comida");
            bomba = Content.Load<Texture2D>("Bomb");
            portal_saida = Content.Load<Texture2D>("portal_saida");
            portal_entrada = Content.Load<Texture2D>("portal_entrada");


            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            myFont = Content.Load<SpriteFont>("MyFont");
            //som
            somComer = Content.Load<SoundEffect>("som\\pacmanComer");
            somAviso = Content.Load<SoundEffect>("som\\avisodaBomba");
            somExplosao = Content.Load<SoundEffect>("som\\explosao");
            somTeleport = Content.Load<SoundEffect>("som\\somteleport");
            Som.playMusica(Content);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            bloco.Dispose();
            paredeFerro.Dispose();
            foreach (Personagem pacman in pacmans)
            {
                pacman.Dispose();
            }
            foreach (Personagem fantasma in fantasmas)
            {
                fantasma.Dispose();
            }
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

            teclado = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (status == GameStatus.inicio && teclado.IsKeyDown(Keys.Enter))
            {
                status = GameStatus.jogo;
            }
            if (status == GameStatus.jogo)
            {

                ultimoMovimento += (float)gameTime.ElapsedGameTime.TotalSeconds;
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                gametime = (int)time;
                contador += 0.5f;


                if (ultimoMovimento > 0.09f)
                {

                    updateInput();




                    foreach (Personagem pacman in pacmans)
                    {
                        pacman.Update(gameTime, pacmans, mapa, fantasmas, tempoExpulão, Content);
                        pacman.UpdateBombs(tempoExpulão, mapa, gameTime, Content);
                    }

                    foreach (Personagem fantasma in fantasmas)
                    {
                        fantasma.Update(gameTime, pacmans, mapa, fantasmas, tempoExpulão, Content);
                    }


                    ultimoMovimento = 0;
                    tempoExpulão = 0;

                }

                comer();

                //atualizar explosões
                SpriteAnimationManager.Update(gameTime);

                //Atualizar camera
                Camera.Update(random);

                //Verificar morte de fantasmas ou pacman
                colisaoBomba();

                //Verificar colisao de fantasmas com pacmans
                colisaoFantasmaPacman();

                //verificar se todos foram assassinados brutalmente
                verificarGameOver();

                verificarJogoGanho();

            }
            //Game reset
            if ((status == GameStatus.GameOver || status == GameStatus.ganhou) && teclado.IsKeyDown(Keys.Enter))
            {
                resetGame();
            }

            base.Update(gameTime);

        }

        private void updateInput()
        {

            //Verificar se há um 1º jogador para entrar
            if ((teclado.IsKeyDown(Keys.W)
                || teclado.IsKeyDown(Keys.S)
                || teclado.IsKeyDown(Keys.D)
                || teclado.IsKeyDown(Keys.A)
                || teclado.IsKeyDown(Keys.Space))
                && pacmans.Find(x => x.player == 1) == null)
            {
                loadPacmans();
            }


            //Verificar se há um 2º jogador para entrar
            if ((teclado.IsKeyDown(Keys.Up)
                || teclado.IsKeyDown(Keys.Down)
                || teclado.IsKeyDown(Keys.Left)
                || teclado.IsKeyDown(Keys.Right)
                || teclado.IsKeyDown(Keys.Insert)
                || teclado.IsKeyDown(Keys.Delete))
                && pacmans.Find(x => x.player == 2) == null)
            {
                criarSegundoJogador();
            }

            foreach (Personagem pacman in pacmans)
            {
                if (pacman.player == 1)
                {
                    #region Player 1
                    if (teclado.IsKeyDown(Keys.W) &&
                            !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y - 1), pacman)
                            && teclado.IsKeyUp(Keys.S)
                            && teclado.IsKeyUp(Keys.A)
                            && teclado.IsKeyUp(Keys.D))
                    {
                        pacman.moverPacMan(Direccao.Cima);
                    }

                    if (teclado.IsKeyDown(Keys.A) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X - 1, pacman.Posicao.Y), pacman)
                        && teclado.IsKeyUp(Keys.S)
                        && teclado.IsKeyUp(Keys.W)
                        && teclado.IsKeyUp(Keys.D))
                    {
                        pacman.moverPacMan(Direccao.Esquerda);
                    }
                    if (teclado.IsKeyDown(Keys.D) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X + 1, pacman.Posicao.Y), pacman)
                        && teclado.IsKeyUp(Keys.S)
                        && teclado.IsKeyUp(Keys.A)
                        && teclado.IsKeyUp(Keys.W))
                    {
                        pacman.moverPacMan(Direccao.Direita);
                    }
                    if (teclado.IsKeyDown(Keys.S) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y + 1), pacman)
                        && teclado.IsKeyUp(Keys.W)
                        && teclado.IsKeyUp(Keys.A)
                        && teclado.IsKeyUp(Keys.D))
                    {
                        pacman.moverPacMan(Direccao.Baixo);
                    }
                    if (teclado.IsKeyDown(Keys.Space))
                    {
                        if (Utils.existePortal(mapa, 4) && !Utils.existePortal(mapa, 5) && Utils.posicaoPortalSaida(mapa).X != (int)pacman.Posicao.X && Utils.posicaoPortalSaida(mapa).Y != (int)pacman.Posicao.Y)
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
                    if (teclado.IsKeyDown(Keys.B) && proximaBombaPac1 == true)
                    {
                        if (pacman.Score > 0 && numerodeBombasimplantadas == 0)
                        {

                            // posição da bomba passa a ser igual à posição do pac neste instante de tempo!!
                            //Bomba bomb=new Bomba(Content, "Bomb", Color.White, pacman.Posicao);
                            //bombas.Add(bomb);
                            //PosiçãoBomba = new Vector2(pacman.Posicao.X, pacman.Posicao.Y);
                            //mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 6;
                              pacman.Score = pacman.insereBomba(pacman.Score, Content);
                              proximaBombaPac1 = false;
                            
                            //numerodeBombasimplantadas = 1;

                        }
                    }
                    if (teclado.IsKeyUp(Keys.B))
                    {
                        proximaBombaPac1 = true;
                    }
                    #endregion
                }
                else if(pacman.player == 2)
                {
                    #region Player 2

                    
                    if (teclado.IsKeyDown(Keys.Up) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y - 1), pacman)
                        && teclado.IsKeyUp(Keys.Down)
                        && teclado.IsKeyUp(Keys.Left)
                        && teclado.IsKeyUp(Keys.Right))
                    {
                        pacman.moverPacMan(Direccao.Cima);
                    }
                    if (teclado.IsKeyDown(Keys.Left) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X - 1, pacman.Posicao.Y), pacman)
                        && teclado.IsKeyUp(Keys.Down)
                        && teclado.IsKeyUp(Keys.Up)
                        && teclado.IsKeyUp(Keys.Right))
                    {
                        pacman.moverPacMan(Direccao.Esquerda);
                    }
                    if (teclado.IsKeyDown(Keys.Right) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X + 1, pacman.Posicao.Y), pacman)
                        && teclado.IsKeyUp(Keys.Down)
                        && teclado.IsKeyUp(Keys.Left)
                        && teclado.IsKeyUp(Keys.Up))
                    {
                        pacman.moverPacMan(Direccao.Direita);
                    }
                    if (teclado.IsKeyDown(Keys.Down) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y + 1), pacman)
                        && teclado.IsKeyUp(Keys.Up)
                        && teclado.IsKeyUp(Keys.Left)
                        && teclado.IsKeyUp(Keys.Right))
                    {
                        pacman.moverPacMan(Direccao.Baixo);
                    }
                    if (teclado.IsKeyDown(Keys.Insert))
                    {
                        if (Utils.existePortal(mapa, 4) && !Utils.existePortal(mapa, 5) && Utils.posicaoPortalSaida(mapa).X != (int)pacman.Posicao.X && Utils.posicaoPortalSaida(mapa).Y != (int)pacman.Posicao.Y)
                        {
                            //Já existe portal de saída, vamos colocar um portal de entrada
                            mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 5;
                        }
                        else if (!Utils.existePortal(mapa, 4) && !Utils.existePortal(mapa, 5))
                        {
                            //Ainda não existe portal de saida, vamos colocar portal de saida
                            mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 4;
                        }
                    }
                    if (teclado.IsKeyDown(Keys.Delete) && proximaBombaPac2 == true)
                    {
                        if (pacman.Score > 0 && numerodeBombasimplantadas == 0)
                        {

                            // posição da bomba passa a ser igual à posição do pac neste instante de tempo!!
                            //Bomba bomb=new Bomba(Content, "Bomb", Color.White, pacman.Posicao);
                            //bombas.Add(bomb);
                            //PosiçãoBomba = new Vector2(pacman.Posicao.X, pacman.Posicao.Y);
                            //mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 6;
                            pacman.Score = pacman.insereBomba(pacman.Score,Content);
                            proximaBombaPac2 = false;
                            //numerodeBombasimplantadas = 1;

                        }


                    }
                    if (teclado.IsKeyUp(Keys.Delete))
                    {
                        proximaBombaPac2 = true;
                    }
                #endregion
                }
                
            }

        }


        /// <summary>
        /// Desenha o jogo
        /// </summary>
        /// <param name="gameTime">GameTime</param>
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
                            spriteBatch.Draw(comida, new Vector2(Camera.WorldPoint2Pixels(new Vector2(x, y)).X + 12,
                                                                 Camera.WorldPoint2Pixels(new Vector2(x, y)).Y + 12), Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(bloco, Camera.WorldPoint2Pixels(new Vector2(x, y)), Color.White);
                            break;
                        case 3:
                            spriteBatch.Draw(paredeFerro, Camera.WorldPoint2Pixels(new Vector2(x, y)), Color.White);
                            break;

                        case 4:
                            spriteBatch.Draw(portal_saida, Camera.WorldPoint2Pixels(new Vector2(x, y)), Color.White);
                            break;
                        case 5:
                            spriteBatch.Draw(portal_entrada, Camera.WorldPoint2Pixels(new Vector2(x, y)), Color.White);
                            break;
                        case 6:
                            spriteBatch.Draw(bomba, Camera.WorldPoint2Pixels(new Vector2(x, y)), Color.White);
                            contador = 0f;

                            break;
                        default:
                            break;
                    }
                }
            }


            foreach (Personagem pacman in pacmans)
            {
                pacman.Draw(spriteBatch, gameTime, mapa);
            }

            foreach (Personagem fantasma in fantasmas)
            {
                fantasma.Draw(spriteBatch, gameTime, mapa);
            }

            //Desenhar explosões
            SpriteAnimationManager.Draw(spriteBatch);

            int offset = 0;
            foreach (Personagem pacman in pacmans)
            {

                //desenhar texto e mostrar pontuaçao
                spriteBatch.DrawString(myFont, "Score", new Vector2(650, 10 + offset), pacman.player == 1 ? Color.Yellow : Color.Pink);
                spriteBatch.DrawString(myFont, pacman.Score + "", new Vector2(680, 50 + offset), pacman.player == 1 ? Color.Yellow : Color.Pink);
                offset += 90;
            }

            spriteBatch.DrawString(myFont, "Game Time", new Vector2(620, 200), Color.Yellow);
            spriteBatch.DrawString(myFont, gametime + "sec", new Vector2(680, 250), Color.Yellow);

            //desenhar menu inicial
            if (status == GameStatus.inicio)
            {

                spriteBatch.Draw(corMenu, new Rectangle(100, 50, 500, 540), Color.White);
                spriteBatch.DrawString(myFont, "Movimento PacMan", new Vector2(110, 50), Color.Yellow);
                spriteBatch.DrawString(myFont, "1", new Vector2(420, 50), Color.White);
                spriteBatch.DrawString(myFont, "2", new Vector2(440, 50), Color.GreenYellow);
                spriteBatch.DrawString(myFont, "   W", new Vector2(110, 75), Color.White);
                spriteBatch.DrawString(myFont, " A S D", new Vector2(110, 110), Color.White);
                spriteBatch.DrawString(myFont, "   ^", new Vector2(250, 75), Color.GreenYellow);
                spriteBatch.DrawString(myFont, " < v >", new Vector2(250, 110), Color.GreenYellow);
                spriteBatch.DrawString(myFont, "Plantar Bomba", new Vector2(110, 150), Color.Yellow);
                spriteBatch.DrawString(myFont, "   B", new Vector2(110, 190), Color.White);
                spriteBatch.DrawString(myFont, " Delete", new Vector2(250, 190), Color.GreenYellow);
                spriteBatch.DrawString(myFont, "Abrir/Fechar Portal", new Vector2(110, 250), Color.Yellow);
                spriteBatch.DrawString(myFont, " Space", new Vector2(110, 290), Color.White);
                //spriteBatch.DrawString(myFont, " Insert", new Vector2(250, 290), Color.GreenYellow);
                spriteBatch.DrawString(myFont, "Para ativar segundo\njogador pressionar\numa das setas. ", new Vector2(110, 370), Color.Yellow);
                spriteBatch.DrawString(myFont, "Pressione Enter para jogar.", new Vector2(110, 520), Color.Red);
            }

            

            if (status == GameStatus.GameOver)
            {
                spriteBatch.Draw(corMenu, new Rectangle(100, 50, 450, 200), Color.White);
                spriteBatch.DrawString(myFont, "Game Over \nPlease Insert Coin\n\n(Enter to restart)", new Vector2(110, 50), Color.Yellow);
            }

            if (status == GameStatus.ganhou)
            {
                spriteBatch.Draw(corMenu, new Rectangle(100, 50, 450, 200), Color.White);
                spriteBatch.DrawString(myFont, "Congratulations! \nYou won PacPortMan\n\n(Enter to restart)", new Vector2(110, 50), Color.Yellow);
            }
            spriteBatch.End();
            base.Draw(gameTime);

        }

        private void criarSegundoJogador()
        {
            Personagem pac = new Personagem(Content, "pac2", TipoPersonagem.Player, mapa, Color.Pink, 0, 2).teleportTo(new Vector2(11, 5));
            pacmans.Add(pac);
        }

        private byte[,] loadMapa()
        {
            /*
         * 0 - Caminho / Comida
         * 1 - Parede
         * 2 - ???
         * 3 - sasa dos fantasmas
         * 4 - Portal de saida
         * 5 - Portal de entrada
         * 6 - Bomba
        */
            byte[,] mapa ={{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
                        {3,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,3},
                        {3,0,1,0,0,1,1,0,1,1,0,1,0,1,1,0,0,1,0,3},
                        {3,0,1,0,0,1,1,0,1,1,0,1,0,1,1,0,0,1,0,3},
                        {3,0,1,1,0,1,1,0,0,0,0,1,0,1,1,0,1,1,0,3},
                        {3,0,0,0,0,0,0,0,1,1,0,1,0,0,0,0,0,0,0,3},
                        {3,0,1,1,1,1,1,0,1,1,0,1,0,1,1,1,1,1,0,3},
                        {3,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,3},
                        {3,1,1,0,1,0,1,1,1,1,1,1,1,1,0,1,0,1,1,3},
                        {2,0,0,0,1,0,2,2,2,2,2,2,2,1,0,1,0,0,0,2},
                        {3,1,1,0,1,0,1,1,1,1,1,1,1,1,0,1,0,1,1,3},
                        {3,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,3},
                        {3,0,1,1,1,1,1,0,1,0,1,1,0,1,1,1,1,1,0,3},
                        {3,0,0,0,0,0,0,0,1,0,1,1,0,0,0,0,0,0,0,3},
                        {3,0,1,1,0,1,1,0,1,0,1,1,0,1,1,0,1,1,0,3},
                        {3,0,1,0,0,1,1,0,1,0,0,0,0,1,1,0,0,1,0,3},
                        {3,0,1,0,0,0,0,0,1,0,1,1,0,0,0,0,0,1,0,3},
                        {3,0,1,1,0,1,1,0,1,0,1,1,0,1,1,0,1,1,0,3},
                        {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                        {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3}};
            return mapa;
        }

        private void loadPacmans()
        {
            Personagem pac = new Personagem(Content, "pac2", TipoPersonagem.Player, mapa, Color.Yellow, 0, 1);
            pacmans.Add(pac);
        }

        private void loadFantasmas()
        {
            Personagem fantasma = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Green, 1, 0).teleportTo(new Vector2(9, 7));
            fantasma.Velocidade = 0.5f;
            fantasmas.Add(fantasma);

            Personagem fantasma2 = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Red, 6, 0).teleportTo(new Vector2(9, 9));
            fantasma2.Velocidade = 0.5f;
            fantasmas.Add(fantasma2);

            Personagem fantasma3 = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Blue, 4, 0).teleportTo(new Vector2(9, 11));
            fantasma3.Velocidade = 0.5f;
            fantasmas.Add(fantasma3);

            Personagem fantasma4 = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Pink, 4, 0).teleportTo(new Vector2(9, 10));
            fantasma4.Velocidade = 0.5f;
            fantasmas.Add(fantasma4);
        }

        private void resetGame()
        {
            pacmans.Clear();
            fantasmas.Clear();
            mapa = loadMapa();
            loadPacmans();
            loadFantasmas();
            Camera.resetShake();
            SpriteAnimationManager.resetAnimations();
            status = GameStatus.jogo;
        }

        /// <summary>
        /// Verifica se não existe comida no mapa
        /// </summary>
        /// <returns>Devolve true se não existe comida no mapa</returns>
        private void verificarJogoGanho()
        {
            int contador = 0;
            for (int i = 0; i < mapa.GetLength(0); i++)
            {
                for (int j = 0; j < mapa.GetLength(1); j++)
                {
                    if (mapa[i, j] == 0)
                    {
                       contador++;
                    }
                }
            }
            if(contador == 0)
                status = GameStatus.ganhou;
        }


        //metodo para eliminar comida apos pacman passar por cima
        private void comer()
        {
            foreach (Personagem pacman in pacmans)
            {

                if (mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] == 0)
                {
                    mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 2;
                    pacman.Score += 10;
                    //som comer
                    Som.playComer(Content);
                }
            }
        }

        /// <summary>
        /// Lida com colisões entre fantasmas e pacmans
        /// </summary>
        public void colisaoFantasmaPacman()
        {
            listaTempPersonagens = Colisoes.fantasmaPacman(fantasmas, pacmans);
            foreach (Personagem pacman in listaTempPersonagens)
            {
                pacman.removeBombas(mapa);
                pacmans.Remove(pacman);
            }
            listaTempPersonagens.Clear();
        }

        private void colisaoBomba()
        {
            foreach (Personagem pacman in pacmans)
            {
                //Para cada pacman..
                foreach (Bomba bomba in pacman.getBombas())
                {
                    //Para cada bomba do pacman..
                    if (bomba.Exploded)
                    {
                        //Esta bomba explodiu, verificar se colide com algum fantasma
                        listaTempPersonagens = Colisoes.bombaFantasmaPacman(bomba.Posicao, fantasmas, pacmans);
                        listaTempBombas.Add(bomba);
                        mapa[(int)bomba.Posicao.X, (int)bomba.Posicao.Y] = 2;
                    }
                }
            }

            //Retirar os fantasmas mortos da lista
            foreach (Personagem personagem in listaTempPersonagens)
            {
                if (personagem.TipoPersonagem() == TipoPersonagem.NPC)
                {
                    fantasmas.Remove(personagem);
                }
                else
                {
                    personagem.removeBombas(mapa);
                    pacmans.Remove(personagem);
                }

            }
            //Retirar as bombas explodidas
            foreach (Bomba bomba in listaTempBombas)
            {
                bomba.Parent.removeBomba(bomba);
            }
            listaTempBombas.Clear();
            listaTempPersonagens.Clear();
        }

        void verificarGameOver()
        {
            if(pacmans.Count==0)
            {
                status = GameStatus.GameOver;
            }
        }
    }

}
