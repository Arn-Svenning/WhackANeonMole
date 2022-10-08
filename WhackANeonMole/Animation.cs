using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhackANeonMole
{
    internal class Animation
    {
        Texture2D animation;
        Rectangle rectangle;
        Vector2 position;
        Vector2 velocity;
        float elapsedTime;
        float frameTime;
      
        int numberOfFrames;
        int currentFrame;
        int frameWidth;
        int frameHeight;

        int windowWidth;
        int windowHeight;
        Vector2 window;

        public Animation (ContentManager Content, string asset, float frameSpeed, int numberOfFrames, Vector2 velocity)
        {
            this.frameTime = frameSpeed;
            this.numberOfFrames = numberOfFrames;
            this.animation = Content.Load<Texture2D>(asset);
            this.position = new Vector2(10,0);
            this.velocity = velocity;
            frameWidth = (animation.Width / numberOfFrames);
            frameHeight = (animation.Height);            
        }
        public void PlayAnimation(GameTime gameTime)
        {
            position = position + velocity;

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);

            /////Looping the spriteSheet
            if (elapsedTime >= frameTime)
            {
                if (currentFrame >= numberOfFrames - 1)
                {
                    currentFrame = 0;

                }
                else
                {
                    currentFrame++;
                }
                elapsedTime = 0;
            }
        }       
        public void DrawBallAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation, position, rectangle, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 1f);
        }
        public void DrawEndAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation, new Vector2(150, 270), rectangle, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 1f);
        }
        
    }
}
