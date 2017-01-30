using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Switch.Menus;
using Microsoft.Xna.Framework.Content;
using Switch.GameObjects.Sound;
using System.Diagnostics;

namespace Switch
{
    class PressStartScreen : GameScreen
    {
        Texture2D greenTileBackground;
        Texture2D orangeTileBackground;
        Texture2D greenTileForeground;
        Texture2D orangeTileForeground;
        ContentManager content;
        float rotationAngle;
        float selectionFade;
        bool loading;
        bool storageScreenTriggered;

        public PressStartScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            rotationAngle = 0.0f;
            loading = false;
            storageScreenTriggered = false;
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            greenTileBackground = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\green");
            orangeTileBackground = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\orange");
            greenTileForeground = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\crescent");
            orangeTileForeground = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\wing_triangle");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Calculate rotation angle based on time elapsed
            float circle = MathHelper.Pi * 2;
            rotationAngle += (elapsed * 3) % circle;

            //to make the text pulsate
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
            selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            SpriteFont bigFont = ScreenManager.BigFont;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;

            Vector2 position = new Vector2(0, 0);

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            if (ScreenState == ScreenState.TransitionOn)
            {
                position.X -= transitionOffset * 256;
            }
            else
            {
                position.X += transitionOffset * 512;
            }

            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * selectionFade;

            spriteBatch.Begin();

            String pressStartString;
            if (!this.loading)
            {
                 pressStartString = "Press Start!";
            }
            else
            {
                 pressStartString = "Loading...";
            }


            Vector2 stringPosition = new Vector2(viewport.Width / 2, viewport.Height / 2);
            Vector2 stringOrigin = Utils.Utils.Instance.GetTextStringCenterOrigin(pressStartString, bigFont);
            spriteBatch.DrawString(bigFont, pressStartString, new Vector2(stringPosition.X + position.X, stringPosition.Y), titleColor, 0, stringOrigin, scale, SpriteEffects.None, 0);
             
            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            PlayerIndex playerIndex;

            if (input.IsNewButtonPress(Buttons.A, null, out playerIndex)
                || input.IsNewButtonPress(Buttons.B, null, out playerIndex)
                || input.IsNewButtonPress(Buttons.Start, null, out playerIndex))
            {
                SoundManager.Instance.PlaySound("menu-select2");

#if XBOX
                // get the save device
                try {
                    if(!storageScreenTriggered) {
                        promptForSaveDevice(playerIndex);
                        storageScreenTriggered = true;
                    }
                }
                catch(Exception e) 
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
#endif

#if WINDOWS
                loadMainMenu(playerIndex);
#endif
            }

            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(Keys.Enter))
            {
                SoundManager.Instance.PlaySound("menu-select2");
                LoadMainMenu(playerIndex);
            }
        }

        private void LoadMainMenu(PlayerIndex playerIndex)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen(), null);
        }
    }
}
