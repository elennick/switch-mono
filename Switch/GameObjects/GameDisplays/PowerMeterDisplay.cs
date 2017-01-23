﻿using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects;

namespace Switch.GameObjects.GameDisplays
{
    class PowerMeterDisplay : GameDisplay
    {
        public static int MaxPower = 100;
        public static int PowerForBulletTime = 25;
        public static int PowerForLasers = 50;
        public static int PowerForNuke = 100;
        private int power;
        protected Vector2 position;
        private Texture2D powerbarTexture;
        private Texture2D powerbarOutlineTexture;
        private Texture2D clockReady;
        private Texture2D clockDisabled;
        private Texture2D laserReady;
        private Texture2D laserDisabled;
        private Texture2D nukeReady;
        private Texture2D nukeDisabled;

        public PowerMeterDisplay(Vector2 position, ContentManager content, SpriteFont font, GameBoard gameBoard)
            : base(position, font, gameBoard)
        {
            powerbarTexture = content.Load<Texture2D>("Sprites\\powerbar");
            powerbarOutlineTexture = content.Load<Texture2D>("Sprites\\powerbar-outline");
            clockReady = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\clock");
            clockDisabled = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\clock_grey");
            laserReady = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\laser");
            laserDisabled = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\laser_grey");
            nukeReady = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\nuke");
            nukeDisabled = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\nuke_grey");

            this.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.SetPower(this.gameBoard.getPower());
            float scaleX = (float)this.power / 100;

            //draw power bar outline and the power bar itself
            spriteBatch.Draw(powerbarOutlineTexture, this.position, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
            spriteBatch.Draw(powerbarTexture, new Vector2(this.position.X + 5, this.position.Y + 5), null, Color.White, 0, Vector2.Zero, new Vector2(scaleX, 1), SpriteEffects.None, 0);

            //draw bullet time icon
            Rectangle btIconRect = new Rectangle((int)this.position.X + 22, 
                                                 (int)this.position.Y + 45, 
                                                 30, 
                                                 30);
            spriteBatch.Draw(GetBulletTimeIcon(), btIconRect, Color.White);

            //draw laser icon
            Rectangle laserIconRect = new Rectangle((int)this.position.X + 74,
                                                 (int)this.position.Y + 45,
                                                 30,
                                                 30);
            spriteBatch.Draw(GetLaserIcon(), laserIconRect, Color.White);

            //draw nuke icon
            Rectangle nukeIconRect = new Rectangle((int)this.position.X + 180,
                                                 (int)this.position.Y + 45,
                                                 30,
                                                 30);
            spriteBatch.Draw(GetNukeIcon(), nukeIconRect, Color.White);
        }

        public void SetPower(int power)
        {
            this.power = power;
            if (this.power >= MaxPower)
            {
                this.power = MaxPower;
            }
        }

        private Texture2D GetBulletTimeIcon()
        {
            if (this.power >= PowerForBulletTime)
            {
                return this.clockReady;
            }
                
            return this.clockDisabled;
        }

        private Texture2D GetLaserIcon()
        {
            if (this.power >= PowerForLasers)
            {
                return this.laserReady;
            }

            return this.laserDisabled;
        }

        private Texture2D GetNukeIcon()
        {
            if (this.power >= PowerForNuke)
            {
                return this.nukeReady;
            }

            return this.nukeDisabled;
        }
    }
}
