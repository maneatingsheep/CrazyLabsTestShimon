using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerModel : MonoBehaviour
{
    public static LevelManagerModel Instance;
    void Awake()
    {
        Instance = this;
    }

    public GameLevelModel GetCurrentLevel()
    {
        GameLevelModel glm = new GameLevelModel();
        glm.BallCount = 100;

        return glm;

    }
}
