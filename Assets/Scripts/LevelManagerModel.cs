using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerModel : MonoBehaviour
{
    public static LevelManagerModel Instance;

    private GameLevelModel[] _levels;
    private int _currentLevel = 0;
    void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        _levels = GetComponent<LevelParser>().ReadLevels();
        
    }
    
    public GameLevelModel GetCurrentLevel()
    {
        return _levels[_currentLevel % _levels.Length];

    }
    [Serializable]
    internal class LevelJsonWrapper
    {
        public GameLevelModel[] levels;
    }

    public void AdvanceLevel()
    {
        _currentLevel++;
    }
}

