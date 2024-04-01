using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Catastrophe
{
    class SpeedPowerUp : PowerUp
    {
        Texture2D texture;
        private Vector2 origin;

        public SpeedPowerUp(Vector2 _position, Texture2D _texture)
        {
            position = _position;
            texture = _texture;
            timer = GameInfo.Instance.SpeedInfo.timer; 
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            type = powerUpType.speed;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void activate()
        {
            base.activate();
        }
        public override void Draw(SpriteBatch sprite)
        {
            base.Draw(sprite);
            sprite.Draw(texture, position, null, GameInfo.Instance.SpeedInfo.Color, 0.0f, origin, GameInfo.Instance.SpeedInfo.Size, SpriteEffects.None, 0.0f);
        }
    }
}
