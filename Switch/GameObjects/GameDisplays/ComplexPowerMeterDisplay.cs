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
    class ComplexPowerMeterDisplay : GameDisplay
    {
        public static int MaxPower = 100;
        public static int PowerForBulletTime = 25;
        public static int PowerForLasers = 50;
        public static int PowerForNuke = 100;
        protected int power;
        protected Texture2D powerbarScaffolding;
        protected Texture2D clockReady;
        protected Texture2D clockDisabled;
        protected Texture2D laserReady;
        protected Texture2D laserDisabled;
        protected Texture2D nukeReady;
        protected Texture2D nukeDisabled;
        protected Texture2D bulletTimePowerBar;
        protected Texture2D laserPowerBar;
        protected Texture2D nukePowerBar;
        protected Texture2D bulletTimeLeftBar;
        protected Texture2D xButtonTexture;
        protected Texture2D yButtonTexture;
        protected Texture2D bButtonTexture;
        protected SpriteEffects spriteEffect;
        protected bool reverseDraw;
        protected int bulletTimeLeft { get; set; } //milliseconds
        protected int maxBulletTime { get; set; } //milliseconds
        private float selectionFade;

        public ComplexPowerMeterDisplay(Vector2 position, ContentManager content, SpriteFont font, GameBoard gameBoard, bool reverseDraw)
            : base(position, font, gameBoard)
        {
            this.reverseDraw = reverseDraw; //this is supposed to be used to flip the display along a vertical access but it isn't totally implemented yet

            powerbarScaffolding = content.Load<Texture2D>("Sprites\\BoardComponents\\powers");
            clockReady = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\clock");
            clockDisabled = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\clock_grey");
            laserReady = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\laser");
            laserDisabled = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\laser_grey");
            nukeReady = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\nuke");
            nukeDisabled = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\nuke_grey");
            bulletTimePowerBar = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\power_25");
            laserPowerBar = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\power_25");
            nukePowerBar = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\power_50");
            bulletTimeLeftBar = content.Load<Texture2D>("Sprites\\BoardComponents\\Powers\\bullet_time_time_bar");
            xButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonX");
            yButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonY");
            bButtonTexture = content.Load<Texture2D>("Sprites\\ControllerImages\\xboxControllerButtonB");

            if (reverseDraw)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffect = SpriteEffects.None;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.SetPower(this.gameBoard.GetPower());
            this.maxBulletTime = this.gameBoard.GetMaxBulletTime();
            this.bulletTimeLeft = this.gameBoard.GetBulletTimeLeft();

            spriteBatch.Draw(powerbarScaffolding, 
                             this.position, 
                             null,
                             Color.White,
                             0,
                             Vector2.Zero,
                             Vector2.One, //scale
                             spriteEffect,
                             0);

            //draw stuff for the nuke icon and text
            Vector2 nukePosition = new Vector2(this.position.X + 49, this.position.Y + 46);
            if (this.power >= PowerForNuke)
            {
                spriteBatch.Draw(nukeReady, nukePosition, Color.White);

                spriteBatch.DrawString(this.font,
                       "Press    To Fire!",
                       new Vector2(this.position.X + 112, this.position.Y + 50),
                       new Color(217, 217, 217));
                Rectangle bButtonRect = new Rectangle((int)this.position.X + 168, (int)this.position.Y + 55, 30, 30);
                spriteBatch.Draw(bButtonTexture, bButtonRect, null, Color.White);
            }
            else
            {
                spriteBatch.Draw(nukeDisabled, nukePosition, Color.White);

                spriteBatch.DrawString(this.font,
                      "Not Ready!",
                      new Vector2(this.position.X + 138, this.position.Y + 50),
                      Color.Red);
            }

            //draw stuff for the laser icon and text
            Vector2 laserPosition = new Vector2(this.position.X + 49, this.position.Y + 346);
            if (this.power >= PowerForLasers)
            {
                spriteBatch.Draw(laserReady, laserPosition, Color.White);

                spriteBatch.DrawString(this.font,
                       "Press    To Fire!",
                       new Vector2(this.position.X + 112, this.position.Y + 350),
                       new Color(217, 217, 217));
                Rectangle yButtonRect = new Rectangle((int)this.position.X + 168, (int)this.position.Y + 355, 30, 30);
                spriteBatch.Draw(yButtonTexture, yButtonRect, null, Color.White);
            }
            else
            {
                spriteBatch.Draw(laserDisabled, laserPosition, Color.White);

                spriteBatch.DrawString(this.font,
                       "Not Ready!",
                       new Vector2(this.position.X + 138, this.position.Y + 350),
                       Color.Red);
            }

            //stuff for the bullet time icon and text
            Vector2 btPosition = new Vector2(this.position.X + 49, this.position.Y + 490);
            if (this.power >= PowerForBulletTime)
            {
                spriteBatch.Draw(clockReady, btPosition, Color.White);

                spriteBatch.DrawString(this.font,
                       "Press    To Fire!",
                       new Vector2(this.position.X + 112, this.position.Y + 495),
                       new Color(217, 217, 217));
                Rectangle xButtonRect = new Rectangle((int)this.position.X + 168, (int)this.position.Y + 500, 30, 30);
                spriteBatch.Draw(xButtonTexture, xButtonRect, null, Color.White);
            }
            else
            {
                spriteBatch.Draw(clockDisabled, btPosition, Color.White);

                spriteBatch.DrawString(this.font,
                       "Not Ready!",
                       new Vector2(this.position.X + 138, this.position.Y + 495),
                       Color.Red);
            }

            //draw the bullet time power bar
            float scaleY = (float)((MathHelper.Clamp(this.power, 0f, 25f) * 4f) / 100f);
            spriteBatch.Draw(bulletTimePowerBar, 
                             new Vector2(this.position.X + 2, this.position.Y + 626), 
                             null, 
                             Color.White, 
                             0, 
                             new Vector2(0, 132), //origin
                             new Vector2(1, scaleY), //scale
                             SpriteEffects.None, 
                             0);

            //draw the laser power bar
            int amountOfLaserPower = (int)(MathHelper.Clamp(this.power, 25, 50) - 25);
            scaleY = (float)((MathHelper.Clamp(amountOfLaserPower, 0f, 25f) * 4f) / 100f);
            spriteBatch.Draw(laserPowerBar,
                             new Vector2(this.position.X + 2, this.position.Y + 479),
                             null,
                             Color.White,
                             0,
                             new Vector2(0, 132), //origin
                             new Vector2(1, scaleY), //scale
                             SpriteEffects.None,
                             0);

            //draw the nuke power bar
            int amountOfNukePower = (int)(MathHelper.Clamp(this.power, 50, 100) - 50);
            scaleY = (float)((MathHelper.Clamp(amountOfNukePower, 0f, 50f) * 2f) / 100f);
            spriteBatch.Draw(nukePowerBar,
                             new Vector2(this.position.X + 2, this.position.Y + 332),
                             null,
                             Color.White,
                             0,
                             new Vector2(0, 285), //origin
                             new Vector2(1, scaleY), //scale
                             SpriteEffects.None,
                             0);

            //draw the bullet time left bar
            float scaleX = (float)this.bulletTimeLeft / (float)this.maxBulletTime;
            spriteBatch.Draw(bulletTimeLeftBar,
                             new Vector2(this.position.X + 50, this.position.Y + 545),
                             null,
                             Color.White,
                             0,
                             new Vector2(0, 2), //origin
                             new Vector2(scaleX * .95f, 1.5f), //scale
                             SpriteEffects.None,
                             0);

            //draw the bullet time is active message
            if (this.gameBoard.IsBulletTimeOn())
            {
                String bulletTimeIsActiveString = "Bullet Time Engaged!";
                Vector2 bulletTimeIsActiveOrigin = Utils.Utils.Instance.GetTextStringCenterOrigin(bulletTimeIsActiveString, font);
                Vector2 bulletTimeIsActivePosition = new Vector2(this.position.X + 176, this.position.Y + 575);

                double time = gameTime.TotalGameTime.TotalSeconds;
                float pulsate = (float)Math.Sin(time * 6) + 1;
                float bulletTimeIsActiveScale = 1.0f + pulsate * 0.05f * selectionFade;

                spriteBatch.DrawString(this.font,
                    bulletTimeIsActiveString,
                    bulletTimeIsActivePosition,
                    Color.Yellow,
                    0,
                    bulletTimeIsActiveOrigin,
                    bulletTimeIsActiveScale,
                    SpriteEffects.None,
                    0);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
            selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
        }

        protected void SetPower(int power)
        {
            this.power = power;
            if (this.power >= MaxPower)
            {
                this.power = MaxPower;
            }
        }
    }
}
