using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Utils.Difficulty
{
    public interface Difficulty
    {
        String GetName();
        String GetDescription();
        int GetNumberOfTilesToDropPerRound(); //how many tiles to drop at a time
        int GetNumberOfTilesInTheGameboardWidth(); //number of tiles across
        int GetNumberOfTilesInTheGameboardHeight(); //number of tiles down
        int GetStartingSpeed(); //in millis
        int GetMaxSpeed(); //in millis, lower is faster so higher numbers are EASIER
        int GetSizeOfTileSet(); //how many basic tile types to use, more is HARDER
    }
}
