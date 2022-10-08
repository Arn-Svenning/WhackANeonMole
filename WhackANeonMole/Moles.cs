using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics.Metrics;
using System.Net.Mime;

namespace WhackANeonMole
{
    internal class Moles
    {
        public Texture2D moleTexture;
        public Texture2D moleTextureDead;
        public Vector2 molePosition;
        public Vector2 moleStartPosition;
        public Texture2D grassTexture;
        public Vector2 grassPosition;
        public Texture2D holeTexture;
        public Vector2 holePosition;

        public float moleMaxPosition;

        public Vector2 direction;
        public Vector2 velocity;

        public Random random;

        public Rectangle moleHitBox;
        public enum MoleState { movingUp, isUp, movingDown, isDown }
        public MoleState currentState = MoleState.isDown;



        public float timer;
        public float cooldown = 3f;
        public float randomCooldownDown;
        public float randomCoolDownUp;

        public bool isHit;
        public Moles(ContentManager content, Texture2D moleTexture, Vector2 molePosition, Vector2 velocity, Vector2 direction,
            Texture2D grassTexture, Vector2 grassPosition, Texture2D holeTexture, int holePosX, int holePosY)
        {
            this.molePosition = molePosition;
            this.moleStartPosition = molePosition;
            this.moleTexture = moleTexture;
            this.holePosition = new Vector2((int)holePosX, (int)holePosY);
            this.grassPosition = grassPosition;
            this.velocity = velocity;
            this.direction = direction;
            this.grassTexture = grassTexture;
            this.holeTexture = holeTexture;

            isHit = false;
            
            moleTextureDead = content.Load<Texture2D>("Yellow-Mole-Neon-Dead");

            moleMaxPosition = moleStartPosition.Y - 150;
        }
        public void Update(float elapsedSeconds)
        {
            moleHitBox = new Rectangle((int)molePosition.X, (int)molePosition.Y, moleTexture.Width, (int)(moleStartPosition.Y - molePosition.Y + 30));
            random = new Random();
            randomCooldownDown = random.Next(2, 5);
            randomCoolDownUp = random.Next(1, 3);


            switch (currentState)
            {
                case MoleState.movingUp:

                    molePosition = molePosition + direction * velocity;

                    if (molePosition.Y > moleStartPosition.Y || molePosition.Y < moleMaxPosition)
                    {
                        direction = direction * -1;
                        currentState = MoleState.isUp;
                    }

                    break;

                case MoleState.isUp:
                    timer -= elapsedSeconds;
                    if (timer <= 0f)
                    {
                        timer += randomCoolDownUp;
                        currentState = MoleState.movingDown;
                    }

                    break;

                case MoleState.movingDown:

                    molePosition = molePosition + direction * velocity;

                    if (molePosition.Y > moleStartPosition.Y || molePosition.Y < moleMaxPosition)
                    {
                        direction = direction * -1;
                        currentState = MoleState.isDown;

                        
                        
                    }


                    break;

                case MoleState.isDown:
                    timer -= elapsedSeconds;
                    if (timer <= 0f)
                    {
                        timer += randomCooldownDown;
                        currentState = MoleState.movingUp;
                        isHit = false;
                    }

                    break;
            }

            /*if (molePosition.Y > moleStartPosition.Y || molePosition.Y < moleMaxPosition)
            {
                direction = direction * -1;
            }*/

        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(holeTexture, holePosition, Color.Brown);
            if (!isHit) spriteBatch.Draw(moleTexture, molePosition, Color.White);
            if (isHit) spriteBatch.Draw(moleTextureDead, molePosition, Color.White);

            spriteBatch.Draw(grassTexture, grassPosition, Color.White);

            //spriteBatch.Draw(grassTexture, moleHitBox, Color.Red);

        }        

    }
}
