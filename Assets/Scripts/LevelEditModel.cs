using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditModel : MonoBehaviour
{
    public int BallCount;
    public int[] AllowedTypes;
    public int[] TargetCounts;


    public GameLevelModel GenerateLevel()
    {
        GameLevelModel glm = new GameLevelModel();
        glm.BallCount = BallCount;
        glm.AllowedTypes = AllowedTypes;
        glm.TargetCounts = TargetCounts;

        return glm;
    }
        
}
