using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Switch.Utils
{
    class Utils
    {
        private static Utils instance;

        public static Utils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utils();
                }
                return instance;
            }
        }

        public Vector2 GetScreenCenterForText(String text, SpriteFont spriteFont, float yPosition) 
        {
            Vector2 textSize = spriteFont.MeasureString(text);
            float xPosition = ((1280 / 2) - (textSize.X / 2));
            Vector2 textCenter = new Vector2(xPosition, yPosition);

            return textCenter;
        }

        public Vector2 GetScreenAlignRightForText(String text, SpriteFont spriteFont, float yPosition)
        {
            Vector2 textSize = spriteFont.MeasureString(text);
            float xPosition = ((1280 / 2) - textSize.X);
            Vector2 textAlignedRight = new Vector2(xPosition, yPosition);

            return textAlignedRight;
        }

        public Vector2 GetTextStringCenterOrigin(String text, SpriteFont spriteFont)
        {
            Vector2 textSize = spriteFont.MeasureString(text);

            float xOrigin = textSize.X / 2;
            float yOrigin = textSize.Y / 2;

            return new Vector2(xOrigin, yOrigin);
        }

        public Vector2 GetTextStringRightOrigin(String text, SpriteFont spriteFont)
        {
            Vector2 textSize = spriteFont.MeasureString(text);

            float xOrigin = textSize.X;
            float yOrigin = textSize.Y / 2;

            return new Vector2(xOrigin, yOrigin);
        }

        public Vector2 GetTextStringLeftOrigin(String text, SpriteFont spriteFont)
        {
            Vector2 textSize = spriteFont.MeasureString(text);

            float xOrigin = 0;
            float yOrigin = textSize.Y / 2;

            return new Vector2(xOrigin, yOrigin);
        }

        public Texture2D CopyTexture2D(GraphicsDevice gd, Texture2D image, Rectangle source)
        {
            Rectangle destination = new Rectangle(0, 0, source.Width, source.Height);

            // Create a new render target the size of the cropping region.
            RenderTarget2D target = new RenderTarget2D(gd, source.Width, source.Height, true, SurfaceFormat.Color, DepthFormat.Depth24);

            // Make it the current render target.
            gd.SetRenderTarget(target);

            // Render the selected portion of the source image into the render target.
            SpriteBatch sb = new SpriteBatch(gd);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(image, destination, source, Color.White);
            sb.End();

            // Resolve the render target.  This copies the target's buffer into a texture buffer.
            gd.SetRenderTarget(null);

            // Finally, return the target.
            return target;
        }
    }
}
