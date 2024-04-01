using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catastrophe
{
    class GameOver
    {
        ContentManager content;
        public bool isPlaySelected = false;

        GraphicsDevice Device;

        private Texture2D background;
        private SpriteFont MenuFont;
        public GameOver(IServiceProvider serviceProvider, GraphicsDevice device)
        {
            content = new ContentManager(serviceProvider, "Content");

            MenuFont = content.Load<SpriteFont>("Fonts/gameFont");
            background = content.Load<Texture2D>("Backgrounds/GameOver");
            Device = device;
        }

        public bool Update(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                return true;
            }
            return false;
        }

        public void Draw(Viewport viewport, SpriteBatch spriteBatch, int score)
        {
            //Draw BG
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            Rectangle titleSafeArea = viewport.TitleSafeArea;
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 5.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f - 10);

            DrawShadowedString(MenuFont, "Score : " + score, new Vector2(center.X,center.Y + 50), Color.Orange, spriteBatch);
            
            DrawShadowedString(MenuFont, "Press R To Restart", new Vector2(center.X,center.Y + 80), Color.Orange, spriteBatch);

        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color, SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
