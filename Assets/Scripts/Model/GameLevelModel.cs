

using System;

namespace Model
{
    [Serializable]
    public class GameLevelModel
    {

        public int BallCount;
        public int[] AllowedTypes;
        public int[] TargetCounts;
    }
}