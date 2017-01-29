using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Switch.GameObjects
{
    class DetailedSpriteObject : SpriteObject
    {
        public Vector2 position { get; set; }
        public Rectangle destRect { get; set; }

        public DetailedSpriteObject(Texture2D texture, Vector2 position)
            : base(texture)
        {
            this.position = position;
        }

        public DetailedSpriteObject(Texture2D texture, Rectangle destRect) 
            : base(texture)
        {
            this.destRect = destRect;
        }
    }
}
