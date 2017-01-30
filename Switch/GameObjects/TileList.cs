using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects.Tiles;
using Switch.GameObjects;
using Switch.GameObjects.Sound;
using Switch;

namespace Switch.GameObjects
{
    class TileList
    {
        private List<List<Tile>> tiles;
        private List<Tile> previewTiles;
        private int tileGridHeight;
        private int tileGridWidth;
        private bool previewRowEnabled;
        private GameboardStats stats;

        /**
         * Creates a new two dimensional List. The outmost List represents the tile
         * game boards width and the Lists contained inside represent each column of
         * Tile objects. This makes it easy to visualize and work with the game board
         * as a collection of columns, rather than tons of individual tiles.
         */
        public TileList(int tileGridWidth, int tileGridHeight, GameboardStats stats)
        {
            this.tileGridWidth = tileGridWidth;
            this.tileGridHeight = tileGridHeight;
            this.previewRowEnabled = true;
            this.stats = stats;

            tiles = new List<List<Tile>>();
            for (int i = 0; i < tileGridWidth; i++)
            {
                List<Tile> columns = new List<Tile>();
                tiles.Add(columns);
            }

            previewTiles = new List<Tile>();
        }

        /**
         * Turn the preview row on or off.
         */
        public void SetPreviewRowEnabled(bool enabled)
        {
            this.previewRowEnabled = enabled;
        }

        /**
         * Takes the two dimensional List and turns it into a one-dimensional List of
         * Tile objects containing every tile on the board regardless of state or column
         * location.
         */
        public List<Tile> GetTilesAsList()
        {
            List<Tile> allTiles = new List<Tile>();

            for (int i = 0; i < tiles.Count; i++)
            {
                foreach (Tile tile in tiles[i])
                {
                    allTiles.Add(tile);
                }
            }

            return allTiles;
        }

        /**
         * Returns all the tiles in one of the columns.
         */
        public List<Tile> GetTilesInColumn(int columnIndex)
        {
            return tiles[columnIndex];
        }

        /**
         * Returns all the tiles in one of the columns that are currently seated and not dropping.
         */
        public int GetSeatedColumnHeight(int columnIndex)
        {
            List<Tile> column = tiles[columnIndex];
            int tilesSeated = 0;

            foreach (Tile tile in column)
            {
                if (tile.seated)
                {
                    tilesSeated++;
                }
            }

            return tilesSeated;
        }

        /**
         * Add a tile to the TileList. Must specify which column it is getting inserted into.
         */
        public void Add(Tile tile, int columnIndex)
        {
            tiles[columnIndex].Add(tile);
        }

        /**
         * Remove a tile from the TileList. Must specify which column it is getting removed from.
         */
        public void Remove(Tile tile, int columnIndex)
        {
            tiles[columnIndex].Remove(tile);
        }

        /**
         * An overload for the Remove() function that is less efficient but doesn't require 
         * the column be passed in.
         */
        public void Remove(Tile tile)
        {
            foreach (Tile thisTile in GetTilesAsList())
            {
                if (thisTile.Equals(tile))
                {
                    this.Remove(tile, tile.X);
                }
            }
        }

        /**
         * Checks to see if any tiles are still floating or if they've all become seated.
         */
        public bool AreAllTilesSeated()
        {
            foreach (Tile tile in GetTilesAsList())
            {
                if (!tile.seated)
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * Checks to see if any tiles are still in the middle of an animation or not.
         */
        public bool AreAllTilesDoneAnimating()
        {
            foreach (Tile tile in GetTilesAsList())
            {
                if (tile.IsAnimating())
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * Checks to see if any tiles will be dropped based on the current game time. Same the
         * dropTilesOlderThanAge() function but does not drop any tiles.
         */
        public bool WillTilesGetDropped(int ageThreshold)
        {
            bool tilesWillGetDropped = false;
            foreach (Tile tile in GetTilesAsList())
            {
                if (!tile.seated && tile.age >= ageThreshold)
                {
                    tilesWillGetDropped = true;
                }
            }

            return tilesWillGetDropped;
        }

        /**
         * Traverses the TileList. Any Tile that is older in age (milliseconds) than the value
         * passed in will be "dropped" (moved one spot lower on the Y grid). Passes back a flag that indicates
         * whether or not any tiles were dropped during this check.
         */
        public bool DropTilesOlderThanAge(int ageThreshold)
        {
            bool tilesGotDropped = false;
            foreach (Tile tile in GetTilesAsList())
            {
                if (!tile.seated && tile.age >= ageThreshold)
                {
                    int newY = tile.Y + 1;
                    tile.SetY(newY);
                    tile.ResetAge();
                    tilesGotDropped = true;
                }
            }

            return tilesGotDropped;
        }

        /**
         * Increases the age of all tiles in the TileList by the amount passed in.
         * "age" is represented in milliseconds and represents how long the Tile has
         * remained in it's current spot on the grid.
         */
        public void AgeTiles(int ageIncrement)
        {
            foreach (Tile tile in GetTilesAsList())
            {
                //if (!tile.seated)
                //{
                tile.BumpAge(ageIncrement);
                //}
            }
        }

        /**
         * Traverses the TileList to find any Tile objects that are at the bottom of the
         * grid. These objects are set "seated" so that they will no longer be dropped and
         * so that the game logic will be able to tell when all Tiles are seated and it is
         * time to drop a new set.
         */
        public void SeatTilesAtFloor()
        {
            foreach (Tile tile in GetTilesAsList())
            {
                if (!tile.seated && tile.Y >= this.tileGridHeight - 1)
                {
                    tile.seated = true;
                }
            }
        }

        /**
         * Check to see if a tile has another tile directly below it. If so, seat this tile
         * as it can go no further down. This tile will be checked to see if the tile below
         * is a match or if it should be destroyed for any reason in another call.
         */
        public void SeatTilesOnTopOfOtherTiles()
        {
            List<Tile> allTiles = GetTilesAsList();

            foreach (Tile tileA in allTiles)
            {
                foreach (Tile tileB in allTiles)
                {
                    if (tileB.Y == tileA.Y + 1 &&
                        tileB.X == tileA.X)
                    {
                        tileA.seated = true;
                        break;
                    }
                }
            }
        }

        /**
         * Returns true if the column specified is completely full of tiles height-wise.
         */
        public bool IsColumnFull(int columnIndex)
        {
            if (tiles[columnIndex].Count >= this.tileGridHeight)
            {
                return true;
            }

            return false;
        }

        /**
         * Check all the tiles to see if any of them match a condition that requires they be
         * destroyed (depends on the tile type when it get destroyed).
         */
        public GameboardStats MarkTilesForDeletion()
        {
            //iterate through all the columns
            foreach (List<Tile> column in tiles)
            {
                //iterate through all the tiles in this column and compare to the rest of the column
                foreach (Tile tile in column)
                {
                    //handle top capper tiles
                    if (tile.type == Tile.TileType.TopCapper)
                    {
                        if (tile.seated)
                        {
                            Tile bottomCapper;
                            if ((bottomCapper = BottomCapperExistsBelow(tile)) != null)
                            {
                                ClearCappedTiles(tile, bottomCapper);
                                stats.numberOfCapsCompleted++;
                            }
                            else
                            {
                                tile.MarkForDeletion();
                                stats.numberOfBlocksDestroyed++;
                            }
                        }
                    }
                    //handle multplier tiles
                    else if (tile.type == Tile.TileType.Multiplier)
                    {
                        if (tile.seated)
                        {
                            Tile bottomCapper;
                            if ((bottomCapper = BottomCapperExistsBelow(tile)) == null)
                            {
                                tile.MarkForDeletion();
                                stats.numberOfBlocksDestroyed++;
                            }
                        }
                    }
                    //handle normal tiles or a bottom capper
                    else
                    {
                        foreach (Tile tileToCompare in column)
                        {
                            if ((tileToCompare.Y == tile.Y + 1 || tileToCompare.Y == tile.Y - 1)
                                && tileToCompare.GetStaticTexture() == tile.GetStaticTexture()
                                && tile.seated
                                && tileToCompare.seated
                                && !tile.markedForDeletion
                                && !tileToCompare.markedForDeletion)
                            {
                                tile.MarkForDeletion();
                                tileToCompare.MarkForDeletion();

                                stats.numberOfBlocksDestroyed += 2;
                                stats.score += (Tile.BaseScoreValue * 2);
                                stats.power += (Tile.BaseScoreValue / 8);
                            }
                        }
                    }
                }
            }

            return stats;
        }

        /**
         * Check every tile to see if any are marked for deletion. If so, remove them from the tile list. We do this separately
         * from the check above because we can't remove objects from the List while in the middle of iterating through it.
         */
        public void DeleteTilesMarkedForDeletion(GameBoard gameBoard)
        {
            bool atLeastOneTileDeleted = false;

            foreach (List<Tile> column in tiles)
            {
                Tile[] columnArray = column.ToArray();
                for (int i = 0; i < columnArray.Length; i++)
                {
                    Tile thisTile = columnArray[i];
                    if (thisTile.markedForDeletion)
                    {
                        column.Remove(thisTile);
                        AnimationManager.Instance.StartAnimation("tile-explode", 25, gameBoard.GetTileRectangle(thisTile));
                        atLeastOneTileDeleted = true;
                    }
                }
            }

            if (atLeastOneTileDeleted)
            {
                SoundManager.Instance.PlaySound("explode-tile");
            }
        }

        /**
         * 
         */
        public void MoveTileIntoColumn(Tile tile, int newColumnIndex)
        {
            //first move its position in the tile list
            int oldColumnIndex = tile.X;
            tiles[oldColumnIndex].Remove(tile);
            tiles[newColumnIndex].Add(tile);

            //then set the tiles new position in the tile object itself
            tile.X = newColumnIndex;
        }

        /**
         * 
         */
        private Tile BottomCapperExistsBelow(Tile topCapper)
        {
            Tile bottomCapper = null;
            int columnIndex = topCapper.X;
            List<Tile> bottomCappersInColumn = new List<Tile>();

            foreach (Tile tile in tiles[columnIndex])
            {
                if (tile.Y > topCapper.Y && tile.type == Tile.TileType.BottomCapper)
                {
                    bottomCapper = tile;
                    bottomCappersInColumn.Add(tile);
                }
            }

            if (bottomCappersInColumn.Count > 1)
            {
                foreach (Tile tile in bottomCappersInColumn)
                {
                    if (tile.Y > bottomCapper.Y)
                    {
                        bottomCapper = tile;
                    }
                }
            }

            return bottomCapper;
        }

        /**
         * 
         */
        private void ClearCappedTiles(Tile topCapper, Tile bottomCapper)
        {
            if (topCapper.X != bottomCapper.X)
            {
                return;
            }

            int columnIndex = topCapper.X;
            int numberOfTilesDestroyed = 0;
            int multiplierValue = 1;
            bool regularTileExistsInSandwich = false;

            foreach (Tile tile in tiles[columnIndex])
            {
                if (tile.Y >= topCapper.Y &&
                    tile.Y <= bottomCapper.Y &&
                    !tile.markedForDeletion)
                {
                    tile.StartAnimation("explode", 25);
                    tile.MarkForDeletion();

                    numberOfTilesDestroyed++;

                    if (tile.type == Tile.TileType.Multiplier)
                    {
                        stats.numberOfMultipliersCapped++;

                        if (tile.multiplier > multiplierValue)
                        {
                            multiplierValue = tile.multiplier;
                        }

                        if (tile.multiplier == 2)
                        {
                            stats.numberOf2xMulipliersCapped++;
                        }
                        else if (tile.multiplier == 3)
                        {
                            stats.numberOf3xMulipliersCapped++;
                        }
                        else
                        {
                            stats.numberOf4xMulipliersCapped++;
                        }
                    }

                    if (tile.type == Tile.TileType.Normal)
                    {
                        regularTileExistsInSandwich = true;
                    }
                }
            }

            if (!regularTileExistsInSandwich)
            {
                multiplierValue = 1;
            }

            stats.score += (numberOfTilesDestroyed * Tile.BaseScoreValue * multiplierValue);
            stats.power += (numberOfTilesDestroyed * (Tile.BaseScoreValue / 8));
            stats.numberOfTilesDestroyedByCapping += numberOfTilesDestroyed;
            stats.numberOfBlocksDestroyed += numberOfTilesDestroyed;
        }

        /**
         * Swaps the tiles in two columns. Usually called when the player requests the rotater to spin so that stacks get swapped.
         **/
        public void SwapColumns(int colLeft, int colRight)
        {
            List<Tile> leftColumn = this.GetTilesInColumn(colLeft);
            List<Tile> rightColumn = this.GetTilesInColumn(colRight);

            int leftColumnHeight = this.GetSeatedColumnHeight(colLeft);
            int rightColumnHeight = this.GetSeatedColumnHeight(colRight);
            int tallestColumn;

            if (leftColumnHeight >= rightColumnHeight)
            {
                tallestColumn = (tileGridHeight - leftColumnHeight) - 1;
            }
            else
            {
                tallestColumn = (tileGridHeight - rightColumnHeight) - 1;
            }

            Tile[] leftColumnArray = leftColumn.ToArray();
            Tile[] rightColumnArray = rightColumn.ToArray();

            for (int i = 0; i < leftColumnArray.Length; i++)
            {
                Tile thisTile = leftColumnArray[i];
                if (thisTile.seated || thisTile.Y > tallestColumn)
                {
                    this.MoveTileIntoColumn(thisTile, colLeft + 1);
                }
            }

            for (int i = 0; i < rightColumnArray.Length; i++)
            {
                Tile thisTile = rightColumnArray[i];
                if (thisTile.seated || thisTile.Y > tallestColumn)
                {
                    this.MoveTileIntoColumn(thisTile, colRight - 1);
                }
            }
        }

        public int ClearColumn(int columnIndex)
        {
            int tilesDestroyed = 0;
            List<Tile> column = tiles[columnIndex];

            foreach (Tile tile in column)
            {
                tile.MarkForDeletion();
                tilesDestroyed++;

                stats.score += Tile.BaseScoreValue;
            }

            return tilesDestroyed;
        }

        public int ClearBoard()
        {
            int tilesDestroyed = GetTilesAsList().Count;

            for (int i = 0; i < tiles.Count; i++)
            {
                ClearColumn(i);
            }

            return tilesDestroyed;
        }

        public List<Tile> GetUnseatedTiles()
        {
            List<Tile> unseatedTiles = new List<Tile>();

            foreach (Tile tile in GetTilesAsList())
            {
                if (!tile.seated)
                {
                    unseatedTiles.Add(tile);
                }
            }

            return unseatedTiles;
        }
    }
}
