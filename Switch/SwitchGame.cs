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

        public static bool DEBUG_MODE = true; //skips logo screen, disables music and gives unlimited power to use abilities during gameplay

        private static List<SupportedResolution> supportedResolutions = new List<SupportedResolution>
        {
            new SupportedResolution(1280, 720),
            new SupportedResolution(1920, 1080)
        };

        public SwitchGame()
        {
            Instance = this;

            Content.RootDirectory = "Content";

            Graphics = new GraphicsDeviceManager(this);

            Graphics.PreferredBackBufferWidth = supportedResolutions[0].Width;
            Graphics.PreferredBackBufferHeight = supportedResolutions[0].Height;

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
