using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Catastrophe
{
    public enum menuSelected { Play, Exit, none}

    class Menu 
    {
        ContentManager content;
        public bool isPlaySelected = true;

        GraphicsDevice Device;

        private Texture2D PlayBG;
        private Texture2D ExitBG;
        private SpriteFont MenuFont;
        public Menu(IServiceProvider serviceProvider, GraphicsDevice device)
        {
            content = new ContentManager(serviceProvider, "Content");

            MenuFont = content.Load<SpriteFont>("Fonts/gameFont");
            PlayBG = content.Load<Texture2D>("Backgrounds/Menu/Play");
            ExitBG = content.Load<Texture2D>("Backgrounds/Menu/Exit");
            Device = device;
        }

        public menuSelected Update(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
                isPlaySelected = true;
            if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
                isPlaySelected = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                if (isPlaySelected)
                    return menuSelected.Play; 
                else
                    return menuSelected.Exit;

            return menuSelected.none ;
        }

        public void DrawLeaderBoard(SpriteBatch spriteBatch)
        {
            Vector2 boardPosition = new Vector2(500, 80);
            DrawShadowedString(MenuFont, "Leader Board: ", boardPosition, Color.Yellow, spriteBatch);
            for (int i = 0; i < 5; i++)
            {
                DrawShadowedString(MenuFont, "HighScore " + i.ToString() + ": " + SaveDataInfo.Instance.highScores[i],
                    boardPosition + new Vector2(10.0f, i * 25f + 30), Color.Yellow, spriteBatch);
               
            }

        }

        public void Draw(Viewport viewport, SpriteBatch spriteBatch)
        {
            //Draw BG
            //spriteBatch.Draw(ExitBG, Vector2.Zero, Color.White);

            Rectangle titleSafeArea = viewport.TitleSafeArea;
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 5.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f - 10);

            string options;

            if(isPlaySelected)
                spriteBatch.Draw(PlayBG, Vector2.Zero, Color.White);
            else
                spriteBatch.Draw(ExitBG, Vector2.Zero, Color.White);

            //DrawShadowedString(MenuFont, options, center, Color.Orange, spriteBatch);


        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color, SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f),Color.Black);
            spriteBatch.DrawString(font, value, position , color);
        }
    }
}
