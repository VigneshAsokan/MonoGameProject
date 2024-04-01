#region File Description
//-----------------------------------------------------------------------------
// PlatformerGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using System.Text.Json;


namespace Catastrophe
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PlatformerGame : Microsoft.Xna.Framework.Game
    {

        public enum GameState { 
            Menu,
            GamePlay,
            GameOver
        };

        GameState CurrentGameState = GameState.Menu; 
        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Global content.
        private SpriteFont hudFont;

        private Texture2D MenuBG;

        private Texture2D winOverlay;
        private Texture2D loseOverlay;
        private Texture2D diedOverlay;

        static public int GameScore =0;
        private Menu menu;
        private GameOver gameover;

        // Meta-level game state.
        private int levelIndex = -1;
        private Level level;
        private bool wasContinuePressed;

        // When the time remaining is less than the warning time, it blinks on the hud
        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);

        // We store our input states so that we only poll once per frame, 
        // then we use the same input state wherever needed
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;
        private AccelerometerState accelerometerState;
        
        // The number of levels in the Levels directory of our content. We assume that
        // levels in our content are 0-based and that all numbers under this constant
        // have a level file present. This allows us to not need to check for the file
        // or handle exceptions, both of which can add unnecessary time to level loading.
        private const int numberOfLevels = 4;

        CommandManager commandManager;
        SaveManager saveManager;
        public PlatformerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            saveManager = new SaveManager();
#if WINDOWS_PHONE
            graphics.IsFullScreen = true;
            TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif
            saveManager.LoadSavedData();
            levelIndex = SaveDataInfo.Instance.Level;
            Accelerometer.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load fonts
            hudFont = Content.Load<SpriteFont>("Fonts/gameFont");


            // Load overlay textures
            winOverlay = Content.Load<Texture2D>("Overlays/you_win");
            loseOverlay = Content.Load<Texture2D>("Overlays/you_lose");
            diedOverlay = Content.Load<Texture2D>("Overlays/you_died");

            //Known issue that you get exceptions if you use Media PLayer while connected to your PC
            //See http://social.msdn.microsoft.com/Forums/en/windowsphone7series/thread/c8a243d2-d360-46b1-96bd-62b1ef268c66
            //Which means its impossible to test this from VS.
            //So we have to catch the exception and throw it away
            try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Content.Load<Song>("Sounds/Music"));
            }
            catch { }

            LoadGameStates();
            //LoadNextLevel();
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        private void LoadGameStates()
        {
            //Load Menu
            menu = new Menu(Services,GraphicsDevice);

            gameover = new GameOver(Services, GraphicsDevice);
        }
        private void SaveData()
        {

        }
        private void LoadData()
        {

        }
        private void InitializeKeyBindings()
        {
            commandManager = new CommandManager();
            commandManager.AddKeyboardBinding(Keys.Escape, StopGame);
            //commandManager.AddKeyboardBinding(Keys.C, EnableCameraSpring);
            commandManager.AddKeyboardBinding(Keys.W, level.Player.Jump);
            commandManager.AddKeyboardBinding(Keys.A, level.Player.TurnLeft);
            commandManager.AddKeyboardBinding(Keys.D, level.Player.TurnRight);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void SwitchState(GameState state)
        {
            if (CurrentGameState == state)
                return;

            CurrentGameState = state;

            if (CurrentGameState == GameState.GamePlay)
                LoadNextLevel();
            else if (CurrentGameState == GameState.Menu)
            {
                levelIndex = -1;
                LoadNextLevel();
            }

        }
        protected override void Update(GameTime gameTime)
        {
            if(commandManager != null)
                commandManager.Update();
            // Handle polling for our input and handling high-level input
            HandleInput();
            switch (CurrentGameState)
            {
                case GameState.Menu:
                    {
                        menuSelected option = menu.Update(gameTime);
                        if (option == menuSelected.Play)
                            SwitchState(GameState.GamePlay);
                        else if (option == menuSelected.Exit)
                            Exit();
                        break;
                    }
                case GameState.GamePlay:
                    // update our level, passing down the GameTime along with all of our input states
                    level.Update(gameTime, keyboardState, gamePadState, touchState,
                                 accelerometerState, Window.CurrentOrientation);
                    break;
                case GameState.GameOver:
                    if (gameover.Update(gameTime))
                    {
                        levelIndex = -1;
                        SwitchState(GameState.GamePlay);
                    }
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }

        private void HandleInput()
        {
            // get all of our input states
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            touchState = TouchPanel.GetState();
            accelerometerState = Accelerometer.GetState();
            
            if (CurrentGameState == GameState.Menu || CurrentGameState == GameState.GameOver)
                return;

            // Exit the game when back is pressed.
            if (gamePadState.Buttons.Back == ButtonState.Pressed)
                Exit();

            bool continuePressed =
                keyboardState.IsKeyDown(Keys.Space) ||
                gamePadState.IsButtonDown(Buttons.A) ||
                touchState.AnyTouch();

            // Perform the appropriate action to advance the game and
            // to get the player back to playing.
            if (!wasContinuePressed && continuePressed)
            {
                if (!level.Player.IsAlive)
                {
                    level.StartNewLife();
                }
                else if (levelIndex == numberOfLevels - 1)
                {
                    SwitchState(GameState.GameOver);
                }
                else if (level.TimeRemaining == TimeSpan.Zero)
                {
                    if (level.ReachedExit)
                        LoadNextLevel();
                    else
                        ReloadCurrentLevel();
                }
            }

            wasContinuePressed = continuePressed;
        }
        private void CheckHighScore()
        {
            for (int i = 0; i < 5; i++)
            {
                if (SaveDataInfo.Instance.highScores[i] <= GameScore)
                {
                    if (i != 4)
                        SaveDataInfo.Instance.highScores[(i + 1)] = SaveDataInfo.Instance.highScores[i];

                    SaveDataInfo.Instance.highScores[i] = GameScore;
                    saveManager.SaveData();
                    return;
                }
            }
        }

        private void LoadNextLevel()
        {
            // move to the next level
            levelIndex = (levelIndex + 1) % numberOfLevels;

            // Unloads the content for the current level before loading the next one.
            if (level != null)
                level.Dispose();

            // Load the level.
            string levelPath = string.Format("Content/Levels/{0}.txt", levelIndex);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(Services, fileStream, levelIndex, GameScore);

            InitializeKeyBindings();
        }

        private void ReloadCurrentLevel()
        {
            --levelIndex;
            LoadNextLevel();
        }
        void ResetGame()
        {
            levelIndex = -1;
            LoadNextLevel();
        }
        /// <summary>
        /// Draws the game from background to foreground.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.Menu:
                    menu.Draw(GraphicsDevice.Viewport, spriteBatch);
                    menu.DrawLeaderBoard(spriteBatch);
                    break;
                case GameState.GamePlay:
                    level.Draw(gameTime, spriteBatch);

                    DrawHud();
                    break;
                case GameState.GameOver:
                    gameover.Draw(GraphicsDevice.Viewport, spriteBatch, GameScore);
                    break;
                default:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawHud()
        {
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f);

            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            string timeString = "TIME: " + level.TimeRemaining.Minutes.ToString("00") + ":" + level.TimeRemaining.Seconds.ToString("00");
            Color timeColor;
            if (level.TimeRemaining > WarningTime ||
                level.ReachedExit ||
                (int)level.TimeRemaining.TotalSeconds % 2 == 0)
            {
                timeColor = Color.Yellow;
            }
            else
            {
                timeColor = Color.Red;
            }
            DrawShadowedString(hudFont, timeString, hudLocation, timeColor);

            // Draw score
            float timeHeight = hudFont.MeasureString(timeString).Y;
            DrawShadowedString(hudFont, "SCORE: " + GameScore.ToString(), hudLocation + new Vector2(0.0f, timeHeight * 1.2f), Color.Yellow);
           
            // Determine the status overlay message to show.
            Texture2D status = null;
            if (level.TimeRemaining == TimeSpan.Zero)
            {
                if (level.ReachedExit)
                {
                    status = winOverlay;
                }
                else
                {
                    status = loseOverlay;
                }
            }
            else if (!level.Player.IsAlive)
            {
                status = diedOverlay;
            }

            if (status != null)
            {
                // Draw status message.
                Vector2 statusSize = new Vector2(status.Width, status.Height);
                spriteBatch.Draw(status, center - statusSize / 2, Color.White);
            }
        }
        public void StopGame(eButtonState buttonState)
        {
            if (buttonState == eButtonState.DOWN)
            {
                SaveCurrentLevel();
                CheckHighScore();
                Exit();
            }
        }

        private void SaveCurrentLevel()
        {
            SaveDataInfo.Instance.Level = levelIndex --;
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
