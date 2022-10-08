using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhackANeonMole
{
    internal class Hammer
    {
        Texture2D tex;
        public Vector2 pos;
        double rotation;
        public int timer;

        MouseState mouseState;

        public Hammer(Texture2D tex)
        {
            this.tex = tex;
        }

        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            pos = new Vector2(mouseState.X + tex.Width - tex.Width / 6.5f, mouseState.Y - tex.Height / 3);
            if (mouseState.LeftButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
            {
                rotation = 0;
            }            
            else
            {
                timer = 0;
                rotation = Math.PI / 6;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, null, Color.White, (float)rotation, new Vector2(tex.Width, tex.Height / 2), 1f, SpriteEffects.None, default);
        }
    }
}
