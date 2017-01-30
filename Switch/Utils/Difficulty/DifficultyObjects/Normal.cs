using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty.DifficultyObjects
{
    class Normal : Difficulty
    {
        public string GetName()
        {
            return "Normal";
        }

        public string GetDescription()
        {
            return "Normal";
        }

        public int GetNumberOfTilesToDropPerRound()
        {
            return 3;
        }

        public int GetNumberOfTilesInTheGameboardWidth()
        {
            return 5;
        }

        public int GetNumberOfTilesInTheGameboardHeight()
        {
            return 9;
        }

        public int GetStartingSpeed()
        {
            return 800;
        }

        public int GetMaxSpeed()
        {
            return 300;
        }

        public int GetSizeOfTileSet()
        {
            return 4;
        }
    }
}
