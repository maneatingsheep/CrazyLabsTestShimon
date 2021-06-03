using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameplayView GameplayViewRef;
    public GameplayUIView UIViewRef;
    private GameLevelModel _currentLevel;

    public Action OnGameOver;
    
    private int[] _currentCounts;
    
    public void Init()
    {
        UIViewRef.Init();
        GameplayViewRef.Init();
        GameplayViewRef.OnBallsEliminated += BallsElimiated;
        
        GameplayViewRef.TransitionOut(false);
    }

    private void BallsElimiated(int typeID, int count)
    {
        if (typeID >= _currentCounts.Length)
        {
            GameplayViewRef.DropBalls();
            return;
        };
        
        
        _currentCounts[typeID] += count;
        UIViewRef.UpdateView(typeID,  Mathf.Max(0, _currentLevel.TargetCounts[typeID] - _currentCounts[typeID]));

        bool IsGameOver = true;
        for (int i = 0; i < _currentCounts.Length; i++)
        {
            IsGameOver &= _currentLevel.TargetCounts[i] - _currentCounts[i] <= 0;
        }

        if (IsGameOver)
        {
            OnGameOver();
        }
        else
        {
            GameplayViewRef.DropBalls();
        }
        
    }

    public void StartGame()
    {
        _currentLevel = LevelManagerModel.Instance.GetCurrentLevel();

        _currentCounts = new int[_currentLevel.TargetCounts.Length];
        
        UIViewRef.Reset();
        GameplayViewRef.Reset();
        GameplayViewRef.TransitionIn(OnTransitionDone);
    }

    private void OnTransitionDone()
    {
        GameplayViewRef.StartGame();
    }

    public void EndGame()
    {
        GameplayViewRef.TransitionOut(true);
    }
}
