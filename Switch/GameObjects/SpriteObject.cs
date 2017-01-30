using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Switch.GameObjects
{
    /* This is the superclass for all sprite objects */
    class SpriteObject
    {
        private Texture2D texture;
        public Texture2D backgroundTexture { get; set; }
        private Dictionary<String, SpriteSheet> animations;
        private SpriteSheet activeAnimation;
        private String activeAnimationName;
        private int activeAnimationFrameChangeTime;
        private int timeSinceLastFrameUpdate;
        private int currentFrame;
        public Rectangle destinationRect { get; set; }
        private bool loopCurrentAnimation;
        public Color color { get; set; }

        public SpriteObject(Texture2D texture)
        {
            this.texture = texture;
            animations = new Dictionary<String, SpriteSheet>();
            activeAnimation = null;
            loopCurrentAnimation = false;
            color = Color.White;
        }

        public SpriteObject(Texture2D texture, Texture2D backgroundTexture)
        {
            this.texture = texture;
            this.backgroundTexture = backgroundTexture;
            animations = new Dictionary<String, SpriteSheet>();
            activeAnimation = null;
            loopCurrentAnimation = false;
            color = Color.White;
        }

        public String GetCurrentAnimation()
        {
            return activeAnimationName;
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public Texture2D GetStaticTexture()
        {
            return this.texture;
        }


        public Texture2D GetTexture()
        {
            Texture2D textureToReturn;

            if (activeAnimation == null)
            {
                textureToReturn = this.texture;
            }
            else
            {
                textureToReturn = this.activeAnimation.spriteSheet;
            }

            return textureToReturn;
        }

        public void AddAnimation(String animationName, SpriteSheet spriteSheet)
        {
            animations.Add(animationName, spriteSheet);
        }

        public void StartAnimation(String animationName, int framesPerSecond, bool looping)
        {
            if (animations.ContainsKey(animationName))
            {
                SpriteSheet animation = animations[animationName];
                activeAnimation = animation;
                activeAnimationName = animationName;
                loopCurrentAnimation = looping;
                activeAnimationFrameChangeTime = (1000 / framesPerSecond);
                currentFrame = 0;
            }
        }

        public void StartAnimation(String animationName, int framesPerSecond)
        {
            this.StartAnimation(animationName, framesPerSecond, false);
        }

        public virtual void UpdateGameTime(int elapsedGameTime)
        {
            timeSinceLastFrameUpdate += elapsedGameTime;

            if (activeAnimation != null && timeSinceLastFrameUpdate >= activeAnimationFrameChangeTime)
            {
                currentFrame++;
                timeSinceLastFrameUpdate = 0;
                if (currentFrame >= activeAnimation.numberOfFrames)
                {
                    if (!loopCurrentAnimation)
                    {
                        activeAnimation = null;
                    }
                    else
                    {
                        currentFrame = 0;
                    }
                }
            }
        }

        public virtual void UpdateGameTime(int elapsedGameTime, int currentGameSpeed, int currentTilePixelHeight)
        {
            this.UpdateGameTime(elapsedGameTime);
        }

        public void SetSpriteSheetDictionary(Dictionary<String, SpriteSheet> animations)
        {
            this.animations = animations;
        }

        public Dictionary<String, SpriteSheet> GetSpriteSheetDictionary()
        {
            return this.animations;
        }

        public Rectangle GetCurrentCelRect()
        {
            Rectangle rect;

            if (!this.IsAnimating())
            {
                rect = new Rectangle(0, 0, this.texture.Width, this.texture.Height);
            }
            else
            {
                int widthOfFrame = activeAnimation.spriteSheet.Width / activeAnimation.numberOfFrames;
                rect = new Rectangle(currentFrame * widthOfFrame, 0, widthOfFrame, this.texture.Height);
            }

            return rect;
        }

        public bool IsAnimating()
        {
            return activeAnimation != null;
        }
    }
}
