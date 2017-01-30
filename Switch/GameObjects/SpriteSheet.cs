using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Switch.GameObjects
{
    class SpriteSheet
    {
        public Texture2D spriteSheet { get; }
        public int numberOfFrames { get; }

        public SpriteSheet(Texture2D spriteSheet, int numberOfFrames)
        {
            this.spriteSheet = spriteSheet;
            this.numberOfFrames = numberOfFrames;
        }
    }
}
