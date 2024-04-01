using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Catastrophe
{
    public enum powerUpType { shield, speed, none};
    public class PowerUp
    {
        public Vector2 position;
        public powerUpType type;
        public float timer;

        public virtual void Update(GameTime gameTime)
        {

        }
        public Circle BoundingCircle
        {
            get
            {
                return new Circle(position, Tile.Width / 3.0f);
            }
        }
        public virtual void activate()
        {

        }
        public virtual void Draw(SpriteBatch sprite)
        {

        }
    }
}
