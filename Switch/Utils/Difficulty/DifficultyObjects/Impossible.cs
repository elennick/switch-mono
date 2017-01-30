using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty.DifficultyObjects
{
    class Impossible : Difficulty
    {
        public string GetName()
        {
            return "Impossible";
        }

        public string GetDescription()
        {
            return "Impossible";
        }

        public int GetNumberOfTilesToDropPerRound()
        {
            return 5;
        }

        public int GetNumberOfTilesInTheGameboardWidth()
        {
            return 7;
        }

        public int GetNumberOfTilesInTheGameboardHeight()
        {
            return 13;
        }

        public int GetStartingSpeed()
        {
            return 250;
        }

        public int GetMaxSpeed()
        {
            return 50;
        }

        public int GetSizeOfTileSet()
        {
            return 6;
        }
    }
}
