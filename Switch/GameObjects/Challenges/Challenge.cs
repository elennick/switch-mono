using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch;
using Switch.Menus;
using Switch.Utils;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges
{
    interface IChallenge
    {
        String GetName();
        String GetDescription();
        Difficulty GetDifficulty();
        bool IsCompleted(GameboardStats stats);
        String GetStatusText(GameboardStats stats);
        int IsSpeedUpEnabled();
        int StartingPower();
    }
}
