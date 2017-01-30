using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty.DifficultyObjects
{
    class Hard : Difficulty
    {
        public string GetName()
        {
            return "Hard";
        }

        public string GetDescription()
        {
            return "Hard";
        }

        public int GetNumberOfTilesToDropPerRound()
        {
            return 4;
        }

        public int GetNumberOfTilesInTheGameboardWidth()
        {
            return 6;
        }

        public int GetNumberOfTilesInTheGameboardHeight()
        {
            return 11;
        }

        public int GetStartingSpeed()
        {
            return 450;
        }

        public int GetMaxSpeed()
        {
            return 200;
        }

        public int GetSizeOfTileSet()
        {
            return 6;
        }
    }
}
