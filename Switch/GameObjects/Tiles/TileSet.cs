using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Tiles
{
    class TileSet
    {
        private Tile topCapper { get; set; }
        private Tile bottomCapper { get; set; }
        public Rotater rotater { get; set; }
        public Texture2D boardBackground { get; set; }
        public Texture2D nukeImage { get; set; }
        public Texture2D blankImage { get; set; }

        private List<Tile> tiles;
        private List<Tile> multiplierTiles;

        public TileSet()
        {
            tiles = new List<Tile>();
            multiplierTiles = new List<Tile>();
        }

        public void AddNormalTile(Tile tile)
        {
            this.tiles.Add(tile);
        }

        public void AddMultiplierTile(Tile tile)
        {
            this.multiplierTiles.Add(tile);
        }

        public int GetSize()
        {
            return this.tiles.Count + this.multiplierTiles.Count + 2;
        }

        /**
         * Just returns this first normal tile in the TileSet. Used by the GameBoard to determine the height/width of tiles in this tileset.
         */
        public Tile GetRefTile()
        {
            if (this.tiles[0] != null)
            {
                return this.tiles[0];
            }
            else
            {
                return null;
            }
        }

        public Tile[] ToArray()
        {
            Tile[] tileArray = new Tile[this.GetSize()];

            int index = 0;

            foreach (Tile thisTile in tiles)
            {
                tileArray[index] = thisTile;
                index++;
            }

            foreach (Tile thisTile in multiplierTiles)
            {
                tileArray[index] = thisTile;
                index++;
            }

            tileArray[index] = this.topCapper;
            tileArray[index + 1] = this.bottomCapper;

            return tileArray;
        }

        public int GetNumberOfNormalTilesInTileSet()
        {
            return tiles.Count;
        }

        public static TileSet LoadAndGetDefaultTileset(ContentManager content, Difficulty difficulty)
        {
            TileSet tileSet = new TileSet();

            //load spritesheets
            SpriteSheet tileExplosionSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\explosion-spritesheet"), 5);
            SpriteSheet rotateSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\rotater-spritesheet"), 6);
            SpriteSheet idleRotaterSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\rotater-idle"), 2);
            SpriteSheet laserSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\laser-spritesheet"), 5);

            SpriteSheet idleDiamondSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\diamond-spritesheet"), 3);
            SpriteSheet idleDropSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\drop-spritesheet"), 4);
            SpriteSheet idleTriangleSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\triangle-spritesheet"), 4);
            SpriteSheet idleOctagonSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\octagon-spritesheet"), 4);
            SpriteSheet idleWingSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\wing-spritesheet"), 3);
            SpriteSheet idleCrescentSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\crescent-spritesheet"), 3);
            SpriteSheet idleBottomCapperSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\cap_bottom-spritesheet"), 2);
            SpriteSheet idleTopCapperSpriteSheet = new SpriteSheet(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\objects\\IdleAnimations\\cap_top-spritesheet"), 2);

            //add normal tiles
            Tile crescentTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\crescent"),
                content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\blue"),
                Tile.TileType.Normal);
            crescentTile.AddAnimation("explode", tileExplosionSpriteSheet);
            crescentTile.AddAnimation("idle", idleCrescentSpriteSheet);
            tileSet.AddNormalTile(crescentTile);

            if (tileSet.GetNumberOfNormalTilesInTileSet() < difficulty.GetSizeOfTileSet())
            {
                Tile diamondTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\diamond"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\green"),
                    Tile.TileType.Normal);
                diamondTile.AddAnimation("explode", tileExplosionSpriteSheet);
                diamondTile.AddAnimation("idle", idleDiamondSpriteSheet);
                tileSet.AddNormalTile(diamondTile);
            }

            if (tileSet.GetNumberOfNormalTilesInTileSet() < difficulty.GetSizeOfTileSet())
            {
                Tile dropTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\drop"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\orange"),
                    Tile.TileType.Normal);
                dropTile.AddAnimation("explode", tileExplosionSpriteSheet);
                dropTile.AddAnimation("idle", idleDropSpriteSheet);
                tileSet.AddNormalTile(dropTile);
            }

            if (tileSet.GetNumberOfNormalTilesInTileSet() < difficulty.GetSizeOfTileSet())
            {
                Tile triangleTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\triangle"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\red"),
                    Tile.TileType.Normal);
                triangleTile.AddAnimation("explode", tileExplosionSpriteSheet);
                triangleTile.AddAnimation("idle", idleTriangleSpriteSheet);
                tileSet.AddNormalTile(triangleTile);
            }

            if (tileSet.GetNumberOfNormalTilesInTileSet() < difficulty.GetSizeOfTileSet())
            {
                Tile octagonTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\octagon"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\purple"),
                    Tile.TileType.Normal);
                octagonTile.AddAnimation("explode", tileExplosionSpriteSheet);
                octagonTile.AddAnimation("idle", idleOctagonSpriteSheet);
                tileSet.AddNormalTile(octagonTile);
            }

            if (tileSet.GetNumberOfNormalTilesInTileSet() < difficulty.GetSizeOfTileSet())
            {
                Tile wingTile = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\wing_triangle"),
                    content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\teal"),
                    Tile.TileType.Normal);
                wingTile.AddAnimation("explode", tileExplosionSpriteSheet);
                wingTile.AddAnimation("idle", idleWingSpriteSheet);
                tileSet.AddNormalTile(wingTile);
            }

            //add capper tiles
            Tile topCapper = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\cap_top"), Tile.TileType.TopCapper);
            topCapper.AddAnimation("explode", tileExplosionSpriteSheet);
            topCapper.AddAnimation("idle", idleTopCapperSpriteSheet);
            tileSet.topCapper = topCapper;

            Tile bottomCapper = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\cap_bottom"), Tile.TileType.BottomCapper);
            bottomCapper.AddAnimation("explode", tileExplosionSpriteSheet);
            bottomCapper.AddAnimation("idle", idleBottomCapperSpriteSheet);
            tileSet.bottomCapper = bottomCapper;

            //add rotater
            Rotater rotater = new Rotater(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\rotater-idle"));
            rotater.AddAnimation("rotate", rotateSpriteSheet);
            rotater.AddAnimation("idle", idleRotaterSpriteSheet);
            tileSet.rotater = rotater;

            //add multiplier tiles
            Tile multiplierTile2X = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\2x"),
                content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\mult_bronze"),
                Tile.TileType.Multiplier);
            multiplierTile2X.AddAnimation("explode", tileExplosionSpriteSheet);
            multiplierTile2X.multiplier = 2;
            tileSet.AddMultiplierTile(multiplierTile2X);

            Tile mutliplierTile3X = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\3x"),
                content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\mult_silver"),
                Tile.TileType.Multiplier);
            mutliplierTile3X.AddAnimation("explode", tileExplosionSpriteSheet);
            mutliplierTile3X.multiplier = 3;
            tileSet.AddMultiplierTile(mutliplierTile3X);

            Tile multiplierTile4X = new Tile(content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Objects\\4x"),
                content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\Backgrounds\\mult_gold"),
                Tile.TileType.Multiplier);
            multiplierTile4X.AddAnimation("explode", tileExplosionSpriteSheet);
            multiplierTile4X.multiplier = 4;
            tileSet.AddMultiplierTile(multiplierTile4X);

            //load board background
            tileSet.boardBackground = content.Load<Texture2D>("Sprites\\BoardComponents\\gameboard");

            //load nuke images
            tileSet.nukeImage = content.Load<Texture2D>("Sprites\\Tiles\\TileSet1\\nuke-image");
            tileSet.blankImage = content.Load<Texture2D>("blank");

            return tileSet;
        }
    }
}
