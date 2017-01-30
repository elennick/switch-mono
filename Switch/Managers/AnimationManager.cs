using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.GameObjects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Switch
{
    class AnimationManager
    {
        private static AnimationManager instance;
        private Dictionary<String, SpriteSheet> animations;
        private List<DetailedSpriteObject> activeAnimations;
        private bool animationsLoaded;

        private AnimationManager()
        {
            animations = new Dictionary<string, SpriteSheet>();
            activeAnimations = new List<DetailedSpriteObject>();
            animationsLoaded = false;
        }

        public static AnimationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AnimationManager();
                }
                return instance;
            }
        }

        public void AddAnimation(String animationName, SpriteSheet spriteSheet)
        {
            animations.Add(animationName, spriteSheet);
        }

        public void StartAnimation(String animationName, int framesPerSecond, Rectangle rect)
        {
            if (animations.ContainsKey(animationName))
            {
                SpriteSheet spriteSheet = animations[animationName];
                DetailedSpriteObject animation = new DetailedSpriteObject(spriteSheet.spriteSheet, new Vector2(rect.X, rect.Y));
                animation.destRect = rect;

                animation.AddAnimation(animationName, spriteSheet);
                animation.StartAnimation(animationName, framesPerSecond);
                activeAnimations.Add(animation);
            }
        }

        public void StartAnimation(String animationName, int framesPerSecond, Vector2 position)
        {
            if (animations.ContainsKey(animationName))
            {
                SpriteSheet spriteSheet = animations[animationName];
                DetailedSpriteObject animation = new DetailedSpriteObject(spriteSheet.spriteSheet, position);

                animation.AddAnimation(animationName, spriteSheet);
                animation.StartAnimation(animationName, framesPerSecond);
                activeAnimations.Add(animation);
            }
        }

        public void UpdateGameTime(int elapsedGameTime)
        {
            foreach (DetailedSpriteObject animation in activeAnimations)
            {
                animation.UpdateGameTime(elapsedGameTime);
            }
        }

        public void DrawAnimations(SpriteBatch spriteBatch)
        {
            //draw any animations that are still going
            foreach (DetailedSpriteObject animation in activeAnimations)
            {
                if (animation.IsAnimating())
                {
                    if (animation.destRect == null)
                    {
                        spriteBatch.Draw(animation.GetTexture(), animation.position, animation.GetCurrentCelRect(), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(animation.GetTexture(), animation.destRect, animation.GetCurrentCelRect(), Color.White);
                    }
                }
            }

            //remove any animations that are done
            DetailedSpriteObject[] activeAnimationsArray = activeAnimations.ToArray();
            for (int i = 0; i < activeAnimationsArray.Length; i++)
            {
                if (!activeAnimationsArray[i].IsAnimating())
                {
                    activeAnimations.Remove(activeAnimationsArray[i]);
                }
            }
        }

        public void LoadAnimations(ContentManager content)
        {
            if (!animationsLoaded)
            {
                this.AddAnimation("laser", new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\laser-spritesheet"), 9));
                this.AddAnimation("tile-explode", new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\explosion-spritesheet"), 6));

                animationsLoaded = true;
            }
        }

        public void ClearAllAnimations()
        {
            activeAnimations.Clear();
        }

        public bool AreAnyAnimationsActive()
        {
            if (activeAnimations.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
