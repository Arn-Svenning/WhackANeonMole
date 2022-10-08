using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security;
using static System.Formats.Asn1.AsnWriter;

namespace WhackANeonMole
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        SpriteFont font;
        MouseState mouseState, previousMouseState;

        Texture2D holeTexture;
        Texture2D grassTexture;
        Texture2D moleTexture;
        Texture2D backgroundTexture;
        Texture2D godHammerBackground;
        Texture2D welcome;
        Texture2D gameOver;
        Animation gameOverAnimation;

        Animation ballAnimation;
        int velocityX;
        int velocityY;
        Vector2 ballVelocity;

        Texture2D regularHammer;
        Hammer normalHammer;
        Texture2D superHammer;
        Hammer godHammer;
        Rectangle hammerRect;
        //Vector2 hammerPosition;
        //float hammerRotation = 0f;

        Moles moles;
        Moles[,] mole;
       

        int holePosX;
        int holePosY;

        Vector2 grassPosition;
        Vector2 molePosition;
        Vector2 moleVelocity;
        int moleVelocityY;
        Vector2 moleDirection;

        Random randomVelocity;
        bool pressed;
        bool released;
        float timer = 30;
        int score;
        enum GameState { start, inGame, gameOver }
        GameState currentGameState = GameState.start;

        SoundEffect hitEffect;
        SoundEffect thunderStrike;
        bool thunder = false;
        Song backgroundSong;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 650;

        }

        protected override void Initialize()
        {
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("Normal-Hammer-Background-Neon");
            godHammerBackground = Content.Load<Texture2D>("Thunder-God-Background-Neon");
            welcome = Content.Load<Texture2D>("GameState-StartScreen");
            gameOver = Content.Load<Texture2D>("GameState-Out-Of-Time");
            font = Content.Load<SpriteFont>("file");

            regularHammer = Content.Load<Texture2D>("Hammer-Neon");
            //hammerPosition = new Vector2(mouseState.X - normalHammer.Width / 2 + 30, mouseState.Y - normalHammer.Height / 2);
            normalHammer = new Hammer(regularHammer);

            superHammer = Content.Load<Texture2D>("Hammer-Neon-Thunder-God");
            godHammer = new Hammer(superHammer);

            moleTexture = Content.Load<Texture2D>("Yellow-Mole-Neon");

            velocityX = 0;
            velocityY = 3;
            ballVelocity = new Vector2(velocityX, velocityY);
            ballAnimation = new Animation(Content, "Ball-SpriteSheet-Neon", 70f, 13, ballVelocity);

            gameOverAnimation = new Animation(Content, "End-Animation", 100f, 6, Vector2.Zero);

            holeTexture = Content.Load<Texture2D>("Hole-Neon");
            grassTexture = Content.Load<Texture2D>("Grass-Neon");

            randomVelocity = new Random();

            moleDirection = new Vector2(0, 1);

            mole = new Moles[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    moleVelocityY = randomVelocity.Next(-5, -3);
                    moleVelocity = new Vector2(0, moleVelocityY);

                    holePosX = i * 200 + 30;
                    holePosY = j * 250 + 50;
                    molePosition = new Vector2(holePosX + 9, holePosY + 170);
                    grassPosition = new Vector2(holePosX, holePosY + 150);

                    moles = new Moles(Content, moleTexture, molePosition, moleVelocity, moleDirection, grassTexture, grassPosition, holeTexture, holePosX, holePosY);
                    mole[i, j] = moles;
                }
            }
            thunderStrike = Content.Load<SoundEffect>("Thunder");
            hitEffect = Content.Load<SoundEffect>("Hit-Sound");
            backgroundSong = Content.Load<Song>("Neon-Music");
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();          

            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
           
            switch (currentGameState)
            {
                case GameState.start:

                    ballAnimation.PlayAnimation(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentGameState = GameState.inGame;
                    }
                    break;

                case GameState.inGame:
                    timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(timer > 0)
                    {
                        if (score < 20)
                        {
                            normalHammer.Update(gameTime);
                        }
                        else
                        {
                            godHammer.Update(gameTime);
                        }

                        if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        {
                            pressed = true;
                            released = true;
                        }
                        else
                        {
                            pressed = false;
                            released = false;
                        }
                        foreach (var moles in mole)
                        {
                            moles.Update((float)gameTime.ElapsedGameTime.TotalSeconds);


                            if (moles.moleHitBox.Contains(mouseState.X, mouseState.Y) && pressed && released && !moles.isHit)
                            {

                                score += 1;
                                moles.isHit = true;
                                hitEffect.Play();
                                moles.currentState = Moles.MoleState.movingDown;
                            }                            
                           
                        }                        
                    }
                    else
                    {
                        currentGameState = GameState.gameOver;
                    }                                      
                    break;

                case GameState.gameOver:
                    gameOverAnimation.PlayAnimation(gameTime);
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0, 1, 46));

            spriteBatch.Begin();
            
            switch (currentGameState)
            {
                case GameState.start:

                    spriteBatch.Draw(holeTexture, new Vector2(0, 500), Color.Brown);
                    spriteBatch.Draw(welcome, new Vector2(-150, 150), Color.White);
                    ballAnimation.DrawBallAnimation(spriteBatch);
                    spriteBatch.Draw(grassTexture, new Vector2(0, 630), Color.White);
                    spriteBatch.DrawString(font, "Press Enter To Start", new Vector2(230, 450), Color.Pink, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 1f);

                    break;

                case GameState.inGame:
                    if(score < 20)
                    {
                        spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(godHammerBackground, Vector2.Zero, Color.White);
                        
                    }
                    
                    for (int i = 0; i < mole.GetLength(0); i++)
                    {
                        for (int j = 0; j < mole.GetLength(1); j++)
                        {
                            mole[i, j].Draw(spriteBatch);
                        }
                    }

                    spriteBatch.DrawString(font, Convert.ToString("Current Score: " + score), new Vector2(0, 765), Color.Pink, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 1f);
                    spriteBatch.DrawString(font, Convert.ToString("Time Left: " + timer), new Vector2(400, 765), Color.Pink, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 1f);                    

                    if (score >= 20)
                    {
                        godHammer.Draw(spriteBatch);
                        if (score == 20 && thunder == false)
                        {
                            thunderStrike.Play();
                            thunder = true;
                        }

                    }
                    else
                    {
                        normalHammer.Draw(spriteBatch);
                    }
                    break;

                case GameState.gameOver:                   
                    spriteBatch.Draw(gameOver, new Vector2(10, 250), Color.White);
                    spriteBatch.DrawString(font, Convert.ToString(score), new Vector2(410, 365), Color.Pink, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 1f);
                    gameOverAnimation.DrawEndAnimation(spriteBatch);
                    break;
            }                                   
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
