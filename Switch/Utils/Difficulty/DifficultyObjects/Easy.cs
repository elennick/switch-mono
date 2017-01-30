using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty.DifficultyObjects
{
    class Easy : Difficulty
    {
        public string GetName()
        {
            return "Easy";
        }

        public string GetDescription()
        {
            return "Easy";
        }

        public int GetNumberOfTilesToDropPerRound()
        {
            return 2;
        }

        public int GetNumberOfTilesInTheGameboardWidth()
        {
            return 4;
        }

        public int GetNumberOfTilesInTheGameboardHeight()
        {
            return 8;
        }

        public int GetStartingSpeed()
        {
            return 1200;
        }

        public int GetMaxSpeed()
        {
            return 1000;
        }

        public int GetSizeOfTileSet()
        {
            return 3;
        }
    }
}
