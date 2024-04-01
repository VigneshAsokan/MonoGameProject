using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Catastrophe
{
    class Shield : PowerUp
    {
        Texture2D texture;
        private Vector2 origin;
        public Shield(Vector2 _position, Texture2D _texture)
        {
            position = _position;
            texture = _texture;
            timer = GameInfo.Instance.ShieldInfo.timer;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            type = powerUpType.shield;
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
            sprite.Draw(texture, position, null, GameInfo.Instance.ShieldInfo.Color, 0.0f, origin, GameInfo.Instance.ShieldInfo.Size, SpriteEffects.None, 0.0f);

        }
    }
}
