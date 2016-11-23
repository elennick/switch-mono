using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Switch.Menus;
using Switch.Utils;
using System;

namespace Switch
{
    public class SwitchGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        public static bool DEBUG_MODE = true;

        public SwitchGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
          

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            screenManager.AddScreen(new LogoScreen(), null);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }
    }
}
