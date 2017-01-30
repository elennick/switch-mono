using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Switch.Menus;
using Switch.Utils;
using System;
using System.Collections.Generic;

namespace Switch
{
    public class SwitchGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager Graphics { get; }
        public ScreenManager screenManager { get; }
        public static SwitchGame Instance { get; set; }

        //skips logo screen, disables music, defaults to windowed mode and gives unlimited power to use abilities during gameplay
        public static bool DEBUG_MODE = false;

        public SwitchGame()
        {
            Instance = this;

            Content.RootDirectory = "Content";

            Graphics = new GraphicsDeviceManager(this);

            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.IsFullScreen = !DEBUG_MODE;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            if (DEBUG_MODE)
            {
                screenManager.AddScreen(new BackgroundScreen(), null);
            }
            else
            {
                screenManager.AddScreen(new LogoScreen(), null);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }
    }

    struct SupportedResolution
    {
        public int Width { get; }
        public int Height { get; }

        public SupportedResolution(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }
    }
}
